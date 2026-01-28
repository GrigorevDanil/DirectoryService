"use client";

import { Spinner } from "@/shared/components/ui/spinner";
import { Error } from "../error";
import { cn } from "@/shared/lib/utils";
import { usePositionList } from "@/entities/positions/hooks/use-position-list";
import { PositionCard } from "./position-card";
import { useIntersectionObserver } from "@/shared/hooks/use-intersection-observer";

export const PositionList = ({
  className,
  ...props
}: React.ComponentProps<"div">) => {
  const {
    isPending,
    isFetching,
    isFetchingNextPage,
    positions,
    error,
    refetch,
    fetchNextPage,
    hasNextPage,
  } = usePositionList();

  const intersectionRef = useIntersectionObserver({
    hasNextPage,
    fetchNextPage,
    isFetchingNextPage,
  });

  if (isPending && positions.length === 0) {
    return <Spinner />;
  }

  if (error) {
    return <Error error={error} reset={refetch} />;
  }

  if (positions.length === 0 && !isFetching) {
    return (
      <div className="text-center py-12 text-muted-foreground">
        Нет доступных позиций
      </div>
    );
  }

  return (
    <div className={cn("space-y-6", className)} {...props}>
      <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4 gap-6">
        {positions.map((position) => (
          <PositionCard key={position.id} position={position} />
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
