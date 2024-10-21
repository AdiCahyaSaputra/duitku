import { getAuthUser } from "~/services/auth.service";

export default defineEventHandler(async (event) => {
  const token = getCookie(event, "token");

  if(token) {
    const user = await getAuthUser(token);

    return user;
  }

  return null;
});
