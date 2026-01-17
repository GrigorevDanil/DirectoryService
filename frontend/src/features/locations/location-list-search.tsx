"use client";

import {
  setLocationSearch,
  useLocationSearch,
} from "@/entities/locations/model/location-list-store";
import {
  InputGroup,
  InputGroupAddon,
  InputGroupInput,
} from "@/shared/components/ui/input-group";
import { Search } from "lucide-react";

export const LocationListSearch = ({
  ...props
}: React.ComponentProps<"input">) => {
  const search = useLocationSearch();

  return (
    <InputGroup>
      <InputGroupInput
        placeholder="Поиск..."
        value={search}
        onChange={(e) => {
          setLocationSearch(e.target.value);
        }}
        {...props}
      />
      <InputGroupAddon>
        <Search />
      </InputGroupAddon>
    </InputGroup>
  );
};
