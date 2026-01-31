"use client";

import {
  DepartmentListId,
  DepartmentParentState,
  setDepartmentIsParent,
  useDepartmentIsParent,
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

export const DepartmentListIsParent = ({
  stateId,
  ...props
}: React.ComponentProps<typeof SelectPrimitive.Trigger> & {
  size?: "sm" | "default";
  stateId?: DepartmentListId;
}) => {
  const parentState = useDepartmentIsParent(stateId);

  return (
    <Select
      value={parentState}
      onValueChange={(value) =>
        setDepartmentIsParent(value as DepartmentParentState, stateId)
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
