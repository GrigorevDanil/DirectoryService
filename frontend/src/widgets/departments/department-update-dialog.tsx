"use client";

import { DepartmentId } from "@/entities/departments/types";
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
import { DepartmentUpdateForm } from "../../features/departments/department-update-form";

export function DepartmentUpdateDialog({
  deparment,
}: {
  deparment: {
    id: DepartmentId;
    name: string;
    identifier: string;
    isActive: boolean;
  };
}) {
  const [open, setOpen] = useState(false);

  const handleSuccess = () => {
    setOpen(false);
  };

  return (
    <Dialog open={open} onOpenChange={(flag) => setOpen(flag)}>
      <DialogTrigger asChild disabled={!deparment.isActive}>
        <Button>Изменить</Button>
      </DialogTrigger>
      <DialogContent className="sm:max-w-175 flex max-h-[90vh] flex-col">
        <DialogHeader>
          <DialogTitle>Обновление подразделения</DialogTitle>
        </DialogHeader>

        <div className="flex-1 overflow-y-auto px-2">
          <DepartmentUpdateForm
            onSuccess={handleSuccess}
            deparment={deparment}
          />
        </div>

        <DialogFooter>
          <DialogClose asChild>
            <Button variant="outline">Отмена</Button>
          </DialogClose>
        </DialogFooter>
      </DialogContent>
    </Dialog>
  );
}
