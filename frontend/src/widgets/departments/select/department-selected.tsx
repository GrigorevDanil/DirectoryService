import { DepartmentId, DepartmentShortDto } from "@/entities/departments/types";
import { Badge } from "@/shared/components/ui/badge";
import { Button } from "@/shared/components/ui/button";
import { cn } from "@/shared/lib/utils";
import { X } from "lucide-react";

interface DepartmentSelectedProps extends React.ComponentProps<"div"> {
  selectedDepartments: DepartmentShortDto[];
  onRemove: (id: DepartmentId) => void;
}

export const DepartmentSelected = ({
  selectedDepartments,
  onRemove,
  className,
  ...props
}: DepartmentSelectedProps) => {
  return (
    <div className={cn("flex gap-2 flex-wrap", className)} {...props}>
      {selectedDepartments.map((dep) => (
        <Badge key={dep.id} asChild>
          <Button onClick={() => onRemove(dep.id)}>
            {dep.name}
            <X />
          </Button>
        </Badge>
      ))}
    </div>
  );
};
