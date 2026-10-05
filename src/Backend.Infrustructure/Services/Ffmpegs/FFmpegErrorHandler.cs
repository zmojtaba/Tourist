using Backend.Application.Common.models.Ffmpegs;

namespace Backend.Infrustructure.Services.Ffmpegs
{
    public class FFmpegErrorHandler : IErrorHandler
    {
        private readonly ILogger<FFmpegErrorHandler> _logger;
        private readonly Dictionary<string, Func<string, FFmpegMessage>> _errorHandlers;

        public FFmpegErrorHandler(ILogger<FFmpegErrorHandler> logger)
        {
            _logger = logger;
            _errorHandlers = new Dictionary<string, Func<string, FFmpegMessage>>(StringComparer.OrdinalIgnoreCase)
            {
                ["Connection Timed out"] = msg => new FFmpegMessage { Status = "failed", Message = "Connection timed out" },
                ["No such file or directory"] = msg => new FFmpegMessage { Status = "failed", Message = "No such file or directory" },
                ["Invalid data"] = msg => new FFmpegMessage { Status = "failed", Message = "Invalid data" }
            };
        }

        public bool TryHandleError(string errorData, out FFmpegMessage errorMessage)
        {
            errorMessage = null;

            if (string.IsNullOrEmpty(errorData))
                return false;

            _logger.LogError($"[FFmpeg Error] {errorData}");

            foreach (var handler in _errorHandlers)
            {
                if (errorData.IndexOf(handler.Key, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    errorMessage = handler.Value(errorData);
                    return true;
                }
            }

            return false;
        }
    }

}
