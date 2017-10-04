using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Snake.Core.Util
{
    public class DateHelper
    {
        private DateHelper()
        {
        }

        /// <summary>
        /// 解析日期字符串，如果解析不成功则返回null
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime? Parse2(string date)
        {
            DateTime? dateTime = null;
            DateTime _dt = DateTime.MinValue;
            if (!StringHelper.IsEmpty(date))
            {
                if (DateTime.TryParse(date, out _dt))
                {
                    return _dt;
                }
                else
                {//ps：java格式的时间戳比c#多0000                    
                    try
                    {
                        DateTime dateTimeStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
                        long lTime = 10000 * long.Parse(date);
                        TimeSpan toNow = new TimeSpan(lTime);
                        dateTime = dateTimeStart.Add(toNow);
                    }
                    catch { }
                }
            }
            return dateTime;
        }

        /// <summary>
        /// 解析日期字符串，如果解析不成功则返回DateTime.MinValue
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime Parse(string date)
        {
            DateTime _dt = DateTime.MinValue;
            if (!StringHelper.IsEmpty(date))
            {
                try
                {
                    DateTimeFormatInfo _format = DateTimeFormatInfo.CurrentInfo;
                    _dt = Convert.ToDateTime(date, _format);
                }
                catch
                {
                }
            }
            return _dt;
        }

        /// <summary>
        /// 解析yyyMMdd这样格式的整形，并转成相应的日期，转换失败则返回DateTime.MinValue
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime Parse(int date)
        {
            var _regExp = new Regex(@"^\d{4}\d{2}\d{2}$");
            if (!_regExp.IsMatch(date.ToString()))
            {
                return DateTime.MinValue;
            }

            var _sb = new StringBuilder(date.ToString());
            _sb.Insert(4, "-", 1).Insert(7, "-", 1);
            Console.WriteLine(_sb.ToString());

            return Parse(_sb.ToString());
        }


        // Convert date string 'yyyy-mm-dd' to string 'yyyymmdd'
        public static string yyyy_mm_dd_2_yyyymmdd(string dateStr)
        {
            return dateStr.Substring(0, 4) + dateStr.Substring(5, 2) + dateStr.Substring(8, 2);
        }

        // Convert date string 'yyyymmdd' to string 'yyyy-mm-dd'
        public static string yyyymmdd_2_yyyy_mm_dd(string dateStr)
        {
            return dateStr.Substring(0, 4) + "-" + dateStr.Substring(4, 2) + "-" +
                   dateStr.Substring(6, 2);
        }

        //返回给定DateTime的前一年
        public static int GetPrevYear()
        {
            return DateTime.Now.Year - 1;
        }

        // 将时间截断 "2003-12-12 02:02:02" -> "2003-12-12 00:00:00"
        public static DateTime TruncTime(DateTime d)
        {
            return new DateTime(d.Year, d.Month, d.Day, 0, 0, 0);
        }

        // 日期所在周的第一天（周一开始）
        public static DateTime GetFirstDayOfWeek(DateTime d)
        {
            var dayOfWeek = (int)d.DayOfWeek;
            if (dayOfWeek > 0)
            {
                return TruncTime(d.AddDays(1 - dayOfWeek));
            }
            else
            {
                return TruncTime(d.AddDays(-6));
            }
        }

        // 日期所在周的最后一天
        public static DateTime GetLastDayOfWeek(DateTime d)
        {
            return GetFirstDayOfWeek(d).AddDays(6);
        }

        // 日期所在下一周的第一天
        public static DateTime GetFirstDayOfNextWeek(DateTime d)
        {
            return GetLastDayOfWeek(d).AddDays(1);
        }

        // 日期所在月份第一天
        public static DateTime GetFirstDayOfMonth(DateTime d)
        {
            return new DateTime(d.Year, d.Month, 1);
        }

        // 日期所在月份最后一天
        public static DateTime GetLastDayOfMonth(DateTime d)
        {
            return GetFirstDayOfMonth(d.AddMonths(1)).AddDays(-1);
        }

        // 日期所在下个月份第一天
        public static DateTime GetFirstDayOfNextMonth(DateTime d)
        {
            return GetLastDayOfMonth(d).AddDays(1);
        }

        /// <summary>
        /// 取得某日期是该月的第几周，每周以星期天开始。
        /// </summary>
        /// <param name="d">给定日期</param>
        /// <returns>返回整数1-6，表示该日期在该月为第几周。</returns>
        public static int GetWeeksOfDate(DateTime d)
        {
            var t = new DateTime(d.Year, d.Month, 1);
            int i = Convert.ToInt16(t.DayOfWeek);
            int x = d.Day;
            return ((x - 1 + i) / 7 + 1);
        }

        /// <summary>
        ///  取得给定日期所在月的第i个周几(i从1-5代表第一到第五个，-1代表最后一个，）
        /// </summary>
        /// <param name="d">给定日期</param>
        /// <param name="i">第几周(i从1-5代表第一到第五个，-1代表最后一个，）</param>
        /// <param name="j">周几</param>
        /// <returns>给定日期所在月的第i个周j</returns>
        public static DateTime GetSpecificDayOfMonth(DateTime d, int i, DayOfWeek j)
        {
            if (i < -1 || i > 5 || i == 0)
            {
                throw new Exception("invalid parameter");
            }
            try
            {
                DateTime dt;
                if (i != -1)
                {
                    if (j < GetFirstDayOfMonth(d).DayOfWeek)
                    {
                        dt = new DateTime(d.Year, d.Month,
                                          i * 7 + Convert.ToInt16(j) - Convert.ToInt16(GetFirstDayOfMonth(d).DayOfWeek) +
                                          1);
                    }
                    else
                    {
                        dt = new DateTime(d.Year, d.Month,
                                          (i - 1) * 7 + Convert.ToInt16(j) -
                                          Convert.ToInt16(GetFirstDayOfMonth(d).DayOfWeek) + 1);
                    }
                }
                else
                {
                    if (j > GetLastDayOfMonth(d).DayOfWeek)
                    {
                        dt = new DateTime(d.Year, d.Month,
                                          GetLastDayOfMonth(d).Day + Convert.ToInt16(j) -
                                          Convert.ToInt16(GetLastDayOfMonth(d).DayOfWeek) - 7);
                    }
                    else
                    {
                        dt = new DateTime(d.Year, d.Month,
                                          GetLastDayOfMonth(d).Day + Convert.ToInt16(j) -
                                          Convert.ToInt16(GetLastDayOfMonth(d).DayOfWeek));
                    }
                }
                return dt;
            }
            catch (Exception e)
            {
                throw new Exception("Out Of Range", e);
            }
        }

        public static string GetStandardNowTimeString()
        {
            return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }

        public static string GetStandardTodayString()
        {
            return DateTime.Now.ToString("yyyy-MM-dd");
        }

        public static string GetStandardDateString(DateTime dateTime)
        {
            return dateTime.ToString("yyyy-MM-dd");
        }

        public static string GetStandardTimeString(DateTime dateTime)
        {
            return dateTime.ToString("yyyy-MM-dd HH:mm:ss");
        }

        /// <summary>
        /// 解析并处理含特殊字符的字符串如果是日期则返回短日期格式字符串，如果解析不成功则返回空值
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ConvertStringToShortDateString(string str)
        {
            string shortdateString = "";
            if (StringHelper.IsDate(str))
            {
                shortdateString = Convert.ToDateTime(StringHelper.TranslateSqlChar(str.Trim())).ToShortDateString();
            }
            return shortdateString;
        }

        /// <summary>
        /// 返回带星期的中文格式日期
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string TodayFormatWithWeek(DateTime date)
        {
            string strResult = string.Empty;
            string strDate = string.Empty;
            string strWeek = string.Empty;

            strDate = date.Year + "年" + date.Month + "月" + date.Day + "日";
            strWeek = date.DayOfWeek.ToString();
            switch (strWeek)
            {
                case "Monday":
                    strWeek = "星期一";
                    break;
                case "Tuesday":
                    strWeek = "星期二";
                    break;
                case "Wednesday":
                    strWeek = "星期三";
                    break;
                case "Thursday":
                    strWeek = "星期四";
                    break;
                case "Friday":
                    strWeek = "星期五";
                    break;
                case "Saturday":
                    strWeek = "星期六";
                    break;
                case "Sunday":
                    strWeek = "星期日";
                    break;
            }

            strResult = strDate + " " + strWeek;

            return strResult;
        }

        public static Int64 DateTimeToInteger()
        {
            return Convert.ToInt64(DateTime.Now.ToLongTimeString());
        }

        public static Int64 UnixStamp()
        {
            TimeSpan ts = DateTime.Now - TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            return Convert.ToInt64(ts.TotalSeconds);
        }

        public static Int64 DateTimeStampToInt64()
        {
            DateTime dt = DateTime.Now;
            return Convert.ToInt64(dt.ToString("yyyyMMddHHmmss") + dt.Millisecond.ToString());
        }

        public static DateTime StringToDateTime(string strDateTime)
        {
            int y = Convert.ToInt32(strDateTime.Substring(0, 4));
            int m = Convert.ToInt32(strDateTime.Substring(4, 2));
            int d = Convert.ToInt32(strDateTime.Substring(6, 2));
            int hh = Convert.ToInt32(strDateTime.Substring(8, 2));
            int mm = Convert.ToInt32(strDateTime.Substring(10, 2));
            int ss = Convert.ToInt32(strDateTime.Substring(12, 2));
            return new DateTime(y, m, d, hh, mm, ss);
        }
        /// <summary>
        /// 判断字符串是否为日期
        /// </summary>
        /// <param name="strDate"></param>
        /// <returns></returns>
        public static bool IsDate(string strDate)
        {
            try
            {
                DateTime.Parse(strDate);
                return true;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// 获取日期是一年中第几个星期
        /// </summary>
        /// <param name="date">需要计算的时间</param>
        /// <returns>一年中第几个星期</returns>
        private static int GetWeekNumber(DateTime date)
        {
            var _cultureInfo = CultureInfo.CurrentCulture;
            return _cultureInfo.Calendar.GetWeekOfYear(date, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
        }

        /// <summary>
        /// 根据一年中的第几周返回该周的相关日期集合
        /// </summary>
        /// <param name="strYear"></param>
        /// <param name="WeekNumber"></param>
        public static List<string> GetDayListByWeekNumberOfYear(string strYear, int WeekNumber)
        {
            List<string> Days = new List<string>();
            var OneDateTemp = Convert.ToDateTime(strYear + "-01-01");
            for (int i = 0; i <= 365; i++)
            {
                var week = GetWeekNumber(OneDateTemp);
                if (week == WeekNumber)
                {
                    Days.Add(OneDateTemp.ToString("yyyy-MM-dd"));
                }
                OneDateTemp = OneDateTemp.AddDays(1);
            }
            return Days;
        }

    }
}
