"use client";

import { LocationDto } from "@/entities/locations/types";
import { LocationDeleteDialog } from "@/features/locations/location-delete-dialog";
import { LocationUpdateDialog } from "@/widgets/locations/location-update-dialog";
import { Badge } from "@/shared/components/ui/badge";
import {
  Card,
  CardContent,
  CardDescription,
  CardFooter,
  CardHeader,
  CardTitle,
} from "@/shared/components/ui/card";
import { MapPin } from "lucide-react";

export const LocationCard = ({ location }: { location: LocationDto }) => {
  const fullAddress = `${location.address.street} ${location.address.houseNumber}, ${location.address.city}, ${location.address.region}, ${location.address.country}`;

  return (
    <Card
      key={location.id}
      className="hover:shadow-lg transition-shadow duration-300 overflow-hidden"
    >
      <CardHeader>
        <div className="flex items-start justify-between">
          <CardTitle className="text-xl">{location.name}</CardTitle>
          <Badge variant={location.isActive ? "default" : "secondary"}>
            {location.isActive ? "Активна" : "Неактивна"}
          </Badge>
        </div>
        <CardDescription className="flex items-center gap-1 mt-2">
          <MapPin className="w-4 h-4" />
          {location.timezone}
        </CardDescription>
      </CardHeader>

      <CardContent>
        <p className="text-sm text-muted-foreground">{fullAddress}</p>
        {location.address.postalCode && (
          <p className="text-sm text-muted-foreground mt-1">
            Почтовый индекс: {location.address.postalCode}
          </p>
        )}
      </CardContent>

      <CardFooter className="flex gap-2 text-xs text-muted-foreground mt-auto">
        <div className="flex flex-col gap-1 w-full">
          <span>
            Создано: {new Date(location.createdAt).toLocaleDateString()}
          </span>
          <span>
            Обновлено: {new Date(location.updatedAt).toLocaleDateString()}
          </span>
        </div>
        <LocationUpdateDialog location={location} />
        <LocationDeleteDialog id={location.id} name={location.name} />
      </CardFooter>
    </Card>
  );
};
