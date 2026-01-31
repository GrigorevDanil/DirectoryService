"use client";

import {
  LocationListId,
  LocationSortBy,
  setLocationSortBy,
  useLocationSortBy,
} from "@/entities/locations/model/location-list-store";
import {
  Select,
  SelectContent,
  SelectGroup,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from "@/shared/components/ui/select";
import * as SelectPrimitive from "@radix-ui/react-select";

export const LocationListSortBy = ({
  stateId,
  ...props
}: React.ComponentProps<typeof SelectPrimitive.Trigger> & {
  size?: "sm" | "default";
  stateId?: LocationListId;
}) => {
  const sortBy = useLocationSortBy(stateId);

  return (
    <Select
      value={sortBy}
      onValueChange={(value) =>
        setLocationSortBy(value as LocationSortBy, stateId)
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
        </SelectGroup>
      </SelectContent>
    </Select>
  );
};
