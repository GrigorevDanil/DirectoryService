import { httpClient } from "@/shared/api/http-client";
import { AddressDto, LocationDto } from "./types";
import {
  Envelope,
  envelopeInfinityQueryOptions,
  PaginationEnvelope,
} from "@/shared/api/envelops";
import { SortDirection } from "@/shared/api/sort-direction";
import { PaginationRequest } from "@/shared/api/pagination-request";
import { infiniteQueryOptions, queryOptions } from "@tanstack/react-query";
import { InfinityScrollRequest } from "@/shared/api/infinity-scroll-request";

export interface GetLocationsInfinityRequest extends InfinityScrollRequest {
  search?: string;
  departmentIds?: string[];
  isActive?: boolean;
  sortBy?: string;
  sortDirection?: SortDirection;
}

export interface GetLocationsRequest extends PaginationRequest {
  search?: string;
  departmentIds?: string[];
  isActive?: boolean;
  sortBy?: string;
  sortDirection?: SortDirection;
}

export interface LocationCreateRequest {
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
  getLocationsInfinityQueryOptions: (request: GetLocationsInfinityRequest) =>
    infiniteQueryOptions({
      queryKey: [
        locationsApi.baseKey,
        request.search,
        request.departmentIds,
        request.isActive,
        request.sortBy,
        request.sortDirection,
        request.pageSize,
      ],
      queryFn: async ({ pageParam }) => {
        const response = await httpClient.get<
          Envelope<PaginationEnvelope<LocationDto>>
        >("/locations", { params: { ...request, page: pageParam } });

        return response.data;
      },
      ...envelopeInfinityQueryOptions<LocationDto>(request),
    }),
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
  locationCreate: async (
    request: LocationCreateRequest,
  ): Promise<Envelope<string>> => {
    const response = await httpClient.post<Envelope<string>>(
      "/locations",
      request,
    );

    return response.data;
  },
  locationUpdate: async (
    request: LocationUpdateRequest & { id: string },
  ): Promise<Envelope<string>> => {
    const response = await httpClient.put<Envelope<string>>(
      "/locations/" + request.id,
      request,
    );

    return response.data;
  },
  locationDelete: async (id: string): Promise<Envelope<string>> => {
    const response = await httpClient.delete<Envelope<string>>(
      "/locations/" + id,
    );

    return response.data;
  },
};
