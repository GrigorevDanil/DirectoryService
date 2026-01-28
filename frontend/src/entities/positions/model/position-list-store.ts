import { DepartmentId, DepartmentShortDto } from "@/entities/departments/types";
import { ActiveState } from "@/shared/api/active-state";
import { DEFAULT_PAGE_SIZE } from "@/shared/api/constants";
import { SortDirection } from "@/shared/api/sort-direction";
import { create } from "zustand";
import { createJSONStorage, persist } from "zustand/middleware";

export type PositionSortBy = "name" | "createdAt" | "countDepartments";

interface PositionListState {
  search: string;
  isActive: ActiveState;
  sortBy: PositionSortBy;
  sortDirection: SortDirection;
  pageSize: number;
  selectedDepartments: DepartmentShortDto[];
}

const initialState: PositionListState = {
  search: "",
  isActive: "all",
  sortBy: "name",
  sortDirection: "asc",
  pageSize: DEFAULT_PAGE_SIZE,
  selectedDepartments: [],
};

const usePositionListStore = create<PositionListState>()(
  persist(
    () => ({
      ...initialState,
    }),
    {
      name: "position-list-storage",
      storage: createJSONStorage(() => localStorage),
    },
  ),
);

export const usePositionSearch = () =>
  usePositionListStore((state) => state.search);

export const setPositionSearch = (search: string) =>
  usePositionListStore.setState({ search });

export const usePositionActive = () =>
  usePositionListStore((state) => state.isActive);

export const setPositionActive = (isActive: ActiveState) =>
  usePositionListStore.setState({ isActive });

export const usePositionSortBy = () =>
  usePositionListStore((state) => state.sortBy);

export const setPositionSortBy = (sortBy: PositionSortBy) =>
  usePositionListStore.setState({ sortBy });

export const usePositionSortDirection = () =>
  usePositionListStore((state) => state.sortDirection);

export const setPositionSortDirection = (sortDirection: SortDirection) =>
  usePositionListStore.setState({ sortDirection });

export const usePositionPageSize = () =>
  usePositionListStore((state) => state.pageSize);

export const setPositionPageSize = (pageSize: number) =>
  usePositionListStore.setState({ pageSize });

export const usePositionSelectedDepartments = () =>
  usePositionListStore((state) => state.selectedDepartments);

export const setPositionSelectedDepartments = (
  selectedDepartments: DepartmentShortDto[],
) => usePositionListStore.setState({ selectedDepartments });

export const removeSelectedDepartmentFromPositionList = (id: DepartmentId) =>
  usePositionListStore.setState((state) => ({
    selectedDepartments: state.selectedDepartments.filter(
      (dep) => dep.id !== id,
    ),
  }));
