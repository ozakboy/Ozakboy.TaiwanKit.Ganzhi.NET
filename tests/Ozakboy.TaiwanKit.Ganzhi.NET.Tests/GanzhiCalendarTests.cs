using System;
using Xunit;

namespace Ozakboy.TaiwanKit.Ganzhi.NET.Tests
{
    public class GanzhiCalendarTests
    {
        // ---- 日柱錨點（兩個獨立公認事實 + 互為一致性驗證） ----

        [Fact]
        public void GetDayGanzhi_KnownAnchors()
        {
            // 1949-10-01 甲子日（公認事實）
            Assert.Equal("甲子", GanzhiCalendar.GetDayGanzhi(new DateTime(1949, 10, 1)).ChineseName);

            // 2000-01-01 戊午日（公認事實；與上一錨點相距 18354 天 ≡ 54 (mod 60)）
            Assert.Equal("戊午", GanzhiCalendar.GetDayGanzhi(new DateTime(2000, 1, 1)).ChineseName);

            // 60 日循環
            Assert.Equal("甲子", GanzhiCalendar.GetDayGanzhi(new DateTime(1949, 10, 1).AddDays(60)).ChineseName);
            Assert.Equal("癸亥", GanzhiCalendar.GetDayGanzhi(new DateTime(1949, 10, 1).AddDays(-1)).ChineseName);
        }

        // ---- 年柱：立春界（精確到分） ----

        [Fact]
        public void GetYearGanzhi_LiChunBoundary_ExactMinute()
        {
            // 2026 立春 = 2026-02-04 04:02
            Assert.Equal("乙巳", GanzhiCalendar.GetYearGanzhi(new DateTime(2026, 2, 4, 4, 1, 0)).ChineseName);
            Assert.Equal("丙午", GanzhiCalendar.GetYearGanzhi(new DateTime(2026, 2, 4, 4, 2, 0)).ChineseName);
        }

        [Theory]
        [InlineData(1984, 6, 1, "甲子")]
        [InlineData(2000, 6, 1, "庚辰")]
        [InlineData(2025, 6, 1, "乙巳")]
        [InlineData(2026, 6, 1, "丙午")]
        public void GetYearGanzhi_MidYear_KnownYears(int year, int month, int day, string expected)
        {
            Assert.Equal(expected, GanzhiCalendar.GetYearGanzhi(new DateTime(year, month, day)).ChineseName);
        }

        // ---- 年柱：正月初一界 ----

        [Fact]
        public void GetYearGanzhi_LunarNewYearBoundary()
        {
            // 2026 春節 = 2/17；立春(2/4)之後、初一之前 → 兩種模式不同
            var between = new DateTime(2026, 2, 10);

            Assert.Equal("丙午", GanzhiCalendar.GetYearGanzhi(between, YearBoundary.LiChun).ChineseName);
            Assert.Equal("乙巳", GanzhiCalendar.GetYearGanzhi(between, YearBoundary.LunarNewYear).ChineseName);
            Assert.Equal("丙午", GanzhiCalendar.GetYearGanzhi(new DateTime(2026, 2, 17), YearBoundary.LunarNewYear).ChineseName);
        }

        // ---- 生肖 ----

        [Fact]
        public void GetZodiac_BoundaryModes()
        {
            var between = new DateTime(2026, 2, 10);

            Assert.Equal(Zodiac.Horse, GanzhiCalendar.GetZodiac(between));                             // 立春界：已入馬年
            Assert.Equal(Zodiac.Snake, GanzhiCalendar.GetZodiac(between, YearBoundary.LunarNewYear));  // 初一界：仍屬蛇年
            Assert.Equal("馬", GanzhiCalendar.GetChineseName(Zodiac.Horse));
        }

        // ---- 月柱（依節氣精確時刻 + 五虎遁） ----

        [Fact]
        public void GetMonthGanzhi_SwitchesAtLiChunExactMinute()
        {
            // 立春前屬乙巳年丑月（己丑）；立春後屬丙午年寅月（庚寅）
            Assert.Equal("己丑", GanzhiCalendar.GetMonthGanzhi(new DateTime(2026, 2, 4, 4, 1, 0)).ChineseName);
            Assert.Equal("庚寅", GanzhiCalendar.GetMonthGanzhi(new DateTime(2026, 2, 4, 4, 2, 0)).ChineseName);
        }

        [Fact]
        public void GetMonthGanzhi_KnownMonths2025()
        {
            // 2025 乙巳年：立春(2/3 22:10)起 戊寅月；驚蟄(3/5 16:07)起 己卯月
            Assert.Equal("戊寅", GanzhiCalendar.GetMonthGanzhi(new DateTime(2025, 2, 20)).ChineseName);
            Assert.Equal("己卯", GanzhiCalendar.GetMonthGanzhi(new DateTime(2025, 3, 10)).ChineseName);
        }

        [Fact]
        public void GetMonthGanzhi_EarlyJanuary_IsZiMonthOfPreviousDaXue()
        {
            // 2026-01-03 在 2026 小寒(1/5)之前 → 屬 2025 大雪起的子月；乙巳年子月 = 戊子
            Assert.Equal("戊子", GanzhiCalendar.GetMonthGanzhi(new DateTime(2026, 1, 3)).ChineseName);
        }

        // ---- 時柱與晚子時 ----

        [Fact]
        public void GetHourGanzhi_FiveRatsRule()
        {
            // 甲子日（1949-10-01）00:30 → 甲子時
            Assert.Equal("甲子", GanzhiCalendar.GetHourGanzhi(new DateTime(1949, 10, 1, 0, 30, 0)).ChineseName);

            // 甲子日 12:00 午時 → 庚午
            Assert.Equal("庚午", GanzhiCalendar.GetHourGanzhi(new DateTime(1949, 10, 1, 12, 0, 0)).ChineseName);
        }

        [Fact]
        public void GetFourPillars_LateZiHour_DayPillarBelongsToNextDay()
        {
            // 甲子日 23:30 → 日柱換次日乙丑；時柱依乙日五鼠遁 = 丙子
            FourPillars pillars = GanzhiCalendar.GetFourPillars(new DateTime(1949, 10, 1, 23, 30, 0));

            Assert.Equal("乙丑", pillars.Day.ChineseName);
            Assert.Equal("丙子", pillars.Hour.ChineseName);

            // 22:59 仍屬甲子日
            Assert.Equal("甲子", GanzhiCalendar.GetFourPillars(new DateTime(1949, 10, 1, 22, 59, 0)).Day.ChineseName);
        }

        [Fact]
        public void GetFourPillars_CompleteExample()
        {
            // 2026-02-20 10:00（丙午年、立春後寅月）
            FourPillars pillars = GanzhiCalendar.GetFourPillars(new DateTime(2026, 2, 20, 10, 0, 0));

            Assert.Equal("丙午", pillars.Year.ChineseName);
            Assert.Equal("庚寅", pillars.Month.ChineseName);
            // 日柱由錨點推算，時柱與日柱一致性由五鼠遁保證
            int dayStem = (int)pillars.Day.Stem;
            int hourBranch = (int)pillars.Hour.Branch;
            Assert.Equal(EarthlyBranch.Si, pillars.Hour.Branch);   // 10:00 → 巳時
            Assert.Equal((HeavenlyStem)((dayStem % 5 * 2 + hourBranch) % 10), pillars.Hour.Stem);
        }

        // ---- 範圍 / Range ----

        [Theory]
        [InlineData(1901, 2, 18)]
        [InlineData(2101, 1, 1)]
        public void GetYearGanzhi_OutOfRange_Throws(int year, int month, int day)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => GanzhiCalendar.GetYearGanzhi(new DateTime(year, month, day)));
        }

        [Fact]
        public void GanzhiValue_Equality_Works()
        {
            var a = new GanzhiValue(HeavenlyStem.Jia, EarthlyBranch.Zi);
            var b = new GanzhiValue(HeavenlyStem.Jia, EarthlyBranch.Zi);

            Assert.True(a == b);
            Assert.Equal(a.GetHashCode(), b.GetHashCode());
            Assert.Equal("甲子", a.ToString());
        }
    }
}
