# Ozakboy.TaiwanKit.Ganzhi.NET

[繁體中文](https://github.com/ozakboy/Ozakboy.TaiwanKit.Ganzhi.NET/blob/main/README_zh-TW.md)

Lunar calendar, 24 solar terms and Ganzhi (four pillars) conversion for .NET — covering **1901–2100**, works fully offline, **zero runtime dependencies**.

Part of the **Ozakboy.TaiwanKit** series.

## Features

- **Gregorian ↔ lunar conversion** — leap months explicitly flagged (`IsLeapMonth`), non-existent dates rejected via `TryToSolar`
- **24 solar terms with minute precision** — transition times in Taiwan time (UTC+8), astronomically computed from the full-precision VSOP87D ephemeris and cross-verified against USNO equinox/solstice times and published almanac data
- **Ganzhi four pillars** — year / month / day / hour pillars for Bazi use:
  - Year boundary selectable: **LiChun exact time** (default, fortune-telling convention) or **Lunar New Year** (folk convention)
  - Month pillars switch at exact solar-term times (five-tigers stem rule)
  - Day pillar anchored to two independently verified dates; hour pillar with the 23:00 late-Zi rule
- **Zodiac** — with the same selectable year boundary
- **Lunar festivals** — New Year's Eve, Lunar New Year, Lantern, Dragon Boat, Qixi, Ghost, Mid-Autumn, Double Ninth, Laba (incl. its two-or-zero-per-year edge case) and DongZhi

## Install

```
dotnet add package Ozakboy.TaiwanKit.Ganzhi.NET
```

Supported frameworks: `netstandard2.0` / `netstandard2.1` / `net8.0` / `net9.0` / `net10.0`

## Quick start

```csharp
using Ozakboy.TaiwanKit.Ganzhi.NET;

// --- Lunar conversion ---
LunarDate lunar = LunarConverter.ToLunar(new DateTime(2026, 2, 17));
// lunar.Year=2026, Month=1, Day=1, IsLeapMonth=false → "2026年正月初一"

DateTime solar = LunarConverter.ToSolar(new LunarDate(2026, 8, 15));  // Mid-Autumn → 2026-09-25
LunarConverter.GetLeapMonth(2025);                                     // 6 (leap 6th month)

// --- Solar terms (minute precision, Taiwan time) ---
SolarTerms.GetExactTime(2026, SolarTerm.LiChun);      // 2026-02-04 04:02
SolarTerms.GetCurrentTerm(new DateTime(2026, 2, 10)); // LiChun (governing term)

// --- Ganzhi four pillars ---
FourPillars p = GanzhiCalendar.GetFourPillars(new DateTime(2026, 2, 20, 10, 0, 0));
// p.ToString() → "丙午 庚寅 [day] [hour]"

GanzhiCalendar.GetYearGanzhi(new DateTime(2026, 2, 10));                            // 丙午 (LiChun boundary)
GanzhiCalendar.GetYearGanzhi(new DateTime(2026, 2, 10), YearBoundary.LunarNewYear); // 乙巳 (folk boundary)
GanzhiCalendar.GetZodiac(new DateTime(2026, 6, 1));                                 // Zodiac.Horse

// --- Lunar festivals ---
LunarFestivals.TryGetFestival(new DateTime(2026, 6, 19), out var f);  // true, DragonBoat
LunarFestivals.GetFestivalDate(2026, LunarFestival.MidAutumn);        // 2026-09-25
```

## API overview

| Type | Highlights |
|------|-----------|
| `LunarConverter` | `ToLunar` / `ToSolar` / `TryToSolar` / `GetLeapMonth`; range 1901-02-19 ~ 2101-01-28 |
| `SolarTerms` | `GetYearTerms(year)` / `GetExactTime(year, term)` / `GetCurrentTerm(moment)` / `TryGetTermOfDay(date)`; 1901–2100 |
| `GanzhiCalendar` | `GetYearGanzhi` / `GetMonthGanzhi` / `GetDayGanzhi` / `GetHourGanzhi` / `GetFourPillars` / `GetZodiac`; 1901-02-19 ~ 2100-12-31 |
| `LunarFestivals` | `TryGetFestival(date)` / `GetFestivalDate(year, festival)` |
| `LunarDate` / `GanzhiValue` / `FourPillars` / `SolarTermInfo` | Immutable value objects with Chinese names (`ChineseName`, `ToString`) |

### Semantics worth knowing

- **All times are Taiwan time (UTC+8)**; no time-zone conversion is performed.
- **Year pillar boundary**: default is the exact LiChun minute. `2026-02-04 04:01` → 乙巳, `04:02` → 丙午.
- **Day pillar** (`GetDayGanzhi`) uses the civil date (switches at 00:00). `GetFourPillars` applies the Bazi convention: from 23:00 the day pillar belongs to the next day.
- **Leap months never count as festivals** (a leap-5th-month 5th day is not Dragon Boat).
- **Out-of-range queries throw** — the library never extrapolates the calendar.

## Data accuracy

- Lunar conversion is backed by .NET's `ChineseLunisolarCalendar` (Microsoft-maintained, 1901–2100).
- The solar term table is generated offline from the **full-precision VSOP87D** ephemeris (IAU1980 nutation main terms, Espenak–Meeus ΔT), rounded to the minute, and verified against:
  - USNO-published equinox/solstice times (2000: 4/4 exact)
  - Purple-Mountain-Observatory-sourced almanac times (2025: 3/3 exact)
  - Multi-source cross-check of all 24 terms of 2026 (24/24)

## Links

- Changelog: [docs/en/changelog.md](https://github.com/ozakboy/Ozakboy.TaiwanKit.Ganzhi.NET/blob/main/docs/en/changelog.md)
- License: MIT

## Ozakboy.TaiwanKit series

| Package | Description |
|---------|-------------|
| `Ozakboy.TaiwanKit.TwIdValidator` | Taiwan ID / BAN / mobile number validation |
| `Ozakboy.TaiwanKit.TaiwanHolidays` | Taiwan national holidays / make-up workday queries |
| `Ozakboy.TaiwanKit.Ganzhi.NET` | Lunar calendar / solar terms / Ganzhi conversion (this package) |
