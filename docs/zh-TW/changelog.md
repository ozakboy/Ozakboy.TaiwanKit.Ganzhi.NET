# 更新紀錄

**Ozakboy.TaiwanKit.Ganzhi.NET** 的所有重要變更記錄於此。

## [1.0.0] - 2026-07-05

### 新增功能

- 國曆↔農曆互轉，涵蓋 1901～2100（`LunarConverter`）：閏月明確語意（`IsLeapMonth`）、不存在日期的 `TryToSolar`、`GetLeapMonth` 查詢
- 二十四節氣交接時刻精確到分（台灣時間，`SolarTerms`）：年度清單、精確時刻、所屬節氣區段與交接日查詢
- 干支四柱（`GanzhiCalendar`）：年柱雙界線模式（立春精確分鐘——預設，或正月初一）、月柱依節氣精確時刻切換（五虎遁）、日柱（雙公認錨點互證）、時柱含 23:00 晚子時慣例、`GetFourPillars`
- 生肖查詢（同樣支援雙界線模式）
- 農曆節日（`LunarFestivals`）：除夕、春節、元宵、端午、七夕、中元、中秋、重陽、臘八（一年兩次/零次特例已處理）與冬至
- 不可變值物件附中文名稱：`LunarDate`、`GanzhiValue`、`FourPillars`、`SolarTermInfo`
- 全公開 API 中英雙語 XML 文件註解

### 技術改進

- 目標框架：`netstandard2.0` / `netstandard2.1` / `net8.0` / `net9.0` / `net10.0`；零執行期依賴
- 農曆核心：BCL `ChineseLunisolarCalendar`；節氣表以全精度 VSOP87D 星曆離線產生（IAU1980 章動主項、Espenak–Meeus ΔT）
- 節氣驗證：USNO 2000 年二分二至（4/4）、紫金山天文台來源 2025 年時刻（3/3）、2026 年全 24 節氣多來源交叉（24/24）
- 可重現建置、SourceLink、符號套件（`snupkg`）
