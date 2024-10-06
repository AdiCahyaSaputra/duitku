export default defineEventHandler(async (event) => {
  const { token } = await readBody(event);

  setCookie(event, "token", token, {
    maxAge: 60 * 5,
    httpOnly: true,
  });

  return {
    token,
    message: "cookie created",
  };
});
