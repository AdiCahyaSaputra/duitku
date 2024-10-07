// https://nuxt.com/docs/api/configuration/nuxt-config
export default defineNuxtConfig({
  compatibilityDate: "2024-04-03",
  devtools: { enabled: true },
  modules: [
    "@nuxtjs/tailwindcss",
    "shadcn-nuxt",
    "@hebilicious/vue-query-nuxt",
    "@nuxt/icon",
    "@nuxt/fonts",
  ],
  fonts: {
    google: {
      families: {
        Inter: {
          wght: [400, 500, 700],
          subsets: ["latin"],
        },
      },
    },
  },
  shadcn: {
    componentDir: "./components/ui",
  },
  typescript: {
    typeCheck: true,
  },
  routeRules: {
    "/duit-ku/api/**": {
      cors: true,
      proxy: "http://localhost:5143/api/**",
    },
  },
  icon: {
    serverBundle: {
      collections: ["lucide"], // <!--- this
    },
  },
});
