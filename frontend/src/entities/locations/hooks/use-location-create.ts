import { useMutation, useQueryClient } from "@tanstack/react-query";
import { locationsApi } from "../api";
import { toast } from "sonner";

export const useLocationCreate = () => {
  const queryClient = useQueryClient();

  const locationCreateMutation = useMutation({
    mutationFn: locationsApi.locationCreate,
    async onSettled() {
      await queryClient.invalidateQueries({ queryKey: [locationsApi.baseKey] });
    },
    onSuccess(_, variables) {
      toast.success(
        `Локация под названием '${variables.name}' была успешно добавлена`,
      );
    },
  });

  return {
    locationCreateAsync: locationCreateMutation.mutateAsync,
    isPending: locationCreateMutation.isPending,
    error: locationCreateMutation.error,
  };
};
