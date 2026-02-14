import { useInfiniteQuery } from "@tanstack/react-query";
import { departmentsApi } from "../api";
import { useCursorRef } from "@/shared/hooks/use-cursor-ref";

export const useDepartmentRoots = () => {
  const {
    data,
    isPending,
    error,
    refetch,
    fetchNextPage,
    hasNextPage,
    isFetching,
    isFetchingNextPage,
  } = useInfiniteQuery({
    ...departmentsApi.getRootDepartmentsInfinityQueryOptions({}),
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
    cursorRef,
  };
};
