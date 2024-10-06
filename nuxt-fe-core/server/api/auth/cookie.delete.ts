export default defineEventHandler(async (event) => {
  deleteCookie(event, "token");

  return {
    message: "cookie deleted",
  };
});
