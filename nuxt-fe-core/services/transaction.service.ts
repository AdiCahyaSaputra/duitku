import { authHeaderAPI } from "@/constant/api";
import type BaseParamFilterDto from "@/dto/BaseParamFilterDto";
import type {
  ApiErrorDto,
  BaseResponseDto,
  BaseResponseFilterDto,
} from "@/dto/BaseResponseDto";
import type CreateTransactionDto from "@/dto/CreateTransactionDto";
import type FilterTransactionDto from "@/dto/FilterTransactionDto";
import type MostExpensiveTransactionDto from "@/dto/MostExpensiveTransactionDto";
import type TotalExpenseFilterDto from "@/dto/TotalExpenseFilterDto";
import type TransactionWithRelationDto from "@/dto/TransactionWithRelationDto";
import { createQueryStringParams } from "@/lib/helper";

type TGetTransactionFilterResponse = BaseResponseFilterDto & {
  transactions: TransactionWithRelationDto[];
};

type TGetTotalExpenseResponse = BaseResponseDto & {
  totalExpense: number;
};

type TGetMostExpensiveTransactionResponse = BaseResponseDto & {
  transactions: MostExpensiveTransactionDto[];
};

export const getTransactions = async (
  param: BaseParamFilterDto & FilterTransactionDto
): Promise<TGetTransactionFilterResponse | null> => {
  console.log("filter berubah, jalan lagi ", param);

  const { getToken } = useUser();

  const token = await getToken();

  if (!token) return null;

  const { data, error } = await useFetch<TGetTransactionFilterResponse>(
    `/duit-ku/api/transactions?${createQueryStringParams(param)}`,
    {
      method: "get",
      headers: authHeaderAPI(token),
    }
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

export const deleteTransaction = async (id: string) => {
  const { getToken } = useUser();
  const token = await getToken();

  if (!token) {
    throw {
      title: "Login dulu bre..",
      status: 401,
      traceId: "",
    } as ApiErrorDto;
  }

  const { data, error } = await useFetch(`/duit-ku/api/transactions/${id}`, {
    headers: authHeaderAPI(token),
    method: "DELETE",
  });

  if (error.value) {
    throw error.value.data as ApiErrorDto;
  }

  return data.value as BaseResponseDto;
};

export const getTotalExpense = async (params: TotalExpenseFilterDto) => {
  const { getToken } = useUser();
  const token = await getToken();

  if (!token) {
    throw {
      title: "Login dulu bre..",
      status: 401,
      traceId: "",
    } as ApiErrorDto;
  }

  const { data, error } = await useFetch<TGetTotalExpenseResponse>(
    `/duit-ku/api/transactions/total-expense?${createQueryStringParams(
      params
    )}`,
    {
      method: "get",
      headers: authHeaderAPI(token),
    }
  );

  if (error.value) {
    throw error.value.data as ApiErrorDto;
  }

  return data.value;
};

export const getMostExpensiveTransactions = async (
  params: TotalExpenseFilterDto
) => {
  const { getToken } = useUser();
  const token = await getToken();

  if (!token) {
    throw {
      title: "Login dulu bre..",
      status: 401,
      traceId: "",
    } as ApiErrorDto;
  }

  const { data, error } = await useFetch<TGetMostExpensiveTransactionResponse>(
    `/duit-ku/api/transactions/top-three-expensive?${createQueryStringParams(
      params
    )}`,
    {
      method: "get",
      headers: authHeaderAPI(token),
    }
  );

  if (error.value) {
    throw error.value.data as ApiErrorDto;
  }

  return data.value;
};
