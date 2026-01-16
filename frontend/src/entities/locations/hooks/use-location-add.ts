import { useMutation, useQueryClient } from "@tanstack/react-query";
import { LocationAddRequest, locationsApi } from "../api";
import { Envelope } from "@/shared/api/envelops";
import { toast } from "sonner";

export const useLocationAdd = () => {
  const queryClient = useQueryClient();

  const locationAddMutation = useMutation({
    mutationFn: locationsApi.locationAdd,
    async onSettled() {
      await queryClient.invalidateQueries({ queryKey: [locationsApi.baseKey] });
    },
    onSuccess(_, variables) {
      toast.success(
        `Локация под названием '${variables.name}' была успешно добавлена`
      );
    },
  });

  const locationAddAsync = async (
    request: LocationAddRequest
  ): Promise<Envelope<string>> => {
    const response = await locationAddMutation.mutateAsync(request);

    return response;
  };

  return {
    locationAddAsync,
    isPending: locationAddMutation.isPending,
    error: locationAddMutation.error,
  };
};
