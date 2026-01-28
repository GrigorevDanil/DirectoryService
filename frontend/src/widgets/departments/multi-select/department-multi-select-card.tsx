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

interface DepartmentMultiSelectCardProps extends React.ComponentProps<"div"> {
  department: DepartmentShortDto;
  checked: boolean;
  onCheckedChange: (selected: boolean, department: DepartmentShortDto) => void;
}

export function DepartmentMultiSelectCard({
  department,
  checked,
  onCheckedChange,
  className,
  ...props
}: DepartmentMultiSelectCardProps) {
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
