export default defineEventHandler(async (event) => {
  const token = getCookie(event, "token");

  return token;
});
