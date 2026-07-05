using System;
using System.Globalization;

namespace Ozakboy.TaiwanKit.Ganzhi.NET
{
    /// <summary>
    /// 國曆↔農曆互轉（核心採用 BCL <see cref="ChineseLunisolarCalendar"/>，支援 1901-02-19 ～ 2101-01-28；閏月語意已包裝為明確的 IsLeapMonth）
    /// Gregorian↔lunar conversion (backed by the BCL <see cref="ChineseLunisolarCalendar"/>, 1901-02-19 to 2101-01-28; leap-month semantics wrapped explicitly)
    /// </summary>
    public static class LunarConverter
    {
        private static readonly ChineseLunisolarCalendar Calendar = new ChineseLunisolarCalendar();

        /// <summary>
        /// 可轉換的最早國曆日期（1901-02-19，農曆 1901 年正月初一）
        /// Earliest convertible Gregorian date (1901-02-19, lunar 1901-01-01)
        /// </summary>
        public static DateTime MinDate
        {
            get { return new DateTime(1901, 2, 19); }
        }

        /// <summary>
        /// 可轉換的最晚國曆日期（2101-01-28，農曆 2100 年臘月廿九）
        /// Latest convertible Gregorian date (2101-01-28, the last day of lunar year 2100)
        /// </summary>
        public static DateTime MaxDate
        {
            get { return new DateTime(2101, 1, 28); }
        }

        /// <summary>
        /// 將國曆日期轉為農曆日期
        /// Converts a Gregorian date to the lunar date
        /// </summary>
        /// <param name="solarDate">國曆日期（僅取日期部分） / Gregorian date (date part only)</param>
        /// <returns>農曆日期 / The lunar date</returns>
        /// <exception cref="ArgumentOutOfRangeException">日期超出 MinDate～MaxDate / Date outside MinDate-MaxDate</exception>
        public static LunarDate ToLunar(DateTime solarDate)
        {
            DateTime day = solarDate.Date;
            if (day < MinDate || day > MaxDate)
            {
                throw new ArgumentOutOfRangeException(nameof(solarDate), day, "日期超出可轉換範圍 1901-02-19～2101-01-28 / Date outside the convertible range.");
            }

            int year = Calendar.GetYear(day);
            int monthSlot = Calendar.GetMonth(day);
            int dayOfMonth = Calendar.GetDayOfMonth(day);

            // BCL 閏月語意：GetLeapMonth 回傳「閏月佔用的月序」（如閏四月回 5）；其後月序皆 +1
            // BCL leap-month semantics: GetLeapMonth returns the slot the leap month occupies (e.g. 5 for leap 4th)
            int leapSlot = Calendar.GetLeapMonth(year);
            bool isLeap = leapSlot > 0 && monthSlot == leapSlot;
            int month = leapSlot > 0 && monthSlot >= leapSlot ? monthSlot - 1 : monthSlot;

            return new LunarDate(year, month, dayOfMonth, isLeap);
        }

        /// <summary>
        /// 將農曆日期轉為國曆日期
        /// Converts a lunar date to the Gregorian date
        /// </summary>
        /// <param name="lunarDate">農曆日期 / The lunar date</param>
        /// <returns>國曆日期 / The Gregorian date</returns>
        /// <exception cref="ArgumentOutOfRangeException">農曆日期不存在（年份超界、無此閏月、無此日） / The lunar date does not exist</exception>
        public static DateTime ToSolar(LunarDate lunarDate)
        {
            if (!TryToSolar(lunarDate, out DateTime solar))
            {
                throw new ArgumentOutOfRangeException(nameof(lunarDate), lunarDate.ToString(), "農曆日期不存在或超出範圍 / The lunar date does not exist or is out of range.");
            }

            return solar;
        }

        /// <summary>
        /// 嘗試將農曆日期轉為國曆日期；日期不存在（無此閏月、大小月無 30 日等）時回傳 false 而不拋例外
        /// Tries to convert a lunar date; returns false (instead of throwing) when the date does not exist
        /// </summary>
        /// <param name="lunarDate">農曆日期 / The lunar date</param>
        /// <param name="solarDate">對應的國曆日期 / The Gregorian date</param>
        /// <returns>是否轉換成功 / Whether the conversion succeeded</returns>
        public static bool TryToSolar(LunarDate lunarDate, out DateTime solarDate)
        {
            solarDate = default;

            if (lunarDate.Year < 1901 || lunarDate.Year > 2100 || lunarDate.Month < 1 || lunarDate.Month > 12 || lunarDate.Day < 1 || lunarDate.Day > 30)
            {
                return false;
            }

            int leapSlot = Calendar.GetLeapMonth(lunarDate.Year);

            int monthSlot;
            if (lunarDate.IsLeapMonth)
            {
                // 指定閏月時，該年該月必須真的是閏月
                // The specified month must actually be the leap month of that year
                if (leapSlot == 0 || lunarDate.Month != leapSlot - 1)
                {
                    return false;
                }

                monthSlot = leapSlot;
            }
            else
            {
                monthSlot = leapSlot > 0 && lunarDate.Month >= leapSlot ? lunarDate.Month + 1 : lunarDate.Month;
            }

            if (lunarDate.Day > Calendar.GetDaysInMonth(lunarDate.Year, monthSlot))
            {
                return false;
            }

            solarDate = Calendar.ToDateTime(lunarDate.Year, monthSlot, lunarDate.Day, 0, 0, 0, 0);
            return true;
        }

        /// <summary>
        /// 取得農曆某年的閏月月號（如 2023 年回傳 2 表示閏二月）；無閏月回傳 0
        /// Gets the leap month number of a lunar year (e.g. 2 for 2023 = leap 2nd month); 0 when none
        /// </summary>
        /// <param name="lunarYear">農曆年（1901～2100） / Lunar year (1901-2100)</param>
        /// <returns>閏月月號；無閏月為 0 / Leap month number; 0 when none</returns>
        /// <exception cref="ArgumentOutOfRangeException">年份超出範圍 / Year out of range</exception>
        public static int GetLeapMonth(int lunarYear)
        {
            if (lunarYear < 1901 || lunarYear > 2100)
            {
                throw new ArgumentOutOfRangeException(nameof(lunarYear), lunarYear, "年份超出 1901～2100 / Year outside 1901-2100.");
            }

            int leapSlot = Calendar.GetLeapMonth(lunarYear);
            return leapSlot > 0 ? leapSlot - 1 : 0;
        }
    }
}
