using System;

namespace Celeste.Core
{
    public static class GameTime
    {
        #region Properties and Fields

        public const int SECONDS_PER_MINUTE = 60;
        public const int SECONDS_PER_HOUR = 3600;
        public const int SECONDS_PER_DAY = 86400;

        public static long Now => DateTimeOffset.UtcNow.ToUnixTimeSeconds();

        #endregion

        public static DateTimeOffset ToDateTimeOffset(long timestamp)
        {
            return DateTimeOffset.FromUnixTimeSeconds(timestamp);
        }
    }
}