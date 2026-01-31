import { useInfiniteQuery } from "@tanstack/react-query";
import { departmentsApi } from "../api";
import { DepartmentId } from "../types";

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

  return {
    departments: data?.result?.items || [],
    isPending,
    error,
    refetch,
    fetchNextPage,
    hasNextPage,
    isFetching,
    isFetchingNextPage,
    isLoading,
  };
};
