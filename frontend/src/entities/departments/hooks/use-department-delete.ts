import { useMutation, useQueryClient } from "@tanstack/react-query";
import { toast } from "sonner";
import { departmentsApi } from "../api";

export const useDepartmentDelete = () => {
  const queryClient = useQueryClient();

  const departmentDeleteMutation = useMutation({
    mutationFn: departmentsApi.departmentDelete,
    async onSettled() {
      await queryClient.invalidateQueries({
        queryKey: [departmentsApi.baseKey],
      });
    },
    onSuccess() {
      toast.info(`Подразделение было успешно удалено`);
    },
  });

  return {
    departmentDeleteAsync: departmentDeleteMutation.mutateAsync,
    isPending: departmentDeleteMutation.isPending,
    error: departmentDeleteMutation.error,
  };
};
