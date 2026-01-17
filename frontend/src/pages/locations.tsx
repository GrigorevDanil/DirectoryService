import { LocationCreateDialog } from "@/features/locations/location-create-dialog";
import { LocationListActive } from "@/features/locations/location-list-active";
import { LocationListPageSize } from "@/features/locations/location-list-page-size";
import { LocationListSearch } from "@/features/locations/location-list-search";
import { LocationListSortBy } from "@/features/locations/location-list-sort-by";
import { LocationListSortDirection } from "@/features/locations/location-sort-direction";
import { LocationInfinityList } from "@/widgets/locations/location-infinity-list";

export const LocationsPage = () => {
  return (
    <div className="flex flex-col gap-2 justify-center bg-background w-full p-2">
      <div className="flex gap-2 justify-between flex-wrap">
        <LocationListSearch />
        <LocationListSortBy />
        <LocationListSortDirection />
        <LocationListActive />
        <LocationListPageSize />
        <LocationCreateDialog />
      </div>

      <LocationInfinityList />
    </div>
  );
};
