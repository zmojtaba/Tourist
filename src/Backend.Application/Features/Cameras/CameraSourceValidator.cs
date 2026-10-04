namespace Backend.Application.Features.Cameras
{
    public static class CameraSourceValidator
    {
        public static bool IsValid(
            CameraSourceType sourceType,
            string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return false;

            switch (sourceType)
            {
                case CameraSourceType.RTSP:
                    return IsUriWithScheme(value, "rtsp", "rtsps");

                case CameraSourceType.HTTP:
                    return IsUriWithScheme(value, "http", "https");

                case CameraSourceType.HLS:
                    return IsUriWithScheme(value, "http", "https");

                case CameraSourceType.File:
                    return IsFilePath(value);

                case CameraSourceType.WebRTC:
                    return IsUriWithScheme(value, "http", "https");

                case CameraSourceType.USB:
                    // USB devices don't necessarily have URLs.
                    return !string.IsNullOrWhiteSpace(value);

                default:
                    return false;
            }
        }

        private static bool IsUriWithScheme(
            string value,
            params string[] allowedSchemes)
        {
            if (!Uri.TryCreate(
                    value,
                    UriKind.Absolute,
                    out var uri))
            {
                return false;
            }

            return allowedSchemes.Any(
                scheme => string.Equals(
                    uri.Scheme,
                    scheme,
                    StringComparison.OrdinalIgnoreCase));
        }

        private static bool IsFilePath(string value)
        {
            // Absolute local file path
            if (Path.IsPathFullyQualified(value))
                return true;

            // Also allow file:// URLs
            if (Uri.TryCreate(
                    value,
                    UriKind.Absolute,
                    out var uri))
            {
                return uri.Scheme.Equals(
                    Uri.UriSchemeFile,
                    StringComparison.OrdinalIgnoreCase);
            }

            return false;
        }
    }
}
