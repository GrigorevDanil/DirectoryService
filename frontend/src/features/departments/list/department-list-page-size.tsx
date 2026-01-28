"use client";

import {
  DepartmentListId,
  setDepartmentPageSize,
  useDepartmentPageSize,
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

export const DepartmentListPageSize = ({
  stateId,
  ...props
}: React.ComponentProps<typeof SelectPrimitive.Trigger> & {
  size?: "sm" | "default";
  stateId: DepartmentListId;
}) => {
  const pageSize = useDepartmentPageSize(stateId);

  return (
    <Select
      value={pageSize.toString()}
      onValueChange={(value) => setDepartmentPageSize(stateId, parseInt(value))}
    >
      <SelectTrigger {...props}>
        <p>Количество элементов:</p>
        <SelectValue />
      </SelectTrigger>
      <SelectContent>
        <SelectGroup>
          <SelectItem value="10">10</SelectItem>
          <SelectItem value="20">20</SelectItem>
          <SelectItem value="30">30</SelectItem>
          <SelectItem value="40">40</SelectItem>
          <SelectItem value="50">50</SelectItem>
        </SelectGroup>
      </SelectContent>
    </Select>
  );
};
