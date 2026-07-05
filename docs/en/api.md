# API Reference

## LunarConverter — Gregorian↔lunar

| Member | Returns | Description |
|--------|---------|-------------|
| `ToLunar(DateTime)` | `LunarDate` | Gregorian → lunar; throws when out of range |
| `ToSolar(LunarDate)` | `DateTime` | Lunar → Gregorian; throws for non-existent dates |
| `TryToSolar(LunarDate, out DateTime)` | `bool` | Returns false (no throw) for non-existent dates |
| `GetLeapMonth(int lunarYear)` | `int` | Leap month number (e.g. 2 for 2023); 0 when none |
| `MinDate` / `MaxDate` | `DateTime` | 1901-02-19 to 2101-01-28 |

### LunarDate (readonly struct)

`Year` / `Month` (1-12; a leap month shares its predecessor's number) / `Day` / `IsLeapMonth`; `ToString()` returns Chinese ("2023年閏二月十五").

## SolarTerms — the 24 solar terms

| Member | Returns | Description |
|--------|---------|-------------|
| `GetYearTerms(int year)` | `IReadOnlyList<SolarTermInfo>` | All 24 terms in Gregorian-year order (XiaoHan → DongZhi) |
| `GetExactTime(int year, SolarTerm)` | `DateTime` | Transition time (Taiwan time, minute precision) |
| `GetCurrentTerm(DateTime)` | `SolarTermInfo` | Governing term of a moment |
| `TryGetTermOfDay(DateTime, out SolarTermInfo?)` | `bool` | Whether the date is a transition day |
| `MinYear` / `MaxYear` | `int` | 1901 to 2100 |

`SolarTermInfo`: `Term` / `ExactTime` / `ChineseName`. The `SolarTerm` enum is in-year ordered: `XiaoHan(0)` … `DongZhi(23)`.

## GanzhiCalendar — the Ganzhi calendar

| Member | Returns | Description |
|--------|---------|-------------|
| `GetYearGanzhi(DateTime, YearBoundary)` | `GanzhiValue` | Year pillar; default boundary = exact LiChun minute |
| `GetMonthGanzhi(DateTime)` | `GanzhiValue` | Month pillar; switches at the 12 "Jie" exact times, five-tigers stems |
| `GetDayGanzhi(DateTime)` | `GanzhiValue` | Day pillar (civil date, switches at 00:00) |
| `GetHourGanzhi(DateTime)` | `GanzhiValue` | Hour pillar; Zi starts at 23:00, five-rats stems |
| `GetFourPillars(DateTime, YearBoundary)` | `FourPillars` | Four pillars; day pillar follows the Bazi 23:00 convention |
| `GetZodiac(DateTime, YearBoundary)` | `Zodiac` | Zodiac; boundary rule identical to the year pillar |
| `GetChineseName(Zodiac)` | `string` | Chinese zodiac name |
| `MinDate` / `MaxDate` | `DateTime` | 1901-02-19 to 2100-12-31 |

`GanzhiValue` (readonly struct): `Stem` / `Branch` / `ChineseName` (e.g. 甲子).
`YearBoundary`: `LiChun` (default) / `LunarNewYear`.

## LunarFestivals — lunar festivals

| Member | Returns | Description |
|--------|---------|-------------|
| `TryGetFestival(DateTime, out LunarFestival)` | `bool` | Whether the date is a festival; leap months never count |
| `GetFestivalDate(int solarYear, LunarFestival)` | `DateTime` | The festival's date within a Gregorian year |

`LunarFestival`: New Year's Eve, Lunar New Year, Lantern, Dragon Boat, Qixi, Ghost, Mid-Autumn, Double Ninth, Laba, DongZhi (solar-term based).

> **Laba edge case**: lunar 12/8 is only 22-23 days before the next New Year, so a Gregorian year may contain two Laba (first returned) or zero (throws `InvalidOperationException`). E.g. 2022 has two (Jan 10, Dec 30); 2023 has none.

## Exception behavior

- Out-of-range queries → `ArgumentOutOfRangeException` (the calendar is never extrapolated)
- `ToSolar` with a non-existent lunar date → `ArgumentOutOfRangeException` (use `TryToSolar` to avoid)
- `GetFestivalDate` when the year has no Laba → `InvalidOperationException`
