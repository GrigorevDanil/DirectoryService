import { useInfiniteQuery } from "@tanstack/react-query";
import { locationsApi } from "../api";
import {
  useLocationActive,
  useLocationPageSize,
  useLocationSearch,
  useLocationSelectedDepartments,
  useLocationSortBy,
  useLocationSortDirection,
} from "../model/location-list-store";
import { useDebounce } from "use-debounce";

export const useLocationInfinityList = () => {
  const search = useLocationSearch();
  const [debouncedSearch] = useDebounce(search, 300);
  const activeState = useLocationActive();
  const sortBy = useLocationSortBy();
  const sortDirection = useLocationSortDirection();
  const pageSize = useLocationPageSize();
  const selectedDepartments = useLocationSelectedDepartments();

  const isActive = activeState === "all" ? undefined : activeState === "active";

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
    ...locationsApi.getLocationsInfinityQueryOptions({
      search: debouncedSearch,
      isActive,
      sortBy,
      sortDirection,
      pageSize,
      departmentIds: selectedDepartments.map((x) => x.id),
    }),
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
