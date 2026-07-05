using System;
using Xunit;

namespace Ozakboy.TaiwanKit.Ganzhi.NET.Tests
{
    public class LunarFestivalsTests
    {
        // ---- 2026 年節日（與 TaiwanHolidays 官方行事曆互相印證的日期） ----

        [Theory]
        [InlineData(2026, 2, 16, LunarFestival.LunarNewYearEve)]   // 除夕（官方行事曆 2/16 農曆除夕）
        [InlineData(2026, 2, 17, LunarFestival.LunarNewYear)]      // 春節
        [InlineData(2026, 3, 3, LunarFestival.LanternFestival)]    // 元宵（正月十五）
        [InlineData(2026, 6, 19, LunarFestival.DragonBoat)]        // 端午（官方行事曆 6/19）
        [InlineData(2026, 9, 25, LunarFestival.MidAutumn)]         // 中秋（官方行事曆 9/25）
        [InlineData(2026, 12, 22, LunarFestival.DongZhi)]          // 冬至（節氣）
        [InlineData(2025, 1, 28, LunarFestival.LunarNewYearEve)]   // 2025 除夕
        [InlineData(2025, 10, 6, LunarFestival.MidAutumn)]         // 2025 中秋（官方行事曆 10/6）
        public void TryGetFestival_KnownDates(int year, int month, int day, LunarFestival expected)
        {
            Assert.True(LunarFestivals.TryGetFestival(new DateTime(year, month, day), out LunarFestival festival));
            Assert.Equal(expected, festival);
        }

        [Theory]
        [InlineData(2026, 3, 4)]
        [InlineData(2026, 7, 1)]
        public void TryGetFestival_OrdinaryDays_ReturnsFalse(int year, int month, int day)
        {
            Assert.False(LunarFestivals.TryGetFestival(new DateTime(year, month, day), out _));
        }

        [Fact]
        public void TryGetFestival_LeapMonthDay_NotAFestival()
        {
            // 2025 閏六月初七（≠七夕）：閏月不算節日
            DateTime leapDay = LunarConverter.ToSolar(new LunarDate(2025, 6, 7, isLeapMonth: true));

            Assert.False(LunarFestivals.TryGetFestival(leapDay, out _));
        }

        // ---- GetFestivalDate ----

        [Theory]
        [InlineData(2026, LunarFestival.LunarNewYear, 2, 17)]
        [InlineData(2026, LunarFestival.LunarNewYearEve, 2, 16)]
        [InlineData(2026, LunarFestival.DragonBoat, 6, 19)]
        [InlineData(2026, LunarFestival.MidAutumn, 9, 25)]
        [InlineData(2026, LunarFestival.DongZhi, 12, 22)]
        [InlineData(2025, LunarFestival.MidAutumn, 10, 6)]
        [InlineData(2024, LunarFestival.MidAutumn, 9, 17)]
        public void GetFestivalDate_KnownDates(int year, LunarFestival festival, int month, int day)
        {
            Assert.Equal(new DateTime(year, month, day), LunarFestivals.GetFestivalDate(year, festival));
        }

        [Fact]
        public void GetFestivalDate_Laba_EdgeCases()
        {
            // 2022 年有兩個臘八（1/10 與 12/30）→ 取第一個
            Assert.Equal(new DateTime(2022, 1, 10), LunarFestivals.GetFestivalDate(2022, LunarFestival.Laba));

            // 2023 年沒有臘八（前一個 2022-12-30、下一個 2024-01-18）→ 拋例外
            Assert.Throws<InvalidOperationException>(() => LunarFestivals.GetFestivalDate(2023, LunarFestival.Laba));

            // 2024 臘八 = 1/18
            Assert.Equal(new DateTime(2024, 1, 18), LunarFestivals.GetFestivalDate(2024, LunarFestival.Laba));
        }

        [Fact]
        public void GetFestivalDate_QixiAndDoubleNinth()
        {
            // 2026 七夕 = 農曆 7/7；重陽 = 農曆 9/9（由轉換器求得後反查一致）
            DateTime qixi = LunarFestivals.GetFestivalDate(2026, LunarFestival.Qixi);
            Assert.Equal(new LunarDate(2026, 7, 7), LunarConverter.ToLunar(qixi));

            DateTime doubleNinth = LunarFestivals.GetFestivalDate(2026, LunarFestival.DoubleNinth);
            Assert.Equal(new LunarDate(2026, 9, 9), LunarConverter.ToLunar(doubleNinth));
        }

        [Fact]
        public void TryGetFestival_OutOfRange_Throws()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => LunarFestivals.TryGetFestival(new DateTime(1901, 1, 1), out _));
        }
    }
}
