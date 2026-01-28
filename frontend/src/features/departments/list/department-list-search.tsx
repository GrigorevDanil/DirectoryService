"use client";

import {
  DepartmentListId,
  setDepartmentSearch,
  useDepartmentSearch,
} from "@/entities/departments/model/department-list-store";
import {
  InputGroup,
  InputGroupAddon,
  InputGroupInput,
} from "@/shared/components/ui/input-group";
import { Search } from "lucide-react";

export const DepartmentListSearch = ({
  stateId,
  ...props
}: React.ComponentProps<"input"> & {
  stateId: DepartmentListId;
}) => {
  const search = useDepartmentSearch(stateId);

  return (
    <InputGroup>
      <InputGroupInput
        placeholder="Поиск..."
        value={search}
        onChange={(e) => {
          setDepartmentSearch(stateId, e.target.value);
        }}
        {...props}
      />
      <InputGroupAddon>
        <Search />
      </InputGroupAddon>
    </InputGroup>
  );
};
