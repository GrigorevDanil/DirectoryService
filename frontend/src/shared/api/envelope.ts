export interface PaginationEnvelope<TResult = unknown> {
  items: TResult[];
  totalCount: number;
}

export interface Envelope<TResult = unknown> {
  result: TResult | null;
  errorList: ApiError[];
  timeGenerated: string;
  isError: boolean;
}

export interface ApiError {
  code: string;
  message: string;
  type: ApiErrorType;
  invalidField?: string | null;
}

export type ApiErrorType =
  | "VALIDATION"
  | "NOT_FOUND"
  | "FAILURE"
  | "CONFLICT"
  | "FORBIDDEN";
