namespace Backend.Infrustructure.Services.Ffmpegs
{
    public sealed class FFmpegMediaProbeService : IMediaProbeService
    {
        private readonly string _ffprobePath;
        private readonly int _timeoutSeconds;

        public FFmpegMediaProbeService(string ffprobePath = "ffprobe", int timeoutSeconds = 10)
        {
            _ffprobePath = ffprobePath;
            _timeoutSeconds = timeoutSeconds;
        }

        public async Task<MediaInfo> GetMediaInfoAsync(
            string input,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return new MediaInfo { Error = "Media input is required." };
            }

            bool isRtsp = input.StartsWith("rtsp://", StringComparison.OrdinalIgnoreCase)
                       || input.StartsWith("rtsps://", StringComparison.OrdinalIgnoreCase);

            var psi = new ProcessStartInfo
            {
                FileName = _ffprobePath,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            // --- Global verbosity ---
            psi.ArgumentList.Add("-v");
            psi.ArgumentList.Add("error");
            psi.ArgumentList.Add("-hide_banner");

            // --- RTSP-only options ---
            if (isRtsp)
            {
                psi.ArgumentList.Add("-rtsp_transport");
                psi.ArgumentList.Add("tcp");

                psi.ArgumentList.Add("-rw_timeout");
                psi.ArgumentList.Add("10000000"); // microseconds

                psi.ArgumentList.Add("-analyzeduration");
                psi.ArgumentList.Add("5000000");

                psi.ArgumentList.Add("-probesize");
                psi.ArgumentList.Add("5000000");
            }

            // --- Select first video stream ---
            psi.ArgumentList.Add("-select_streams");
            psi.ArgumentList.Add("v:0");

            // --- FIX: single -show_entries, sections joined by ':' ---
            psi.ArgumentList.Add("-show_entries");
            psi.ArgumentList.Add(
                "stream=width,height,r_frame_rate,codec_name:format=format_name,duration");

            psi.ArgumentList.Add("-of");
            psi.ArgumentList.Add("default=noprint_wrappers=1:nokey=0");

            psi.ArgumentList.Add(input);

            using var process = new Process { StartInfo = psi };

            try
            {
                if (!process.Start())
                {
                    return new MediaInfo { Error = "Unable to start FFprobe." };
                }

                var stdoutTask = process.StandardOutput.ReadToEndAsync();
                var stderrTask = process.StandardError.ReadToEndAsync();

                using var timeoutCts =
                    CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
                timeoutCts.CancelAfter(TimeSpan.FromSeconds(_timeoutSeconds));

                try
                {
                    await process.WaitForExitAsync(timeoutCts.Token);
                }
                catch (OperationCanceledException)
                    when (!cancellationToken.IsCancellationRequested)
                {
                    TryKill(process);
                    await Task.WhenAll(stdoutTask, stderrTask);
                    return new MediaInfo { Error = "FFprobe timed out." };
                }

                var output = await stdoutTask;
                var error = await stderrTask;

                if (process.ExitCode != 0)
                {
                    return new MediaInfo
                    {
                        Error = string.IsNullOrWhiteSpace(error)
                            ? $"FFprobe exited with code {process.ExitCode}."
                            : error.Trim()
                    };
                }

                return ParseOutput(output);
            }
            catch (OperationCanceledException)
            {
                TryKill(process);
                throw;
            }
            catch (Exception ex)
            {
                TryKill(process);
                return new MediaInfo { Error = $"Unable to probe media: {ex.Message}" };
            }
        }

        private static void TryKill(Process process)
        {
            try
            {
                if (!process.HasExited)
                {
                    process.Kill(entireProcessTree: true);
                }
            }
            catch
            {
                // Ignore cleanup errors.
            }
        }

        private static MediaInfo ParseOutput(string output)
        {
            int width = 0;
            int height = 0;

            double? fps = null;
            double? duration = null;

            string? codec = null;
            string? format = null;

            foreach (var rawLine in output.Split(
                         new[] { '\r', '\n' },
                         StringSplitOptions.RemoveEmptyEntries))
            {
                var line = rawLine.Trim();

                // Skip section wrappers just in case.
                if (line.Length == 0 || line[0] == '[') continue;

                if (line.StartsWith("width=", StringComparison.Ordinal))
                {
                    int.TryParse(line.AsSpan("width=".Length), out width);
                }
                else if (line.StartsWith("height=", StringComparison.Ordinal))
                {
                    int.TryParse(line.AsSpan("height=".Length), out height);
                }
                else if (line.StartsWith("r_frame_rate=", StringComparison.Ordinal))
                {
                    fps = ParseFrameRate(line["r_frame_rate=".Length..]);
                }
                else if (line.StartsWith("codec_name=", StringComparison.Ordinal))
                {
                    codec = line["codec_name=".Length..].Trim();
                }
                else if (line.StartsWith("format_name=", StringComparison.Ordinal))
                {
                    format = line["format_name=".Length..].Trim();
                }
                else if (line.StartsWith("duration=", StringComparison.Ordinal))
                {
                    if (double.TryParse(
                            line["duration=".Length..],
                            NumberStyles.Float,
                            CultureInfo.InvariantCulture,
                            out var parsedDuration))
                    {
                        duration = parsedDuration;
                    }
                }
            }

            if (width <= 0 || height <= 0)
            {
                return new MediaInfo
                {
                    Error = "No video stream detected."
                };
            }

            return new MediaInfo
            {
                Width = width,
                Height = height,
                Fps = fps,
                VideoCodec = codec,
                Format = format,
                DurationSeconds = duration
            };
        }

        private static double? ParseFrameRate(string value)
        {
            var parts = value.Split('/');

            if (parts.Length == 2 &&
                double.TryParse(parts[0], NumberStyles.Float, CultureInfo.InvariantCulture, out var num) &&
                double.TryParse(parts[1], NumberStyles.Float, CultureInfo.InvariantCulture, out var den) &&
                den != 0)
            {
                return num / den;
            }

            if (double.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out var fps))
                return fps;

            return null;
        }
    }
}
