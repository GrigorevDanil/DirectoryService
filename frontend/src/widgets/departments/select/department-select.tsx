"use client";

import { useDepartmentList } from "@/entities/departments/hooks/use-department-list";
import { DepartmentId, DepartmentShortDto } from "@/entities/departments/types";
import { Spinner } from "@/shared/components/ui/spinner";
import { useIntersectionObserver } from "@/shared/hooks/use-intersection-observer";
import { Error } from "@/widgets/error";
import { DepartmentSelectCard } from "./department-select-card";
import { DepartmentListId } from "@/entities/departments/model/department-list-store";
import { DepartmentListSearch } from "@/features/departments/list/department-list-search";
import { DepartmentListSortBy } from "@/features/departments/list/department-list-sort-by";
import { DepartmentListSortDirection } from "@/features/departments/list/department-list-sort-direction";
import { DepartmentListActive } from "@/features/departments/list/department-list-active";
import { DepartmentListPageSize } from "@/features/departments/list/department-list-page-size";
import { ListLayout } from "@/shared/components/ui/list-layout";
import { DepartmentListIsParent } from "@/features/departments/list/department-list-is-parent";
import { DepartmentSelected } from "./department-selected";
import { GetDepartmentsRequest } from "@/entities/departments/api";
import { useMemo } from "react";

const { Header, Container, Content, Filters } = ListLayout;

interface DepartmentSelectProps extends React.ComponentProps<"div"> {
  selectedDepartments: DepartmentShortDto[];
  onChangeChecked: (selectedDepartments: DepartmentShortDto[]) => void;
  stateId: DepartmentListId;
  request?: GetDepartmentsRequest;
  multiselect?: boolean;
  excludeIds?: DepartmentId[];
}

export const DepartmentSelect = ({
  selectedDepartments,
  onChangeChecked,
  stateId,
  className,
  request,
  multiselect = true,
  excludeIds = [],
  ...props
}: DepartmentSelectProps) => {
  const {
    departments: fetchedDepartments,
    error,
    fetchNextPage,
    hasNextPage,
    isFetching,
    isFetchingNextPage,
    isPending,
    refetch,
  } = useDepartmentList({ stateId, request });

  const departments = useMemo(() => {
    if (excludeIds.length === 0) {
      return fetchedDepartments;
    }

    const excludeSet = new Set(excludeIds);
    return fetchedDepartments.filter((d) => !excludeSet.has(d.id));
  }, [fetchedDepartments, excludeIds]);

  const intersectionRef = useIntersectionObserver({
    hasNextPage,
    fetchNextPage,
    isFetchingNextPage,
  });

  const handleCheckedChange = (
    selected: boolean,
    department: DepartmentShortDto,
  ) => {
    if (multiselect) {
      if (selected) {
        onChangeChecked([...selectedDepartments, department]);
      } else {
        onChangeChecked(
          selectedDepartments.filter((dep) => dep.id !== department.id),
        );
      }
      return;
    }

    if (selected) {
      onChangeChecked([department]);
    } else {
      onChangeChecked([]);
    }
  };

  const handleRemoveDepartment = (departmentId: DepartmentId) => {
    onChangeChecked(
      selectedDepartments.filter((dep) => dep.id !== departmentId),
    );
  };

  const isSelected = (departmentId: DepartmentId) => {
    return selectedDepartments.some((dep) => dep.id === departmentId);
  };

  const selectedResolved = useMemo(() => {
    const byId = new Map(departments.map((d) => [d.id, d]));

    return selectedDepartments.map((sel) => byId.get(sel.id) ?? sel);
  }, [departments, selectedDepartments]);

  const renderContent = () => {
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
          <DepartmentSelectCard
            key={department.id}
            department={department}
            checked={isSelected(department.id)}
            onCheckedChange={handleCheckedChange}
          />
        ))}
        <div ref={intersectionRef} className="flex justify-center">
          {isFetchingNextPage && <Spinner />}
        </div>
      </div>
    );
  };

  return (
    <ListLayout className={className} {...props}>
      <DepartmentSelected
        selectedDepartments={selectedResolved}
        onRemove={handleRemoveDepartment}
      />
      <Header>
        <DepartmentListSearch stateId={stateId} />
      </Header>
      <Container>
        <Content className="min-w-75">{renderContent()}</Content>
        <Filters>
          <h3 className="font-medium text-sm mb-3">Фильтры</h3>
          <div className="space-y-3">
            {!request?.sortBy && <DepartmentListSortBy stateId={stateId} />}
            {!request?.sortDirection && (
              <DepartmentListSortDirection stateId={stateId} />
            )}
            {!request?.isActive && <DepartmentListActive stateId={stateId} />}
            {!request?.isParent && <DepartmentListIsParent stateId={stateId} />}
            {!request?.pageSize && <DepartmentListPageSize stateId={stateId} />}
          </div>
        </Filters>
      </Container>
    </ListLayout>
  );
};
