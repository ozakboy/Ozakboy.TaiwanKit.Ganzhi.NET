# Changelog

All notable changes to **Ozakboy.TaiwanKit.Ganzhi.NET** are documented here.

## [1.0.0] - 2026-07-05

### Added

- Gregorian↔lunar conversion for 1901–2100 (`LunarConverter`), with explicit leap-month semantics (`IsLeapMonth`), `TryToSolar` for non-existent dates and `GetLeapMonth`
- 24 solar terms with minute-precision transition times in Taiwan time (`SolarTerms`): per-year lists, exact times, governing-term and transition-day queries
- Ganzhi four pillars (`GanzhiCalendar`): year pillar with selectable boundary (LiChun exact minute — default, or Lunar New Year), month pillars switched at exact solar-term times with the five-tigers stem rule, day pillar (dual verified anchors), hour pillar with the 23:00 late-Zi convention, and `GetFourPillars`
- Zodiac queries with the same boundary options
- Lunar festivals (`LunarFestivals`): New Year's Eve, Lunar New Year, Lantern, Dragon Boat, Qixi, Ghost, Mid-Autumn, Double Ninth, Laba (two-or-zero-per-year edge case handled) and DongZhi
- Immutable value objects with Chinese names: `LunarDate`, `GanzhiValue`, `FourPillars`, `SolarTermInfo`
- Bilingual (Traditional Chinese / English) XML documentation for the whole public API

### Technical

- Target frameworks: `netstandard2.0` / `netstandard2.1` / `net8.0` / `net9.0` / `net10.0`; zero runtime dependencies
- Lunar core: BCL `ChineseLunisolarCalendar`; solar term table generated offline from the full-precision VSOP87D ephemeris (IAU1980 nutation main terms, Espenak–Meeus ΔT)
- Solar term verification: USNO 2000 equinoxes/solstices (4/4), Purple-Mountain-Observatory-sourced 2025 times (3/3), multi-source cross-check of all 24 terms of 2026 (24/24)
- Deterministic build, SourceLink, symbol package (`snupkg`)
