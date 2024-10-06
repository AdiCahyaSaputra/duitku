// https://nuxt.com/docs/api/configuration/nuxt-config
export default defineNuxtConfig({
  compatibilityDate: "2024-04-03",
  devtools: { enabled: true },
  modules: ["@nuxtjs/tailwindcss", "shadcn-nuxt", "@hebilicious/vue-query-nuxt"],
  shadcn: {
    componentDir: "./components/ui",
  },
  typescript: {
    typeCheck: true
  },
  routeRules: {
    "/duit-ku/api/**": {
      cors: true,
      proxy: "http://localhost:5143/api/**"
    }
  }
});
