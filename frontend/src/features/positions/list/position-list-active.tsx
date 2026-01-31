"use client";

import {
  PositionListId,
  setPositionActive,
  usePositionActive,
} from "@/entities/positions/model/position-list-store";
import { ActiveState } from "@/shared/api/active-state";
import {
  Select,
  SelectContent,
  SelectGroup,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from "@/shared/components/ui/select";
import * as SelectPrimitive from "@radix-ui/react-select";

export const PositionListActive = ({
  stateId,
  ...props
}: React.ComponentProps<typeof SelectPrimitive.Trigger> & {
  size?: "sm" | "default";
  stateId?: PositionListId;
}) => {
  const activeState = usePositionActive(stateId);

  return (
    <Select
      value={activeState}
      onValueChange={(value) =>
        setPositionActive(value as ActiveState, stateId)
      }
    >
      <SelectTrigger {...props}>
        <p>Состояние активности:</p>
        <SelectValue />
      </SelectTrigger>
      <SelectContent>
        <SelectGroup>
          <SelectItem value="all">Все</SelectItem>
          <SelectItem value="active">Активные</SelectItem>
          <SelectItem value="unactive">Неактивные</SelectItem>
        </SelectGroup>
      </SelectContent>
    </Select>
  );
};
