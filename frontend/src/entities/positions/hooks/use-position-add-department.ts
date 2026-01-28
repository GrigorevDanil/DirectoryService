import { useMutation, useQueryClient } from "@tanstack/react-query";
import { positionsApi } from "../api";

export const usePositionAddDepartment = () => {
  const queryClient = useQueryClient();

  const positionAddDepartmentMutation = useMutation({
    mutationFn: positionsApi.addDepartmentToPosition,
    async onSettled() {
      await queryClient.invalidateQueries({ queryKey: [positionsApi.baseKey] });
    },
  });

  return {
    addDepartmentToPosition: positionAddDepartmentMutation.mutateAsync,
    isPending: positionAddDepartmentMutation.isPending,
    error: positionAddDepartmentMutation.error,
  };
};
