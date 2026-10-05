namespace Backend.Infrustructure.Services.Ffmpegs
{
    public class FrameBufferProcessor : IFrameBufferProcessor
    {
        private readonly ILogger<FrameBufferProcessor> _logger;
        private readonly byte[] _jpegEndMarker = { 0xFF, 0xD9 };

        public FrameBufferProcessor(
            ILogger<FrameBufferProcessor> logger)
        {
            _logger = logger;
        }

        public async Task ProcessStreamAsync(Stream outputStream, CameraId sourceId, CancellationToken cancellationToken)
        {
            var frameBuffer = new List<byte>();
            var readBuffer = new byte[8192];

            try
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    var bytesRead = await ReadWithTimeoutAsync(outputStream, readBuffer, cancellationToken);

                    if (bytesRead <= 0)
                        break;

                    await ProcessBufferChunk(readBuffer, bytesRead, frameBuffer, sourceId, cancellationToken);
                }
            }
            catch (Exception ex) when (ex is not OperationCanceledException)
            {
                _logger.LogError(ex, "Error processing stream");
                throw;
            }
        }

        private async Task<int> ReadWithTimeoutAsync(Stream stream, byte[] buffer, CancellationToken cancellationToken)
        {
            var readTask = stream.ReadAsync(buffer, 0, buffer.Length, cancellationToken);
            var timeoutTask = Task.Delay(TimeSpan.FromSeconds(30), cancellationToken);

            var completed = await Task.WhenAny(readTask, timeoutTask);

            if (completed == timeoutTask)
                throw new TimeoutException("Camera disconnected");

            return await readTask;
        }

        private async Task ProcessBufferChunk(byte[] chunk, int bytesRead, List<byte> frameBuffer, CameraId sourceId, CancellationToken cancellationToken)
        {
            for (int i = 0; i < bytesRead; i++)
            {
                frameBuffer.Add(chunk[i]);

                if (IsJpegEndMarker(frameBuffer))
                {
                    await ProcessCompleteFrame(frameBuffer.ToArray(), sourceId, cancellationToken);
                    frameBuffer.Clear();
                }
            }
        }

        private bool IsJpegEndMarker(List<byte> buffer)
        {
            return buffer.Count >= 2 &&
                   buffer[^2] == _jpegEndMarker[0] &&
                   buffer[^1] == _jpegEndMarker[1];
        }

        private async Task ProcessCompleteFrame(byte[] jpegBytes, CameraId sourceId, CancellationToken cancellationToken)
        {
            var frameData = new FrameData(jpegBytes, sourceId);

            Console.WriteLine("++++++ frame extracted ***");
        }


    }

}
