<script setup lang="ts">
// JSON-LD 結構化資料 — 讓 Google 認得這是 SoftwareApplication
const jsonLd = {
  '@context': 'https://schema.org',
  '@type': 'SoftwareApplication',
  name: 'Ozakboy.TaiwanKit.Ganzhi.NET',
  alternateName: ['Ganzhi.NET', 'Lunar Calendar for .NET'],
  applicationCategory: 'DeveloperApplication',
  applicationSubCategory: 'Calendar Library',
  operatingSystem: 'Cross-platform (.NET Standard 2.0+, .NET 8/9/10)',
  description:
    'Lunar calendar, 24 solar terms and Ganzhi (four pillars) conversion for .NET covering 1901-2100. Minute-precision solar term times astronomically computed from the VSOP87D ephemeris and cross-verified. Leap-month-aware lunar conversion, zodiac and lunar festivals. Zero runtime dependencies.',
  url: 'https://ozakboy.github.io/Ozakboy.TaiwanKit.Ganzhi.NET/',
  downloadUrl: 'https://www.nuget.org/packages/Ozakboy.TaiwanKit.Ganzhi.NET/',
  image: 'https://ozakboy.github.io/Ozakboy.TaiwanKit.Ganzhi.NET/logo.png',
  author: {
    '@type': 'Person',
    name: 'ozakboy',
    url: 'https://github.com/ozakboy',
  },
  license: 'https://opensource.org/licenses/MIT',
  programmingLanguage: 'C#',
  offers: {
    '@type': 'Offer',
    price: '0',
    priceCurrency: 'USD',
  },
  softwareVersion: '1.0.1',
}

useHead({
  script: [
    {
      type: 'application/ld+json',
      innerHTML: JSON.stringify(jsonLd),
    },
  ],
})
</script>

<template>
  <NuxtLayout>
    <NuxtPage />
  </NuxtLayout>
</template>
