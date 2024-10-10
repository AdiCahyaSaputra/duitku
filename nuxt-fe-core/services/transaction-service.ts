import { authHeaderAPI } from "@/constant/api";
import type TransactionWithRelation from "@/dto/TransactionWithRelation";

export const getTransactions = async () => {
  const { data: token } = await useFetch("/api/auth/cookie");

  if (!token.value) return [];

  const { data, error } = await useFetch(
    "/duit-ku/api/transactions/with?category=true&subcategory=true&account=true",
    {
      method: "get",
      headers: authHeaderAPI(token.value),
    },
  );

  if(error.value) {
    throw error.value.data;
  }

  return data.value as TransactionWithRelation[];
};
