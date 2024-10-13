import { authHeaderAPI } from "@/constant/api";
import type AccountDto from "@/dto/AccountDto";
import type BaseParamFilterDto from "@/dto/BaseParamFilterDto";
import type { BaseResponseFilterDto } from "@/dto/BaseResponseDto";
import { createQueryStringParams } from "@/lib/helper";

type TGetAkunFilterResponse = BaseResponseFilterDto & {
  accounts: AccountDto[];
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

  return data.value;
};
