using Backend.Application.Interfaces.Frames;

namespace Backend.Infrustructure.Services.Frames
{
    public sealed class FrameChannel : IFramePublisher
    {
        private readonly Channel<RawFrame> _channel;

        public FrameChannel(IOptions<FramePipelineOptions> options)
        {
            var capacity = Math.Max(1, options.Value.ChannelCapacity);
            _channel = Channel.CreateBounded<RawFrame>(new BoundedChannelOptions(capacity)
            {
                FullMode = BoundedChannelFullMode.DropOldest,
                SingleReader = true,
                SingleWriter = false,
                AllowSynchronousContinuations = false
            });
        }

        public ValueTask PublishAsync(RawFrame frame, CancellationToken ct)
            => _channel.Writer.WriteAsync(frame, ct);

        public IAsyncEnumerable<RawFrame> ReadAllAsync(CancellationToken ct)
            => _channel.Reader.ReadAllAsync(ct);
    }
}
