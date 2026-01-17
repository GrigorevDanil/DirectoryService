import { useMutation, useQueryClient } from "@tanstack/react-query";
import { locationsApi } from "../api";
import { toast } from "sonner";

export const useLocationUpdate = () => {
  const queryClient = useQueryClient();

  const locationUpdateMutation = useMutation({
    mutationFn: locationsApi.locationUpdate,
    async onSettled() {
      await queryClient.invalidateQueries({ queryKey: [locationsApi.baseKey] });
    },
    onSuccess(_, variables) {
      toast.success(
        `Локация под названием '${variables.name}' была успешно обновлена`,
      );
    },
  });

  return {
    locationUpdateAsync: locationUpdateMutation.mutateAsync,
    isPending: locationUpdateMutation.isPending,
    error: locationUpdateMutation.error,
  };
};
