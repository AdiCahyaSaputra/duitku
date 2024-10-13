import { authHeaderAPI } from "@/constant/api";
import type BaseParamFilterDto from "@/dto/BaseParamFilterDto";
import type { BaseResponseFilterDto } from "@/dto/BaseResponseDto";
import type CreateTransactionDto from "@/dto/CreateTransactionDto";
import type TransactionWithRelationDto from "@/dto/TransactionWithRelationDto";
import { createQueryStringParams } from "@/lib/helper";

type TGetTransactionFilterResponse = BaseResponseFilterDto & {
  transactions: TransactionWithRelationDto[];
};

export const getTransactions = async (
  param: BaseParamFilterDto,
): Promise<TGetTransactionFilterResponse | null> => {
  const { data: token } = await useFetch("/api/auth/cookie");

  if (!token.value) return null;

  const { data, error } = await useFetch<TGetTransactionFilterResponse>(
    `/duit-ku/api/transactions?${createQueryStringParams(param)}`,
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

export const createTransaction = async (formData: CreateTransactionDto) => {
  console.log(formData);
};
