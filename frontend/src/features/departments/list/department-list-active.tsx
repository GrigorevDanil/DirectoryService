"use client";

import {
  DepartmentListId,
  setDepartmentActive,
  useDepartmentActive,
} from "@/entities/departments/model/department-list-store";
import { ActiveState } from "@/shared/api/active-state";
import {
  Select,
  SelectContent,
  SelectGroup,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from "@/shared/components/ui/select";
import * as SelectPrimitive from "@radix-ui/react-select";

export const DepartmentListActive = ({
  stateId,
  ...props
}: React.ComponentProps<typeof SelectPrimitive.Trigger> & {
  size?: "sm" | "default";
  stateId: DepartmentListId;
}) => {
  const activeState = useDepartmentActive(stateId);

  return (
    <Select
      value={activeState}
      onValueChange={(value) =>
        setDepartmentActive(stateId, value as ActiveState)
      }
    >
      <SelectTrigger {...props}>
        <p>Состояние активности:</p>
        <SelectValue />
      </SelectTrigger>
      <SelectContent>
        <SelectGroup>
          <SelectItem value="all">Все</SelectItem>
          <SelectItem value="active">Активные</SelectItem>
          <SelectItem value="unactive">Неактивные</SelectItem>
        </SelectGroup>
      </SelectContent>
    </Select>
  );
};
