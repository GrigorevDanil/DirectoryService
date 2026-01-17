import { useInfiniteQuery } from "@tanstack/react-query";
import { locationsApi } from "../api";

export const useLocationInfinityList = () => {
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
    ...locationsApi.getLocationsInfinityQueryOptions({}),
  });

  return {
    locations: data?.result?.items || [],
    isPending,
    error,
    refetch,
    fetchNextPage,
    hasNextPage,
    isFetching,
    isFetchingNextPage,
  };
};
