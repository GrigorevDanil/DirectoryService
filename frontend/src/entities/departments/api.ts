import { httpClient } from "@/shared/api/http-client";
import {
  Envelope,
  envelopeInfinityQueryOptions,
  PaginationEnvelope,
} from "@/shared/api/envelops";
import { SortDirection } from "@/shared/api/sort-direction";
import { infiniteQueryOptions } from "@tanstack/react-query";
import { InfinityScrollRequest } from "@/shared/api/infinity-scroll-request";
import { DepartmentShortDto } from "./types";

export type DepartmentSortBy = "name" | "createdAt" | "path";

export interface GetDepartmentsRequest extends InfinityScrollRequest {
  search?: string;
  departmentIds?: string[];
  isActive?: boolean;
  isParent?: boolean;
  sortBy?: string;
  sortDirection?: SortDirection;
}

export const departmentsApi = {
  baseKey: "departments",
  getDepartmentsInfinityQueryOptions: (request: GetDepartmentsRequest) =>
    infiniteQueryOptions({
      queryKey: [
        departmentsApi.baseKey,
        request.search,
        request.departmentIds,
        request.isActive,
        request.isParent,
        request.sortBy,
        request.sortDirection,
        request.pageSize,
      ],
      queryFn: async ({ pageParam }) => {
        const response = await httpClient.get<
          Envelope<PaginationEnvelope<DepartmentShortDto>>
        >("/departments", { params: { ...request, page: pageParam } });

        return response.data;
      },
      ...envelopeInfinityQueryOptions<DepartmentShortDto>(request),
    }),
};
