type TSetToken = {
  message: string;
  token: string;
} | null;

export const useUser = () => {
  const { data: user } = useFetch('/api/auth/user');
  const { data: token } = useFetch('/api/auth/cookie');

  const setAuthToken = async (token: string) => {
    const { data } = await useFetch("/api/auth/cookie", {
      method: "POST",
      body: JSON.stringify({ token }),
    });

    const createdToken = (data.value as TSetToken)?.token;

    if (createdToken) {
      navigateTo("/beranda");
    }
  };

  const revokeAuthToken = async () => {
    await $fetch("/api/auth/cookie", {
      method: "DELETE",
    });

    navigateTo("/login");
  };

  return {
    user: user.value,
    token: token.value,
    setAuthToken,
    revokeAuthToken
  };
};
