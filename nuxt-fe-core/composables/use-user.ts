import type UserDto from "@/dto/UserDto";
import { userStore } from "@/store/user.store";

type TSetToken = {
  message: string;
  token: string;
} | null;

export const useUser = () => {
  const user = ref<UserDto | null>(null);

  const userStoreInstance = userStore();

  const getToken = async () => {
    const token = await $fetch('/api/auth/cookie');

    return token || null;
  }

  const setAuthToken = async (token: string) => {
    const { data } = await useFetch("/api/auth/cookie", {
      method: "POST",
      body: JSON.stringify({ token }),
    });

    const createdToken = (data.value as TSetToken)?.token;

    if (createdToken) {
      userStoreInstance.setUser();

      navigateTo("/beranda");
    }
  };

  const revokeAuthToken = async () => {
    await $fetch("/api/auth/cookie", {
      method: "DELETE",
    });

    userStoreInstance.revokeUser();

    navigateTo("/login");
  };

  onMounted(() => {
    userStoreInstance.getUser();
  });

  userStoreInstance.$subscribe((_, state) => {
    user.value = state.user;
  });

  return {
    user,
    getToken,
    setAuthToken,
    revokeAuthToken,
  };
};
