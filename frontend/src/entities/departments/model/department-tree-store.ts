import { create } from "zustand";
import { createJSONStorage, persist } from "zustand/middleware";

interface DepartmentTreeState {
  expandedNodes: string[];
}

const initialState: DepartmentTreeState = {
  expandedNodes: [],
};

const useDepartmentTreeStore = create<DepartmentTreeState>()(
  persist(
    () => ({
      ...initialState,
    }),
    {
      name: "department-tree-storage",
      storage: createJSONStorage(() => localStorage),
    },
  ),
);

export const useDepartmentExpandedNodes = () =>
  useDepartmentTreeStore((state) => state.expandedNodes);

export const setDepartmentExpandedNodes = (expandedNodes: string[]) =>
  useDepartmentTreeStore.setState({ expandedNodes });
