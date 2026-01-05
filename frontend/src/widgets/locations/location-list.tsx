"use client";

import {
  Card,
  CardContent,
  CardDescription,
  CardFooter,
  CardHeader,
  CardTitle,
} from "@/shared/components/ui/card";
import { Badge } from "@/shared/components/ui/badge";
import { MapPin } from "lucide-react";
import { useLocationList } from "@/entities/locations/hooks/use-location-list";
import { Spinner } from "@/shared/components/ui/spinner";
import { Error } from "../error";

export const LocationList = () => {
  const { isPending, locations, error, refetch } = useLocationList();

  if (isPending) {
    return <Spinner />;
  }

  if (error) {
    return <Error error={error} reset={refetch} />;
  }

  if (locations.length === 0) {
    return (
      <div className="text-center py-12 text-muted-foreground">
        Нет доступных локаций
      </div>
    );
  }

  return (
    <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4 gap-6">
      {locations.map((location) => {
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

            <CardFooter className="text-xs text-muted-foreground">
              <div className="flex flex-col gap-1 w-full">
                <span>
                  Создано: {new Date(location.createdAt).toLocaleDateString()}
                </span>
                <span>
                  Обновлено: {new Date(location.updatedAt).toLocaleDateString()}
                </span>
              </div>
            </CardFooter>
          </Card>
        );
      })}
    </div>
  );
};
