using System;

namespace Celeste.Core
{
    public static class GameTime
    {
        #region Properties and Fields

        public const int SECONDS_PER_MINUTE = 60;
        public const int SECONDS_PER_HOUR = 3600;
        public const int SECONDS_PER_DAY = 86400;

        public static DateTimeOffset UtcNow => DateTimeOffset.UtcNow;
        public static long UtcNowTimestamp => DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        public static DateTimeOffset LocalNow => DateTimeOffset.Now;
        public static long LocalNowTimestamp => DateTimeOffset.Now.ToUnixTimeSeconds();
        public static DateTimeOffset Epoch => DateTimeOffset.UnixEpoch;
        public static long EpochTimestamp => DateTimeOffset.UnixEpoch.ToUnixTimeSeconds();

        #endregion

        public static DateTimeOffset ToDateTimeOffset(long timestamp)
        {
            return DateTimeOffset.FromUnixTimeSeconds(timestamp);
        }

        public static long ElapsedSecondsUntilNow(long then)
        {
            return UtcNowTimestamp - then;
        }

        public static bool IsTodayUtc(this DateTimeOffset dateTime)
        {
            return AreSameDay(dateTime, UtcNow);
        }

        public static bool IsTodayLocal(this DateTimeOffset dateTime)
        {
            return AreSameDay(dateTime, LocalNow);
        }

        public static bool AreSameDay(DateTimeOffset dateTime1, DateTimeOffset dateTime2)
        {
            return dateTime1.Year == dateTime2.Year &&
                   dateTime1.Month == dateTime2.Month &&
                   dateTime1.Day == dateTime2.Day;
        }

        public static DateTimeOffset TruncateToDay(this DateTimeOffset dateTimeOffset)
        {
            return new DateTimeOffset(
                dateTimeOffset.Year, 
                dateTimeOffset.Month, 
                dateTimeOffset.Day, 
                0, 
                0, 
                0, 
                dateTimeOffset.Offset);
        }
    }
}