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
  removeSelectedLocationFromDepartmentList,
  setDepartmentSelectedLocations,
  useDepartmentSelectedLocations,
} from "@/entities/departments/model/department-list-store";
import { LocationSelected } from "@/widgets/locations/select/location-selected";
import { LocationSelect } from "@/widgets/locations/select/location-select";

export function DepartmentListSelectLocations({
  stateId,
}: {
  stateId?: DepartmentListId;
}) {
  const selectedLocations = useDepartmentSelectedLocations(stateId);

  const [open, setOpen] = useState(false);

  return (
    <>
      <LocationSelected
        selectedLocations={selectedLocations}
        onRemove={(id) => removeSelectedLocationFromDepartmentList(id, stateId)}
      />

      <Dialog open={open} onOpenChange={(flag) => setOpen(flag)}>
        <DialogTrigger asChild>
          <Button>Выбрать локации</Button>
        </DialogTrigger>
        <DialogContent className="h-[70vh] flex flex-col w-fit max-w-162.5">
          <DialogHeader>
            <DialogTitle>Выбор локаций</DialogTitle>
          </DialogHeader>

          <LocationSelect
            className="flex-1 min-h-0"
            selectedLocations={selectedLocations}
            onChangeChecked={(selectedLocations) =>
              setDepartmentSelectedLocations(selectedLocations, stateId)
            }
            stateId="multi-select-departments"
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
