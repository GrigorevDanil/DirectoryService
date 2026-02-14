import { ActiveState } from "@/shared/api/active-state";
import { SortDirection } from "@/shared/api/sort-direction";
import { create } from "zustand";
import { DepartmentSortBy } from "../api";
import { createJSONStorage, persist } from "zustand/middleware";
import { LocationDto, LocationId } from "@/entities/locations/types";
import { DepartmentShortDto } from "../types";
import { DEFAULT_PAGE_SIZE } from "@/shared/api/pagination-request";

export type DepartmentListId = string;

export type DepartmentParentState = "all" | "parent" | "child";

interface DepartmentListState {
  search: string;
  isActive: ActiveState;
  isParent: DepartmentParentState;
  sortBy: DepartmentSortBy;
  sortDirection: SortDirection;
  pageSize: number;
  selectedLocations: LocationDto[];
  parent: DepartmentShortDto | null;
}

type DepartmentListStates = Record<
  DepartmentListId,
  DepartmentListState | undefined
>;

const DEFAULT_STATE_ID = "__default__";

const initialState: DepartmentListState = {
  search: "",
  isActive: "all",
  isParent: "all",
  sortBy: "name",
  sortDirection: "asc",
  pageSize: DEFAULT_PAGE_SIZE,
  selectedLocations: [],
  parent: null,
};

const initialStates: DepartmentListStates = {};

const resolveStateId = (stateId?: DepartmentListId) =>
  stateId ?? DEFAULT_STATE_ID;

const getOrCreate = (
  state: DepartmentListStates,
  stateId?: DepartmentListId,
): DepartmentListState => {
  const id = resolveStateId(stateId);

  if (!state[id]) {
    state[id] = { ...initialState };
  }

  return state[id];
};

const useDepartmentListStore = create<DepartmentListStates>()(
  persist(
    () => ({
      ...initialStates,
    }),
    {
      name: "department-list-storage",
      storage: createJSONStorage(() => localStorage),
      partialize: (state) =>
        Object.fromEntries(
          Object.entries(state).filter(([key]) => key === DEFAULT_STATE_ID),
        ),
    },
  ),
);

export const useDepartmentSearch = (stateId?: DepartmentListId) =>
  useDepartmentListStore((states) => getOrCreate(states, stateId).search);

export const setDepartmentSearch = (
  search: string,
  stateId?: DepartmentListId,
) =>
  useDepartmentListStore.setState((states) => ({
    [resolveStateId(stateId)]: {
      ...getOrCreate(states, stateId),
      search,
    },
  }));

export const useDepartmentActive = (stateId?: DepartmentListId) =>
  useDepartmentListStore((states) => getOrCreate(states, stateId).isActive);

export const setDepartmentActive = (
  isActive: ActiveState,
  stateId?: DepartmentListId,
) =>
  useDepartmentListStore.setState((states) => ({
    [resolveStateId(stateId)]: {
      ...getOrCreate(states, stateId),
      isActive,
    },
  }));

export const useDepartmentIsParent = (stateId?: DepartmentListId) =>
  useDepartmentListStore((states) => getOrCreate(states, stateId).isParent);

export const setDepartmentIsParent = (
  isParent: DepartmentParentState,
  stateId?: DepartmentListId,
) =>
  useDepartmentListStore.setState((states) => ({
    [resolveStateId(stateId)]: {
      ...getOrCreate(states, stateId),
      isParent,
    },
  }));

export const useDepartmentSortBy = (stateId?: DepartmentListId) =>
  useDepartmentListStore((states) => getOrCreate(states, stateId).sortBy);

export const setDepartmentSortBy = (
  sortBy: DepartmentSortBy,
  stateId?: DepartmentListId,
) =>
  useDepartmentListStore.setState((states) => ({
    [resolveStateId(stateId)]: {
      ...getOrCreate(states, stateId),
      sortBy,
    },
  }));

export const useDepartmentSortDirection = (stateId?: DepartmentListId) =>
  useDepartmentListStore(
    (states) => getOrCreate(states, stateId).sortDirection,
  );

export const setDepartmentSortDirection = (
  sortDirection: SortDirection,
  stateId?: DepartmentListId,
) =>
  useDepartmentListStore.setState((states) => ({
    [resolveStateId(stateId)]: {
      ...getOrCreate(states, stateId),
      sortDirection,
    },
  }));

export const useDepartmentPageSize = (stateId?: DepartmentListId) =>
  useDepartmentListStore((states) => getOrCreate(states, stateId).pageSize);

export const setDepartmentPageSize = (
  pageSize: number,
  stateId?: DepartmentListId,
) =>
  useDepartmentListStore.setState((states) => ({
    [resolveStateId(stateId)]: {
      ...getOrCreate(states, stateId),
      pageSize,
    },
  }));

export const useDepartmentSelectedLocations = (stateId?: DepartmentListId) =>
  useDepartmentListStore(
    (states) => getOrCreate(states, stateId).selectedLocations,
  );

export const setDepartmentSelectedLocations = (
  selectedLocations: LocationDto[],
  stateId?: DepartmentListId,
) =>
  useDepartmentListStore.setState((states) => ({
    [resolveStateId(stateId)]: {
      ...getOrCreate(states, stateId),
      selectedLocations,
    },
  }));

export const removeSelectedLocationFromDepartmentList = (
  id: LocationId,
  stateId?: DepartmentListId,
) =>
  useDepartmentListStore.setState((states) => {
    const state = getOrCreate(states, stateId);
    return {
      [resolveStateId(stateId)]: {
        ...state,
        selectedLocations: state.selectedLocations.filter(
          (loc) => loc.id !== id,
        ),
      },
    };
  });

export const useDepartmentParent = (stateId?: DepartmentListId) =>
  useDepartmentListStore((states) => getOrCreate(states, stateId).parent);

export const setDepartmentParent = (
  parent: DepartmentShortDto | null,
  stateId?: DepartmentListId,
) =>
  useDepartmentListStore.setState((states) => ({
    [resolveStateId(stateId)]: {
      ...getOrCreate(states, stateId),
      parent,
    },
  }));
