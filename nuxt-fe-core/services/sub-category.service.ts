import { authHeaderAPI } from "@/constant/api";
import type BaseParamFilterDto from "@/dto/BaseParamFilterDto";
import type { BaseResponseFilterDto } from "@/dto/BaseResponseDto";
import type CategoryDto from "@/dto/CategoryDto";
import { createQueryStringParams } from "@/lib/helper";

type TGetSubCategoryFilterResponse = BaseResponseFilterDto & {
  subCategories: CategoryDto[];
};

const api = useApi();

export const getSubCategories = async (
  params: BaseParamFilterDto & { categoryId: string },
): Promise<TGetSubCategoryFilterResponse | null> => {
  const { getToken } = useUser();
  const token = await getToken();

  if (!token) return null;

  const data = await api<TGetSubCategoryFilterResponse>(
    `/sub-categories?${createQueryStringParams(params)}`,
    {
      headers: authHeaderAPI(token),
    },
  );

  return data;
};
