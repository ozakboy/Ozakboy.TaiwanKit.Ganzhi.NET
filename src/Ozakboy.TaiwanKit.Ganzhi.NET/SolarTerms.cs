using System;
using System.Collections.Generic;
using Ozakboy.TaiwanKit.Ganzhi.NET.Core;

namespace Ozakboy.TaiwanKit.Ganzhi.NET
{
    /// <summary>
    /// 二十四節氣查詢（1901～2100，交接時刻精確到分，台灣時間 UTC+8；資料以 VSOP87D 天文模型計算並經多來源驗證）
    /// 24 solar term queries (1901-2100, minute-precision transition times in Taiwan time UTC+8; astronomically computed and cross-verified)
    /// </summary>
    public static class SolarTerms
    {
        /// <summary>
        /// 資料涵蓋起始年（1901）
        /// First covered year (1901)
        /// </summary>
        public static int MinYear
        {
            get { return SolarTermTable.MinYear; }
        }

        /// <summary>
        /// 資料涵蓋結束年（2100）
        /// Last covered year (2100)
        /// </summary>
        public static int MaxYear
        {
            get { return SolarTermTable.MaxYear; }
        }

        /// <summary>
        /// 取得某年全部 24 個節氣（依年內順序小寒→冬至，含精確交接時刻）
        /// Gets all 24 solar terms of a year (in-year order, with exact transition times)
        /// </summary>
        /// <param name="year">年份（1901～2100） / The year (1901-2100)</param>
        /// <returns>24 筆節氣資訊 / 24 solar term infos</returns>
        /// <exception cref="ArgumentOutOfRangeException">年份超出範圍 / Year out of range</exception>
        public static IReadOnlyList<SolarTermInfo> GetYearTerms(int year)
        {
            EnsureYear(year);
            var list = new SolarTermInfo[24];
            for (int k = 0; k < 24; k++)
            {
                list[k] = new SolarTermInfo((SolarTerm)k, GetExactTime(year, (SolarTerm)k));
            }

            return list;
        }

        /// <summary>
        /// 取得某年某節氣的精確交接時刻（台灣時間）
        /// Gets the exact transition time of a term in a year (Taiwan time)
        /// </summary>
        /// <param name="year">年份（1901～2100） / The year (1901-2100)</param>
        /// <param name="term">節氣 / The solar term</param>
        /// <returns>交接時刻（精確到分） / Transition time (minute precision)</returns>
        /// <exception cref="ArgumentOutOfRangeException">年份超出範圍 / Year out of range</exception>
        public static DateTime GetExactTime(int year, SolarTerm term)
        {
            EnsureYear(year);
            int code = SolarTermTable.Codes[(year - SolarTermTable.MinYear) * 24 + (int)term];
            int minute = code % 100;
            int hour = code / 100 % 100;
            int day = code / 10000 % 100;
            int month = code / 1000000;
            return new DateTime(year, month, day, hour, minute, 0);
        }

        /// <summary>
        /// 取得指定時刻所屬的節氣區段（最近一個交接時刻 ≤ 指定時刻的節氣）
        /// Gets the governing solar term of a moment (the latest term whose transition time is ≤ the moment)
        /// </summary>
        /// <param name="dateTime">時刻（台灣時間） / The moment (Taiwan time)</param>
        /// <returns>所屬節氣資訊 / The governing term</returns>
        /// <exception cref="ArgumentOutOfRangeException">時刻早於 1901 年小寒或晚於 2100-12-31 / Moment before XiaoHan 1901 or after 2100-12-31</exception>
        public static SolarTermInfo GetCurrentTerm(DateTime dateTime)
        {
            if (dateTime > new DateTime(2100, 12, 31, 23, 59, 59))
            {
                throw new ArgumentOutOfRangeException(nameof(dateTime), dateTime, "超出資料涵蓋範圍 / Beyond the covered range.");
            }

            int year = dateTime.Year;
            if (year >= SolarTermTable.MinYear)
            {
                for (int k = 23; k >= 0; k--)
                {
                    DateTime time = GetExactTime(year, (SolarTerm)k);
                    if (time <= dateTime)
                    {
                        return new SolarTermInfo((SolarTerm)k, time);
                    }
                }
            }

            // 年初早於當年小寒 → 屬前一年冬至
            // Before XiaoHan of the year → governed by DongZhi of the previous year
            if (year - 1 < SolarTermTable.MinYear)
            {
                throw new ArgumentOutOfRangeException(nameof(dateTime), dateTime, "早於資料涵蓋範圍（1901 年小寒前） / Before the covered range.");
            }

            return new SolarTermInfo(SolarTerm.DongZhi, GetExactTime(year - 1, SolarTerm.DongZhi));
        }

        /// <summary>
        /// 嘗試取得指定日期當天的節氣（該日是否為交接日）
        /// Tries to get the solar term transitioning on the given calendar date
        /// </summary>
        /// <param name="date">日期（僅取日期部分） / The date (date part only)</param>
        /// <param name="term">當天交接的節氣；非交接日為 null / The term of the day; null when none</param>
        /// <returns>該日是否為節氣交接日 / Whether the date is a transition day</returns>
        public static bool TryGetTermOfDay(DateTime date, out SolarTermInfo? term)
        {
            term = null;
            DateTime day = date.Date;
            if (day.Year < SolarTermTable.MinYear || day.Year > SolarTermTable.MaxYear)
            {
                return false;
            }

            for (int k = 0; k < 24; k++)
            {
                DateTime time = GetExactTime(day.Year, (SolarTerm)k);
                if (time.Date == day)
                {
                    term = new SolarTermInfo((SolarTerm)k, time);
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 檢查年份在範圍內 / Ensures the year is in range
        /// </summary>
        private static void EnsureYear(int year)
        {
            if (year < SolarTermTable.MinYear || year > SolarTermTable.MaxYear)
            {
                throw new ArgumentOutOfRangeException(nameof(year), year, "年份超出 1901～2100 / Year outside 1901-2100.");
            }
        }
    }
}
