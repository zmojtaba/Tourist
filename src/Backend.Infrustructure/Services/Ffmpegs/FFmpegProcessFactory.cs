namespace Backend.Infrustructure.Services.Ffmpegs
{
    public class FFmpegProcessFactory : IFFmpegProcessFactory
    {
        public Process CreateProcess(string streamUrl)
        {
            var process = new Process();
            process.StartInfo.FileName = "ffmpeg";
            process.StartInfo.Arguments = BuildArguments(streamUrl);
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;

            return process;
        }

        private string BuildArguments(string streamUrl)
        {
            if (streamUrl.StartsWith("rtsp://", StringComparison.OrdinalIgnoreCase))
            {
                return $"-rtsp_transport tcp -re -i \"{streamUrl}\" -f image2pipe -vcodec mjpeg -q:v 1 -r 5 -vf scale=1200:900 -an -";
            }

            return $"-re -hide_banner -i \"{streamUrl}\" -f image2pipe -vcodec mjpeg -q:v 30 -pix_fmt yuvj420p -r 5 -vf scale=1228:921 -an -";
        }
    }

}
