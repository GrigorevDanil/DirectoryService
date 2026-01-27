import { InfiniteData } from "@tanstack/react-query";
import { ApiError } from "./errors";
import { DEFAULT_PAGE_SIZE } from "./constants";

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

export const envelopeInfinityQueryOptions = <T>(request: {
  pageSize?: number;
}) => ({
  initialPageParam: 1,
  getNextPageParam(
    lastPage: Envelope<PaginationEnvelope<T>>,
    _: Envelope<PaginationEnvelope<T>>[],
    lastPageParam: number,
  ) {
    const totalPages = Math.ceil(
      lastPage.result!.totalCount / (request.pageSize ?? DEFAULT_PAGE_SIZE),
    );
    return lastPageParam < totalPages ? lastPageParam + 1 : undefined;
  },
  select(
    data: InfiniteData<Envelope<PaginationEnvelope<T>>>,
  ): Envelope<PaginationEnvelope<T>> {
    const firstPage = data.pages[0];

    return {
      ...firstPage,
      result: firstPage.result && {
        items: data.pages.flatMap((page) => page.result?.items ?? []),
        totalCount: firstPage.result.totalCount,
      },
      isError: data.pages.some((page) => page.isError),
      errorList: data.pages.flatMap((page) => page.errorList || []),
      timeGenerated:
        data.pages.at(-1)?.timeGenerated || firstPage.timeGenerated,
    };
  },
});
