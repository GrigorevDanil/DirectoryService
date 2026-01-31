import { httpClient } from "@/shared/api/http-client";
import {
  Envelope,
  envelopeInfinityQueryOptions,
  PaginationEnvelope,
} from "@/shared/api/envelops";
import { SortDirection } from "@/shared/api/sort-direction";
import { infiniteQueryOptions, queryOptions } from "@tanstack/react-query";
import { InfinityScrollRequest } from "@/shared/api/infinity-scroll-request";
import { DepartmentDto, DepartmentId, DepartmentShortDto } from "./types";
import { LocationId } from "../locations/types";

export type DepartmentSortBy = "name" | "createdAt" | "path";

export interface GetDepartmentsRequest extends InfinityScrollRequest {
  search?: string;
  locationIds?: LocationId[];
  parentId?: DepartmentId;
  isActive?: boolean;
  isParent?: boolean;
  sortBy?: string;
  sortDirection?: SortDirection;
}

export interface GetRootDepartmentsRequest extends InfinityScrollRequest {
  prefetch?: number;
}

export interface DepartmentCreateRequest {
  name: string;
  identifier: string;
  parentId: DepartmentId | null;
  locationIds: LocationId[];
}

export interface DepartmentUpdateRequest {
  name: string;
  identifier: string;
}

export const departmentsApi = {
  baseKey: "departments",
  getDepartmentsInfinityQueryOptions: (request: GetDepartmentsRequest) =>
    infiniteQueryOptions({
      queryKey: [
        departmentsApi.baseKey,
        request.search,
        request.locationIds,
        request.isActive,
        request.parentId,
        request.isParent,
        request.sortBy,
        request.sortDirection,
        request.pageSize,
      ],
      queryFn: async ({ pageParam, signal }) => {
        const response = await httpClient.get<
          Envelope<PaginationEnvelope<DepartmentShortDto>>
        >("/departments", {
          params: { ...request, page: pageParam },
          signal,
        });

        return response.data;
      },
      ...envelopeInfinityQueryOptions<DepartmentShortDto>(request),
    }),
  getRootDepartmentsInfinityQueryOptions: (
    request: GetRootDepartmentsRequest,
  ) =>
    infiniteQueryOptions({
      queryKey: [departmentsApi.baseKey, request.prefetch, request.pageSize],
      queryFn: async ({ pageParam, signal }) => {
        const response = await httpClient.get<
          Envelope<PaginationEnvelope<DepartmentDto>>
        >("/departments/roots", {
          params: { ...request, page: pageParam },
          signal,
        });

        return response.data;
      },
      ...envelopeInfinityQueryOptions<DepartmentDto>(request),
    }),
  getChildrenDepartmentInfinityQueryOptions: (
    request: InfinityScrollRequest & { parentId: DepartmentId },
  ) =>
    infiniteQueryOptions({
      queryKey: [departmentsApi.baseKey, request.parentId, request.pageSize],
      queryFn: async ({ pageParam, signal }) => {
        const response = await httpClient.get<
          Envelope<PaginationEnvelope<DepartmentDto>>
        >("/departments/" + request.parentId + "/children", {
          params: { ...request, page: pageParam },
          signal,
        });

        return response.data;
      },
      ...envelopeInfinityQueryOptions<DepartmentDto>(request),
    }),
  getDepartmentQueryOptions: (id: DepartmentId) =>
    queryOptions({
      queryKey: [departmentsApi.baseKey, id],
      queryFn: async ({ signal }) => {
        const response = await httpClient.get<Envelope<DepartmentDto>>(
          "/departments/" + id,
          { signal },
        );

        return response.data;
      },
    }),
  departmentCreate: async (
    request: DepartmentCreateRequest,
  ): Promise<Envelope<DepartmentId>> => {
    const response = await httpClient.post<Envelope<DepartmentId>>(
      "/departments",
      request,
    );

    return response.data;
  },
  departmentUpdate: async (
    request: DepartmentUpdateRequest & { id: DepartmentId },
  ): Promise<Envelope<DepartmentId>> => {
    const response = await httpClient.put<Envelope<DepartmentId>>(
      "/departments/" + request.id,
      request,
    );

    return response.data;
  },
  departmentDelete: async (
    id: DepartmentId,
  ): Promise<Envelope<DepartmentId>> => {
    const response = await httpClient.delete<Envelope<DepartmentId>>(
      "/departments/" + id,
    );

    return response.data;
  },
  departmentMove: async (
    request: { parentId: DepartmentId | null } & { id: DepartmentId },
  ): Promise<Envelope<DepartmentId>> => {
    const response = await httpClient.patch<Envelope<DepartmentId>>(
      "/departments/" + request.id + "/parent",
      request,
    );

    return response.data;
  },
  departmentSetLocation: async (
    request: { locationIds: LocationId[] } & { id: DepartmentId },
  ): Promise<Envelope<DepartmentId>> => {
    const response = await httpClient.patch<Envelope<DepartmentId>>(
      "/departments/" + request.id + "/locations",
      request,
    );

    return response.data;
  },
};
