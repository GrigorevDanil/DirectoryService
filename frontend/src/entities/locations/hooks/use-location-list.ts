import { useQuery } from "@tanstack/react-query";
import { locationsApi } from "../api";

export const useLocationList = () => {
  const { data, isPending, error, refetch } = useQuery({
    ...locationsApi.getLocationsQueryOptions({}),
  });

  return {
    locations: data?.result?.items || [],
    isPending,
    error,
    refetch,
  };
};
