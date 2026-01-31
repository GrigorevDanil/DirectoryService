import { useInfiniteQuery } from "@tanstack/react-query";
import { useDebounce } from "use-debounce";
import {
  useDepartmentSearch,
  useDepartmentActive,
  useDepartmentSortBy,
  useDepartmentSortDirection,
  useDepartmentPageSize,
  DepartmentListId,
  useDepartmentIsParent,
  useDepartmentSelectedLocations,
  useDepartmentParent,
} from "../model/department-list-store";
import { departmentsApi, GetDepartmentsRequest } from "../api";

interface UseDepartmentListProps {
  stateId?: DepartmentListId;
  request?: GetDepartmentsRequest;
}

export const useDepartmentList = ({
  request,
  stateId,
}: UseDepartmentListProps) => {
  const search = useDepartmentSearch(stateId);
  const [debouncedSearch] = useDebounce(search, 300);
  const activeState = useDepartmentActive(stateId);
  const parentState = useDepartmentIsParent(stateId);
  const sortBy = useDepartmentSortBy(stateId);
  const sortDirection = useDepartmentSortDirection(stateId);
  const pageSize = useDepartmentPageSize(stateId);
  const selectedLocations = useDepartmentSelectedLocations(stateId);
  const parent = useDepartmentParent(stateId);

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
      locationIds: selectedLocations.map((loc) => loc.id),
      parentId: parent?.id,
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
