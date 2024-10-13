import { authHeaderAPI } from "@/constant/api";
import type BaseParamFilterDto from "@/dto/BaseParamFilterDto";
import type { BaseResponseFilterDto } from "@/dto/BaseResponseDto";
import type CategoryDto from "@/dto/CategoryDto";
import { createQueryStringParams } from "@/lib/helper";

type TGetSubCategoryFilterResponse = BaseResponseFilterDto & {
  subCategories: CategoryDto[];
}

export const getSubCategories = async (
  params: BaseParamFilterDto & { categoryId: string },
): Promise<TGetSubCategoryFilterResponse | null> => {
  const { getToken } = useUser();
  const token = await getToken();

  if (!token) return null;

  const { data, error } = await useFetch<TGetSubCategoryFilterResponse>(
    `/duit-ku/api/sub-categories?${createQueryStringParams(params)}`,
    {
      headers: authHeaderAPI(token),
    },
  );

  if(error.value) {
    throw error.value.data;
  }

  return data.value;
};
