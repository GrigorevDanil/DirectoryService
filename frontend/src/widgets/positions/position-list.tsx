"use client";

import { Spinner } from "@/shared/components/ui/spinner";
import { Error } from "../error";
import { cn } from "@/shared/lib/utils";
import { usePositionList } from "@/entities/positions/hooks/use-position-list";
import { PositionCard } from "./position-card";
import { PositionListId } from "@/entities/positions/model/position-list-store";
import { GetPositionsRequest } from "@/entities/positions/api";

export const PositionList = ({
  stateId,
  request,
  className,
  ...props
}: React.ComponentProps<"div"> & {
  stateId?: PositionListId;
  request?: GetPositionsRequest;
}) => {
  const {
    isPending,
    isFetching,
    isFetchingNextPage,
    positions,
    error,
    refetch,
    cursorRef,
  } = usePositionList({ stateId, request });

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
      <div className="grid gap-6 grid-cols-[repeat(auto-fit,minmax(280px,1fr))]">
        {positions.map((position) => (
          <PositionCard key={position.id} position={position} />
        ))}
      </div>

      <div ref={cursorRef} className="col-span-full flex justify-center py-8">
        {isFetchingNextPage && <Spinner />}
      </div>
    </div>
  );
};
