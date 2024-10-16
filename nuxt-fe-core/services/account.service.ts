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

export const getAccounts = async (
  params: BaseParamFilterDto,
): Promise<TGetAkunFilterResponse | null> => {
  const { getToken } = useUser();
  const token = await getToken();

  if (!token) return null;

  const { data, error } = await useFetch<TGetAkunFilterResponse>(
    `/duit-ku/api/accounts?${createQueryStringParams(params)}`,
    {
      headers: authHeaderAPI(token),
    },
  );

  if (error.value) {
    throw error.value.data;
  }

  return data.value;
};

export const getTotalAssets = async (
  params: BaseParamFilterDto & { accountId?: string },
) => {
  const { getToken } = useUser();
  const token = await getToken();

  if (!token) return null;

  const { data, error } = await useFetch<TGetTotalAssetResponse>(
    `/duit-ku/api/accounts/total-asset?${createQueryStringParams(params)}`,
    {
      headers: authHeaderAPI(token),
    },
  );

  if (error.value) {
    throw error.value.data;
  }

  return data.value;
};

export const createAccount = async (
  formData: Pick<AccountDto, "name" | "balance">,
) => {
  const { getToken } = useUser();
  const token = await getToken();

  if (!token) return null;

  const { data, error } = await useFetch<BaseResponseDto>(
    `/duit-ku/api/accounts`,
    {
      method: "post",
      headers: authHeaderAPI(token),
      body: JSON.stringify(formData),
    },
  );

  if (error.value) {
    throw error.value.data;
  }

  return data.value;
};

export const editAccount = async (
  formData: Pick<AccountDto, "name" | "balance">, id: string
) => {
  const { getToken } = useUser();
  const token = await getToken();

  if (!token) return null;

  const { data, error } = await useFetch<BaseResponseDto>(
    `/duit-ku/api/accounts/${id}`,
    {
      method: "put",
      headers: authHeaderAPI(token),
      body: JSON.stringify(formData),
    },
  );

  if (error.value) {
    throw error.value.data;
  }

  return data.value;
};

export const deleteAccount = async (id: string) => {
  const { getToken } = useUser();
  const token = await getToken();

  if (!token) return null;

  const { data, error } = await useFetch<BaseResponseDto>(
    `/duit-ku/api/accounts/${id}`,
    {
      method: "delete",
      headers: authHeaderAPI(token),
    },
  );

  if (error.value) {
    throw error.value.data;
  }

  return data.value;
}
