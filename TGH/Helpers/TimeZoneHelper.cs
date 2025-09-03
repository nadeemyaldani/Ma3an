using System;
using TimeZoneConverter;

namespace TGH.Helpers
{
    public static class TimeZoneHelper
    {
        public static DateTime ToTimeZoneTime(this DateTime time)
        {
            var timeZone = TZConvert.GetTimeZoneInfo("Asia/Amman");

            var localTime = TimeZoneInfo.ConvertTime(time.ToUniversalTime(), timeZone);

            return localTime;
        }
    }
}
