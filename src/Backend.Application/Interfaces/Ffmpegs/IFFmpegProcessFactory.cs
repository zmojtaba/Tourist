namespace Backend.Application.Interfaces.Ffmpegs
{
    public interface IFFmpegProcessFactory
    {
        Process CreateProcess(string streamUrl);
    }
}
