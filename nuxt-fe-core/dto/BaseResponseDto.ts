export interface BaseResponseDto {
  message: "Ada yang salah dari sisi server wkwkwk" | string;
  [key: string]: any;
}

export interface BaseResponseFilterDto {
  isPreviousExists: boolean;
  isNextExists: boolean;
}

export interface ApiErrorDto {
  title: string;
  errors?: string | object;
  status: number;
  traceId: string;
}
