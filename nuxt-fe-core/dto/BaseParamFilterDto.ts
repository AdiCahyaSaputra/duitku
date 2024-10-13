export default interface BaseParamFilterDto {
  paginate: boolean;
  pageNumber?: number;
  limit?: number;
  search?: string;
}
