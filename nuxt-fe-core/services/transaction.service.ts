import { authHeaderAPI } from "@/constant/api";
import type BaseParamFilterDto from "@/dto/BaseParamFilterDto";
import type {
  ApiErrorDto,
  BaseResponseDto,
  BaseResponseFilterDto,
} from "@/dto/BaseResponseDto";
import type CreateTransactionDto from "@/dto/CreateTransactionDto";
import type TransactionWithRelationDto from "@/dto/TransactionWithRelationDto";
import { createQueryStringParams } from "@/lib/helper";

type TGetTransactionFilterResponse = BaseResponseFilterDto & {
  transactions: TransactionWithRelationDto[];
};

export const getTransactions = async (
  param: BaseParamFilterDto,
): Promise<TGetTransactionFilterResponse | null> => {
  console.log('Get transaction');

  const { getToken } = useUser();

  const token = await getToken();

  if (!token) return null;

  const { data, error } = await useFetch<TGetTransactionFilterResponse>(
    `/duit-ku/api/transactions?${createQueryStringParams(param)}`,
    {
      method: "get",
      headers: authHeaderAPI(token),
    },
  );

  if (error.value) {
    throw error.value.data;
  }

  return data.value;
};

export const createTransaction = async (formData: CreateTransactionDto) => {
  const { getToken } = useUser();
  const token = await getToken();

  if (!token) {
    throw {
      title: "Login dulu bre..",
      status: 401,
      traceId: "",
    } as ApiErrorDto;
  }

  const { data, error } = await useFetch("/duit-ku/api/transactions", {
    headers: authHeaderAPI(token),
    body: JSON.stringify(formData),
    method: "POST",
  });

  if (error.value) {
    throw error.value.data as ApiErrorDto;
  }

  return data.value as BaseResponseDto;
};
