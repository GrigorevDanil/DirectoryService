"use client";

import { useDepartmentRoots } from "@/entities/departments/hooks/use-department-roots";
import {
  setDepartmentExpandedNodes,
  useDepartmentExpandedNodes,
} from "@/entities/departments/model/department-tree-store";
import { DepartmentId } from "@/entities/departments/types";
import { TreeProvider, TreeView } from "@/shared/components/ui/tree";
import { useIntersectionObserver } from "@/shared/hooks/use-intersection-observer";
import { DepartmentNode } from "./department-node";
import { useDepartmentChildren } from "@/entities/departments/hooks/use-department-children";
import { Spinner } from "@/shared/components/ui/spinner";
import { Error } from "@/widgets/error";

export const DepartmentTree = ({ parentId }: { parentId?: DepartmentId }) => {
  if (parentId) return <DepartmentTreeChildren parentId={parentId} />;
  return <DepartmentTreeRoots />;
};

export const DepartmentTreeRoots = () => {
  const expandedNodes = useDepartmentExpandedNodes();

  const {
    departments,
    hasNextPage,
    fetchNextPage,
    isFetchingNextPage,
    isPending,
    error,
    isFetching,
    refetch,
  } = useDepartmentRoots();

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
    <TreeProvider
      showLines
      showIcons
      selectable
      multiSelect={false}
      animateExpand
      expandedIds={expandedNodes}
      onExpandedChange={setDepartmentExpandedNodes}
    >
      <TreeView className="flex flex-col gap-2">
        {departments.map((dept, index) => {
          const isLast = index === departments.length - 1 && !hasNextPage;

          return (
            <DepartmentNode
              key={dept.id}
              department={dept}
              level={0}
              isLast={isLast}
            />
          );
        })}

        {hasNextPage && <div ref={intersectionRef} className="h-4" />}
      </TreeView>
    </TreeProvider>
  );
};

export const DepartmentTreeChildren = ({
  parentId,
}: {
  parentId: DepartmentId;
}) => {
  const expandedNodes = useDepartmentExpandedNodes();

  const {
    departments,
    hasNextPage,
    fetchNextPage,
    isFetchingNextPage,
    isPending,
    error,
    isFetching,
    refetch,
  } = useDepartmentChildren(parentId, { enabled: true });

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
    <TreeProvider
      showLines
      showIcons
      selectable
      multiSelect={false}
      animateExpand
      expandedIds={expandedNodes}
      onExpandedChange={setDepartmentExpandedNodes}
    >
      <TreeView className="flex flex-col gap-2">
        {departments.map((dept, index) => {
          const isLast = index === departments.length - 1 && !hasNextPage;

          return (
            <DepartmentNode
              key={dept.id}
              department={dept}
              level={0}
              isLast={isLast}
            />
          );
        })}

        {hasNextPage && <div ref={intersectionRef} className="h-4" />}
      </TreeView>
    </TreeProvider>
  );
};
