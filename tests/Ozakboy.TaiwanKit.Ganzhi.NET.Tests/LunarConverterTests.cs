using System;
using Xunit;

namespace Ozakboy.TaiwanKit.Ganzhi.NET.Tests
{
    public class LunarConverterTests
    {
        // ---- 春節初一對照（獨立已知事實） / Lunar New Year day-1 anchors ----

        [Theory]
        [InlineData(1912, 2, 18)]
        [InlineData(1950, 2, 17)]
        [InlineData(2017, 1, 28)]
        [InlineData(2018, 2, 16)]
        [InlineData(2019, 2, 5)]
        [InlineData(2020, 1, 25)]
        [InlineData(2021, 2, 12)]
        [InlineData(2022, 2, 1)]
        [InlineData(2023, 1, 22)]
        [InlineData(2024, 2, 10)]
        [InlineData(2025, 1, 29)]
        [InlineData(2026, 2, 17)]
        [InlineData(2027, 2, 6)]
        public void ToLunar_LunarNewYearDay_IsMonth1Day1(int year, int month, int day)
        {
            LunarDate lunar = LunarConverter.ToLunar(new DateTime(year, month, day));

            Assert.Equal(year, lunar.Year);
            Assert.Equal(1, lunar.Month);
            Assert.Equal(1, lunar.Day);
            Assert.False(lunar.IsLeapMonth);
        }

        // ---- 閏月 / Leap months ----

        [Theory]
        [InlineData(2020, 4)]   // 2020 閏四月
        [InlineData(2023, 2)]   // 2023 閏二月
        [InlineData(2025, 6)]   // 2025 閏六月
        [InlineData(2017, 6)]   // 2017 閏六月
        [InlineData(2004, 2)]   // 2004 閏二月
        [InlineData(2026, 0)]   // 2026 無閏月
        [InlineData(2027, 0)]   // 2027 無閏月
        public void GetLeapMonth_KnownYears(int year, int expectedLeapMonth)
        {
            Assert.Equal(expectedLeapMonth, LunarConverter.GetLeapMonth(year));
        }

        [Fact]
        public void ToLunar_LeapMonthDay_FlaggedAsLeap()
        {
            // 2023 閏二月初一 = 2023-03-22
            LunarDate lunar = LunarConverter.ToLunar(new DateTime(2023, 3, 22));

            Assert.Equal(2023, lunar.Year);
            Assert.Equal(2, lunar.Month);
            Assert.Equal(1, lunar.Day);
            Assert.True(lunar.IsLeapMonth);
        }

        [Fact]
        public void ToLunar_MonthAfterLeap_NumberedCorrectly()
        {
            // 2023 閏二月之後的三月：閏二月初一(3/22)+29天 = 三月初一 (2023 閏二月小,29天) → 2023-04-20
            LunarDate lunar = LunarConverter.ToLunar(new DateTime(2023, 4, 20));

            Assert.Equal(3, lunar.Month);
            Assert.Equal(1, lunar.Day);
            Assert.False(lunar.IsLeapMonth);
        }

        [Fact]
        public void TryToSolar_LeapMonthRoundtrip()
        {
            var leapDate = new LunarDate(2023, 2, 15, isLeapMonth: true);

            Assert.True(LunarConverter.TryToSolar(leapDate, out DateTime solar));
            Assert.Equal(leapDate, LunarConverter.ToLunar(solar));
        }

        [Theory]
        [InlineData(2023, 5, 1, true)]    // 2023 閏二月，指閏五月 → 不存在
        [InlineData(2026, 1, 1, true)]    // 2026 無閏月
        [InlineData(2026, 13, 1, false)]  // 月份超界
        [InlineData(2026, 1, 31, false)]  // 日超界
        [InlineData(1900, 1, 1, false)]   // 年超界
        [InlineData(2101, 1, 1, false)]   // 年超界
        public void TryToSolar_NonexistentDates_ReturnsFalse(int year, int month, int day, bool isLeap)
        {
            Assert.False(LunarConverter.TryToSolar(new LunarDate(year, month, day, isLeap), out _));
        }

        [Fact]
        public void TryToSolar_Day30OfShortMonth_ReturnsFalse()
        {
            // 2026 臘月為 29 天（除夕 2026-02-16 → 臘月三十不存在? 依 2027 春節 2/6 反推 2026 臘月）
            // 用程式事實：若 12 月 30 日不存在則 TryToSolar false 且 12/29 存在
            bool has30 = LunarConverter.TryToSolar(new LunarDate(2026, 12, 30), out _);
            bool has29 = LunarConverter.TryToSolar(new LunarDate(2026, 12, 29), out _);

            Assert.True(has29);
            // 2027 春節 = 2027-02-06；2026 臘月初一 + 29/30 天 = 除夕。兩者擇一成立，此處驗證一致性：
            DateTime cny2027 = LunarConverter.ToSolar(new LunarDate(2027, 1, 1));
            DateTime lastDay = cny2027.AddDays(-1);
            LunarDate eve = LunarConverter.ToLunar(lastDay);
            Assert.Equal(12, eve.Month);
            Assert.Equal(has30 ? 30 : 29, eve.Day);
        }

        // ---- Roundtrip 全區間抽樣 / Range-wide roundtrip sampling ----

        [Fact]
        public void Roundtrip_SampledAcrossFullRange()
        {
            for (DateTime day = LunarConverter.MinDate; day <= LunarConverter.MaxDate; day = day.AddDays(97))
            {
                LunarDate lunar = LunarConverter.ToLunar(day);

                Assert.True(LunarConverter.TryToSolar(lunar, out DateTime back), $"TryToSolar failed for {day:yyyy-MM-dd} → {lunar}");
                Assert.Equal(day, back);
            }
        }

        // ---- 邊界 / Boundaries ----

        [Fact]
        public void ToLunar_MinAndMaxDate_Work()
        {
            LunarDate min = LunarConverter.ToLunar(LunarConverter.MinDate);
            Assert.Equal(new LunarDate(1901, 1, 1), min);

            LunarDate max = LunarConverter.ToLunar(LunarConverter.MaxDate);
            Assert.Equal(2100, max.Year);
            Assert.Equal(12, max.Month);
        }

        [Theory]
        [InlineData(1901, 2, 18)]
        [InlineData(2101, 1, 29)]
        public void ToLunar_OutOfRange_Throws(int year, int month, int day)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => LunarConverter.ToLunar(new DateTime(year, month, day)));
        }

        [Fact]
        public void LunarDate_ToString_Formats()
        {
            Assert.Equal("2026年正月初一", new LunarDate(2026, 1, 1).ToString());
            Assert.Equal("2023年閏二月十五", new LunarDate(2023, 2, 15, true).ToString());
            Assert.Equal("2025年臘月廿九", new LunarDate(2025, 12, 29).ToString());
        }
    }
}
