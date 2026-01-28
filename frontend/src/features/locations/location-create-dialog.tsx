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
import { LocationCreateForm } from "./location-create-form";
import { useState } from "react";

export function LocationCreateDialog() {
  const [open, setOpen] = useState(false);

  const handleSuccess = () => {
    setOpen(false);
  };

  return (
    <Dialog open={open} onOpenChange={(flag) => setOpen(flag)}>
      <DialogTrigger asChild>
        <Button>Добавить локацию</Button>
      </DialogTrigger>
      <DialogContent className="sm:max-w-175 flex max-h-[90vh] flex-col">
        <DialogHeader>
          <DialogTitle>Добавление локации</DialogTitle>
        </DialogHeader>

        <div className="flex-1 overflow-y-auto px-2">
          <LocationCreateForm onSuccess={handleSuccess} />
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
