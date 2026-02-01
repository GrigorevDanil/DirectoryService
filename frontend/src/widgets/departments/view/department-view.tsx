"use client";

import { useDepartmentVariant } from "@/entities/departments/model/department-view-store";
import { DepartmentTree } from "../tree/department-tree";
import { DepartmentList } from "../department-list";
import { DepartmentId } from "@/entities/departments/types";
import { DepartmentListId } from "@/entities/departments/model/department-list-store";

interface DepartmentViewProps {
  parentId?: DepartmentId;
  stateId?: DepartmentListId;
}

export const DepartmentView = ({ parentId, stateId }: DepartmentViewProps) => {
  const variant = useDepartmentVariant();

  return variant === "tree" ? (
    <DepartmentTree parentId={parentId} />
  ) : (
    <DepartmentList request={{ parentId }} stateId={stateId} />
  );
};
