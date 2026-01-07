import { ApiError } from "./errors";

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
