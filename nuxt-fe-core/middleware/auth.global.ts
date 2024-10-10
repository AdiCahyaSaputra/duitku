export default defineNuxtRouteMiddleware(async (to, from) => {
  const isGuestRoute = ["/login", "/daftar"].includes(to.fullPath);

  const token = await $fetch("/api/auth/cookie", {
    method: "GET",
  });

  if (!token) {
    if (!isGuestRoute) {
      return navigateTo("/login");
    }
  } else {
    if (isGuestRoute) {
      return navigateTo("/beranda");
    }
  }
});
