import { useQuery } from "@tanstack/react-query";
import { DepartmentId } from "@/entities/departments/types";
import { departmentsApi } from "../api";

export const useDepartment = (id: DepartmentId) => {
  const { data, isPending, error, refetch, isFetching } = useQuery({
    ...departmentsApi.getDepartmentQueryOptions(id),
  });

  return {
    department: data?.result || undefined,
    isPending,
    error,
    refetch,
    isFetching,
  };
};
