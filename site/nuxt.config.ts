// https://nuxt.com/docs/api/configuration/nuxt-config
export default defineNuxtConfig({
  compatibilityDate: '2025-01-01',
  devtools: { enabled: true },

  // Nuxt site config (used by sitemap module to prepend absolute URL)
  site: {
    url: 'https://ganzhi.ozakboy.life',
    name: 'Ozakboy.TaiwanKit.Ganzhi.NET',
    description:
      'Lunar calendar, 24 solar terms (minute precision) and Ganzhi four-pillars conversion for .NET, covering 1901-2100. Zero dependencies, works offline.',
  },

  app: {
    baseURL: '/',
    head: {
      title: 'Ganzhi.NET — 農曆/節氣/干支轉換 .NET 套件',
      htmlAttrs: { lang: 'zh-TW' },
      meta: [
        { charset: 'utf-8' },
        { name: 'viewport', content: 'width=device-width, initial-scale=1' },
        {
          name: 'description',
          content:
            'Ozakboy.TaiwanKit.Ganzhi.NET — lunar calendar conversion, minute-precision solar terms and Ganzhi four pillars for .NET (1901-2100). Astronomically computed and verified. Zero dependencies.',
        },
        {
          name: 'keywords',
          content:
            '農曆轉換, 節氣, 干支, 八字, 四柱, Ganzhi, lunar calendar, solar terms, bazi, TaiwanKit, .NET, csharp, NuGet, dotnet',
        },
        { name: 'author', content: 'ozakboy' },

        // Search engine verification
        {
          name: 'google-site-verification',
          content: '7B6Z2O-JFfFD6I0jeayWg1SFeDWKZmf4RwSVGbQHmVk',
        },
        { name: 'msvalidate.01', content: '4928B4223346F74DB53D9754C37164AB' },

        // Open Graph
        { property: 'og:type', content: 'website' },
        { property: 'og:site_name', content: 'Ozakboy.TaiwanKit.Ganzhi.NET' },
        { property: 'og:title', content: 'Ganzhi.NET — Lunar Calendar / Solar Terms / Ganzhi for .NET' },
        {
          property: 'og:description',
          content:
            'Gregorian↔lunar conversion (1901-2100), minute-precision solar terms and Ganzhi four pillars. Astronomically computed, cross-verified, zero dependencies.',
        },
        { property: 'og:url', content: 'https://ganzhi.ozakboy.life/' },
        { property: 'og:image', content: 'https://ganzhi.ozakboy.life/logo.png' },
        { property: 'og:image:alt', content: 'Ozakboy.TaiwanKit logo' },
        { property: 'og:locale', content: 'zh_TW' },
        { property: 'og:locale:alternate', content: 'en_US' },

        // Twitter Card
        { name: 'twitter:card', content: 'summary' },
        { name: 'twitter:title', content: 'Ganzhi.NET — Lunar Calendar / Solar Terms / Ganzhi for .NET' },
        {
          name: 'twitter:description',
          content:
            'Lunar conversion 1901-2100, minute-precision solar terms, Ganzhi four pillars. Zero dependencies.',
        },
        { name: 'twitter:image', content: 'https://ganzhi.ozakboy.life/logo.png' },
      ],
      link: [
        { rel: 'icon', type: 'image/png', href: '/logo.png' },
        { rel: 'apple-touch-icon', href: '/logo.png' },
        { rel: 'canonical', href: 'https://ganzhi.ozakboy.life/' },
      ],
    },
  },

  modules: [
    '@nuxtjs/i18n',
    '@nuxtjs/tailwindcss',
    '@nuxt/content',
    '@nuxtjs/sitemap',
  ],

  // @nuxt/content 讀 site/content/,由 scripts/sync-docs.mjs 自 ../docs/ 同步
  content: {
    build: {
      markdown: {
        toc: { depth: 3 },
        highlight: {
          theme: 'github-light',
        },
      },
    },
  },

  i18n: {
    baseUrl: 'https://ganzhi.ozakboy.life',
    locales: [
      { code: 'zh-TW', name: '繁體中文', file: 'zh-TW.json' },
      { code: 'en', name: 'English', file: 'en.json' },
    ],
    defaultLocale: 'zh-TW',
    strategy: 'prefix_except_default',
    langDir: 'locales/',
    detectBrowserLanguage: {
      useCookie: true,
      cookieKey: 'i18n_redirected',
      redirectOn: 'root',
    },
  },

  sitemap: {
    autoLastmod: true,
  },

  nitro: {
    preset: 'github_pages',
  },

  ssr: true,
})
