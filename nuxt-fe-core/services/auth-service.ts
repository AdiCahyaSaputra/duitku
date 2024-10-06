import { authHeaderAPI, defaultHeaderAPI } from "@/constant/api";
import type LoginUserDto from "@/dto/LoginUserDto";
import type RegisterUserDto from "@/dto/RegisterUserDto";
import type UserDto from "@/dto/UserDto";

export const registerUser = async (formData: RegisterUserDto) => {
  const { data, error } = await useFetch("/api/auth/register", {
    body: JSON.stringify(formData),
    method: "POST",
    headers: defaultHeaderAPI,
  });

  if (error.value) {
    throw error.value.data;
  }

  return data.value as { token: string };
};

export const login = async (formData: LoginUserDto) => {
  const { data, error } = await useFetch("/api/auth/login", {
    body: JSON.stringify(formData),
    method: "POST",
    headers: defaultHeaderAPI,
  });

  if (error.value) {
    throw error.value.data;
  }

  return data.value as { token: string };
};

export const getAuthUser = async (
  token: string | null,
): Promise<UserDto | null> => {
  if (!token) return null;

  const { data, error } = await useFetch("/api/auth/user", {
    method: "POST",
    headers: authHeaderAPI(token),
  });

  if (error.value) {
    return null;
  }

  return (data.value as { user: UserDto }).user;
};
