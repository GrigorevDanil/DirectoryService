import { useMutation, useQueryClient } from "@tanstack/react-query";
import { toast } from "sonner";
import { positionsApi } from "../api";

export const usePositionUpdate = () => {
  const queryClient = useQueryClient();

  const positionUpdateMutation = useMutation({
    mutationFn: positionsApi.positionUpdate,
    async onSettled() {
      await queryClient.invalidateQueries({ queryKey: [positionsApi.baseKey] });
    },
    onSuccess(_, variables) {
      toast.success(
        `Позиция под названием '${variables.name}' была успешно обновлена`,
      );
    },
  });

  return {
    positionUpdateAsync: positionUpdateMutation.mutateAsync,
    isPending: positionUpdateMutation.isPending,
    error: positionUpdateMutation.error,
  };
};
