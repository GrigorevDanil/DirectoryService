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
import {
  removeSelectedDepartmentFromLocationList,
  setLocationSelectedDepartments,
  useLocationSelectedDepartments,
} from "@/entities/locations/model/location-list-store";
import { DepartmentMultiSelectSelected } from "@/widgets/departments/multi-select/department-multi-select-selected";

export function LocationListSelectDepartments() {
  const selectedDepartments = useLocationSelectedDepartments();

  const [open, setOpen] = useState(false);

  return (
    <>
      <DepartmentMultiSelectSelected
        selectedDepartments={selectedDepartments}
        onRemove={removeSelectedDepartmentFromLocationList}
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
            onChangeChecked={setLocationSelectedDepartments}
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
