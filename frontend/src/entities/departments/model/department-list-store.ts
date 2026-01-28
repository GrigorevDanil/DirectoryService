import { ActiveState } from "@/shared/api/active-state";
import { DEFAULT_PAGE_SIZE } from "@/shared/api/constants";
import { SortDirection } from "@/shared/api/sort-direction";
import { create } from "zustand";
import { DepartmentSortBy } from "../api";

export type DepartmentListId = string;

export type DepartmentParentState = "all" | "parent" | "child";

interface DepartmentListState {
  search: string;
  isActive: ActiveState;
  isParent: DepartmentParentState;
  sortBy: DepartmentSortBy;
  sortDirection: SortDirection;
  pageSize: number;
}

type DepartmentListStates = Record<
  DepartmentListId,
  DepartmentListState | undefined
>;

const initialState: DepartmentListState = {
  search: "",
  isActive: "all",
  isParent: "all",
  sortBy: "name",
  sortDirection: "asc",
  pageSize: DEFAULT_PAGE_SIZE,
};

const initialStates: DepartmentListStates = {};

const getOrCreate = (
  state: DepartmentListStates,
  id: DepartmentListId,
): DepartmentListState => {
  if (!state[id]) {
    state[id] = { ...initialState };
  }
  return state[id]!;
};

const useDepartmentListStore = create<DepartmentListStates>()(() => ({
  ...initialStates,
}));

export const useDepartmentSearch = (stateId: DepartmentListId) =>
  useDepartmentListStore((states) => getOrCreate(states, stateId).search);

export const setDepartmentSearch = (
  stateId: DepartmentListId,
  search: string,
) =>
  useDepartmentListStore.setState((states) => ({
    [stateId]: {
      ...getOrCreate(states, stateId),
      search,
    },
  }));

export const useDepartmentActive = (stateId: DepartmentListId) =>
  useDepartmentListStore((states) => getOrCreate(states, stateId).isActive);

export const setDepartmentActive = (
  stateId: DepartmentListId,
  isActive: ActiveState,
) =>
  useDepartmentListStore.setState((states) => ({
    [stateId]: {
      ...getOrCreate(states, stateId),
      isActive,
    },
  }));

export const useDepartmentParent = (stateId: DepartmentListId) =>
  useDepartmentListStore((states) => getOrCreate(states, stateId).isParent);

export const setDepartmentParent = (
  stateId: DepartmentListId,
  isParent: DepartmentParentState,
) =>
  useDepartmentListStore.setState((states) => ({
    [stateId]: {
      ...getOrCreate(states, stateId),
      isParent,
    },
  }));

export const useDepartmentSortBy = (stateId: DepartmentListId) =>
  useDepartmentListStore((states) => getOrCreate(states, stateId).sortBy);

export const setDepartmentSortBy = (
  stateId: DepartmentListId,
  sortBy: DepartmentSortBy,
) =>
  useDepartmentListStore.setState((states) => ({
    [stateId]: {
      ...getOrCreate(states, stateId),
      sortBy,
    },
  }));

export const useDepartmentSortDirection = (stateId: DepartmentListId) =>
  useDepartmentListStore(
    (states) => getOrCreate(states, stateId).sortDirection,
  );

export const setDepartmentSortDirection = (
  stateId: DepartmentListId,
  sortDirection: SortDirection,
) =>
  useDepartmentListStore.setState((states) => ({
    [stateId]: {
      ...getOrCreate(states, stateId),
      sortDirection,
    },
  }));

export const useDepartmentPageSize = (stateId: DepartmentListId) =>
  useDepartmentListStore((states) => getOrCreate(states, stateId).pageSize);

export const setDepartmentPageSize = (
  stateId: DepartmentListId,
  pageSize: number,
) =>
  useDepartmentListStore.setState((states) => ({
    [stateId]: {
      ...getOrCreate(states, stateId),
      pageSize,
    },
  }));
