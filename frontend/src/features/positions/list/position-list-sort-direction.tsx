"use client";

import {
  setPositionSortDirection,
  usePositionSortDirection,
} from "@/entities/positions/model/position-list-store";
import { SortDirection } from "@/shared/api/sort-direction";
import {
  Select,
  SelectContent,
  SelectGroup,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from "@/shared/components/ui/select";
import * as SelectPrimitive from "@radix-ui/react-select";

export const PositionListSortDirection = ({
  ...props
}: React.ComponentProps<typeof SelectPrimitive.Trigger> & {
  size?: "sm" | "default";
}) => {
  const sortDirection = usePositionSortDirection();

  return (
    <Select
      value={sortDirection}
      onValueChange={(value) =>
        setPositionSortDirection(value as SortDirection)
      }
    >
      <SelectTrigger {...props}>
        <p>Сортировать по:</p>
        <SelectValue />
      </SelectTrigger>
      <SelectContent>
        <SelectGroup>
          <SelectItem value="asc">Возрастанию</SelectItem>
          <SelectItem value="desc">Убыванию</SelectItem>
        </SelectGroup>
      </SelectContent>
    </Select>
  );
};
