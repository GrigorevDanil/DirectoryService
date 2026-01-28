"use client";

import {
  setPositionPageSize,
  usePositionPageSize,
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

export const PositionListPageSize = ({
  ...props
}: React.ComponentProps<typeof SelectPrimitive.Trigger> & {
  size?: "sm" | "default";
}) => {
  const pageSize = usePositionPageSize();

  return (
    <Select
      value={pageSize.toString()}
      onValueChange={(value) => setPositionPageSize(parseInt(value))}
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
