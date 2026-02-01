import { DepartmentShortDto } from "@/entities/departments/types";
import { Badge } from "@/shared/components/ui/badge";
import {
  FieldLabel,
  Field,
  FieldContent,
  FieldTitle,
  FieldDescription,
} from "@/shared/components/ui/field";
import Link from "next/link";
import { DepartmentPath } from "./department-path";

export const DepartmentShortCard = ({
  department,
  className,
  ...props
}: React.ComponentProps<"div"> & {
  department: DepartmentShortDto;
}) => {
  return (
    <FieldLabel>
      <Field className={className} orientation="horizontal" {...props}>
        <FieldContent>
          <FieldTitle>
            <Link
              href={"/departments/" + department.id}
              className="border-primary hover:border-b-2"
            >
              {department.name}
            </Link>
          </FieldTitle>
          <FieldDescription>{department.identifier}</FieldDescription>
          <DepartmentPath path={department.path} />
        </FieldContent>

        <Badge variant={department.isActive ? "default" : "secondary"}>
          {department.isActive ? "Активна" : "Неактивна"}
        </Badge>
      </Field>
    </FieldLabel>
  );
};
