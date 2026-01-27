import { useMutation, useQueryClient } from "@tanstack/react-query";
import { toast } from "sonner";
import { positionsApi } from "../api";

export const usePositionCreate = () => {
  const queryClient = useQueryClient();

  const positionCreateMutation = useMutation({
    mutationFn: positionsApi.positionCreate,
    async onSettled() {
      await queryClient.invalidateQueries({ queryKey: [positionsApi.baseKey] });
    },
    onSuccess(_, variables) {
      toast.success(
        `Позиция под названием '${variables.name}' была успешно добавлена`,
      );
    },
  });

  return {
    positionCreateAsync: positionCreateMutation.mutateAsync,
    isPending: positionCreateMutation.isPending,
    error: positionCreateMutation.error,
  };
};
