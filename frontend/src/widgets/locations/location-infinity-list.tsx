"use client";

import { Spinner } from "@/shared/components/ui/spinner";
import { Error } from "../error";
import { cn } from "@/shared/lib/utils";
import { LocationCard } from "./location-card";
import { useLocationInfinityList } from "@/entities/locations/hooks/use-location-infinity-list";
import { useIntersectionObserver } from "@/shared/hooks/use-intersection-observer";

export const LocationInfinityList = ({
  className,
  ...props
}: React.ComponentProps<"div">) => {
  const {
    isPending,
    isFetching,
    isFetchingNextPage,
    locations,
    error,
    refetch,
    fetchNextPage,
    hasNextPage,
  } = useLocationInfinityList();

  const intersectionRef = useIntersectionObserver({
    hasNextPage,
    fetchNextPage,
    isFetchingNextPage,
  });

  if (isPending && locations.length === 0) {
    return <Spinner />;
  }

  if (error) {
    return <Error error={error} reset={refetch} />;
  }

  if (locations.length === 0 && !isFetching) {
    return (
      <div className="text-center py-12 text-muted-foreground">
        Нет доступных локаций
      </div>
    );
  }

  return (
    <div className={cn("space-y-6", className)} {...props}>
      <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4 gap-6">
        {locations.map((location) => (
          <LocationCard key={location.id} location={location} />
        ))}
      </div>

      <div
        ref={intersectionRef}
        className="col-span-full flex justify-center py-8"
      >
        {isFetchingNextPage && <Spinner />}
      </div>
    </div>
  );
};
