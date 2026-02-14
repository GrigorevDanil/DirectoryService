import { useInfiniteQuery } from "@tanstack/react-query";
import { departmentsApi } from "../api";
import { DepartmentId } from "../types";
import { useCursorRef } from "@/shared/hooks/use-cursor-ref";

export const useDepartmentChildren = (
  parentId: DepartmentId,
  options: { enabled?: boolean },
) => {
  const {
    data,
    isPending,
    error,
    refetch,
    fetchNextPage,
    hasNextPage,
    isFetching,
    isFetchingNextPage,
    isLoading,
  } = useInfiniteQuery({
    ...departmentsApi.getChildrenDepartmentInfinityQueryOptions({ parentId }),
    ...options,
  });

  const cursorRef = useCursorRef({
    hasNextPage,
    fetchNextPage,
    isFetchingNextPage,
  });

  return {
    departments: data?.result?.items || [],
    isPending,
    error,
    refetch,
    hasNextPage,
    isFetching,
    isLoading,
    cursorRef,
  };
};
