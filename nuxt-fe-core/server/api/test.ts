export default defineEventHandler((event) => {
  return {
    NUXT_PUBLIC_API: process.env.NUXT_PUBLIC_API,
    runtimeConfig: useRuntimeConfig(),
    headers: getHeaders(event),
  }
})
