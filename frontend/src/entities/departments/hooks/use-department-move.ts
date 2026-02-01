import { useMutation, useQueryClient } from "@tanstack/react-query";
import { toast } from "sonner";
import { departmentsApi } from "../api";

export const useDepartmentMove = () => {
  const queryClient = useQueryClient();

  const deparmentMoveMutation = useMutation({
    mutationFn: departmentsApi.departmentMove,
    async onSettled() {
      await queryClient.invalidateQueries({
        queryKey: [departmentsApi.baseKey],
      });
    },
    onSuccess() {
      toast.success(`Подразделение было успешно перенесено`);
    },
  });

  return {
    departmentMoveAsync: deparmentMoveMutation.mutateAsync,
    isPending: deparmentMoveMutation.isPending,
    error: deparmentMoveMutation.error,
  };
};
