import { locationsApi } from "@/entities/locations/api";
import { LocationList } from "@/widgets/locations/location-list";

export const LocationsPage = async () => {
  const locations = await locationsApi.getLocations();

  return (
    <div className="flex justify-center items-center bg-background w-full">
      <LocationList locations={locations} />
    </div>
  );
};