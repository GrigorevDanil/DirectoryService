import { DepartmentId, DepartmentShortDto } from "@/entities/departments/types";
import { ActiveState } from "@/shared/api/active-state";
import { DEFAULT_PAGE_SIZE } from "@/shared/api/pagination-request";
import { SortDirection } from "@/shared/api/sort-direction";
import { create } from "zustand";
import { createJSONStorage, persist } from "zustand/middleware";

export type LocationListId = string;
export type LocationSortBy = "name" | "createdAt";

interface LocationListState {
  search: string;
  isActive: ActiveState;
  sortBy: LocationSortBy;
  sortDirection: SortDirection;
  pageSize: number;
  selectedDepartments: DepartmentShortDto[];
}

type LocationListStates = Record<LocationListId, LocationListState | undefined>;

const DEFAULT_STATE_ID = "__default__";

const initialState: LocationListState = {
  search: "",
  isActive: "all",
  sortBy: "name",
  sortDirection: "asc",
  pageSize: DEFAULT_PAGE_SIZE,
  selectedDepartments: [],
};

const initialStates: LocationListStates = {};

const resolveStateId = (stateId?: LocationListId) =>
  stateId ?? DEFAULT_STATE_ID;

const getOrCreate = (
  state: LocationListStates,
  stateId?: LocationListId,
): LocationListState => {
  const id = resolveStateId(stateId);

  if (!state[id]) {
    state[id] = { ...initialState };
  }

  return state[id];
};

const useLocationListStore = create<LocationListStates>()(
  persist(
    () => ({
      ...initialStates,
    }),
    {
      name: "location-list-storage",
      storage: createJSONStorage(() => localStorage),
      partialize: (state) =>
        Object.fromEntries(
          Object.entries(state).filter(([key]) => key === DEFAULT_STATE_ID),
        ),
    },
  ),
);

export const useLocationSearch = (stateId?: LocationListId) =>
  useLocationListStore((states) => getOrCreate(states, stateId).search);

export const setLocationSearch = (search: string, stateId?: LocationListId) =>
  useLocationListStore.setState((states) => ({
    [resolveStateId(stateId)]: {
      ...getOrCreate(states, stateId),
      search,
    },
  }));

export const useLocationActive = (stateId?: LocationListId) =>
  useLocationListStore((states) => getOrCreate(states, stateId).isActive);

export const setLocationActive = (
  isActive: ActiveState,
  stateId?: LocationListId,
) =>
  useLocationListStore.setState((states) => ({
    [resolveStateId(stateId)]: {
      ...getOrCreate(states, stateId),
      isActive,
    },
  }));

export const useLocationSortBy = (stateId?: LocationListId) =>
  useLocationListStore((states) => getOrCreate(states, stateId).sortBy);

export const setLocationSortBy = (
  sortBy: LocationSortBy,
  stateId?: LocationListId,
) =>
  useLocationListStore.setState((states) => ({
    [resolveStateId(stateId)]: {
      ...getOrCreate(states, stateId),
      sortBy,
    },
  }));

export const useLocationSortDirection = (stateId?: LocationListId) =>
  useLocationListStore((states) => getOrCreate(states, stateId).sortDirection);

export const setLocationSortDirection = (
  sortDirection: SortDirection,
  stateId?: LocationListId,
) =>
  useLocationListStore.setState((states) => ({
    [resolveStateId(stateId)]: {
      ...getOrCreate(states, stateId),
      sortDirection,
    },
  }));

export const useLocationPageSize = (stateId?: LocationListId) =>
  useLocationListStore((states) => getOrCreate(states, stateId).pageSize);

export const setLocationPageSize = (
  pageSize: number,
  stateId?: LocationListId,
) =>
  useLocationListStore.setState((states) => ({
    [resolveStateId(stateId)]: {
      ...getOrCreate(states, stateId),
      pageSize,
    },
  }));

export const useLocationSelectedDepartments = (stateId?: LocationListId) =>
  useLocationListStore(
    (states) => getOrCreate(states, stateId).selectedDepartments,
  );

export const setLocationSelectedDepartments = (
  selectedDepartments: DepartmentShortDto[],
  stateId?: LocationListId,
) =>
  useLocationListStore.setState((states) => ({
    [resolveStateId(stateId)]: {
      ...getOrCreate(states, stateId),
      selectedDepartments,
    },
  }));

export const removeSelectedDepartmentFromLocationList = (
  id: DepartmentId,
  stateId?: LocationListId,
) =>
  useLocationListStore.setState((states) => {
    const state = getOrCreate(states, stateId);
    return {
      [resolveStateId(stateId)]: {
        ...state,
        selectedDepartments: state.selectedDepartments.filter(
          (dep) => dep.id !== id,
        ),
      },
    };
  });
