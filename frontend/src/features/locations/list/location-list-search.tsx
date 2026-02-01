"use client";

import {
  LocationListId,
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
  stateId,
  ...props
}: React.ComponentProps<"input"> & {
  stateId?: LocationListId;
}) => {
  const search = useLocationSearch(stateId);

  return (
    <InputGroup>
      <InputGroupInput
        placeholder="Поиск..."
        value={search}
        onChange={(e) => {
          setLocationSearch(e.target.value, stateId);
        }}
        {...props}
      />
      <InputGroupAddon>
        <Search />
      </InputGroupAddon>
    </InputGroup>
  );
};
