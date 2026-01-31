"use client";

import { DepartmentListId } from "@/entities/departments/model/department-list-store";
import { useDepartmentVariant } from "@/entities/departments/model/department-view-store";
import { DepartmentListSearch } from "@/features/departments/list/department-list-search";

export const DepartmentViewSearch = ({
  stateId,
}: {
  stateId?: DepartmentListId;
}) => {
  const variant = useDepartmentVariant();

  return variant === "tree" ? (
    <></>
  ) : (
    <>
      <DepartmentListSearch stateId={stateId} />
    </>
  );
};
