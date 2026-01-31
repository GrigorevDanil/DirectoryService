"use client";

import { PositionDto } from "@/entities/positions/types";
import { PositionDeleteDialog } from "@/features/positions/position-delete-dialog";
import { Badge } from "@/shared/components/ui/badge";
import {
  Card,
  CardDescription,
  CardFooter,
  CardHeader,
  CardTitle,
} from "@/shared/components/ui/card";
import { Separator } from "@/shared/components/ui/separator";
import Link from "next/link";

export const PositionCard = ({ position }: { position: PositionDto }) => {
  return (
    <Card
      key={position.id}
      className="hover:shadow-lg transition-shadow duration-300 overflow-hidden"
    >
      <CardHeader>
        <div className="flex items-start justify-between">
          <CardTitle className="text-xl">
            <Link
              href={"/positions/" + position.id}
              className="border-primary hover:border-b-2"
            >
              {position.name}
            </Link>
          </CardTitle>

          <Badge variant={position.isActive ? "default" : "secondary"}>
            {position.isActive ? "Активна" : "Неактивна"}
          </Badge>
        </div>
        <CardDescription className="flex flex-col gap-1 mt-2">
          {position.description}
          <Separator />
          {`Количество подразделений: ${position.countDepartments}`}
        </CardDescription>
      </CardHeader>
      <CardFooter className="flex gap-2 text-xs text-muted-foreground mt-auto">
        <div className="flex flex-col gap-1 w-full">
          <span>
            Создано: {new Date(position.createdAt).toLocaleDateString()}
          </span>
          <span>
            Обновлено: {new Date(position.updatedAt).toLocaleDateString()}
          </span>
          {position.deletedAt && (
            <span>
              Удалено: {new Date(position.deletedAt).toLocaleDateString()}
            </span>
          )}
        </div>
        <PositionDeleteDialog id={position.id} name={position.name} />
      </CardFooter>
    </Card>
  );
};
