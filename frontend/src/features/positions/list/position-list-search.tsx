"use client";

import {
  PositionListId,
  setPositionSearch,
  usePositionSearch,
} from "@/entities/positions/model/position-list-store";
import {
  InputGroup,
  InputGroupAddon,
  InputGroupInput,
} from "@/shared/components/ui/input-group";
import { Search } from "lucide-react";

export const PositionListSearch = ({
  stateId,
  ...props
}: React.ComponentProps<"input"> & {
  stateId?: PositionListId;
}) => {
  const search = usePositionSearch(stateId);

  return (
    <InputGroup>
      <InputGroupInput
        placeholder="Поиск..."
        value={search}
        onChange={(e) => {
          setPositionSearch(e.target.value, stateId);
        }}
        {...props}
      />
      <InputGroupAddon>
        <Search />
      </InputGroupAddon>
    </InputGroup>
  );
};
