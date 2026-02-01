"use client";

import { Button } from "@/shared/components/ui/button";
import {
  Dialog,
  DialogClose,
  DialogContent,
  DialogFooter,
  DialogHeader,
  DialogTitle,
  DialogTrigger,
} from "@/shared/components/ui/dialog";
import { useState } from "react";
import {
  PositionListId,
  removeSelectedDepartmentFromPositionList,
  setPositionSelectedDepartments,
  usePositionSelectedDepartments,
} from "@/entities/positions/model/position-list-store";
import { DepartmentSelected } from "@/widgets/departments/select/department-selected";
import { DepartmentSelect } from "@/widgets/departments/select/department-select";

export function PositionListSelectDepartments({
  stateId,
}: {
  stateId?: PositionListId;
}) {
  const selectedDepartments = usePositionSelectedDepartments(stateId);

  const [open, setOpen] = useState(false);

  return (
    <>
      <DepartmentSelected
        selectedDepartments={selectedDepartments}
        onRemove={(id) => removeSelectedDepartmentFromPositionList(id, stateId)}
      />

      <Dialog open={open} onOpenChange={(flag) => setOpen(flag)}>
        <DialogTrigger asChild>
          <Button>Выбрать подразделения</Button>
        </DialogTrigger>
        <DialogContent className="h-[70vh] flex flex-col w-fit max-w-162.5">
          <DialogHeader>
            <DialogTitle>Выбор подразделений</DialogTitle>
          </DialogHeader>

          <DepartmentSelect
            className="flex-1 min-h-0"
            selectedDepartments={selectedDepartments}
            onChangeChecked={(deps) =>
              setPositionSelectedDepartments(deps, stateId)
            }
            stateId="multi-select-positions"
          />

          <DialogFooter>
            <DialogClose asChild>
              <Button variant="outline">Отмена</Button>
            </DialogClose>
          </DialogFooter>
        </DialogContent>
      </Dialog>
    </>
  );
}
