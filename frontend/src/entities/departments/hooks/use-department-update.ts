import { useMutation, useQueryClient } from "@tanstack/react-query";
import { toast } from "sonner";
import { departmentsApi } from "../api";

export const useDepartmentUpdate = () => {
  const queryClient = useQueryClient();

  const deparmentUpdateMutation = useMutation({
    mutationFn: departmentsApi.departmentUpdate,
    async onSettled() {
      await queryClient.invalidateQueries({ queryKey: [departmentsApi.baseKey] });
    },
    onSuccess(_, variables) {
      toast.success(
        `Подразделение под названием '${variables.name}' было успешно обновлено`,
      );
    },
  });

  return {
    departmentUpdateAsync: deparmentUpdateMutation.mutateAsync,
    isPending: deparmentUpdateMutation.isPending,
    error: deparmentUpdateMutation.error,
  };
};
