import { useMutation, useQueryClient } from "@tanstack/react-query";
import { positionsApi } from "../api";

export const usePositionRemoveDepartment = () => {
  const queryClient = useQueryClient();

  const positionRemoveDepartmentMutation = useMutation({
    mutationFn: positionsApi.removeDepartmentFromPosition,
    async onSettled() {
      await queryClient.invalidateQueries({ queryKey: [positionsApi.baseKey] });
    },
  });

  return {
    removeDepartmentFromPosition: positionRemoveDepartmentMutation.mutateAsync,
    isPending: positionRemoveDepartmentMutation.isPending,
    error: positionRemoveDepartmentMutation.error,
  };
};
