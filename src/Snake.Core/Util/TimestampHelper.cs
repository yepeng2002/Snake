using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snake.Core.Util
{
    public static class TimestampHelper
    {
        public static DateTime ToDateTime(this string timestamp)
        {
            if (string.IsNullOrEmpty(timestamp))
            {
                throw new ArgumentNullException(timestamp);
            }

            DateTime dateTimeStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            long lTime = long.Parse(timestamp + "0000000");
            TimeSpan toNow = new TimeSpan(lTime);

            return dateTimeStart.Add(toNow);
        }
        public static int GetTimeSpan()
        {
            var time = DateTime.Now;
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            return (int)(time - startTime).TotalSeconds;
        }
    }
}
