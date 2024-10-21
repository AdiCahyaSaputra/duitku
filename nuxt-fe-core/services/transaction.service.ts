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

const api = useApi();

export const getTransactions = async (
  param: BaseParamFilterDto & FilterTransactionDto,
): Promise<TGetTransactionFilterResponse | null> => {
  const { token } = useUser();

  if (!token) return null;

  const data = await api<TGetTransactionFilterResponse>(
    `/transactions?${createQueryStringParams(param)}`,
    {
      method: "get",
      headers: authHeaderAPI(token),
    },
  );

  return data;
};

export const createTransaction = async (formData: CreateTransactionDto) => {
  const { token } = useUser();

  if (!token) {
    throw {
      title: "Login dulu bre..",
      status: 401,
      traceId: "",
    } as ApiErrorDto;
  }

  const data = await api<BaseResponseDto>("/transactions", {
    headers: authHeaderAPI(token),
    body: JSON.stringify(formData),
    method: "POST",
  });

  return data;
};

export const deleteTransaction = async (id: string) => {
  const { token } = useUser();

  if (!token) {
    throw {
      title: "Login dulu bre..",
      status: 401,
      traceId: "",
    } as ApiErrorDto;
  }

  const data = await api<BaseResponseDto>(`/transactions/${id}`, {
    headers: authHeaderAPI(token),
    method: "DELETE",
  });

  return data;
};

export const getTotalExpense = async (params: TotalExpenseFilterDto) => {
  const { token } = useUser();

  if (!token) {
    throw {
      title: "Login dulu bre..",
      status: 401,
      traceId: "",
    } as ApiErrorDto;
  }

  const data = await api<TGetTotalExpenseResponse>(
    `/transactions/total-expense?${createQueryStringParams(params)}`,
    {
      method: "get",
      headers: authHeaderAPI(token),
    },
  );

  return data;
};

export const getMostExpensiveTransactions = async (
  params: TotalExpenseFilterDto,
) => {
  const { token } = useUser();

  if (!token) {
    throw {
      title: "Login dulu bre..",
      status: 401,
      traceId: "",
    } as ApiErrorDto;
  }

  const data = await api<TGetMostExpensiveTransactionResponse>(
    `/transactions/top-three-expensive?${createQueryStringParams(params)}`,
    {
      method: "get",
      headers: authHeaderAPI(token),
    },
  );

  return data;
};
