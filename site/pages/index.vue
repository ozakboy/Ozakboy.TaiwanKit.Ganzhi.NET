<script setup>
useHead({ title: 'Ganzhi.NET — 農曆/節氣/干支轉換 .NET 套件' })
const localePath = useLocalePath()

const features = [
  { key: 'lunar', icon: '🌙' },
  { key: 'terms', icon: '☀️' },
  { key: 'ganzhi', icon: '🀄' },
  { key: 'festivals', icon: '🏮' },
]
</script>

<template>
  <div>
    <!-- Hero -->
    <section class="bg-gradient-to-br from-brand-900 via-brand-700 to-brand-500 text-white">
      <div class="max-w-6xl mx-auto px-4 py-20 sm:py-28 text-center">
        <h1 class="text-4xl sm:text-6xl font-extrabold tracking-tight">
          {{ $t('home.hero.title') }}
        </h1>
        <p class="mt-6 text-lg sm:text-xl text-brand-50 max-w-3xl mx-auto leading-relaxed">
          {{ $t('home.hero.subtitle') }}
        </p>
        <div class="mt-10 flex justify-center gap-3 flex-wrap">
          <NuxtLink
            :to="localePath('/docs/getting-started')"
            class="bg-white text-brand-900 px-6 py-3 rounded font-semibold hover:bg-brand-50 transition"
          >
            {{ $t('home.hero.install') }}
          </NuxtLink>
          <a
            href="https://github.com/ozakboy/Ozakboy.TaiwanKit.Ganzhi.NET"
            target="_blank"
            rel="noopener"
            class="bg-brand-900/40 text-white px-6 py-3 rounded font-semibold hover:bg-brand-900/60 border border-white/30 transition"
          >
            {{ $t('home.hero.github') }}
          </a>
        </div>
      </div>
    </section>

    <!-- Features -->
    <section class="max-w-6xl mx-auto px-4 py-16 sm:py-20">
      <h2 class="text-3xl sm:text-4xl font-bold text-center mb-12">
        {{ $t('home.features.title') }}
      </h2>
      <div class="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-4 gap-6">
        <div
          v-for="f in features"
          :key="f.key"
          class="bg-white border border-slate-200 rounded-lg p-6 hover:shadow-lg hover:border-brand-500 transition"
        >
          <div class="text-3xl mb-3">{{ f.icon }}</div>
          <h3 class="font-bold text-lg mb-2">
            {{ $t(`home.features.items.${f.key}.title`) }}
          </h3>
          <p class="text-slate-600 text-sm leading-relaxed">
            {{ $t(`home.features.items.${f.key}.desc`) }}
          </p>
        </div>
      </div>
    </section>

    <!-- Quick Start -->
    <section class="bg-white py-16 sm:py-20 border-t border-slate-200">
      <div class="max-w-4xl mx-auto px-4">
        <h2 class="text-3xl sm:text-4xl font-bold text-center mb-12">
          {{ $t('home.quickstart.title') }}
        </h2>
        <div class="space-y-8">
          <div>
            <h3 class="font-semibold text-lg mb-3">{{ $t('home.quickstart.step1') }}</h3>
            <pre class="bg-slate-900 text-slate-100 rounded p-4 text-sm overflow-x-auto"><code>dotnet add package Ozakboy.TaiwanKit.Ganzhi.NET</code></pre>
          </div>
          <div>
            <h3 class="font-semibold text-lg mb-3">{{ $t('home.quickstart.step2') }}</h3>
            <pre class="bg-slate-900 text-slate-100 rounded p-4 text-sm overflow-x-auto"><code>using Ozakboy.TaiwanKit.Ganzhi.NET;

// 國曆 ↔ 農曆
LunarDate lunar = LunarConverter.ToLunar(new DateTime(2026, 2, 17));
// → "2026年正月初一"

// 節氣（精確到分，台灣時間）
SolarTerms.GetExactTime(2026, SolarTerm.LiChun);   // 2026-02-04 04:02</code></pre>
          </div>
          <div>
            <h3 class="font-semibold text-lg mb-3">{{ $t('home.quickstart.step3') }}</h3>
            <pre class="bg-slate-900 text-slate-100 rounded p-4 text-sm overflow-x-auto"><code>// 干支四柱（年柱可選立春/初一界線）
FourPillars p = GanzhiCalendar.GetFourPillars(new DateTime(2026, 2, 20, 10, 0, 0));
// p.Year.ChineseName → "丙午"、p.Month.ChineseName → "庚寅"

// 生肖與農曆節日
GanzhiCalendar.GetZodiac(new DateTime(2026, 6, 1));            // Horse（馬）
LunarFestivals.GetFestivalDate(2026, LunarFestival.MidAutumn); // 2026-09-25</code></pre>
          </div>
        </div>
      </div>
    </section>
  </div>
</template>
