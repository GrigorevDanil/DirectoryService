import { DepartmentId, DepartmentShortDto } from "@/entities/departments/types";
import { ActiveState } from "@/shared/api/active-state";
import { DEFAULT_PAGE_SIZE } from "@/shared/api/constants";
import { SortDirection } from "@/shared/api/sort-direction";
import { create } from "zustand";
import { createJSONStorage, persist } from "zustand/middleware";

export type LocationSortBy = "name" | "createdAt";

interface LocationListState {
  search: string;
  isActive: ActiveState;
  sortBy: LocationSortBy;
  sortDirection: SortDirection;
  pageSize: number;
  selectedDepartments: DepartmentShortDto[];
}

const initialState: LocationListState = {
  search: "",
  isActive: "all",
  sortBy: "name",
  sortDirection: "asc",
  pageSize: DEFAULT_PAGE_SIZE,
  selectedDepartments: [],
};

const useLocationListStore = create<LocationListState>()(
  persist(
    () => ({
      ...initialState,
    }),
    {
      name: "location-list-storage",
      storage: createJSONStorage(() => localStorage),
    },
  ),
);

export const useLocationSearch = () =>
  useLocationListStore((state) => state.search);

export const setLocationSearch = (search: string) =>
  useLocationListStore.setState({ search });

export const useLocationActive = () =>
  useLocationListStore((state) => state.isActive);

export const setLocationActive = (isActive: ActiveState) =>
  useLocationListStore.setState({ isActive });

export const useLocationSortBy = () =>
  useLocationListStore((state) => state.sortBy);

export const setLocationSortBy = (sortBy: LocationSortBy) =>
  useLocationListStore.setState({ sortBy });

export const useLocationSortDirection = () =>
  useLocationListStore((state) => state.sortDirection);

export const setLocationSortDirection = (sortDirection: SortDirection) =>
  useLocationListStore.setState({ sortDirection });

export const useLocationPageSize = () =>
  useLocationListStore((state) => state.pageSize);

export const setLocationPageSize = (pageSize: number) =>
  useLocationListStore.setState({ pageSize });

export const useLocationSelectedDepartments = () =>
  useLocationListStore((state) => state.selectedDepartments);

export const setLocationSelectedDepartments = (
  selectedDepartments: DepartmentShortDto[],
) => useLocationListStore.setState({ selectedDepartments });

export const removeSelectedDepartmentFromLocationList = (id: DepartmentId) =>
  useLocationListStore.setState((state) => ({
    selectedDepartments: state.selectedDepartments.filter(
      (dep) => dep.id !== id,
    ),
  }));
