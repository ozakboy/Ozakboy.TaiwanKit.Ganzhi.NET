using System;

namespace Ozakboy.TaiwanKit.Ganzhi.NET
{
    /// <summary>
    /// 干支曆查詢：年/月/日/時四柱與生肖（支援 1901-02-19 ～ 2100-12-31，台灣時間）。
    /// 年柱界線可選立春（預設，命理慣例）或正月初一（民俗慣例）；月柱依節氣精確時刻切換；晚子時（23:00 起）日柱歸次日。
    /// Ganzhi calendar: four pillars and zodiac (1901-02-19 to 2100-12-31, Taiwan time).
    /// Year boundary is LiChun (default) or Lunar New Year; month pillars switch at exact solar term times; the day pillar switches at 23:00.
    /// </summary>
    public static class GanzhiCalendar
    {
        /// <summary>
        /// 日柱錨點：1949-10-01 為甲子日（另與 2000-01-01 戊午日交叉驗證）
        /// Day-pillar anchor: 1949-10-01 was a 甲子 day (cross-checked against 2000-01-01 being 戊午)
        /// </summary>
        private static readonly DateTime DayAnchor = new DateTime(1949, 10, 1);

        /// <summary>
        /// 可查詢的最早日期（1901-02-19）
        /// Earliest supported date (1901-02-19)
        /// </summary>
        public static DateTime MinDate
        {
            get { return new DateTime(1901, 2, 19); }
        }

        /// <summary>
        /// 可查詢的最晚日期（2100-12-31）
        /// Latest supported date (2100-12-31)
        /// </summary>
        public static DateTime MaxDate
        {
            get { return new DateTime(2100, 12, 31); }
        }

        /// <summary>
        /// 取得年柱干支。立春模式以立春交接「時刻」為界（精確到分）；正月初一模式以農曆年為界。
        /// Gets the year pillar. LiChun mode uses the exact transition time; LunarNewYear mode uses the lunar year.
        /// </summary>
        /// <param name="dateTime">時刻（台灣時間） / The moment (Taiwan time)</param>
        /// <param name="boundary">年界線模式，預設立春 / Year-boundary convention, defaults to LiChun</param>
        /// <returns>年柱干支 / The year pillar</returns>
        /// <exception cref="ArgumentOutOfRangeException">超出 MinDate～MaxDate / Outside MinDate-MaxDate</exception>
        public static GanzhiValue GetYearGanzhi(DateTime dateTime, YearBoundary boundary = YearBoundary.LiChun)
        {
            EnsureInRange(dateTime);
            return new GanzhiValue(SexagenaryOfYear(GetGanzhiYearNumber(dateTime, boundary)));
        }

        /// <summary>
        /// 取得月柱干支（依節氣「節」的精確時刻切換：立春起寅月、驚蟄起卯月……小寒起丑月；月干依五虎遁）
        /// Gets the month pillar (switched at the exact times of the 12 "Jie" terms; stems follow the five-tigers rule)
        /// </summary>
        /// <param name="dateTime">時刻（台灣時間） / The moment (Taiwan time)</param>
        /// <returns>月柱干支 / The month pillar</returns>
        /// <exception cref="ArgumentOutOfRangeException">超出 MinDate～MaxDate / Outside MinDate-MaxDate</exception>
        public static GanzhiValue GetMonthGanzhi(DateTime dateTime)
        {
            EnsureInRange(dateTime);

            // 十二節：立春(2)、驚蟄(4)、清明(6)、立夏(8)、芒種(10)、小暑(12)、立秋(14)、白露(16)、寒露(18)、立冬(20)、大雪(22)、小寒(0)
            // 找出最近一個已過的「節」，決定月支（寅=立春起）
            // Find the latest passed "Jie" term to decide the month branch
            int year = dateTime.Year;
            int monthIndexFromYin = -1;   // 0=寅月
            for (int jie = 11; jie >= 0; jie--)
            {
                SolarTerm term = (SolarTerm)(jie * 2);   // 0=小寒,2=立春,4=驚蟄...22=大雪
                DateTime time = SolarTerms.GetExactTime(year, term);
                if (time <= dateTime)
                {
                    // term 序 → 月序（立春(2)=寅月0、驚蟄(4)=卯月1、…、大雪(22)=子月10、小寒(0)=丑月11）
                    monthIndexFromYin = jie == 0 ? 11 : jie - 1;
                    break;
                }
            }

            if (monthIndexFromYin < 0)
            {
                // 當年小寒之前（1/1～1/5 附近）→ 前一年大雪起的子月
                // Before XiaoHan of the year → the Zi month started by DaXue of the previous year
                monthIndexFromYin = 10;
            }

            // 月干（五虎遁）以立春界的干支年為準
            // The month stem (five-tigers rule) is based on the LiChun-boundary year
            int ganzhiYear = GetGanzhiYearNumber(dateTime, YearBoundary.LiChun);
            int yearStem = Positive(ganzhiYear - 4, 10);
            int stem = (yearStem * 2 + 2 + monthIndexFromYin) % 10;
            int branch = (monthIndexFromYin + 2) % 12;
            return new GanzhiValue((HeavenlyStem)stem, (EarthlyBranch)branch);
        }

        /// <summary>
        /// 取得日柱干支（以「日曆日」為準，00:00 換日；八字場景的 23:00 換日規則請用 <see cref="GetFourPillars"/>）
        /// Gets the day pillar (by calendar date, switching at 00:00; for the 23:00 Bazi rule use <see cref="GetFourPillars"/>)
        /// </summary>
        /// <param name="date">日期（僅取日期部分） / The date (date part only)</param>
        /// <returns>日柱干支 / The day pillar</returns>
        /// <exception cref="ArgumentOutOfRangeException">超出 MinDate～MaxDate / Outside MinDate-MaxDate</exception>
        public static GanzhiValue GetDayGanzhi(DateTime date)
        {
            EnsureInRange(date);
            int days = (int)(date.Date - DayAnchor).TotalDays;
            return new GanzhiValue(Positive(days, 60));
        }

        /// <summary>
        /// 取得時柱干支（時支每兩小時一換，23:00 起為子時；時干依五鼠遁由「當時的日柱」推得，晚子時採次日日干）
        /// Gets the hour pillar (branches change every two hours, Zi starts at 23:00; stems follow the five-rats rule using the effective day)
        /// </summary>
        /// <param name="dateTime">時刻（台灣時間） / The moment (Taiwan time)</param>
        /// <returns>時柱干支 / The hour pillar</returns>
        /// <exception cref="ArgumentOutOfRangeException">超出 MinDate～MaxDate / Outside MinDate-MaxDate</exception>
        public static GanzhiValue GetHourGanzhi(DateTime dateTime)
        {
            EnsureInRange(dateTime);
            int branch = (dateTime.Hour + 1) / 2 % 12;
            int dayStem = (int)EffectiveDayGanzhi(dateTime).Stem;
            int stem = (dayStem % 5 * 2 + branch) % 10;
            return new GanzhiValue((HeavenlyStem)stem, (EarthlyBranch)branch);
        }

        /// <summary>
        /// 取得四柱（年/月/日/時）。日柱採八字慣例：23:00 起算次日。
        /// Gets the four pillars. The day pillar follows the Bazi convention of switching at 23:00.
        /// </summary>
        /// <param name="dateTime">時刻（台灣時間） / The moment (Taiwan time)</param>
        /// <param name="boundary">年柱界線模式，預設立春 / Year-boundary convention, defaults to LiChun</param>
        /// <returns>四柱 / The four pillars</returns>
        /// <exception cref="ArgumentOutOfRangeException">超出 MinDate～MaxDate / Outside MinDate-MaxDate</exception>
        public static FourPillars GetFourPillars(DateTime dateTime, YearBoundary boundary = YearBoundary.LiChun)
        {
            EnsureInRange(dateTime);
            return new FourPillars(
                GetYearGanzhi(dateTime, boundary),
                GetMonthGanzhi(dateTime),
                EffectiveDayGanzhi(dateTime),
                GetHourGanzhi(dateTime));
        }

        /// <summary>
        /// 取得生肖（界線規則同年柱）
        /// Gets the zodiac animal (boundary rule identical to the year pillar)
        /// </summary>
        /// <param name="dateTime">時刻（台灣時間） / The moment (Taiwan time)</param>
        /// <param name="boundary">年界線模式，預設立春 / Year-boundary convention, defaults to LiChun</param>
        /// <returns>生肖 / The zodiac animal</returns>
        /// <exception cref="ArgumentOutOfRangeException">超出 MinDate～MaxDate / Outside MinDate-MaxDate</exception>
        public static Zodiac GetZodiac(DateTime dateTime, YearBoundary boundary = YearBoundary.LiChun)
        {
            EnsureInRange(dateTime);
            return (Zodiac)Positive(GetGanzhiYearNumber(dateTime, boundary) - 4, 12);
        }

        /// <summary>
        /// 取得生肖的中文名稱
        /// Gets the Chinese name of a zodiac animal
        /// </summary>
        /// <param name="zodiac">生肖 / The zodiac animal</param>
        /// <returns>中文名（如「馬」） / Chinese name (e.g. 馬)</returns>
        public static string GetChineseName(Zodiac zodiac)
        {
            return Core.GanzhiNames.ZodiacName(zodiac);
        }

        /// <summary>
        /// 依界線模式取得干支年份數字
        /// Gets the Ganzhi year number under the given boundary convention
        /// </summary>
        private static int GetGanzhiYearNumber(DateTime dateTime, YearBoundary boundary)
        {
            if (boundary == YearBoundary.LunarNewYear)
            {
                return LunarConverter.ToLunar(dateTime).Year;
            }

            int year = dateTime.Year;
            DateTime liChun = SolarTerms.GetExactTime(year, SolarTerm.LiChun);
            return dateTime < liChun ? year - 1 : year;
        }

        /// <summary>
        /// 八字換日規則下的日柱（23:00 起算次日）
        /// The day pillar under the 23:00-switch rule
        /// </summary>
        private static GanzhiValue EffectiveDayGanzhi(DateTime dateTime)
        {
            DateTime effective = dateTime.Hour >= 23 ? dateTime.Date.AddDays(1) : dateTime.Date;
            int days = (int)(effective - DayAnchor).TotalDays;
            return new GanzhiValue(Positive(days, 60));
        }

        /// <summary>
        /// 年份數字 → 六十甲子序（1984 甲子）
        /// Year number → sexagenary index (1984 = 甲子)
        /// </summary>
        private static int SexagenaryOfYear(int year)
        {
            return Positive(year - 4, 60);
        }

        /// <summary>
        /// 正模運算 / Positive modulo
        /// </summary>
        private static int Positive(int value, int modulus)
        {
            int r = value % modulus;
            return r < 0 ? r + modulus : r;
        }

        /// <summary>
        /// 檢查時刻在支援範圍內 / Ensures the moment is within range
        /// </summary>
        private static void EnsureInRange(DateTime dateTime)
        {
            if (dateTime < MinDate || dateTime.Date > MaxDate)
            {
                throw new ArgumentOutOfRangeException(nameof(dateTime), dateTime, "超出支援範圍 1901-02-19～2100-12-31 / Outside the supported range.");
            }
        }
    }
}
