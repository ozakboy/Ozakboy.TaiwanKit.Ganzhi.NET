using System;

namespace Ozakboy.TaiwanKit.Ganzhi.NET
{
    /// <summary>
    /// 農曆日期（不可變；閏月以 <see cref="IsLeapMonth"/> 明確表示）
    /// A lunar date (immutable; leap months are explicitly flagged by <see cref="IsLeapMonth"/>)
    /// </summary>
    public readonly struct LunarDate : IEquatable<LunarDate>
    {
        /// <summary>
        /// 建立農曆日期（僅建構，不驗證該日期是否存在；轉換請用 <see cref="LunarConverter.TryToSolar"/>）
        /// Creates a lunar date (no existence validation; use <see cref="LunarConverter.TryToSolar"/> for conversion)
        /// </summary>
        /// <param name="year">農曆年（1901～2100） / Lunar year (1901-2100)</param>
        /// <param name="month">農曆月（1～12） / Lunar month (1-12)</param>
        /// <param name="day">農曆日（1～30） / Lunar day (1-30)</param>
        /// <param name="isLeapMonth">是否為閏月 / Whether the month is the leap month</param>
        public LunarDate(int year, int month, int day, bool isLeapMonth = false)
        {
            Year = year;
            Month = month;
            Day = day;
            IsLeapMonth = isLeapMonth;
        }

        /// <summary>
        /// 農曆年
        /// Lunar year
        /// </summary>
        public int Year { get; }

        /// <summary>
        /// 農曆月（1～12；閏月時月號與被閏的月相同，由 <see cref="IsLeapMonth"/> 區別）
        /// Lunar month (1-12; a leap month shares the number of the month it follows)
        /// </summary>
        public int Month { get; }

        /// <summary>
        /// 農曆日（1～30）
        /// Lunar day (1-30)
        /// </summary>
        public int Day { get; }

        /// <summary>
        /// 是否為閏月
        /// Whether the month is the leap month
        /// </summary>
        public bool IsLeapMonth { get; }

        /// <summary>
        /// 判斷兩個農曆日期是否相等
        /// Determines equality of two lunar dates
        /// </summary>
        public bool Equals(LunarDate other)
        {
            return Year == other.Year && Month == other.Month && Day == other.Day && IsLeapMonth == other.IsLeapMonth;
        }

        /// <summary>
        /// 判斷與物件是否相等
        /// Determines equality with an object
        /// </summary>
        public override bool Equals(object? obj)
        {
            return obj is LunarDate other && Equals(other);
        }

        /// <summary>
        /// 取得雜湊碼
        /// Gets the hash code
        /// </summary>
        public override int GetHashCode()
        {
            return ((Year * 16 + Month) * 32 + Day) * 2 + (IsLeapMonth ? 1 : 0);
        }

        /// <summary>
        /// 相等運算子
        /// Equality operator
        /// </summary>
        public static bool operator ==(LunarDate left, LunarDate right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// 不等運算子
        /// Inequality operator
        /// </summary>
        public static bool operator !=(LunarDate left, LunarDate right)
        {
            return !left.Equals(right);
        }

        /// <summary>
        /// 轉為字串（如「2026年正月初一」「2023年閏二月十五」）
        /// Converts to a string like "2026年正月初一"
        /// </summary>
        public override string ToString()
        {
            return Year + "年" + (IsLeapMonth ? "閏" : string.Empty) + Core.GanzhiNames.LunarMonthName(Month) + Core.GanzhiNames.LunarDayName(Day);
        }
    }
}
