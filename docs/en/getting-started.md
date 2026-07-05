# Getting Started

## Install

```bash
dotnet add package Ozakboy.TaiwanKit.Ganzhi.NET
```

Supported frameworks: `netstandard2.0` / `netstandard2.1` / `net8.0` / `net9.0` / `net10.0`. Zero runtime dependencies, works fully offline.

## Gregorian ↔ lunar conversion

```csharp
using Ozakboy.TaiwanKit.Ganzhi.NET;

LunarDate lunar = LunarConverter.ToLunar(new DateTime(2026, 2, 17));
// lunar.Year=2026, Month=1, Day=1, IsLeapMonth=false
// lunar.ToString() → "2026年正月初一"

DateTime solar = LunarConverter.ToSolar(new LunarDate(2026, 8, 15));   // Mid-Autumn → 2026-09-25
```

**Leap months** are explicitly flagged via `IsLeapMonth`; non-existent dates (missing leap month, day 30 of a short month) are handled gracefully with `TryToSolar`:

```csharp
LunarConverter.GetLeapMonth(2025);   // 6 (2025 has a leap 6th month)

LunarConverter.TryToSolar(new LunarDate(2023, 2, 15, isLeapMonth: true), out DateTime d);   // true
LunarConverter.TryToSolar(new LunarDate(2026, 5, 5, isLeapMonth: true), out _);             // false (no leap month in 2026)
```

## 24 solar terms (minute precision, Taiwan time)

```csharp
SolarTerms.GetExactTime(2026, SolarTerm.LiChun);        // 2026-02-04 04:02
SolarTerms.GetYearTerms(2026);                          // all 24 terms with exact times
SolarTerms.GetCurrentTerm(new DateTime(2026, 2, 10));   // LiChun (governing term)
SolarTerms.TryGetTermOfDay(new DateTime(2026, 2, 4), out var term);   // true — transition day
```

## Ganzhi four pillars & zodiac

```csharp
// Year boundary: LiChun exact time (fortune-telling convention, default) or Lunar New Year (folk)
GanzhiCalendar.GetYearGanzhi(new DateTime(2026, 2, 10));                            // 丙午 (LiChun)
GanzhiCalendar.GetYearGanzhi(new DateTime(2026, 2, 10), YearBoundary.LunarNewYear); // 乙巳 (folk)

// Four pillars (Bazi): month pillars switch at exact term times; day switches at 23:00
FourPillars p = GanzhiCalendar.GetFourPillars(new DateTime(2026, 2, 20, 10, 0, 0));
Console.WriteLine(p);   // e.g. "丙午 庚寅 甲子 己巳"

// Zodiac
GanzhiCalendar.GetZodiac(new DateTime(2026, 6, 1));   // Zodiac.Horse
```

## Lunar festivals

```csharp
LunarFestivals.TryGetFestival(new DateTime(2026, 6, 19), out var f);   // true, DragonBoat
LunarFestivals.GetFestivalDate(2026, LunarFestival.MidAutumn);         // 2026-09-25
LunarFestivals.GetFestivalDate(2026, LunarFestival.LunarNewYearEve);   // 2026-02-16
```

## Semantics worth knowing

- **All times are Taiwan time (UTC+8)**; no time-zone conversion
- The default year boundary is the **exact LiChun minute**: 2026-02-04 04:01 → 乙巳, 04:02 → 丙午
- `GetDayGanzhi` uses the civil date (switches at 00:00); `GetFourPillars` follows the Bazi 23:00 convention
- **Leap months never count as festivals** (a leap-5th-month 5th is not Dragon Boat)
- Out-of-range queries (outside 1901-02-19 to 2100-12-31) always throw — the calendar is never extrapolated
