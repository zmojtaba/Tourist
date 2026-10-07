using RabbitMQ.Client.Events;
using System.Text;

namespace Backend.Infrustructure.Services.Frames
{
    public sealed class AiResultConsumerService : BackgroundService
    {
        private readonly IConnectionFactory _rabbitFactory;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<AiResultConsumerService> _logger;

        private IConnection? _connection;
        private IChannel? _channel;

        public AiResultConsumerService(
            IConnectionFactory rabbitFactory,
            IServiceScopeFactory scopeFactory,
            ILogger<AiResultConsumerService> logger)
        {
            _rabbitFactory = rabbitFactory;
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _connection = await _rabbitFactory.CreateConnectionAsync(stoppingToken);
            _channel = await _connection.CreateChannelAsync(cancellationToken: stoppingToken);

            await _channel.QueueDeclareAsync(
                queue: RabbitTopology.ResultQueue,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null,
                cancellationToken: stoppingToken);

            // Process one result at a time; results are cheap to handle.
            await _channel.BasicQosAsync(prefetchSize: 0, prefetchCount: 8, global: false,
                                         cancellationToken: stoppingToken);

            var consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.ReceivedAsync += OnReceivedAsync;

            await _channel.BasicConsumeAsync(
                queue: RabbitTopology.ResultQueue,
                autoAck: false,
                consumer: consumer,
                cancellationToken: stoppingToken);

            _logger.LogInformation("AiResultConsumerService listening on {Queue}",
                RabbitTopology.ResultQueue);

            try
            {
                await Task.Delay(Timeout.Infinite, stoppingToken);
            }
            catch (OperationCanceledException) { /* shutdown */ }
            finally
            {
                try { if (_channel is not null) await _channel.CloseAsync(); } catch { }
                try { if (_channel is not null) await _channel.DisposeAsync(); } catch { }
                try { if (_connection is not null) await _connection.CloseAsync(); } catch { }
                try { if (_connection is not null) await _connection.DisposeAsync(); } catch { }
            }
        }

        private async Task OnReceivedAsync(object sender, BasicDeliverEventArgs ea)
        {
            var channel = _channel!;
            try
            {
                var json = Encoding.UTF8.GetString(ea.Body.Span);
                var result = JsonSerializer.Deserialize<AiDetectionResult>(json);

                if (result is null)
                {
                    _logger.LogWarning("Received null or malformed AI result, acking");
                    await channel.BasicAckAsync(ea.DeliveryTag, multiple: false);
                    return;
                }

                using var scope = _scopeFactory.CreateScope();
                var handler = scope.ServiceProvider.GetRequiredService<IAiResultHandler>();
                await handler.HandleAsync(result, CancellationToken.None);

                await channel.BasicAckAsync(ea.DeliveryTag, multiple: false);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to process AI result");
                // requeue: false to avoid poison-message loops.
                await channel.BasicNackAsync(ea.DeliveryTag, multiple: false, requeue: false);
            }
        }
    }

    public interface IAiResultHandler
    {
        Task HandleAsync(AiDetectionResult result, CancellationToken ct);
    }

}
