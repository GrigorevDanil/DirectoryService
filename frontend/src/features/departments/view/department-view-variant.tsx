"use client";

import {
  DepartmentVariant,
  setDepartmentVariant,
  useDepartmentVariant,
} from "@/entities/departments/model/department-view-store";
import {
  SegmentControl,
  SegmentControlItem,
  SegmentControlProps,
} from "@/shared/components/ui/segment-control";
import { List, ListTree } from "lucide-react";

export const DepartmentViewVariant = ({ ...props }: SegmentControlProps) => {
  const variant = useDepartmentVariant();

  return (
    <SegmentControl
      value={variant}
      onValueChange={(value) =>
        setDepartmentVariant(value as DepartmentVariant)
      }
      {...props}
    >
      <SegmentControlItem value="tree">
        <ListTree />
      </SegmentControlItem>
      <SegmentControlItem value="list">
        <List />
      </SegmentControlItem>
    </SegmentControl>
  );
};
