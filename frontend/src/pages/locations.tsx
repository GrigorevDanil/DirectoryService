import { LocationCreateDialog } from "@/features/locations/location-create-dialog";
import { LocationList } from "@/widgets/locations/location-list";

export const LocationsPage = () => {
  return (
    <div className="flex flex-col gap-2 justify-center bg-background w-full p-2">
      <LocationCreateDialog />
      <LocationList />
    </div>
  );
};
