import { httpClient } from "@/shared/api/http-client";
import { AddressDto, LocationDto } from "./types";
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

export interface LocationAddRequest {
  name: string;
  timezone: string;
  address: AddressDto;
}

export interface LocationUpdateRequest {
  name: string;
  timezone: string;
  address: AddressDto;
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
  locationAdd: async (
    request: LocationAddRequest
  ): Promise<Envelope<string>> => {
    const response = await httpClient.post<Envelope<string>>(
      "/locations",
      request
    );

    return response.data;
  },
  locationUpdate: async (
    request: LocationUpdateRequest & { id: string }
  ): Promise<Envelope<string>> => {
    const response = await httpClient.put<Envelope<string>>(
      "/locations/" + request.id,
      request
    );

    return response.data;
  },
  locationDelete: async (id: string): Promise<Envelope<string>> => {
    const response = await httpClient.delete<Envelope<string>>(
      "/locations/" + id
    );

    return response.data;
  },
};
