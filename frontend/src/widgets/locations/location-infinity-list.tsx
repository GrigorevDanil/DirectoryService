"use client";

import { useCallback, useRef } from "react";
import { Spinner } from "@/shared/components/ui/spinner";
import { Error } from "../error";
import { cn } from "@/shared/lib/utils";
import { LocationCard } from "./location-card";
import { useLocationInfinityList } from "@/entities/locations/hooks/use-location-infinity-list";

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

  const observerRef = useRef<IntersectionObserver>(null);

  const loadMoreRef = useCallback(
    (node: HTMLDivElement | null) => {
      if (observerRef.current) {
        observerRef.current.disconnect();
      }

      if (!node || !hasNextPage) return;

      observerRef.current = new IntersectionObserver((entries) => {
        if (entries[0].isIntersecting && !isFetchingNextPage) {
          fetchNextPage();
        }
      });

      observerRef.current.observe(node);
    },
    [hasNextPage, isFetchingNextPage, fetchNextPage],
  );

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

      <div ref={loadMoreRef} className="col-span-full flex justify-center py-8">
        {isFetchingNextPage && <Spinner />}
      </div>
    </div>
  );
};
