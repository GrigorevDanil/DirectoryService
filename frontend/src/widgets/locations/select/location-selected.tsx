import { DepartmentId } from "@/entities/departments/types";
import { LocationDto } from "@/entities/locations/types";
import { Badge } from "@/shared/components/ui/badge";
import { Button } from "@/shared/components/ui/button";
import { cn } from "@/shared/lib/utils";
import { X } from "lucide-react";

interface LocationSelectedProps extends React.ComponentProps<"div"> {
  selectedLocations: LocationDto[];
  onRemove: (id: DepartmentId) => void;
}

export const LocationSelected = ({
  selectedLocations,
  onRemove,
  className,
  ...props
}: LocationSelectedProps) => {
  return (
    <div className={cn("flex gap-2 flex-wrap", className)} {...props}>
      {selectedLocations.map((location) => (
        <Badge key={location.id} asChild>
          <Button onClick={() => onRemove(location.id)}>
            {location.name}
            <X />
          </Button>
        </Badge>
      ))}
    </div>
  );
};
