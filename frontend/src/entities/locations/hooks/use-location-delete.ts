import { useMutation, useQueryClient } from "@tanstack/react-query";
import { locationsApi } from "../api";
import { Envelope } from "@/shared/api/envelops";
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

  const locationDeleteAsync = async (id: string): Promise<Envelope<string>> => {
    const response = await locationDeleteMutation.mutateAsync(id);

    return response;
  };

  return {
    locationDeleteAsync,
    isPending: locationDeleteMutation.isPending,
    error: locationDeleteMutation.error,
  };
};
