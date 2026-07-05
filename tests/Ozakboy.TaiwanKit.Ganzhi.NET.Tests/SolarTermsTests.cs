using System;
using System.Linq;
using Xunit;

namespace Ozakboy.TaiwanKit.Ganzhi.NET.Tests
{
    public class SolarTermsTests
    {
        // ---- 對照已驗證的權威時刻（USNO 二分二至 / 紫金山天文台 / 多來源交叉） ----

        [Theory]
        [InlineData(2000, SolarTerm.ChunFen, 3, 20, 15, 35)]   // USNO: 2000-03-20 07:35 UTC
        [InlineData(2000, SolarTerm.XiaZhi, 6, 21, 9, 48)]     // USNO: 2000-06-21 01:48 UTC
        [InlineData(2000, SolarTerm.QiuFen, 9, 23, 1, 28)]     // USNO: 2000-09-22 17:28 UTC
        [InlineData(2000, SolarTerm.DongZhi, 12, 21, 21, 37)]  // USNO: 2000-12-21 13:37 UTC
        [InlineData(2025, SolarTerm.LiChun, 2, 3, 22, 10)]     // 紫金山: 22:10:13
        [InlineData(2025, SolarTerm.QingMing, 4, 4, 20, 48)]   // 紫金山: 20:48:21
        [InlineData(2025, SolarTerm.MangZhong, 6, 5, 17, 56)]  // 紫金山: 17:56:16
        [InlineData(2026, SolarTerm.LiChun, 2, 4, 4, 2)]       // 多來源: 04:01:51
        [InlineData(2026, SolarTerm.JingZhe, 3, 5, 21, 59)]    // 多來源: 21:58:43
        [InlineData(2026, SolarTerm.QingMing, 4, 5, 2, 40)]    // 多來源: 02:39:43
        [InlineData(2026, SolarTerm.XiaZhi, 6, 21, 16, 24)]    // 多來源: 16:24:12
        [InlineData(2026, SolarTerm.DongZhi, 12, 22, 4, 50)]
        [InlineData(2027, SolarTerm.XiaoHan, 1, 5, 22, 10)]
        [InlineData(2027, SolarTerm.DaHan, 1, 20, 15, 30)]
        public void GetExactTime_VerifiedAnchors(int year, SolarTerm term, int month, int day, int hour, int minute)
        {
            Assert.Equal(new DateTime(year, month, day, hour, minute, 0), SolarTerms.GetExactTime(year, term));
        }

        // ---- 結構性檢查 / Structural checks ----

        [Fact]
        public void GetYearTerms_Returns24InIncreasingOrder()
        {
            for (int year = 1901; year <= 2100; year += 13)
            {
                var terms = SolarTerms.GetYearTerms(year);

                Assert.Equal(24, terms.Count);
                for (int i = 1; i < 24; i++)
                {
                    Assert.True(terms[i].ExactTime > terms[i - 1].ExactTime, $"{year} terms not increasing at {i}");
                }
            }
        }

        [Fact]
        public void GetExactTime_LiChunAlwaysFeb3To5_DongZhiAlwaysDec21To23()
        {
            for (int year = 1901; year <= 2100; year++)
            {
                DateTime liChun = SolarTerms.GetExactTime(year, SolarTerm.LiChun);
                Assert.Equal(2, liChun.Month);
                Assert.InRange(liChun.Day, 3, 5);

                DateTime dongZhi = SolarTerms.GetExactTime(year, SolarTerm.DongZhi);
                Assert.Equal(12, dongZhi.Month);
                Assert.InRange(dongZhi.Day, 21, 23);
            }
        }

        // ---- 區段查詢 / Governing term ----

        [Fact]
        public void GetCurrentTerm_ReturnsGoverningTerm()
        {
            // 2026-02-10 介於立春(2/4)與雨水(2/18)之間
            SolarTermInfo info = SolarTerms.GetCurrentTerm(new DateTime(2026, 2, 10));
            Assert.Equal(SolarTerm.LiChun, info.Term);

            // 立春交接分鐘的前一分鐘仍屬大寒
            Assert.Equal(SolarTerm.DaHan, SolarTerms.GetCurrentTerm(new DateTime(2026, 2, 4, 4, 1, 0)).Term);
            Assert.Equal(SolarTerm.LiChun, SolarTerms.GetCurrentTerm(new DateTime(2026, 2, 4, 4, 2, 0)).Term);
        }

        [Fact]
        public void GetCurrentTerm_EarlyJanuary_IsPreviousYearDongZhi()
        {
            SolarTermInfo info = SolarTerms.GetCurrentTerm(new DateTime(2026, 1, 3));

            Assert.Equal(SolarTerm.DongZhi, info.Term);
            Assert.Equal(2025, info.ExactTime.Year);
        }

        [Fact]
        public void TryGetTermOfDay_TransitionDayDetection()
        {
            Assert.True(SolarTerms.TryGetTermOfDay(new DateTime(2026, 2, 4), out SolarTermInfo? term));
            Assert.Equal(SolarTerm.LiChun, term!.Term);
            Assert.Equal("立春", term.ChineseName);

            Assert.False(SolarTerms.TryGetTermOfDay(new DateTime(2026, 2, 5), out _));
        }

        // ---- 範圍 / Range ----

        [Theory]
        [InlineData(1900)]
        [InlineData(2101)]
        public void GetYearTerms_OutOfRange_Throws(int year)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => SolarTerms.GetYearTerms(year));
        }

        [Fact]
        public void GetCurrentTerm_BeforeCoverage_Throws()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => SolarTerms.GetCurrentTerm(new DateTime(1901, 1, 3)));
        }

        [Fact]
        public void ChineseNames_AllTermsMapped()
        {
            var names = SolarTerms.GetYearTerms(2026).Select(t => t.ChineseName).ToArray();

            Assert.Equal("小寒", names[0]);
            Assert.Equal("立春", names[2]);
            Assert.Equal("夏至", names[11]);
            Assert.Equal("冬至", names[23]);
            Assert.Equal(24, names.Distinct().Count());
        }
    }
}
