using System;

namespace Celeste.Core
{
    public static class GameTime
    {
        public const int SECONDS_PER_MINUTE = 60;
        public const int SECONDS_PER_HOUR = 3600;
        public const int SECONDS_PER_DAY = 86400;

        public static long Now
        {
            get { return DateTimeOffset.UtcNow.ToUnixTimeSeconds(); }
        }
    }
}