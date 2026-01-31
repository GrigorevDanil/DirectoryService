import { useMutation, useQueryClient } from "@tanstack/react-query";
import { toast } from "sonner";
import { departmentsApi } from "../api";

export const useDepartmentCreate = () => {
  const queryClient = useQueryClient();

  const departmentCreateMutation = useMutation({
    mutationFn: departmentsApi.departmentCreate,
    async onSettled() {
      await queryClient.invalidateQueries({
        queryKey: [departmentsApi.baseKey],
      });
    },
    onSuccess(_, variables) {
      toast.success(
        `Подразделение под названием '${variables.name}' была успешно добавлена`,
      );
    },
  });

  return {
    departmentCreateAsync: departmentCreateMutation.mutateAsync,
    isPending: departmentCreateMutation.isPending,
    error: departmentCreateMutation.error,
  };
};
