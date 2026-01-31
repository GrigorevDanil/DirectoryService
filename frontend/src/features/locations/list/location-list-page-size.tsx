"use client";

import {
  LocationListId,
  setLocationPageSize,
  useLocationPageSize,
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

export const LocationListPageSize = ({
  stateId,
  ...props
}: React.ComponentProps<typeof SelectPrimitive.Trigger> & {
  size?: "sm" | "default";
  stateId?: LocationListId;
}) => {
  const pageSize = useLocationPageSize(stateId);

  return (
    <Select
      value={pageSize.toString()}
      onValueChange={(value) => setLocationPageSize(parseInt(value), stateId)}
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
