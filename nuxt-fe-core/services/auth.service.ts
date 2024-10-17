import { useApi } from "@/composables/use-api";
import { authHeaderAPI, defaultHeaderAPI } from "@/constant/api";
import type LoginUserDto from "@/dto/LoginUserDto";
import type RegisterUserDto from "@/dto/RegisterUserDto";
import type UserDto from "@/dto/UserDto";

const api = useApi();

export const registerUser = async (formData: RegisterUserDto) => {
  const data = await api("/auth/register", {
    body: JSON.stringify(formData),
    method: "POST",
    headers: defaultHeaderAPI,
  });

  return data as { token: string };
};

export const login = async (formData: LoginUserDto) => {
  const data = await api("/auth/login", {
    body: JSON.stringify(formData),
    method: "POST",
    headers: defaultHeaderAPI,
  });

  return data as { token: string };
};

export const getAuthUser = async (
  token: string | null,
): Promise<UserDto | null> => {
  if (!token) return null;

  try {
    const data = await api("/auth/user", {
      method: "POST",
      headers: authHeaderAPI(token),
    });

    return (data as { user: UserDto }).user;
  } catch (err) {
    throw err;
  }
};
