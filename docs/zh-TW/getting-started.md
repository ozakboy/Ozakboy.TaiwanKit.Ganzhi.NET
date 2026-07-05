# 快速開始

## 安裝

```bash
dotnet add package Ozakboy.TaiwanKit.Ganzhi.NET
```

支援框架：`netstandard2.0` / `netstandard2.1` / `net8.0` / `net9.0` / `net10.0`，零執行期依賴、離線可用。

## 國曆 ↔ 農曆互轉

```csharp
using Ozakboy.TaiwanKit.Ganzhi.NET;

LunarDate lunar = LunarConverter.ToLunar(new DateTime(2026, 2, 17));
// lunar.Year=2026, Month=1, Day=1, IsLeapMonth=false
// lunar.ToString() → "2026年正月初一"

DateTime solar = LunarConverter.ToSolar(new LunarDate(2026, 8, 15));   // 中秋 → 2026-09-25
```

**閏月**以 `IsLeapMonth` 明確表示；不存在的日期（沒有的閏月、小月三十）用 `TryToSolar` 優雅處理：

```csharp
LunarConverter.GetLeapMonth(2025);   // 6（2025 閏六月）

LunarConverter.TryToSolar(new LunarDate(2023, 2, 15, isLeapMonth: true), out DateTime d);   // true（2023 有閏二月）
LunarConverter.TryToSolar(new LunarDate(2026, 5, 5, isLeapMonth: true), out _);             // false（2026 無閏月）
```

## 二十四節氣（精確到分，台灣時間）

```csharp
SolarTerms.GetExactTime(2026, SolarTerm.LiChun);        // 2026-02-04 04:02
SolarTerms.GetYearTerms(2026);                          // 全年 24 筆（含精確時刻與中文名）
SolarTerms.GetCurrentTerm(new DateTime(2026, 2, 10));   // 立春（所屬節氣區段）
SolarTerms.TryGetTermOfDay(new DateTime(2026, 2, 4), out var term);   // true，當天是立春交接日
```

## 干支四柱與生肖

```csharp
// 年柱界線可選：立春（命理慣例，預設）或正月初一（民俗慣例）
GanzhiCalendar.GetYearGanzhi(new DateTime(2026, 2, 10));                            // 丙午（立春界）
GanzhiCalendar.GetYearGanzhi(new DateTime(2026, 2, 10), YearBoundary.LunarNewYear); // 乙巳（初一界）

// 四柱（八字）：月柱依節氣精確時刻切換、23:00 起日柱歸次日
FourPillars p = GanzhiCalendar.GetFourPillars(new DateTime(2026, 2, 20, 10, 0, 0));
Console.WriteLine(p);   // 例如 "丙午 庚寅 甲子 己巳"

// 生肖
GanzhiCalendar.GetZodiac(new DateTime(2026, 6, 1));   // Zodiac.Horse（馬）
```

## 農曆節日

```csharp
LunarFestivals.TryGetFestival(new DateTime(2026, 6, 19), out var f);   // true, DragonBoat（端午）
LunarFestivals.GetFestivalDate(2026, LunarFestival.MidAutumn);         // 2026-09-25
LunarFestivals.GetFestivalDate(2026, LunarFestival.LunarNewYearEve);   // 2026-02-16（除夕）
```

## 重要語意

- **所有時間皆為台灣時間（UTC+8）**，不做時區換算
- 年柱界線預設以**立春交接分鐘**為界：2026-02-04 04:01 → 乙巳、04:02 → 丙午
- `GetDayGanzhi` 以日曆日為準（00:00 換日）；`GetFourPillars` 採八字慣例 23:00 換日
- **閏月不算節日**（閏五月初五不是端午）
- 超出支援範圍（1901-02-19 ～ 2100-12-31）一律拋例外，絕不外插曆法
