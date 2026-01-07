import { httpClient } from "@/shared/api/http-client";
import { LocationDto } from "./types";
import { Envelope, PaginationEnvelope } from "@/shared/api/envelops";
import { SortDirection } from "@/shared/api/sort-direction";
import { PaginationRequest } from "@/shared/api/pagination-request";
import { queryOptions } from "@tanstack/react-query";

export interface GetLocationsRequest extends PaginationRequest {
  search?: string;
  departmentIds?: string[];
  isActive?: boolean;
  sortBy?: string;
  sortDirection?: SortDirection;
}

export const locationsApi = {
  baseKey: "locations",
  getLocationsQueryOptions: (request: GetLocationsRequest) =>
    queryOptions({
      queryKey: [
        locationsApi.baseKey,
        request.search,
        request.departmentIds,
        request.isActive,
        request.sortBy,
        request.sortDirection,
        request.page,
        request.pageSize,
      ],
      queryFn: async () => {
        const response = await httpClient.get<
          Envelope<PaginationEnvelope<LocationDto>>
        >("/locations", { params: request });

        return response.data;
      },
    }),
};
