namespace Backend.Infrustructure.Interfaces
{
    public interface IRabbitMqConnectionProvider
    {
        Task<IConnection> GetConnectionAsync(CancellationToken ct);
        Task<IChannel> CreateChannelAsync(CreateChannelOptions? options, CancellationToken ct);
    }
}
