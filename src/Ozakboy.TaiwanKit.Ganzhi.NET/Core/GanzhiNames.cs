namespace Ozakboy.TaiwanKit.Ganzhi.NET.Core
{
    /// <summary>
    /// 干支/節氣/生肖/農曆月日的中文名稱對照
    /// Chinese names for stems, branches, terms, zodiac and lunar month/day
    /// </summary>
    internal static class GanzhiNames
    {
        private const string Stems = "甲乙丙丁戊己庚辛壬癸";
        private const string Branches = "子丑寅卯辰巳午未申酉戌亥";
        private const string Zodiacs = "鼠牛虎兔龍蛇馬羊猴雞狗豬";

        private static readonly string[] Terms =
        {
            "小寒", "大寒", "立春", "雨水", "驚蟄", "春分", "清明", "穀雨",
            "立夏", "小滿", "芒種", "夏至", "小暑", "大暑", "立秋", "處暑",
            "白露", "秋分", "寒露", "霜降", "立冬", "小雪", "大雪", "冬至",
        };

        private static readonly string[] LunarMonths =
        {
            "正月", "二月", "三月", "四月", "五月", "六月", "七月", "八月", "九月", "十月", "十一月", "臘月",
        };

        private static readonly string[] LunarDays =
        {
            "初一", "初二", "初三", "初四", "初五", "初六", "初七", "初八", "初九", "初十",
            "十一", "十二", "十三", "十四", "十五", "十六", "十七", "十八", "十九", "二十",
            "廿一", "廿二", "廿三", "廿四", "廿五", "廿六", "廿七", "廿八", "廿九", "三十",
        };

        /// <summary>
        /// 天干中文名 / Chinese name of a stem
        /// </summary>
        internal static string StemName(HeavenlyStem stem)
        {
            return Stems[(int)stem].ToString();
        }

        /// <summary>
        /// 地支中文名 / Chinese name of a branch
        /// </summary>
        internal static string BranchName(EarthlyBranch branch)
        {
            return Branches[(int)branch].ToString();
        }

        /// <summary>
        /// 生肖中文名 / Chinese name of a zodiac animal
        /// </summary>
        internal static string ZodiacName(Zodiac zodiac)
        {
            return Zodiacs[(int)zodiac].ToString();
        }

        /// <summary>
        /// 節氣中文名 / Chinese name of a solar term
        /// </summary>
        internal static string TermName(SolarTerm term)
        {
            return Terms[(int)term];
        }

        /// <summary>
        /// 農曆月中文名（1=正月 … 12=臘月） / Chinese name of a lunar month
        /// </summary>
        internal static string LunarMonthName(int month)
        {
            return month >= 1 && month <= 12 ? LunarMonths[month - 1] : month.ToString(System.Globalization.CultureInfo.InvariantCulture) + "月";
        }

        /// <summary>
        /// 農曆日中文名（1=初一 … 30=三十） / Chinese name of a lunar day
        /// </summary>
        internal static string LunarDayName(int day)
        {
            return day >= 1 && day <= 30 ? LunarDays[day - 1] : day.ToString(System.Globalization.CultureInfo.InvariantCulture) + "日";
        }
    }
}
