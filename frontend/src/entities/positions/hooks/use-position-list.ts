import { useInfiniteQuery } from "@tanstack/react-query";
import { useDebounce } from "use-debounce";
import {
  usePositionActive,
  usePositionPageSize,
  usePositionSearch,
  usePositionSelectedDepartments,
  usePositionSortBy,
  usePositionSortDirection,
} from "../model/position-list-store";
import { positionsApi } from "../api";

export const usePositionList = () => {
  const search = usePositionSearch();
  const [debouncedSearch] = useDebounce(search, 300);
  const activeState = usePositionActive();
  const sortBy = usePositionSortBy();
  const sortDirection = usePositionSortDirection();
  const pageSize = usePositionPageSize();
  const selectedDepartments = usePositionSelectedDepartments();

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
    ...positionsApi.getPositionsInfinityQueryOptions({
      search: debouncedSearch,
      isActive,
      sortBy,
      sortDirection,
      pageSize,
      departmentIds: selectedDepartments.map((x) => x.id),
    }),
  });

  return {
    positions: data?.result?.items || [],
    isPending,
    error,
    refetch,
    fetchNextPage,
    hasNextPage,
    isFetching,
    isFetchingNextPage,
  };
};
