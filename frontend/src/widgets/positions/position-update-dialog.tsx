"use client";

import { PositionDto } from "@/entities/positions/types";
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
import { PositionUpdateForm } from "../../features/positions/position-update-form";

export function PositionUpdateDialog({ position }: { position: PositionDto }) {
  const [open, setOpen] = useState(false);

  const handleSuccess = () => {
    setOpen(false);
  };

  return (
    <Dialog open={open} onOpenChange={(flag) => setOpen(flag)}>
      <DialogTrigger asChild>
        <Button>Изменить</Button>
      </DialogTrigger>
      <DialogContent className="sm:max-w-175 flex max-h-[90vh] flex-col">
        <DialogHeader>
          <DialogTitle>Обновление локации</DialogTitle>
        </DialogHeader>

        <div className="flex-1 overflow-y-auto px-2">
          <PositionUpdateForm onSuccess={handleSuccess} position={position} />
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
