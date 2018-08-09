using System;

namespace Gym.App.Common
{
    public static class DateTimeExtensions
    {
        public static string WeekString(this DateTime dateTime)
        {
            string[] weeks = new string[] { "星期日", "星期一", "星期二", "星期三", "星期四", "星期五", "星期六" };
            DayOfWeek dayOfWeek = dateTime.DayOfWeek;
            return weeks[(int)dayOfWeek];
        }

        public static string ShortWeekString(this DateTime dateTime)
        {
            string[] weeks = new string[] { "日", "一", "二", "三", "四", "五", "六" };
            DayOfWeek dayOfWeek = dateTime.DayOfWeek;
            return weeks[(int)dayOfWeek];
        }

        public static string ShortWeekString2(this DateTime dateTime)
        {
            string[] weeks = new string[] { "周日", "周一", "周二", "周三", "周四", "周五", "周六" };
            DayOfWeek dayOfWeek = dateTime.DayOfWeek;
            return weeks[(int)dayOfWeek];
        }

        public static bool IsMinDate(this DateTime dateTime)
        {
            return dateTime == FixedVariable.MinDate;
        }

        public static DateTime BeginOfDay(this DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 0, 0, 0);
        }

        public static DateTime EndOfDay(this DateTime dateTime)
        {
            return dateTime.BeginOfDay().AddDays(1).AddSeconds(-1);
        }

        public static DateTime BeginOfMonth(this DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, 1, 0, 0, 0);
        }

        public static DateTime EndOfMonth(this DateTime dateTime)
        {
            return dateTime.BeginOfMonth().AddMonths(1).AddSeconds(-1);
        }

        public static DateTime BeginOfYear(this DateTime dateTime)
        {
            return new DateTime(dateTime.Year, 1, 1, 0, 0, 0);
        }

        public static DateTime EndOfYear(this DateTime dateTime)
        {
            return dateTime.BeginOfYear().AddYears(1).AddSeconds(-1);
        }
        /// <summary>
        /// 所在周的开始日期(星期一为第一天)
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static DateTime BeginOfWeek(this DateTime dateTime)
        {
            DayOfWeek dayOfWeek = dateTime.DayOfWeek;
            int val = (int)dayOfWeek;
            if (dayOfWeek == DayOfWeek.Sunday) val = 7;
            var result = dateTime.AddDays(-(int)val + 1);
            return result.BeginOfDay();
        }
        /// <summary>
        /// 所在周的结束日期(星期日为最后一天)
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static DateTime EndOfWeek(this DateTime dateTime)
        {
            return dateTime.BeginOfWeek().AddDays(6).EndOfDay();
        }

        /// <summary>
        /// 根据时间计算年龄
        /// </summary>
        /// <param name="birthdate"></param>
        /// <returns></returns>
        public static int GetAgeByDateTime(this DateTime dateTime)
        {
            DateTime now = DateTime.Now;
            int age = now.Year - dateTime.Year;
            if (now.Month < dateTime.Month || (now.Month == dateTime.Month && now.Day < dateTime.Day))
            {
                age--;
            }
            return age < 0 ? 0 : age;
        }

        public static string ConvertionMonth(int month)
        {
            switch (month)
            {
                case 1:
                    return "一月";
                case 2:
                    return "二月";
                case 3:
                    return "三月";
                case 4:
                    return "四月";
                case 5:
                    return "五月";
                case 6:
                    return "六月";
                case 7:
                    return "七月";
                case 8:
                    return "八月";
                case 9:
                    return "九月";
                case 10:
                    return "十月";
                case 11:
                    return "十一月";
                case 12:
                    return "十二月";
            }
            return string.Empty;
        }

        public static DateTime GetDate(this DateTime dateTime)
        {
            return dateTime.Date;
        }
    }
}
