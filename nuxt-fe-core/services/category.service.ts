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

const api = useApi();

export const getCategories = async (
  params: BaseParamFilterDto,
): Promise<TGetCategoryFilterResponse | null> => {
  const { getToken } = useUser();
  const token = await getToken();

  if (!token) return null;

  const data = await api<TGetCategoryFilterResponse>(
    `/categories?${createQueryStringParams(params)}`,
    {
      headers: authHeaderAPI(token),
    },
  );

  return data;
};

export const createCategory = async (formData: Pick<CategoryDto, "name">) => {
  const { getToken } = useUser();
  const token = await getToken();

  if (!token) return null;

  const data = await api<BaseResponseDto>(`/categories`, {
    headers: authHeaderAPI(token),
    method: "post",
    body: JSON.stringify(formData),
  });

  return data;
};

export const editCategory = async (formData: Pick<CategoryDto, "name">, id: CategoryDto['id']) => {
  const { getToken } = useUser();
  const token = await getToken();

  if (!token) return null;

  const data = await api<BaseResponseDto>(`/categories/${id}`, {
    headers: authHeaderAPI(token),
    method: "put",
    body: JSON.stringify(formData),
  });

  return data;
};

export const deleteCategory = async (id: CategoryDto["id"]) => {
  const { getToken } = useUser();
  const token = await getToken();

  if (!token) return null;

  const data = await api<BaseResponseDto>(`/categories/${id}`, {
    headers: authHeaderAPI(token),
    method: "DELETE",
  });

  return data;
};
