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
import { usePositionDelete } from "@/entities/positions/hooks/use-position-delete";
import { PositionId } from "@/entities/positions/types";

interface PositionDeleteDialogProps {
  id: PositionId;
  name: string;
}

export function PositionDeleteDialog({ id, name }: PositionDeleteDialogProps) {
  const { positionDeleteAsync, isPending } = usePositionDelete();

  const [open, setOpen] = useState(false);

  const handleDelete = async () => {
    await positionDeleteAsync(id);
    setOpen(false);
  };

  return (
    <Dialog open={open} onOpenChange={(flag) => setOpen(flag)}>
      <DialogTrigger asChild>
        <Button variant="destructive">
          {isPending && <Spinner />}
          <Trash />
        </Button>
      </DialogTrigger>
      <DialogContent className="sm:max-w-175 flex max-h-[90vh] flex-col">
        <DialogHeader>
          <DialogTitle>Удаление позиции</DialogTitle>
        </DialogHeader>

        <DialogDescription>
          {`Вы действительно хотите удалить позицию '${name}'?`}
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
