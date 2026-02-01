import { DepartmentId, DepartmentShortDto } from "@/entities/departments/types";
import { ActiveState } from "@/shared/api/active-state";
import { DEFAULT_PAGE_SIZE } from "@/shared/api/constants";
import { SortDirection } from "@/shared/api/sort-direction";
import { create } from "zustand";
import { createJSONStorage, persist } from "zustand/middleware";

export type PositionSortBy = "name" | "createdAt" | "countDepartments";

export type PositionListId = string;

interface PositionListState {
  search: string;
  isActive: ActiveState;
  sortBy: PositionSortBy;
  sortDirection: SortDirection;
  pageSize: number;
  selectedDepartments: DepartmentShortDto[];
}

type PositionListStates = Record<PositionListId, PositionListState | undefined>;

const DEFAULT_STATE_ID = "__default__";

const initialState: PositionListState = {
  search: "",
  isActive: "all",
  sortBy: "name",
  sortDirection: "asc",
  pageSize: DEFAULT_PAGE_SIZE,
  selectedDepartments: [],
};

const initialStates: PositionListStates = {};

const resolveStateId = (stateId?: PositionListId) =>
  stateId ?? DEFAULT_STATE_ID;

const getOrCreate = (
  state: PositionListStates,
  stateId?: PositionListId,
): PositionListState => {
  const id = resolveStateId(stateId);

  if (!state[id]) {
    state[id] = { ...initialState };
  }

  return state[id];
};

const usePositionListStore = create<PositionListStates>()(
  persist(
    () => ({
      ...initialStates,
    }),
    {
      name: "position-list-storage",
      storage: createJSONStorage(() => localStorage),
      partialize: (state) =>
        Object.fromEntries(
          Object.entries(state).filter(([key]) => key === DEFAULT_STATE_ID),
        ),
    },
  ),
);

export const usePositionSearch = (stateId?: PositionListId) =>
  usePositionListStore((states) => getOrCreate(states, stateId).search);

export const setPositionSearch = (search: string, stateId?: PositionListId) =>
  usePositionListStore.setState((states) => ({
    [resolveStateId(stateId)]: {
      ...getOrCreate(states, stateId),
      search,
    },
  }));

export const usePositionActive = (stateId?: PositionListId) =>
  usePositionListStore((states) => getOrCreate(states, stateId).isActive);

export const setPositionActive = (
  isActive: ActiveState,
  stateId?: PositionListId,
) =>
  usePositionListStore.setState((states) => ({
    [resolveStateId(stateId)]: {
      ...getOrCreate(states, stateId),
      isActive,
    },
  }));

export const usePositionSortBy = (stateId?: PositionListId) =>
  usePositionListStore((states) => getOrCreate(states, stateId).sortBy);

export const setPositionSortBy = (
  sortBy: PositionSortBy,
  stateId?: PositionListId,
) =>
  usePositionListStore.setState((states) => ({
    [resolveStateId(stateId)]: {
      ...getOrCreate(states, stateId),
      sortBy,
    },
  }));

export const usePositionSortDirection = (stateId?: PositionListId) =>
  usePositionListStore((states) => getOrCreate(states, stateId).sortDirection);

export const setPositionSortDirection = (
  sortDirection: SortDirection,
  stateId?: PositionListId,
) =>
  usePositionListStore.setState((states) => ({
    [resolveStateId(stateId)]: {
      ...getOrCreate(states, stateId),
      sortDirection,
    },
  }));

export const usePositionPageSize = (stateId?: PositionListId) =>
  usePositionListStore((states) => getOrCreate(states, stateId).pageSize);

export const setPositionPageSize = (
  pageSize: number,
  stateId?: PositionListId,
) =>
  usePositionListStore.setState((states) => ({
    [resolveStateId(stateId)]: {
      ...getOrCreate(states, stateId),
      pageSize,
    },
  }));

export const usePositionSelectedDepartments = (stateId?: PositionListId) =>
  usePositionListStore(
    (states) => getOrCreate(states, stateId).selectedDepartments,
  );

export const setPositionSelectedDepartments = (
  selectedDepartments: DepartmentShortDto[],
  stateId?: PositionListId,
) =>
  usePositionListStore.setState((states) => ({
    [resolveStateId(stateId)]: {
      ...getOrCreate(states, stateId),
      selectedDepartments,
    },
  }));

export const removeSelectedDepartmentFromPositionList = (
  id: DepartmentId,
  stateId?: PositionListId,
) =>
  usePositionListStore.setState((states) => {
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
