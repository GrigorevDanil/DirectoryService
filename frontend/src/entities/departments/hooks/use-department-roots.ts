import { useInfiniteQuery } from "@tanstack/react-query";
import { departmentsApi } from "../api";

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

  return {
    departments: data?.result?.items || [],
    isPending,
    error,
    refetch,
    fetchNextPage,
    hasNextPage,
    isFetching,
    isFetchingNextPage,
  };
};
