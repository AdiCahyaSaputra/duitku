import { apiBaseUrl, defaultHeaderAPI } from "@/constant/api";
import type LoginUserDto from "@/dto/LoginUserDto";
import type RegisterUserDto from "@/dto/RegisterUserDto";

export const registerUser = async (formData: RegisterUserDto) => {
  const { data, error } = await useFetch("/api/auth/register", {
    body: JSON.stringify(formData),
    method: 'POST',
    headers: defaultHeaderAPI
  });

  if(error.value) {
    throw new Error(error.value.message);
  }

  return data.value;
};

export const login = async (formData: LoginUserDto) => {
  const { data, error } = await useFetch("/api/auth/login", {
    body: JSON.stringify(formData),
    method: 'POST',
    headers: defaultHeaderAPI
  });

  if(error.value) {
    throw new Error(error.value.message);
  }

  return data.value;
};
