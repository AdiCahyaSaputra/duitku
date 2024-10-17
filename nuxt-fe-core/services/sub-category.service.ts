import { authHeaderAPI } from "@/constant/api";
import type BaseParamFilterDto from "@/dto/BaseParamFilterDto";
import type { BaseResponseDto, BaseResponseFilterDto } from "@/dto/BaseResponseDto";
import type SubCategoryDto from "@/dto/SubCategoryDto";
import { createQueryStringParams } from "@/lib/helper";

type TGetSubCategoryFilterResponse = BaseResponseFilterDto & {
  subCategories: SubCategoryDto[];
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

export const createSubCategory = async (formData: Pick<SubCategoryDto, "name"> & { categoryId: string }) => {
  const { getToken } = useUser();
  const token = await getToken();

  if (!token) return null;

  const data = await api<BaseResponseDto>(`/sub-categories`, {
    headers: authHeaderAPI(token),
    method: "post",
    body: JSON.stringify(formData),
  });

  return data;
};

export const editSubCategory = async (formData: Pick<SubCategoryDto, "name"> & { categoryId: string }, id: string) => {
  const { getToken } = useUser();
  const token = await getToken();

  if (!token) return null;

  const data = await api<BaseResponseDto>(`/sub-categories/${id}`, {
    headers: authHeaderAPI(token),
    method: "put",
    body: JSON.stringify(formData),
  });

  return data;
};

export const deleteSubCategory = async (id: SubCategoryDto["id"]) => {
  const { getToken } = useUser();
  const token = await getToken();

  if (!token) return null;

  const data = await api<BaseResponseDto>(`/sub-categories/${id}`, {
    headers: authHeaderAPI(token),
    method: "DELETE",
  });

  return data;
};
