"use client";

import { useDepartmentVariant } from "@/entities/departments/model/department-view-store";
import { DepartmentListActive } from "@/features/departments/list/department-list-active";
import { DepartmentListPageSize } from "@/features/departments/list/department-list-page-size";
import { DepartmentListIsParent } from "@/features/departments/list/department-list-is-parent";
import { DepartmentListSelectLocations } from "@/features/departments/list/department-list-select-locations";
import { DepartmentListParent } from "@/features/departments/list/department-list-parent";
import { DepartmentListSortBy } from "@/features/departments/list/department-list-sort-by";
import { DepartmentListSortDirection } from "@/features/departments/list/department-list-sort-direction";
import { ListLayout } from "@/shared/components/ui/list-layout";
import { DepartmentListId } from "@/entities/departments/model/department-list-store";

export const DepartmentViewFilters = ({
  stateId,
}: {
  stateId?: DepartmentListId;
}) => {
  const variant = useDepartmentVariant();

  return variant === "tree" ? (
    <></>
  ) : (
    <>
      <ListLayout.Filters>
        <h3 className="font-medium text-sm mb-3">Фильтры</h3>
        <div className="space-y-3">
          <DepartmentListSortBy stateId={stateId} />
          <DepartmentListSortDirection stateId={stateId} />
          <DepartmentListActive stateId={stateId} />
          <DepartmentListIsParent stateId={stateId} />
          <DepartmentListParent stateId={stateId} />
          <DepartmentListPageSize stateId={stateId} />
          <DepartmentListSelectLocations stateId={stateId} />
        </div>
      </ListLayout.Filters>
    </>
  );
};
