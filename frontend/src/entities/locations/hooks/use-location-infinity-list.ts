import { useInfiniteQuery } from "@tanstack/react-query";
import { GetLocationsRequest, locationsApi } from "../api";
import {
  LocationListId,
  useLocationActive,
  useLocationPageSize,
  useLocationSearch,
  useLocationSelectedDepartments,
  useLocationSortBy,
  useLocationSortDirection,
} from "../model/location-list-store";
import { useDebounce } from "use-debounce";
import { useCursorRef } from "@/shared/hooks/use-cursor-ref";

interface UseLocationInfinityListProps {
  stateId?: LocationListId;
  request?: GetLocationsRequest;
}

export const useLocationInfinityList = ({
  request,
  stateId,
}: UseLocationInfinityListProps) => {
  const search = useLocationSearch(stateId);
  const [debouncedSearch] = useDebounce(search, 300);
  const activeState = useLocationActive(stateId);
  const sortBy = useLocationSortBy(stateId);
  const sortDirection = useLocationSortDirection(stateId);
  const pageSize = useLocationPageSize(stateId);
  const departments = useLocationSelectedDepartments(stateId);

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
      departmentIds: departments.map((dep) => dep.id),
      ...request,
    }),
  });

  const cursorRef = useCursorRef({
    hasNextPage,
    fetchNextPage,
    isFetchingNextPage,
  });

  return {
    locations: data?.result?.items || [],
    isPending,
    error,
    refetch,
    isFetchingNextPage,
    isFetching,
    cursorRef,
  };
};
