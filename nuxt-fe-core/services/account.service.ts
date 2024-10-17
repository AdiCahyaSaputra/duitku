import { authHeaderAPI } from "@/constant/api";
import type AccountDto from "@/dto/AccountDto";
import type BaseParamFilterDto from "@/dto/BaseParamFilterDto";
import type {
  BaseResponseDto,
  BaseResponseFilterDto,
} from "@/dto/BaseResponseDto";
import type TotalIncomeDto from "@/dto/TotalIncomeDto";
import { createQueryStringParams } from "@/lib/helper";

type TGetAkunFilterResponse = BaseResponseFilterDto & {
  accounts: AccountDto[];
};

type TGetTotalAssetResponse = BaseResponseDto & {
  totalIncome: TotalIncomeDto;
};

const api = useApi();

export const getAccounts = async (
  params: BaseParamFilterDto,
): Promise<TGetAkunFilterResponse | null> => {
  const { getToken } = useUser();
  const token = await getToken();

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
  const { getToken } = useUser();
  const token = await getToken();

  if (!token) return null;

  const data = await api<TGetTotalAssetResponse>(
    `/accounts/total-asset?${createQueryStringParams(params)}`,
    {
      headers: authHeaderAPI(token),
    },
  );

  return data;
};

export const createAccount = async (
  formData: Pick<AccountDto, "name" | "balance">,
) => {
  const { getToken } = useUser();
  const token = await getToken();

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
  const { getToken } = useUser();
  const token = await getToken();

  if (!token) return null;

  const data = await api<BaseResponseDto>(`/accounts/${id}`, {
    method: "put",
    headers: authHeaderAPI(token),
    body: JSON.stringify(formData),
  });

  return data;
};

export const deleteAccount = async (id: string) => {
  const { getToken } = useUser();
  const token = await getToken();

  if (!token) return null;

  const data = await api<BaseResponseDto>(`/accounts/${id}`, {
    method: "DELETE",
    headers: authHeaderAPI(token),
  });

  return data;
};
