namespace Backend.Infrustructure.Services.Ffmpegs
{
    public sealed class FFmpegMediaProbeService : IMediaProbeService
    {
        private readonly string _ffprobePath;

        public FFmpegMediaProbeService(string ffprobePath = "ffprobe")
        {
            _ffprobePath = ffprobePath;
        }

        public async Task<MediaInfo> GetMediaInfoAsync(
            string input,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return new MediaInfo
                {
                    Error = "Media input is required."
                };
            }

            var processStartInfo = new ProcessStartInfo
            {
                FileName = _ffprobePath,

                RedirectStandardOutput = true,
                RedirectStandardError = true,

                UseShellExecute = false,
                CreateNoWindow = true
            };

            /*
             * Do NOT construct:
             *
             * Arguments = $"... \"{input}\""
             *
             * ArgumentList is safer and handles escaping correctly.
             */

            processStartInfo.ArgumentList.Add("-v");
            processStartInfo.ArgumentList.Add("error");

            processStartInfo.ArgumentList.Add("-rtsp_transport");
            processStartInfo.ArgumentList.Add("tcp");

            processStartInfo.ArgumentList.Add("-rw_timeout");
            processStartInfo.ArgumentList.Add("10000000");

            processStartInfo.ArgumentList.Add("-analyzeduration");
            processStartInfo.ArgumentList.Add("5000000");

            processStartInfo.ArgumentList.Add("-probesize");
            processStartInfo.ArgumentList.Add("5000000");

            processStartInfo.ArgumentList.Add("-select_streams");
            processStartInfo.ArgumentList.Add("v:0");

            processStartInfo.ArgumentList.Add("-show_entries");
            processStartInfo.ArgumentList.Add(
                "stream=width,height,r_frame_rate,codec_name");

            processStartInfo.ArgumentList.Add("-show_entries");
            processStartInfo.ArgumentList.Add(
                "format=format_name,duration");

            processStartInfo.ArgumentList.Add("-of");
            processStartInfo.ArgumentList.Add("default=noprint_wrappers=1");

            processStartInfo.ArgumentList.Add(input);

            using var process = new Process
            {
                StartInfo = processStartInfo
            };

            try
            {
                if (!process.Start())
                {
                    return new MediaInfo
                    {
                        Error = "Unable to start FFprobe."
                    };
                }

                var stdoutTask = process.StandardOutput.ReadToEndAsync();
                var stderrTask = process.StandardError.ReadToEndAsync();

                using var timeoutCts =
                    CancellationTokenSource.CreateLinkedTokenSource(
                        cancellationToken);

                timeoutCts.CancelAfter(TimeSpan.FromSeconds(10));

                try
                {
                    await process.WaitForExitAsync(timeoutCts.Token);
                }
                catch (OperationCanceledException)
                    when (!cancellationToken.IsCancellationRequested)
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

                    await Task.WhenAll(stdoutTask, stderrTask);

                    return new MediaInfo
                    {
                        Error = "FFprobe timed out."
                    };
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

                throw;
            }
            catch (Exception ex)
            {
                return new MediaInfo
                {
                    Error = $"Unable to probe media: {ex.Message}"
                };
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
                         '\r',
                         '\n',
                         StringSplitOptions.RemoveEmptyEntries))
            {
                var line = rawLine.Trim();

                if (line.StartsWith("width="))
                {
                    int.TryParse(
                        line["width=".Length..],
                        out width);
                }
                else if (line.StartsWith("height="))
                {
                    int.TryParse(
                        line["height=".Length..],
                        out height);
                }
                else if (line.StartsWith("r_frame_rate="))
                {
                    var value = line["r_frame_rate=".Length..];

                    fps = ParseFrameRate(value);
                }
                else if (line.StartsWith("codec_name="))
                {
                    codec = line["codec_name=".Length..].Trim();
                }
                else if (line.StartsWith("format_name="))
                {
                    format = line["format_name=".Length..].Trim();
                }
                else if (line.StartsWith("duration="))
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
                double.TryParse(
                    parts[0],
                    NumberStyles.Float,
                    CultureInfo.InvariantCulture,
                    out var numerator) &&
                double.TryParse(
                    parts[1],
                    NumberStyles.Float,
                    CultureInfo.InvariantCulture,
                    out var denominator) &&
                denominator != 0)
            {
                return numerator / denominator;
            }

            if (double.TryParse(
                    value,
                    NumberStyles.Float,
                    CultureInfo.InvariantCulture,
                    out var fps))
            {
                return fps;
            }

            return null;
        }
    }
}
