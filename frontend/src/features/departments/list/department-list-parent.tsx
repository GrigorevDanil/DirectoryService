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
  DepartmentListId,
  setDepartmentParent,
  useDepartmentParent,
} from "@/entities/departments/model/department-list-store";
import { DepartmentSelected } from "@/widgets/departments/select/department-selected";
import { DepartmentSelect } from "@/widgets/departments/select/department-select";

export function DepartmentListParent({
  stateId,
}: {
  stateId?: DepartmentListId;
}) {
  const parent = useDepartmentParent(stateId);

  const selectedDepartments = parent ? [parent] : [];

  const [open, setOpen] = useState(false);

  const handleRemove = () => setDepartmentParent(null, stateId);

  return (
    <>
      <DepartmentSelected
        selectedDepartments={selectedDepartments}
        onRemove={handleRemove}
      />

      <Dialog open={open} onOpenChange={(flag) => setOpen(flag)}>
        <DialogTrigger asChild>
          <Button>Выбрать родителя</Button>
        </DialogTrigger>
        <DialogContent className="h-[70vh] flex flex-col w-fit max-w-162.5">
          <DialogHeader>
            <DialogTitle>Выбор родителя</DialogTitle>
          </DialogHeader>

          <DepartmentSelect
            className="flex-1 min-h-0"
            selectedDepartments={selectedDepartments}
            onChangeChecked={(departments) =>
              setDepartmentParent(departments[0], stateId)
            }
            stateId="select-departments"
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
