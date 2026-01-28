import { useMutation, useQueryClient } from "@tanstack/react-query";
import { toast } from "sonner";
import { positionsApi } from "../api";

export const usePositionDelete = () => {
  const queryClient = useQueryClient();

  const locationDeleteMutation = useMutation({
    mutationFn: positionsApi.positionDelete,
    async onSettled() {
      await queryClient.invalidateQueries({ queryKey: [positionsApi.baseKey] });
    },
    onSuccess() {
      toast.info(`Позиция была успешно удалена`);
    },
  });

  return {
    positionDeleteAsync: locationDeleteMutation.mutateAsync,
    isPending: locationDeleteMutation.isPending,
    error: locationDeleteMutation.error,
  };
};
