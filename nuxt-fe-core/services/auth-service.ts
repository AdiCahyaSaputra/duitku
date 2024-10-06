import { authHeaderAPI, defaultHeaderAPI } from "@/constant/api";
import type LoginUserDto from "@/dto/LoginUserDto";
import type RegisterUserDto from "@/dto/RegisterUserDto";
import type UserDto from "@/dto/UserDto";

export const registerUser = async (formData: RegisterUserDto) => {
  const { data, error } = await useFetch("/duit-ku/api/auth/register", {
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
  const { data, error } = await useFetch("/duit-ku/api/auth/login", {
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

  try {
    const data = await $fetch("/duit-ku/api/auth/user", {
      method: "POST",
      headers: authHeaderAPI(token),
    });

    return (data as { user: UserDto }).user;
  } catch (err) {
    return null;
  }
};
