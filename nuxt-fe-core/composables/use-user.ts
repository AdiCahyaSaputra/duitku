import { getAuthUser } from "@/services/auth-service";

export const useUser = () => {
  const queryClient = useQueryClient();

  const jwtToken = useState<null | string>("token", () => null);

  const setAuthToken = async (token: string) => {
    jwtToken.value = token;
    localStorage.setItem("token", token);

    await queryClient.invalidateQueries({
      queryKey: ["get_user"],
    });

    navigateTo("/beranda");
  };

  const revokeAuthToken = () => {
    jwtToken.value = null;

    localStorage.removeItem("token");

    queryClient.setQueryData(["get_user"], null);

    navigateTo("/login");
  };

  const {
    data: user,
    error,
    isLoading,
  } = useQuery({
    queryKey: ["get_user", jwtToken],
    queryFn: () => getAuthUser(jwtToken.value!),
    suspense: true,
  });

  onMounted(async () => {
    const token = localStorage.getItem("token");

    if (token) {
      jwtToken.value = token;
    }
  });

  watch(
    () => jwtToken.value,
    async (newToken) => {
      if (newToken) {
        queryClient.invalidateQueries({
          queryKey: ["get_user"],
        });
      }
    },
  );

  return {
    user,
    isLoading,
    error,
    setAuthToken,
    revokeAuthToken,
  };
};
