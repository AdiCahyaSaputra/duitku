import { getAuthUser } from "@/services/auth-service";

type TSetToken = {
  message: string;
  token: string;
} | null;

export const useUser = () => {
  const router = useRouter();
  const jwtToken = useState<string | null>("token", () => null);

  const isGuestRoute = ["/login", "/daftar"].includes(
    router.currentRoute.value.fullPath,
  );

  const {
    data: user,
    isLoading,
    refetch,
  } = useQuery({
    queryKey: ["get_user", jwtToken],
    queryFn: async () => await getAuthUser(jwtToken.value),
  });

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
      jwtToken.value = createdToken;

      refetch();

      navigateTo("/beranda");
    }
  };

  const revokeAuthToken = async () => {
    const { data } = await useFetch("/api/auth/cookie", {
      method: "DELETE",
    });

    navigateTo("/login");
  };

  onMounted(async () => {
    const token = await getToken();

    if (token) {
      if (isGuestRoute) {
        navigateTo("/beranda");
      }

      jwtToken.value = token;

      refetch();
    } else {
      if (!isGuestRoute) {
        return navigateTo("/login");
      }
    }
  });

  return {
    user,
    refetch,
    isLoading,
    getToken,
    setAuthToken,
    revokeAuthToken,
  };
};
