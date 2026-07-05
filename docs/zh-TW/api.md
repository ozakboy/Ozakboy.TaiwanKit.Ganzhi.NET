# API 參考

## LunarConverter — 國曆↔農曆

| 成員 | 回傳 | 說明 |
|------|------|------|
| `ToLunar(DateTime)` | `LunarDate` | 國曆轉農曆；超出範圍拋例外 |
| `ToSolar(LunarDate)` | `DateTime` | 農曆轉國曆；日期不存在拋例外 |
| `TryToSolar(LunarDate, out DateTime)` | `bool` | 不存在的日期回 false 不拋 |
| `GetLeapMonth(int lunarYear)` | `int` | 該年閏月月號（如 2023 回 2）；無閏月回 0 |
| `MinDate` / `MaxDate` | `DateTime` | 1901-02-19 ～ 2101-01-28 |

### LunarDate（readonly struct）

`Year` / `Month`（1~12，閏月與被閏的月同號）/ `Day` / `IsLeapMonth`；`ToString()` 回中文（「2023年閏二月十五」）。

## SolarTerms — 二十四節氣

| 成員 | 回傳 | 說明 |
|------|------|------|
| `GetYearTerms(int year)` | `IReadOnlyList<SolarTermInfo>` | 全年 24 筆，依年內順序（小寒→冬至） |
| `GetExactTime(int year, SolarTerm)` | `DateTime` | 交接時刻（台灣時間，精確到分） |
| `GetCurrentTerm(DateTime)` | `SolarTermInfo` | 指定時刻所屬節氣區段 |
| `TryGetTermOfDay(DateTime, out SolarTermInfo?)` | `bool` | 該日是否為交接日 |
| `MinYear` / `MaxYear` | `int` | 1901 ～ 2100 |

`SolarTermInfo`：`Term` / `ExactTime` / `ChineseName`。`SolarTerm` enum 依年內順序：`XiaoHan(0)` … `DongZhi(23)`。

## GanzhiCalendar — 干支曆

| 成員 | 回傳 | 說明 |
|------|------|------|
| `GetYearGanzhi(DateTime, YearBoundary)` | `GanzhiValue` | 年柱；界線預設立春精確分鐘 |
| `GetMonthGanzhi(DateTime)` | `GanzhiValue` | 月柱；依十二節精確時刻切換，五虎遁月干 |
| `GetDayGanzhi(DateTime)` | `GanzhiValue` | 日柱（日曆日，00:00 換日） |
| `GetHourGanzhi(DateTime)` | `GanzhiValue` | 時柱；23:00 起子時，五鼠遁時干 |
| `GetFourPillars(DateTime, YearBoundary)` | `FourPillars` | 四柱；日柱採八字慣例 23:00 換日 |
| `GetZodiac(DateTime, YearBoundary)` | `Zodiac` | 生肖；界線規則同年柱 |
| `GetChineseName(Zodiac)` | `string` | 生肖中文名 |
| `MinDate` / `MaxDate` | `DateTime` | 1901-02-19 ～ 2100-12-31 |

`GanzhiValue`（readonly struct）：`Stem`（天干）/ `Branch`（地支）/ `ChineseName`（如「甲子」）。
`YearBoundary`：`LiChun`（預設）/ `LunarNewYear`。

## LunarFestivals — 農曆節日

| 成員 | 回傳 | 說明 |
|------|------|------|
| `TryGetFestival(DateTime, out LunarFestival)` | `bool` | 該日是否為節日；閏月不算節日 |
| `GetFestivalDate(int solarYear, LunarFestival)` | `DateTime` | 節日在該國曆年的日期 |

`LunarFestival`：除夕、春節、元宵、端午、七夕、中元、中秋、重陽、臘八、冬至（節氣連動）。

> **臘八特例**：臘月初八距次年初一僅 22~23 天，某些國曆年會出現兩次（取第一次）或零次（拋 `InvalidOperationException`）。例如 2022 年有兩次（1/10、12/30）、2023 年零次。

## 例外行為

- 超出各 API 支援範圍 → `ArgumentOutOfRangeException`（絕不外插）
- `ToSolar` 遇到不存在的農曆日期 → `ArgumentOutOfRangeException`（改用 `TryToSolar` 可避免）
- `GetFestivalDate` 該年無臘八 → `InvalidOperationException`
