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
import { useLocationDelete } from "@/entities/locations/hooks/use-location-delete";
import { Spinner } from "@/shared/components/ui/spinner";
import { Trash } from "lucide-react";

interface LocationDeleteDialogProps {
  id: string;
  name: string;
}

export function LocationDeleteDialog({ id, name }: LocationDeleteDialogProps) {
  const { locationDeleteAsync, isPending } = useLocationDelete();

  const [open, setOpen] = useState(false);

  const handleDelete = async () => {
    await locationDeleteAsync(id);
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
          <DialogTitle>Удаление локации</DialogTitle>
        </DialogHeader>

        <DialogDescription>
          {`Вы действительно хотите удалить локацию '${name}'?`}
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
