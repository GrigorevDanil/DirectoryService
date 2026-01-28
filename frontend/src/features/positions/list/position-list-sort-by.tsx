"use client";

import {
  PositionSortBy,
  setPositionSortBy,
  usePositionSortBy,
} from "@/entities/positions/model/position-list-store";
import {
  Select,
  SelectContent,
  SelectGroup,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from "@/shared/components/ui/select";
import * as SelectPrimitive from "@radix-ui/react-select";

export const PositionListSortBy = ({
  ...props
}: React.ComponentProps<typeof SelectPrimitive.Trigger> & {
  size?: "sm" | "default";
}) => {
  const sortBy = usePositionSortBy();

  return (
    <Select
      value={sortBy}
      onValueChange={(value) => setPositionSortBy(value as PositionSortBy)}
    >
      <SelectTrigger {...props}>
        <p>Сортировать по:</p>
        <SelectValue />
      </SelectTrigger>
      <SelectContent>
        <SelectGroup>
          <SelectItem value="name">Названию</SelectItem>
          <SelectItem value="countDepartments">
            Количеству подразделений
          </SelectItem>
          <SelectItem value="createdAt">Дате создания</SelectItem>
        </SelectGroup>
      </SelectContent>
    </Select>
  );
};
