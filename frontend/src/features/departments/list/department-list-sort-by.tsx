"use client";

import { DepartmentSortBy } from "@/entities/departments/api";
import {
  DepartmentListId,
  setDepartmentSortBy,
  useDepartmentSortBy,
} from "@/entities/departments/model/department-list-store";
import {
  Select,
  SelectContent,
  SelectGroup,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from "@/shared/components/ui/select";
import * as SelectPrimitive from "@radix-ui/react-select";

export const DepartmentListSortBy = ({
  stateId,
  ...props
}: React.ComponentProps<typeof SelectPrimitive.Trigger> & {
  size?: "sm" | "default";
  stateId: DepartmentListId;
}) => {
  const sortBy = useDepartmentSortBy(stateId);

  return (
    <Select
      value={sortBy}
      onValueChange={(value) =>
        setDepartmentSortBy(stateId, value as DepartmentSortBy)
      }
    >
      <SelectTrigger {...props}>
        <p>Сортировать по:</p>
        <SelectValue />
      </SelectTrigger>
      <SelectContent>
        <SelectGroup>
          <SelectItem value="name">Названию</SelectItem>
          <SelectItem value="createdAt">Дате создания</SelectItem>
          <SelectItem value="path">Пути</SelectItem>
        </SelectGroup>
      </SelectContent>
    </Select>
  );
};
