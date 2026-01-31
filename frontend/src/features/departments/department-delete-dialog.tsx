"use client";

import { Button } from "@/shared/components/ui/button";
import {
  Dialog,
  DialogClose,
  DialogContent,
  DialogDescription,
  DialogFooter,
  DialogHeader,
  DialogTitle,
  DialogTrigger,
} from "@/shared/components/ui/dialog";
import { useState } from "react";
import { Spinner } from "@/shared/components/ui/spinner";
import { Trash } from "lucide-react";
import { useDepartmentDelete } from "@/entities/departments/hooks/use-department-delete";
import { DepartmentId } from "@/entities/departments/types";

export function DepartmentDeleteDialog({
  deparment,
}: {
  deparment: {
    id: DepartmentId;
    name: string;
    isActive: boolean;
  };
}) {
  const { departmentDeleteAsync, isPending } = useDepartmentDelete();

  const [open, setOpen] = useState(false);

  const handleDelete = async () => {
    await departmentDeleteAsync(deparment.id);
    setOpen(false);
  };

  return (
    <Dialog open={open} onOpenChange={(flag) => setOpen(flag)}>
      <DialogTrigger asChild disabled={!deparment.isActive}>
        <Button variant="destructive">
          {isPending && <Spinner />}
          <Trash />
        </Button>
      </DialogTrigger>
      <DialogContent className="sm:max-w-175 flex max-h-[90vh] flex-col">
        <DialogHeader>
          <DialogTitle>Удаление подразделения</DialogTitle>
        </DialogHeader>

        <DialogDescription>
          {`Вы действительно хотите удалить подразделение '${deparment.name}'?`}
        </DialogDescription>

        <DialogFooter>
          <Button onClick={handleDelete}>
            {isPending && <Spinner />}Удалить
          </Button>

          <DialogClose asChild>
            <Button variant="outline">Отмена</Button>
          </DialogClose>
        </DialogFooter>
      </DialogContent>
    </Dialog>
  );
}
