namespace Celeste.Core
{
    public static class TimeUtility
    {
        public static string FormatTimeString(long seconds)
        {
            long days = seconds / GameTime.SECONDS_PER_DAY;
            seconds -= days * GameTime.SECONDS_PER_DAY;

            long hours = seconds / GameTime.SECONDS_PER_HOUR;
            seconds -= hours * GameTime.SECONDS_PER_HOUR;

            long minutes = seconds / GameTime.SECONDS_PER_MINUTE;
            seconds -= minutes * GameTime.SECONDS_PER_MINUTE;

            if (days > 0)
            {
                return $"{days}d {hours}h";
            }
            else if (hours > 0)
            {
                return $"{hours}h {minutes}m";
            }
            else if (minutes > 0)
            {
                return $"{minutes}m {seconds}s";
            }

            return $"{seconds}s";
        }
    }
}
