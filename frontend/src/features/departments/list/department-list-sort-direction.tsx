"use client";

import {
  DepartmentListId,
  setDepartmentSortDirection,
  useDepartmentSortDirection,
} from "@/entities/departments/model/department-list-store";
import { SortDirection } from "@/shared/api/sort-direction";
import {
  Select,
  SelectContent,
  SelectGroup,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from "@/shared/components/ui/select";
import * as SelectPrimitive from "@radix-ui/react-select";

export const DepartmentListSortDirection = ({
  stateId,
  ...props
}: React.ComponentProps<typeof SelectPrimitive.Trigger> & {
  size?: "sm" | "default";
  stateId?: DepartmentListId;
}) => {
  const sortDirection = useDepartmentSortDirection(stateId);

  return (
    <Select
      value={sortDirection}
      onValueChange={(value) =>
        setDepartmentSortDirection(value as SortDirection, stateId)
      }
    >
      <SelectTrigger {...props}>
        <p>Сортировать по:</p>
        <SelectValue />
      </SelectTrigger>
      <SelectContent>
        <SelectGroup>
          <SelectItem value="asc">Возрастанию</SelectItem>
          <SelectItem value="desc">Убыванию</SelectItem>
        </SelectGroup>
      </SelectContent>
    </Select>
  );
};
