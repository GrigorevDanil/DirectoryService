import { create } from "zustand";
import { createJSONStorage, persist } from "zustand/middleware";

export type DepartmentVariant = "tree" | "list";

interface DepartmentViewState {
  variant: DepartmentVariant;
}

const initialState: DepartmentViewState = {
  variant: "tree",
};

const useDepartmentViewStore = create<DepartmentViewState>()(
  persist(
    () => ({
      ...initialState,
    }),
    {
      name: "department-view-storage",
      storage: createJSONStorage(() => localStorage),
    },
  ),
);

export const useDepartmentVariant = () =>
  useDepartmentViewStore((state) => state.variant);

export const setDepartmentVariant = (variant: DepartmentVariant) =>
  useDepartmentViewStore.setState({ variant });
