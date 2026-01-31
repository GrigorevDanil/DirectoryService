import { useMutation, useQueryClient } from "@tanstack/react-query";
import { departmentsApi } from "../api";

export const useDepartmentSetLocations = () => {
  const queryClient = useQueryClient();

  const deparmentSetLocationsMutation = useMutation({
    mutationFn: departmentsApi.departmentSetLocation,
    async onSettled() {
      await queryClient.invalidateQueries({
        queryKey: [departmentsApi.baseKey],
      });
    },
  });

  return {
    departmentSetLocationsAsync: deparmentSetLocationsMutation.mutateAsync,
    isPending: deparmentSetLocationsMutation.isPending,
    error: deparmentSetLocationsMutation.error,
  };
};
