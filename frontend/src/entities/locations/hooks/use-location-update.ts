import { useMutation, useQueryClient } from "@tanstack/react-query";
import { locationsApi, LocationUpdateRequest } from "../api";
import { Envelope } from "@/shared/api/envelops";
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
        `Локация под названием '${variables.name}' была успешно обновлена`
      );
    },
  });

  const locationUpdateAsync = async (
    id: string,
    request: LocationUpdateRequest
  ): Promise<Envelope<string>> => {
    const response = await locationUpdateMutation.mutateAsync({
      ...request,
      id,
    });

    return response;
  };

  return {
    locationUpdateAsync,
    isPending: locationUpdateMutation.isPending,
    error: locationUpdateMutation.error,
  };
};
