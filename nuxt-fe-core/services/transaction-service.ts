import { authHeaderAPI } from "@/constant/api";
import type TransactionWithRelation from "@/dto/TransactionWithRelation";
import { createQueryStringParams } from "@/lib/helper";

type TGetTransactionResponse = {
  isPreviousExists: boolean;
  isNextExists: boolean;
  transactions: TransactionWithRelation[];
};

export const getTransactions = async (
  pageNumber: number = 1,
): Promise<TGetTransactionResponse | null> => {
  const { data: token } = await useFetch("/api/auth/cookie");

  if (!token.value) return null;

  const params = {
    category: true,
    subcategory: true,
    account: true,
    pageNumber,
  };

  const { data, error } = await useFetch<TGetTransactionResponse>(
    `/duit-ku/api/transactions/with?${createQueryStringParams(params)}`,
    {
      method: "get",
      headers: authHeaderAPI(token.value),
    },
  );

  if (error.value) {
    throw error.value.data;
  }

  return data.value;
};

export const createTransaction = async () => { };
