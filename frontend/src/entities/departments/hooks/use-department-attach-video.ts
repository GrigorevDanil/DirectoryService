import { useMutation, useQueryClient } from "@tanstack/react-query";
import { toast } from "sonner";
import { departmentsApi } from "../api";

export const useDepartmentAttachVideo = () => {
  const queryClient = useQueryClient();

  const departmentAttachVideoMutation = useMutation({
    mutationFn: departmentsApi.departmentAttachVideo,
    async onSettled() {
      await queryClient.invalidateQueries({
        queryKey: [departmentsApi.baseKey],
      });
    },
    onSuccess() {
      toast.info(`Видео было успешно прикреплено`);
    },
  });

  return {
    departmentAttachVideoAsync: departmentAttachVideoMutation.mutateAsync,
    isPending: departmentAttachVideoMutation.isPending,
    error: departmentAttachVideoMutation.error,
  };
};
