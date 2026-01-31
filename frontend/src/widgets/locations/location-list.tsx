"use client";

import { useLocationList } from "@/entities/locations/hooks/use-location-list";
import { Spinner } from "@/shared/components/ui/spinner";
import { Error } from "../error";
import { cn } from "@/shared/lib/utils";
import { LocationCard } from "./location-card";

export const LocationList = ({
  className,
  ...props
}: React.ComponentProps<"div">) => {
  const { isPending, locations, error, refetch } = useLocationList();

  if (isPending) {
    return <Spinner />;
  }

  if (error) {
    return <Error error={error} reset={refetch} />;
  }

  if (locations.length === 0) {
    return (
      <div className="text-center py-12 text-muted-foreground">
        Нет доступных локаций
      </div>
    );
  }

  return (
    <div
      className={cn(
        "grid gap-6 grid-cols-[repeat(auto-fit,minmax(280px,1fr))]",
        className,
      )}
      {...props}
    >
      {locations.map((location) => (
        <LocationCard key={location.id} location={location} />
      ))}
    </div>
  );
};
