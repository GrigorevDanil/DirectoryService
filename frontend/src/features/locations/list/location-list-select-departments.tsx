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
  LocationListId,
  removeSelectedDepartmentFromLocationList,
  setLocationSelectedDepartments,
  useLocationSelectedDepartments,
} from "@/entities/locations/model/location-list-store";
import { DepartmentSelected } from "@/widgets/departments/select/department-selected";
import { DepartmentSelect } from "@/widgets/departments/select/department-select";

export function LocationListSelectDepartments({
  stateId,
}: {
  stateId?: LocationListId;
}) {
  const selectedDepartments = useLocationSelectedDepartments(stateId);

  const [open, setOpen] = useState(false);

  return (
    <>
      <DepartmentSelected
        selectedDepartments={selectedDepartments}
        onRemove={(id) => removeSelectedDepartmentFromLocationList(id, stateId)}
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
            onChangeChecked={(selectedDepartments) =>
              setLocationSelectedDepartments(selectedDepartments, stateId)
            }
            stateId="multi-select-locations"
            multiselect={false}
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
