"use client";

import {
  DepartmentListId,
  DepartmentParentState,
  setDepartmentParent,
  useDepartmentParent,
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

export const DepartmentListParent = ({
  stateId,
  ...props
}: React.ComponentProps<typeof SelectPrimitive.Trigger> & {
  size?: "sm" | "default";
  stateId: DepartmentListId;
}) => {
  const parentState = useDepartmentParent(stateId);

  return (
    <Select
      value={parentState}
      onValueChange={(value) =>
        setDepartmentParent(stateId, value as DepartmentParentState)
      }
    >
      <SelectTrigger {...props}>
        <p>Подразделения:</p>
        <SelectValue />
      </SelectTrigger>
      <SelectContent>
        <SelectGroup>
          <SelectItem value="all">Все</SelectItem>
          <SelectItem value="parent">Родители</SelectItem>
          <SelectItem value="child">Дочерние</SelectItem>
        </SelectGroup>
      </SelectContent>
    </Select>
  );
};
