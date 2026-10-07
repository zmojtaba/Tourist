namespace Backend.Application.Common.Redis
{
    public static class RedisKeys
    {
        private const string RawFramePrefix = "frame";
        private const string FrameSignalPrefix = "frame-signal";

        public static string RawFrame(Guid frameId)
            => $"{RawFramePrefix}:{frameId:N}";

        public static string FrameSignal(Guid frameId)
            => $"{FrameSignalPrefix}:{frameId:N}";
    }
}
