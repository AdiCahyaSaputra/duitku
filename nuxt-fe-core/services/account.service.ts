import { authHeaderAPI } from "@/constant/api";
import type AccountDto from "@/dto/AccountDto";
import type BaseParamFilterDto from "@/dto/BaseParamFilterDto";
import type {
  BaseResponseDto,
  BaseResponseFilterDto,
} from "@/dto/BaseResponseDto";
import type TotalIncomeDto from "@/dto/TotalIncomeDto";
import { createQueryStringParams } from "@/lib/helper";
import { getAuthToken } from "./auth.service";

type TGetAkunFilterResponse = BaseResponseFilterDto & {
  accounts: AccountDto[];
};

type TGetTotalAssetResponse = BaseResponseDto & {
  totalIncome: TotalIncomeDto;
};

const api = useApi();
const token = await getAuthToken();

export const getAccounts = async (
  params: BaseParamFilterDto,
): Promise<TGetAkunFilterResponse | null> => {
  if (!token) return null;

  const data = await api<TGetAkunFilterResponse>(
    `/accounts?${createQueryStringParams(params)}`,
    {
      headers: authHeaderAPI(token),
    },
  );

  return data;
};

export const getTotalAssets = async (
  params: BaseParamFilterDto & { accountId?: string },
) => {
  if (!token) return null;

  const data = await api<TGetTotalAssetResponse>(
    `/accounts/total-asset?${createQueryStringParams(params)}`,
    {
      headers: authHeaderAPI(token),
    },
  );

  return data;
};

export const topUpBalance = async (
  formData: Pick<AccountDto, "id" | "balance">,
  id: string
) => {
  if (!token) return null;

  const data = await api<BaseResponseDto>(`/accounts/top-up/${id}`, {
    method: "post",
    headers: authHeaderAPI(token),
    body: JSON.stringify(formData),
  });

  return data;
};

export const createAccount = async (
  formData: Pick<AccountDto, "name" | "balance">,
) => {
  if (!token) return null;

  const data = await api<BaseResponseDto>(`/accounts`, {
    method: "post",
    headers: authHeaderAPI(token),
    body: JSON.stringify(formData),
  });

  return data;
};

export const editAccount = async (
  formData: Pick<AccountDto, "name" | "balance">,
  id: string,
) => {
  if (!token) return null;

  const data = await api<BaseResponseDto>(`/accounts/${id}`, {
    method: "put",
    headers: authHeaderAPI(token),
    body: JSON.stringify(formData),
  });

  return data;
};

export const deleteAccount = async (id: string) => {
  if (!token) return null;

  const data = await api<BaseResponseDto>(`/accounts/${id}`, {
    method: "DELETE",
    headers: authHeaderAPI(token),
  });

  return data;
};
