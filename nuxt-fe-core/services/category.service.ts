import { authHeaderAPI } from "@/constant/api";
import type BaseParamFilterDto from "@/dto/BaseParamFilterDto";
import type { BaseResponseFilterDto } from "@/dto/BaseResponseDto";
import type CategoryDto from "@/dto/CategoryDto";
import { createQueryStringParams } from "@/lib/helper";

type TGetCategoryFilterResponse = BaseResponseFilterDto & {
  categories: CategoryDto[];
}

export const getCategories = async (
  params: BaseParamFilterDto,
): Promise<TGetCategoryFilterResponse | null> => {
  const { getToken } = useUser();
  const token = await getToken();

  if (!token) return null;

  const { data, error } = await useFetch<TGetCategoryFilterResponse>(
    `/duit-ku/api/categories?${createQueryStringParams(params)}`,
    {
      headers: authHeaderAPI(token),
    },
  );

  if(error.value) {
    throw error.value.data;
  }

  return data.value;
};
