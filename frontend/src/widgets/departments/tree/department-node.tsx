import { useDepartmentChildren } from "@/entities/departments/hooks/use-department-children";
import { DepartmentDto } from "@/entities/departments/types";
import { Spinner } from "@/shared/components/ui/spinner";
import {
  useTree,
  TreeNode,
  TreeNodeTrigger,
  TreeExpander,
  TreeIcon,
  TreeNodeContent,
} from "@/shared/components/ui/tree";
import { useIntersectionObserver } from "@/shared/hooks/use-intersection-observer";
import { Layers } from "lucide-react";
import { DepartmentShortCard } from "../department-short-card";

interface DepartmentNodeProps {
  department: DepartmentDto;
  level: number;
  isLast: boolean;
}

export const DepartmentNode = ({
  department,
  level,
  isLast,
}: DepartmentNodeProps) => {
  const { expandedIds } = useTree();
  const isExpanded = expandedIds.has(department.id);

  const shouldFetchChildren = isExpanded && department.hasMoreChildren;

  const {
    departments,
    hasNextPage,
    fetchNextPage,
    isFetchingNextPage,
    isLoading,
  } = useDepartmentChildren(department.id, {
    enabled: shouldFetchChildren,
  });
  const allChildren = [...department.children, ...(departments ?? [])];

  const hasChildren =
    department.children.length > 0 || department.hasMoreChildren;

  const intersectionRef = useIntersectionObserver({
    hasNextPage,
    fetchNextPage,
    isFetchingNextPage,
  });

  const renderIcon = () => {
    if (isLoading) return <Spinner />;
    return hasChildren ? undefined : <Layers />;
  };

  return (
    <TreeNode
      className="flex flex-col gap-2"
      nodeId={department.id}
      level={level}
      isLast={isLast}
    >
      <div className="flex">
        <TreeNodeTrigger>
          <TreeExpander hasChildren={hasChildren} />

          <TreeIcon hasChildren={hasChildren} icon={renderIcon()} />
        </TreeNodeTrigger>

        <DepartmentShortCard department={department} />
      </div>

      <TreeNodeContent
        className="flex flex-col gap-2"
        hasChildren={hasChildren}
      >
        {allChildren.map((child, index) => {
          const childIsLast = index === allChildren.length - 1 && !hasNextPage;

          return (
            <DepartmentNode
              key={child.id}
              department={child}
              level={level + 1}
              isLast={childIsLast}
            />
          );
        })}

        {hasNextPage && <div ref={intersectionRef} className="h-4" />}
      </TreeNodeContent>
    </TreeNode>
  );
};
