export default defineNuxtRouteMiddleware(async (to, from) => {
  if(to.fullPath === '/') return navigateTo('/beranda');

  if (import.meta.server) return; // skip middleware on server

  const isGuestRoute = ["/login", "/daftar"].includes(to.fullPath);

  if (import.meta.client) {

    const { getToken, setAuthUser } = useUser();

    const token = await getToken();

    if (!token && !isGuestRoute) {
      return navigateTo("/login");
    }

    if (!!token) {
      setAuthUser(token);

      if (isGuestRoute) {
        return navigateTo("/beranda");
      }
    }
  }
});
