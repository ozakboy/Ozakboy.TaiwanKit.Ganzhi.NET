namespace Ozakboy.TaiwanKit.Ganzhi.NET
{
    /// <summary>
    /// 天干（甲乙丙丁戊己庚辛壬癸）
    /// The ten Heavenly Stems
    /// </summary>
    public enum HeavenlyStem
    {
        /// <summary>甲 / Jia</summary>
        Jia = 0,
        /// <summary>乙 / Yi</summary>
        Yi = 1,
        /// <summary>丙 / Bing</summary>
        Bing = 2,
        /// <summary>丁 / Ding</summary>
        Ding = 3,
        /// <summary>戊 / Wu</summary>
        Wu = 4,
        /// <summary>己 / Ji</summary>
        Ji = 5,
        /// <summary>庚 / Geng</summary>
        Geng = 6,
        /// <summary>辛 / Xin</summary>
        Xin = 7,
        /// <summary>壬 / Ren</summary>
        Ren = 8,
        /// <summary>癸 / Gui</summary>
        Gui = 9,
    }

    /// <summary>
    /// 地支（子丑寅卯辰巳午未申酉戌亥）
    /// The twelve Earthly Branches
    /// </summary>
    public enum EarthlyBranch
    {
        /// <summary>子 / Zi</summary>
        Zi = 0,
        /// <summary>丑 / Chou</summary>
        Chou = 1,
        /// <summary>寅 / Yin</summary>
        Yin = 2,
        /// <summary>卯 / Mao</summary>
        Mao = 3,
        /// <summary>辰 / Chen</summary>
        Chen = 4,
        /// <summary>巳 / Si</summary>
        Si = 5,
        /// <summary>午 / Wu</summary>
        Wu = 6,
        /// <summary>未 / Wei</summary>
        Wei = 7,
        /// <summary>申 / Shen</summary>
        Shen = 8,
        /// <summary>酉 / You</summary>
        You = 9,
        /// <summary>戌 / Xu</summary>
        Xu = 10,
        /// <summary>亥 / Hai</summary>
        Hai = 11,
    }

    /// <summary>
    /// 生肖
    /// The twelve zodiac animals
    /// </summary>
    public enum Zodiac
    {
        /// <summary>鼠 / Rat</summary>
        Rat = 0,
        /// <summary>牛 / Ox</summary>
        Ox = 1,
        /// <summary>虎 / Tiger</summary>
        Tiger = 2,
        /// <summary>兔 / Rabbit</summary>
        Rabbit = 3,
        /// <summary>龍 / Dragon</summary>
        Dragon = 4,
        /// <summary>蛇 / Snake</summary>
        Snake = 5,
        /// <summary>馬 / Horse</summary>
        Horse = 6,
        /// <summary>羊 / Goat</summary>
        Goat = 7,
        /// <summary>猴 / Monkey</summary>
        Monkey = 8,
        /// <summary>雞 / Rooster</summary>
        Rooster = 9,
        /// <summary>狗 / Dog</summary>
        Dog = 10,
        /// <summary>豬 / Pig</summary>
        Pig = 11,
    }

    /// <summary>
    /// 二十四節氣（依國曆年內順序：小寒（約 1/5）起至冬至（約 12/22））
    /// The 24 solar terms in Gregorian-year order (XiaoHan around Jan 5 to DongZhi around Dec 22)
    /// </summary>
    public enum SolarTerm
    {
        /// <summary>小寒（約 1/5，太陽黃經 285°） / Minor Cold</summary>
        XiaoHan = 0,
        /// <summary>大寒（約 1/20，300°） / Major Cold</summary>
        DaHan = 1,
        /// <summary>立春（約 2/4，315°） / Beginning of Spring</summary>
        LiChun = 2,
        /// <summary>雨水（約 2/19，330°） / Rain Water</summary>
        YuShui = 3,
        /// <summary>驚蟄（約 3/6，345°） / Awakening of Insects</summary>
        JingZhe = 4,
        /// <summary>春分（約 3/21，0°） / Spring Equinox</summary>
        ChunFen = 5,
        /// <summary>清明（約 4/5，15°） / Clear and Bright</summary>
        QingMing = 6,
        /// <summary>穀雨（約 4/20，30°） / Grain Rain</summary>
        GuYu = 7,
        /// <summary>立夏（約 5/6，45°） / Beginning of Summer</summary>
        LiXia = 8,
        /// <summary>小滿（約 5/21，60°） / Grain Buds</summary>
        XiaoMan = 9,
        /// <summary>芒種（約 6/6，75°） / Grain in Ear</summary>
        MangZhong = 10,
        /// <summary>夏至（約 6/21，90°） / Summer Solstice</summary>
        XiaZhi = 11,
        /// <summary>小暑（約 7/7，105°） / Minor Heat</summary>
        XiaoShu = 12,
        /// <summary>大暑（約 7/23，120°） / Major Heat</summary>
        DaShu = 13,
        /// <summary>立秋（約 8/8，135°） / Beginning of Autumn</summary>
        LiQiu = 14,
        /// <summary>處暑（約 8/23，150°） / End of Heat</summary>
        ChuShu = 15,
        /// <summary>白露（約 9/8，165°） / White Dew</summary>
        BaiLu = 16,
        /// <summary>秋分（約 9/23，180°） / Autumn Equinox</summary>
        QiuFen = 17,
        /// <summary>寒露（約 10/8，195°） / Cold Dew</summary>
        HanLu = 18,
        /// <summary>霜降（約 10/24，210°） / Frost's Descent</summary>
        ShuangJiang = 19,
        /// <summary>立冬（約 11/7，225°） / Beginning of Winter</summary>
        LiDong = 20,
        /// <summary>小雪（約 11/22，240°） / Minor Snow</summary>
        XiaoXue = 21,
        /// <summary>大雪（約 12/7，255°） / Major Snow</summary>
        DaXue = 22,
        /// <summary>冬至（約 12/22，270°） / Winter Solstice</summary>
        DongZhi = 23,
    }

    /// <summary>
    /// 年柱/生肖的年界線模式
    /// Year-boundary convention for the year pillar and zodiac
    /// </summary>
    public enum YearBoundary
    {
        /// <summary>
        /// 以立春交接時刻為界（命理慣例，預設）
        /// LiChun (Beginning of Spring) exact time — the convention used in Bazi/fortune-telling (default)
        /// </summary>
        LiChun = 0,

        /// <summary>
        /// 以農曆正月初一為界（民俗慣例）
        /// Lunar New Year's Day — the folk convention
        /// </summary>
        LunarNewYear = 1,
    }

    /// <summary>
    /// 農曆節日（含以節氣定義的冬至）
    /// Lunar festivals (incl. DongZhi which is solar-term based)
    /// </summary>
    public enum LunarFestival
    {
        /// <summary>除夕（臘月最後一日） / Lunar New Year's Eve</summary>
        LunarNewYearEve = 0,
        /// <summary>春節（正月初一） / Lunar New Year</summary>
        LunarNewYear = 1,
        /// <summary>元宵節（正月十五） / Lantern Festival</summary>
        LanternFestival = 2,
        /// <summary>端午節（五月初五） / Dragon Boat Festival</summary>
        DragonBoat = 3,
        /// <summary>七夕（七月初七） / Qixi Festival</summary>
        Qixi = 4,
        /// <summary>中元節（七月十五） / Ghost Festival</summary>
        GhostFestival = 5,
        /// <summary>中秋節（八月十五） / Mid-Autumn Festival</summary>
        MidAutumn = 6,
        /// <summary>重陽節（九月初九） / Double Ninth Festival</summary>
        DoubleNinth = 7,
        /// <summary>臘八節（臘月初八） / Laba Festival</summary>
        Laba = 8,
        /// <summary>冬至（節氣） / Winter Solstice</summary>
        DongZhi = 9,
    }
}
