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
import { DepartmentMultiSelect } from "@/widgets/departments/multi-select";
import { DepartmentMultiSelectSelected } from "@/widgets/departments/multi-select/department-multi-select-selected";
import {
  removeSelectedDepartmentFromPositionList,
  setPositionSelectedDepartments,
  usePositionSelectedDepartments,
} from "@/entities/positions/model/position-list-store";

export function PositionListSelectDepartments() {
  const selectedDepartments = usePositionSelectedDepartments();

  const [open, setOpen] = useState(false);

  return (
    <>
      <DepartmentMultiSelectSelected
        selectedDepartments={selectedDepartments}
        onRemove={removeSelectedDepartmentFromPositionList}
      />

      <Dialog open={open} onOpenChange={(flag) => setOpen(flag)}>
        <DialogTrigger asChild>
          <Button>Выбрать подразделения</Button>
        </DialogTrigger>
        <DialogContent className="h-[70vh] flex flex-col w-fit max-w-162.5">
          <DialogHeader>
            <DialogTitle>Выбор подразделений</DialogTitle>
          </DialogHeader>

          <DepartmentMultiSelect
            className="flex-1 min-h-0"
            selectedDepartments={selectedDepartments}
            onChangeChecked={setPositionSelectedDepartments}
            stateId="multi-select"
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
