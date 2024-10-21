import { authHeaderAPI } from "@/constant/api";
import type BaseParamFilterDto from "@/dto/BaseParamFilterDto";
import type {
  BaseResponseDto,
  BaseResponseFilterDto,
} from "@/dto/BaseResponseDto";
import type CategoryDto from "@/dto/CategoryDto";
import { createQueryStringParams } from "@/lib/helper";

type TGetCategoryFilterResponse = BaseResponseFilterDto & {
  categories: CategoryDto[];
};

type TGetSingleCategory = BaseResponseDto & {
  category: CategoryDto
}

const api = useApi();

export const getCategories = async (
  params: BaseParamFilterDto,
): Promise<TGetCategoryFilterResponse | null> => {
  const { token } = useUser();

  if (!token) return null;

  const data = await api<TGetCategoryFilterResponse>(
    `/categories?${createQueryStringParams(params)}`,
    {
      headers: authHeaderAPI(token),
    },
  );

  return data;
};

export const getCategoryById = async (id: string) => {
  const { token } = useUser();

  if (!token) return null;

  const data = await api<TGetSingleCategory>(
    `/categories/${id}`,
    {
      headers: authHeaderAPI(token),
      method: 'get'
    },
  );

  return data;
}

export const createCategory = async (formData: Pick<CategoryDto, "name">) => {
  const { token } = useUser();

  if (!token) return null;

  const data = await api<BaseResponseDto>(`/categories`, {
    headers: authHeaderAPI(token),
    method: "post",
    body: JSON.stringify(formData),
  });

  return data;
};

export const editCategory = async (formData: Pick<CategoryDto, "name">, id: CategoryDto['id']) => {
  const { token } = useUser();

  if (!token) return null;

  const data = await api<BaseResponseDto>(`/categories/${id}`, {
    headers: authHeaderAPI(token),
    method: "put",
    body: JSON.stringify(formData),
  });

  return data;
};

export const deleteCategory = async (id: CategoryDto["id"]) => {
  const { token } = useUser();

  if (!token) return null;

  const data = await api<BaseResponseDto>(`/categories/${id}`, {
    headers: authHeaderAPI(token),
    method: "DELETE",
  });

  return data;
};
