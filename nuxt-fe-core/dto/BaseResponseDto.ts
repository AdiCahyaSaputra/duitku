export interface BaseResponseDto {
  message: "Ada yang salah dari sisi server wkwkwk" | string;
  data: any;
}

export interface ApiErrorDto {
  title: string;
  errors?: string | object;
  status: number;
  traceId: string;
}
