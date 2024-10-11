import { getAuthUser } from "@/services/auth-service";
import { userStore } from "@/store/user.store";

type TSetToken = {
  message: string;
  token: string;
} | null;

export const useUser = () => {
  const { user, setUser } = userStore();

  const getToken = async () => {
    // Kalo pake useFetch sering undefined pas di client nya
    const data = await $fetch("/api/auth/cookie", {
      method: "GET",
    });

    return data;
  };

  const setAuthToken = async (token: string) => {
    const { data } = await useFetch("/api/auth/cookie", {
      method: "POST",
      body: JSON.stringify({ token }),
    });

    const createdToken = (data.value as TSetToken)?.token;

    if (createdToken) {
      const user = await getAuthUser(createdToken);

      setUser(user);

      navigateTo("/beranda");
    }
  };

  const revokeAuthToken = async () => {
    const { data } = await useFetch("/api/auth/cookie", {
      method: "DELETE",
    });

    setUser(null);

    navigateTo("/login");
  };

  const setAuthUser = async (token: string) => {
    const user = await getAuthUser(token);
    
    setUser(user);
  }

  return {
    user,
    setAuthUser,
    getToken,
    setAuthToken,
    revokeAuthToken,
  };
};
