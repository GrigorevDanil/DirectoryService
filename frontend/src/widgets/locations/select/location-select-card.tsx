import { LocationDto } from "@/entities/locations/types";
import { Badge } from "@/shared/components/ui/badge";
import { Checkbox } from "@/shared/components/ui/checkbox";
import {
  Field,
  FieldContent,
  FieldDescription,
  FieldLabel,
  FieldTitle,
} from "@/shared/components/ui/field";

interface LocationSelectCardProps extends React.ComponentProps<"div"> {
  location: LocationDto;
  checked: boolean;
  onCheckedChange: (selected: boolean, location: LocationDto) => void;
}

export function LocationSelectCard({
  location,
  checked,
  onCheckedChange,
  className,
  ...props
}: LocationSelectCardProps) {
  const handleCheckedChange = (checked: boolean) => {
    onCheckedChange(checked, location);
  };

  return (
    <FieldLabel>
      <Field className={className} orientation="horizontal" {...props}>
        <Checkbox
          id={location.id}
          name={location.id}
          checked={checked}
          onCheckedChange={handleCheckedChange}
        />
        <FieldContent>
          <FieldTitle>{location.name}</FieldTitle>
          <FieldDescription>{location.timezone}</FieldDescription>
        </FieldContent>

        <Badge variant={location.isActive ? "default" : "secondary"}>
          {location.isActive ? "Активна" : "Неактивна"}
        </Badge>
      </Field>
    </FieldLabel>
  );
}
