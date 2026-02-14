import { API_V1, httpClient } from "@/shared/api/http-client";
import {
  Envelope,
  envelopeInfinityQueryOptions,
  PaginationEnvelope,
} from "@/shared/api/envelops";
import { SortDirection } from "@/shared/api/sort-direction";
import { infiniteQueryOptions, queryOptions } from "@tanstack/react-query";
import { InfinityScrollRequest } from "@/shared/api/infinity-scroll-request";
import { PositionDetailDto, PositionDto, PositionId } from "./types";
import { DepartmentId } from "../departments/types";

export interface GetPositionsRequest extends InfinityScrollRequest {
  search?: string;
  departmentIds?: string[];
  isActive?: boolean;
  sortBy?: string;
  sortDirection?: SortDirection;
}

export interface PositionCreateRequest {
  name: string;
  description: string;
  departmentIds: string[];
}

export interface PositionUpdateRequest {
  name: string;
  description: string;
}

export interface AddDepartmentToPositionRequest {
  departmentIds: DepartmentId[];
}

export interface RemoveDepartmentFromPositionRequest {
  departmentIds: DepartmentId[];
}

export const positionsApi = {
  baseKey: "positions",
  getPositionsInfinityQueryOptions: (request: GetPositionsRequest) =>
    infiniteQueryOptions({
      queryKey: [
        positionsApi.baseKey,
        request.search,
        request.departmentIds,
        request.isActive,
        request.sortBy,
        request.sortDirection,
        request.pageSize,
      ],
      queryFn: async ({ pageParam, signal }) => {
        const response = await httpClient.get<
          Envelope<PaginationEnvelope<PositionDto>>
        >(API_V1 + "/positions", {
          params: { ...request, page: pageParam },
          signal,
        });

        return response.data;
      },
      ...envelopeInfinityQueryOptions<PositionDto>(request),
    }),
  getPositionQueryOptions: (id: PositionId) =>
    queryOptions({
      queryKey: [positionsApi.baseKey, id],
      queryFn: async ({ signal }) => {
        const response = await httpClient.get<Envelope<PositionDetailDto>>(
          API_V1 + "/positions/" + id,
          { signal },
        );

        return response.data;
      },
    }),
  positionCreate: async (
    request: PositionCreateRequest,
  ): Promise<Envelope<string>> => {
    const response = await httpClient.post<Envelope<string>>(
      API_V1 + "/positions",
      request,
    );

    return response.data;
  },
  positionUpdate: async (
    request: PositionUpdateRequest & { id: PositionId },
  ): Promise<Envelope<string>> => {
    const response = await httpClient.put<Envelope<string>>(
      API_V1 + "/positions/" + request.id,
      request,
    );

    return response.data;
  },
  positionDelete: async (id: PositionId): Promise<Envelope<string>> => {
    const response = await httpClient.delete<Envelope<string>>(
      API_V1 + "/positions/" + id,
    );

    return response.data;
  },
  addDepartmentToPosition: async (
    request: AddDepartmentToPositionRequest & { id: PositionId },
  ): Promise<Envelope<string>> => {
    const response = await httpClient.post<Envelope<string>>(
      API_V1 + "/positions/" + request.id + "/department",
      request,
    );

    return response.data;
  },
  removeDepartmentFromPosition: async (
    request: RemoveDepartmentFromPositionRequest & { id: PositionId },
  ): Promise<Envelope<string>> => {
    const response = await httpClient.delete<Envelope<string>>(
      API_V1 + "/positions/" + request.id + "/department",
      { data: request },
    );

    return response.data;
  },
};
