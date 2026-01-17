import { useMutation, useQueryClient } from "@tanstack/react-query";
import { locationsApi } from "../api";
import { toast } from "sonner";

export const useLocationDelete = () => {
  const queryClient = useQueryClient();

  const locationDeleteMutation = useMutation({
    mutationFn: locationsApi.locationDelete,
    async onSettled() {
      await queryClient.invalidateQueries({ queryKey: [locationsApi.baseKey] });
    },
    onSuccess() {
      toast.info(`Локация была успешно удалена`);
    },
  });

  return {
    locationDeleteAsync: locationDeleteMutation.mutateAsync,
    isPending: locationDeleteMutation.isPending,
    error: locationDeleteMutation.error,
  };
};
