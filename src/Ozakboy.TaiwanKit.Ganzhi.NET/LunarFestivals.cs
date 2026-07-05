using System;

namespace Ozakboy.TaiwanKit.Ganzhi.NET
{
    /// <summary>
    /// 農曆節日查詢（除夕、春節、元宵、端午、七夕、中元、中秋、重陽、臘八；冬至以節氣時刻定義）。閏月不算節日（如閏五月初五非端午）。
    /// Lunar festival queries. Leap months never count (e.g. leap-5th-month 5th is not Dragon Boat).
    /// </summary>
    public static class LunarFestivals
    {
        /// <summary>
        /// 各節日的農曆月/日（冬至與除夕除外，另行計算）
        /// Lunar month/day of each festival (DongZhi and New Year's Eve computed separately)
        /// </summary>
        private static readonly int[,] MonthDay =
        {
            { 0, 0 },    // LunarNewYearEve（臘月最後一日，另算）
            { 1, 1 },    // LunarNewYear
            { 1, 15 },   // LanternFestival
            { 5, 5 },    // DragonBoat
            { 7, 7 },    // Qixi
            { 7, 15 },   // GhostFestival
            { 8, 15 },   // MidAutumn
            { 9, 9 },    // DoubleNinth
            { 12, 8 },   // Laba
            { 0, 0 },    // DongZhi（節氣，另算）
        };

        /// <summary>
        /// 嘗試取得指定國曆日期當天的農曆節日
        /// Tries to get the lunar festival on the given Gregorian date
        /// </summary>
        /// <param name="solarDate">國曆日期（僅取日期部分，1901-02-19～2100-12-31） / Gregorian date (date part only)</param>
        /// <param name="festival">當天的節日 / The festival of the day</param>
        /// <returns>當天是否為節日 / Whether the date is a festival</returns>
        /// <exception cref="ArgumentOutOfRangeException">日期超出範圍 / Date out of range</exception>
        public static bool TryGetFestival(DateTime solarDate, out LunarFestival festival)
        {
            DateTime day = solarDate.Date;
            if (day < GanzhiCalendar.MinDate || day > GanzhiCalendar.MaxDate)
            {
                throw new ArgumentOutOfRangeException(nameof(solarDate), day, "日期超出支援範圍 1901-02-19～2100-12-31 / Date outside the supported range.");
            }

            LunarDate lunar = LunarConverter.ToLunar(day);

            if (!lunar.IsLeapMonth)
            {
                for (int i = 1; i <= 8; i++)
                {
                    if (lunar.Month == MonthDay[i, 0] && lunar.Day == MonthDay[i, 1])
                    {
                        festival = (LunarFestival)i;
                        return true;
                    }
                }

                // 除夕：臘月且次日為正月初一
                // New Year's Eve: 12th month and the next day is lunar 1/1
                if (lunar.Month == 12 && day < GanzhiCalendar.MaxDate)
                {
                    LunarDate next = LunarConverter.ToLunar(day.AddDays(1));
                    if (next.Month == 1 && next.Day == 1)
                    {
                        festival = LunarFestival.LunarNewYearEve;
                        return true;
                    }
                }
            }

            // 冬至（節氣交接日）
            // DongZhi (the solar-term transition day)
            if (SolarTerms.GetExactTime(day.Year, SolarTerm.DongZhi).Date == day)
            {
                festival = LunarFestival.DongZhi;
                return true;
            }

            festival = default;
            return false;
        }

        /// <summary>
        /// 取得節日在指定國曆年內的日期。臘八特例：某些國曆年可能出現兩次（取第一次）或零次（拋例外）。
        /// Gets the festival date within a Gregorian year. Laba edge case: a year may contain two (first returned) or zero (throws).
        /// </summary>
        /// <param name="solarYear">國曆年（1902～2100） / Gregorian year (1902-2100)</param>
        /// <param name="festival">節日 / The festival</param>
        /// <returns>節日的國曆日期 / The Gregorian date of the festival</returns>
        /// <exception cref="ArgumentOutOfRangeException">年份超出範圍 / Year out of range</exception>
        /// <exception cref="InvalidOperationException">該國曆年內無此節日（僅臘八可能發生） / The festival does not occur in that year (Laba only)</exception>
        public static DateTime GetFestivalDate(int solarYear, LunarFestival festival)
        {
            if (solarYear < 1902 || solarYear > 2100)
            {
                throw new ArgumentOutOfRangeException(nameof(solarYear), solarYear, "年份超出 1902～2100 / Year outside 1902-2100.");
            }

            if (festival == LunarFestival.DongZhi)
            {
                return SolarTerms.GetExactTime(solarYear, SolarTerm.DongZhi).Date;
            }

            if (festival == LunarFestival.LunarNewYearEve)
            {
                return LunarConverter.ToSolar(new LunarDate(solarYear, 1, 1)).AddDays(-1);
            }

            if (festival == LunarFestival.Laba)
            {
                // 臘八屬農曆年尾，通常落在「次一國曆年」的一月；逐一檢查兩個候選農曆年
                // Laba belongs to the tail of a lunar year; check both candidate lunar years
                for (int lunarYear = solarYear - 1; lunarYear <= solarYear; lunarYear++)
                {
                    if (lunarYear >= 1901 && LunarConverter.TryToSolar(new LunarDate(lunarYear, 12, 8), out DateTime laba) && laba.Year == solarYear)
                    {
                        return laba;
                    }
                }

                throw new InvalidOperationException("該國曆年內沒有臘八（臘八特例，詳見文件） / No Laba falls within this Gregorian year.");
            }

            int month = MonthDay[(int)festival, 0];
            int dayOfMonth = MonthDay[(int)festival, 1];
            return LunarConverter.ToSolar(new LunarDate(solarYear, month, dayOfMonth));
        }
    }
}
