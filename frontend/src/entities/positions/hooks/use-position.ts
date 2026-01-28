import { useQuery } from "@tanstack/react-query";
import { positionsApi } from "../api";
import { DepartmentId } from "@/entities/departments/types";

export const usePosition = (id: DepartmentId) => {
  const { data, isPending, error, refetch, isFetching } = useQuery({
    ...positionsApi.getPositionQueryOptions(id),
  });

  return {
    position: data?.result || undefined,
    isPending,
    error,
    refetch,
    isFetching,
  };
};
