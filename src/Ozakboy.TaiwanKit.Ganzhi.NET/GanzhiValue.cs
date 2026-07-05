using System;

namespace Ozakboy.TaiwanKit.Ganzhi.NET
{
    /// <summary>
    /// 一組干支（天干＋地支，如「甲子」；不可變）
    /// A Ganzhi pair (Heavenly Stem + Earthly Branch, e.g. 甲子; immutable)
    /// </summary>
    public readonly struct GanzhiValue : IEquatable<GanzhiValue>
    {
        /// <summary>
        /// 以干支建立
        /// Creates from a stem and a branch
        /// </summary>
        /// <param name="stem">天干 / Heavenly Stem</param>
        /// <param name="branch">地支 / Earthly Branch</param>
        public GanzhiValue(HeavenlyStem stem, EarthlyBranch branch)
        {
            Stem = stem;
            Branch = branch;
        }

        /// <summary>
        /// 以六十甲子序建立（0=甲子 … 59=癸亥）
        /// Creates from the sexagenary index (0=甲子 … 59=癸亥)
        /// </summary>
        /// <param name="sexagenaryIndex">六十甲子序（0～59） / Sexagenary index (0-59)</param>
        internal GanzhiValue(int sexagenaryIndex)
        {
            Stem = (HeavenlyStem)(sexagenaryIndex % 10);
            Branch = (EarthlyBranch)(sexagenaryIndex % 12);
        }

        /// <summary>
        /// 天干
        /// The Heavenly Stem
        /// </summary>
        public HeavenlyStem Stem { get; }

        /// <summary>
        /// 地支
        /// The Earthly Branch
        /// </summary>
        public EarthlyBranch Branch { get; }

        /// <summary>
        /// 中文名稱（如「甲子」）
        /// Chinese name (e.g. 甲子)
        /// </summary>
        public string ChineseName
        {
            get { return Core.GanzhiNames.StemName(Stem) + Core.GanzhiNames.BranchName(Branch); }
        }

        /// <summary>
        /// 判斷兩組干支是否相等
        /// Determines equality of two Ganzhi pairs
        /// </summary>
        public bool Equals(GanzhiValue other)
        {
            return Stem == other.Stem && Branch == other.Branch;
        }

        /// <summary>
        /// 判斷與物件是否相等
        /// Determines equality with an object
        /// </summary>
        public override bool Equals(object? obj)
        {
            return obj is GanzhiValue other && Equals(other);
        }

        /// <summary>
        /// 取得雜湊碼
        /// Gets the hash code
        /// </summary>
        public override int GetHashCode()
        {
            return (int)Stem * 12 + (int)Branch;
        }

        /// <summary>
        /// 相等運算子
        /// Equality operator
        /// </summary>
        public static bool operator ==(GanzhiValue left, GanzhiValue right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// 不等運算子
        /// Inequality operator
        /// </summary>
        public static bool operator !=(GanzhiValue left, GanzhiValue right)
        {
            return !left.Equals(right);
        }

        /// <summary>
        /// 轉為中文名稱字串
        /// Converts to the Chinese name
        /// </summary>
        public override string ToString()
        {
            return ChineseName;
        }
    }

    /// <summary>
    /// 四柱（年/月/日/時干支；不可變）
    /// The four pillars (year/month/day/hour Ganzhi; immutable)
    /// </summary>
    public sealed class FourPillars
    {
        /// <summary>
        /// 建立四柱
        /// Creates the four pillars
        /// </summary>
        internal FourPillars(GanzhiValue year, GanzhiValue month, GanzhiValue day, GanzhiValue hour)
        {
            Year = year;
            Month = month;
            Day = day;
            Hour = hour;
        }

        /// <summary>
        /// 年柱
        /// Year pillar
        /// </summary>
        public GanzhiValue Year { get; }

        /// <summary>
        /// 月柱（依節氣切換）
        /// Month pillar (switched by solar terms)
        /// </summary>
        public GanzhiValue Month { get; }

        /// <summary>
        /// 日柱（23:00 起算次日）
        /// Day pillar (switches to the next day at 23:00)
        /// </summary>
        public GanzhiValue Day { get; }

        /// <summary>
        /// 時柱
        /// Hour pillar
        /// </summary>
        public GanzhiValue Hour { get; }

        /// <summary>
        /// 轉為中文（如「丙午 庚寅 甲子 甲子」）
        /// Converts to Chinese (e.g. "丙午 庚寅 甲子 甲子")
        /// </summary>
        public override string ToString()
        {
            return Year.ChineseName + " " + Month.ChineseName + " " + Day.ChineseName + " " + Hour.ChineseName;
        }
    }

    /// <summary>
    /// 單一節氣資訊（不可變）
    /// Information of one solar term (immutable)
    /// </summary>
    public sealed class SolarTermInfo
    {
        /// <summary>
        /// 建立節氣資訊
        /// Creates a solar term info
        /// </summary>
        internal SolarTermInfo(SolarTerm term, DateTime exactTime)
        {
            Term = term;
            ExactTime = exactTime;
        }

        /// <summary>
        /// 節氣
        /// The solar term
        /// </summary>
        public SolarTerm Term { get; }

        /// <summary>
        /// 交接時刻（台灣時間 UTC+8，精確到分）
        /// Exact transition time (Taiwan time UTC+8, minute precision)
        /// </summary>
        public DateTime ExactTime { get; }

        /// <summary>
        /// 中文名稱（如「立春」）
        /// Chinese name (e.g. 立春)
        /// </summary>
        public string ChineseName
        {
            get { return Core.GanzhiNames.TermName(Term); }
        }
    }
}
