# Ozakboy.TaiwanKit.Ganzhi.NET

[English](https://github.com/ozakboy/Ozakboy.TaiwanKit.Ganzhi.NET/blob/main/README.md)

.NET 農曆/節氣/干支（四柱）轉換套件——涵蓋 **1901～2100**，完全離線可用、**零執行期依賴**。

**Ozakboy.TaiwanKit** 系列套件之一。

## 功能特色

- **國曆 ↔ 農曆互轉** — 閏月以 `IsLeapMonth` 明確表示，不存在的日期用 `TryToSolar` 優雅拒絕
- **二十四節氣精確到分** — 交接時刻為台灣時間（UTC+8），以全精度 VSOP87D 星曆天文計算，並經 USNO 二分二至與紫金山天文台等多來源逐分驗證
- **干支四柱** — 八字用的年/月/日/時柱：
  - 年柱界線可選：**立春精確時刻**（預設，命理慣例）或**正月初一**（民俗慣例）
  - 月柱依節氣精確時刻切換（五虎遁月干）
  - 日柱以兩個獨立公認錨點互證；時柱含 23:00 晚子時規則
- **生肖** — 同樣可選年界線
- **農曆節日** — 除夕、春節、元宵、端午、七夕、中元、中秋、重陽、臘八（含一年兩次/零次特例）與冬至

## 安裝

```
dotnet add package Ozakboy.TaiwanKit.Ganzhi.NET
```

支援框架：`netstandard2.0` / `netstandard2.1` / `net8.0` / `net9.0` / `net10.0`

## 快速開始

```csharp
using Ozakboy.TaiwanKit.Ganzhi.NET;

// --- 農曆互轉 ---
LunarDate lunar = LunarConverter.ToLunar(new DateTime(2026, 2, 17));
// lunar.Year=2026, Month=1, Day=1, IsLeapMonth=false → "2026年正月初一"

DateTime solar = LunarConverter.ToSolar(new LunarDate(2026, 8, 15));  // 中秋 → 2026-09-25
LunarConverter.GetLeapMonth(2025);                                     // 6（閏六月）

// --- 節氣（精確到分，台灣時間） ---
SolarTerms.GetExactTime(2026, SolarTerm.LiChun);      // 2026-02-04 04:02
SolarTerms.GetCurrentTerm(new DateTime(2026, 2, 10)); // 立春（所屬節氣區段）

// --- 干支四柱 ---
FourPillars p = GanzhiCalendar.GetFourPillars(new DateTime(2026, 2, 20, 10, 0, 0));
// p.ToString() → "丙午 庚寅 [日柱] [時柱]"

GanzhiCalendar.GetYearGanzhi(new DateTime(2026, 2, 10));                            // 丙午（立春界）
GanzhiCalendar.GetYearGanzhi(new DateTime(2026, 2, 10), YearBoundary.LunarNewYear); // 乙巳（初一界）
GanzhiCalendar.GetZodiac(new DateTime(2026, 6, 1));                                 // Zodiac.Horse（馬）

// --- 農曆節日 ---
LunarFestivals.TryGetFestival(new DateTime(2026, 6, 19), out var f);  // true, DragonBoat（端午）
LunarFestivals.GetFestivalDate(2026, LunarFestival.MidAutumn);        // 2026-09-25
```

## API 總覽

| 類別 | 重點 |
|------|------|
| `LunarConverter` | `ToLunar` / `ToSolar` / `TryToSolar` / `GetLeapMonth`；範圍 1901-02-19 ～ 2101-01-28 |
| `SolarTerms` | `GetYearTerms(year)` / `GetExactTime(year, term)` / `GetCurrentTerm(時刻)` / `TryGetTermOfDay(日期)`；1901～2100 |
| `GanzhiCalendar` | `GetYearGanzhi` / `GetMonthGanzhi` / `GetDayGanzhi` / `GetHourGanzhi` / `GetFourPillars` / `GetZodiac`；1901-02-19 ～ 2100-12-31 |
| `LunarFestivals` | `TryGetFestival(日期)` / `GetFestivalDate(年, 節日)` |
| `LunarDate` / `GanzhiValue` / `FourPillars` / `SolarTermInfo` | 不可變值物件，附中文名稱（`ChineseName`、`ToString`） |

### 重要語意

- **所有時間皆為台灣時間（UTC+8）**，不做時區換算。
- **年柱界線**：預設以立春交接「分鐘」為界。`2026-02-04 04:01` → 乙巳、`04:02` → 丙午。
- **日柱**（`GetDayGanzhi`）以日曆日為準（00:00 換日）；`GetFourPillars` 採八字慣例：23:00 起日柱歸次日。
- **閏月不算節日**（閏五月初五不是端午）。
- **超出範圍一律拋例外**——本套件絕不外插曆法。

## 資料正確性

- 農曆轉換核心採用 .NET 內建 `ChineseLunisolarCalendar`（微軟維護，1901～2100）。
- 節氣表以**全精度 VSOP87D** 星曆離線計算（IAU1980 章動主項、Espenak–Meeus ΔT），捨入到分，並經以下驗證：
  - USNO 發布的二分二至時刻（2000 年：4/4 全中）
  - 紫金山天文台來源的曆書時刻（2025 年：3/3 全中）
  - 2026 年全部 24 節氣多來源交叉比對（24/24）

## 連結

- 更新紀錄：[docs/zh-TW/changelog.md](https://github.com/ozakboy/Ozakboy.TaiwanKit.Ganzhi.NET/blob/main/docs/zh-TW/changelog.md)
- 授權：MIT

## Ozakboy.TaiwanKit 系列

| 套件 | 說明 |
|------|------|
| `Ozakboy.TaiwanKit.TwIdValidator` | 台灣身分證/統編/手機驗證 |
| `Ozakboy.TaiwanKit.TaiwanHolidays` | 台灣國定假日/補班日查詢 |
| `Ozakboy.TaiwanKit.Ganzhi.NET` | 農曆/節氣/干支轉換（本套件） |
