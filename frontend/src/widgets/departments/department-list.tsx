"use client";

import { useDepartmentList } from "@/entities/departments/hooks/use-department-list";
import { Spinner } from "@/shared/components/ui/spinner";
import { useIntersectionObserver } from "@/shared/hooks/use-intersection-observer";
import { Error } from "../error";
import { DepartmentShortCard } from "./department-short-card";
import { GetDepartmentsRequest } from "@/entities/departments/api";
import { DepartmentListId } from "@/entities/departments/model/department-list-store";

interface DepartmentListProps {
  request?: GetDepartmentsRequest;
  stateId?: DepartmentListId;
}

export const DepartmentList = ({ request, stateId }: DepartmentListProps) => {
  const {
    departments,
    error,
    fetchNextPage,
    hasNextPage,
    isFetching,
    isFetchingNextPage,
    isPending,
    refetch,
  } = useDepartmentList({ request, stateId });

  const intersectionRef = useIntersectionObserver({
    hasNextPage,
    fetchNextPage,
    isFetchingNextPage,
  });

  if (isPending && departments.length === 0) {
    return (
      <div className="flex justify-center">
        <Spinner />
      </div>
    );
  }

  if (error) {
    return <Error error={error} reset={refetch} />;
  }

  if (departments.length === 0 && !isFetching) {
    return (
      <div className="text-center text-muted-foreground">
        Нет доступных подразделений
      </div>
    );
  }

  return (
    <div className="flex flex-col gap-2 w-full">
      {departments.map((department) => (
        <DepartmentShortCard key={department.id} department={department} />
      ))}
      <div ref={intersectionRef} className="flex justify-center">
        {isFetchingNextPage && <Spinner />}
      </div>
    </div>
  );
};
