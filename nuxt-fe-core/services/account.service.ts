import { authHeaderAPI } from "@/constant/api";
import type AccountDto from "@/dto/AccountDto";
import type BaseParamFilterDto from "@/dto/BaseParamFilterDto";
import type {
  BaseResponseDto,
  BaseResponseFilterDto,
} from "@/dto/BaseResponseDto";
import { createQueryStringParams } from "@/lib/helper";

type TGetAkunFilterResponse = BaseResponseFilterDto & {
  accounts: AccountDto[];
};

type TGetTotalAssetResponse = BaseResponseDto & {
  totalAsset: number;
};

export const getAccounts = async (
  params: BaseParamFilterDto
): Promise<TGetAkunFilterResponse | null> => {
  const { getToken } = useUser();
  const token = await getToken();

  if (!token) return null;

  const { data, error } = await useFetch<TGetAkunFilterResponse>(
    `/duit-ku/api/accounts?${createQueryStringParams(params)}`,
    {
      headers: authHeaderAPI(token),
    }
  );

  if (error.value) {
    throw error.value.data;
  }

  return data.value;
};

export const getTotalAssets = async (params: { accountId?: string }) => {
  const { getToken } = useUser();
  const token = await getToken();

  if (!token) return null;

  const { data, error } = await useFetch<TGetTotalAssetResponse>(
    `/duit-ku/api/accounts/total-asset?${createQueryStringParams(params)}`,
    {
      headers: authHeaderAPI(token),
    }
  );

  if (error.value) {
    throw error.value.data;
  }

  return data.value;
};
