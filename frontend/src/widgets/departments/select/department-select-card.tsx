import { DepartmentShortDto } from "@/entities/departments/types";
import { Badge } from "@/shared/components/ui/badge";
import { Checkbox } from "@/shared/components/ui/checkbox";
import {
  Field,
  FieldContent,
  FieldDescription,
  FieldLabel,
  FieldTitle,
} from "@/shared/components/ui/field";

interface DepartmentSelectCardProps extends React.ComponentProps<"div"> {
  department: DepartmentShortDto;
  checked: boolean;
  onCheckedChange: (selected: boolean, department: DepartmentShortDto) => void;
}

export function DepartmentSelectCard({
  department,
  checked,
  onCheckedChange,
  className,
  ...props
}: DepartmentSelectCardProps) {
  const handleCheckedChange = (checked: boolean) => {
    onCheckedChange(checked, department);
  };

  return (
    <FieldLabel>
      <Field className={className} orientation="horizontal" {...props}>
        <Checkbox
          id={department.id}
          name={department.id}
          checked={checked}
          onCheckedChange={handleCheckedChange}
        />

        <FieldContent>
          <FieldTitle>{department.name}</FieldTitle>
          <FieldDescription>{department.identifier}</FieldDescription>
        </FieldContent>

        <Badge variant={department.isActive ? "default" : "secondary"}>
          {department.isActive ? "Активна" : "Неактивна"}
        </Badge>
      </Field>
    </FieldLabel>
  );
}
