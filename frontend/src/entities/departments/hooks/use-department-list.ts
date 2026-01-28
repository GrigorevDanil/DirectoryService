import { useInfiniteQuery } from "@tanstack/react-query";
import { useDebounce } from "use-debounce";
import {
  useDepartmentSearch,
  useDepartmentActive,
  useDepartmentSortBy,
  useDepartmentSortDirection,
  useDepartmentPageSize,
  DepartmentListId,
  useDepartmentParent,
} from "../model/department-list-store";
import { departmentsApi, GetDepartmentsRequest } from "../api";

export const useDepartmentList = (
  stateId: DepartmentListId,
  request?: GetDepartmentsRequest,
) => {
  const search = useDepartmentSearch(stateId);
  const [debouncedSearch] = useDebounce(search, 300);
  const activeState = useDepartmentActive(stateId);
  const parentState = useDepartmentParent(stateId);
  const sortBy = useDepartmentSortBy(stateId);
  const sortDirection = useDepartmentSortDirection(stateId);
  const pageSize = useDepartmentPageSize(stateId);

  const isActive = activeState === "all" ? undefined : activeState === "active";

  const isParent = parentState === "all" ? undefined : parentState === "parent";

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
    ...departmentsApi.getDepartmentsInfinityQueryOptions({
      search: debouncedSearch,
      isActive,
      isParent,
      sortBy,
      sortDirection,
      pageSize,
      ...request,
    }),
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
