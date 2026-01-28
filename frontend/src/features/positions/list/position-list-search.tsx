"use client";

import {
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
  ...props
}: React.ComponentProps<"input">) => {
  const search = usePositionSearch();

  return (
    <InputGroup>
      <InputGroupInput
        placeholder="Поиск..."
        value={search}
        onChange={(e) => {
          setPositionSearch(e.target.value);
        }}
        {...props}
      />
      <InputGroupAddon>
        <Search />
      </InputGroupAddon>
    </InputGroup>
  );
};
