import { useInfiniteQuery } from "@tanstack/react-query";
import { useDebounce } from "use-debounce";
import {
  PositionListId,
  usePositionActive,
  usePositionPageSize,
  usePositionSearch,
  usePositionSelectedDepartments,
  usePositionSortBy,
  usePositionSortDirection,
} from "../model/position-list-store";
import { GetPositionsRequest, positionsApi } from "../api";
import { useCursorRef } from "@/shared/hooks/use-cursor-ref";

export const usePositionList = ({
  stateId,
  request,
}: {
  stateId?: PositionListId;
  request?: GetPositionsRequest;
}) => {
  const search = usePositionSearch(stateId);
  const [debouncedSearch] = useDebounce(search, 300);
  const activeState = usePositionActive(stateId);
  const sortBy = usePositionSortBy(stateId);
  const sortDirection = usePositionSortDirection(stateId);
  const pageSize = usePositionPageSize(stateId);
  const selectedDepartments = usePositionSelectedDepartments(stateId);

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
      ...request,
    }),
  });

  const cursorRef = useCursorRef({
    hasNextPage,
    fetchNextPage,
    isFetchingNextPage,
  });

  return {
    positions: data?.result?.items || [],
    isPending,
    error,
    refetch,
    isFetching,
    isFetchingNextPage,
    cursorRef,
  };
};
