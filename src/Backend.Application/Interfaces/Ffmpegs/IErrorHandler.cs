namespace Backend.Application.Interfaces.Ffmpegs
{
    public interface IErrorHandler
    {
        bool TryHandleError(string errorData, out FFmpegMessage errorMessage);
    }
}
