"use client";

import { usePosition } from "@/entities/positions/hooks/use-position";
import { PositionId } from "@/entities/positions/types";
import { Spinner } from "@/shared/components/ui/spinner";
import { Error } from "@/widgets/error";
import { Button } from "@/shared/components/ui/button";
import {
  Card,
  CardContent,
  CardHeader,
  CardTitle,
} from "@/shared/components/ui/card";
import { Separator } from "@/shared/components/ui/separator";
import { ArrowLeft, Calendar, FileText, Building } from "lucide-react";
import { useRouter } from "next/navigation";
import { PositionUpdateDialog } from "@/features/positions/position-update-dialog";
import { PositionUpdateDepartments } from "@/features/positions/position-update-departments";
import { PositionDeleteDialog } from "@/features/positions/position-delete-dialog";

export const PositionDetailPage = ({ id }: { id: PositionId }) => {
  const router = useRouter();
  const { error, isPending, position, refetch } = usePosition(id);

  if (isPending) {
    return <Spinner />;
  }

  if (error) {
    return <Error error={error} reset={refetch} />;
  }

  if (position === undefined) {
    return (
      <div className="text-center py-12 text-muted-foreground">
        Позиция не найдена
      </div>
    );
  }

  return (
    <div className="flex flex-col gap-6 p-2 mx-auto">
      <div className="flex items-center justify-between">
        <div className="flex items-center gap-4">
          <Button variant="outline" size="icon" onClick={() => router.back()}>
            <ArrowLeft className="h-4 w-4" />
          </Button>
          <div className="flex items-center gap-3">
            <div
              className={`h-3 w-3 rounded-full ${position.isActive ? "bg-green-500" : "bg-red-500"}`}
            />
          </div>
          <h1 className="text-3xl font-bold tracking-tight">{position.name}</h1>
        </div>
        <div className="flex gap-2">
          <PositionUpdateDialog position={position} />
          <PositionDeleteDialog id={position.id} name={position.name} />
        </div>
      </div>

      <div className="grid grid-cols-1 lg:grid-cols-3 gap-6">
        <div className="lg:col-span-2 space-y-6">
          <Card>
            <CardHeader>
              <div className="flex items-center gap-2">
                <FileText className="h-5 w-5 text-muted-foreground" />
                <CardTitle>Описание</CardTitle>
              </div>
            </CardHeader>
            <CardContent>
              <p className="text-muted-foreground whitespace-pre-line">
                {position.description || "Описание отсутствует"}
              </p>
            </CardContent>
          </Card>

          <Card>
            <CardHeader>
              <div className="flex items-center justify-between">
                <div className="flex items-center gap-2">
                  <Building className="h-5 w-5 text-muted-foreground" />
                  <CardTitle>Отделы</CardTitle>
                </div>
              </div>
            </CardHeader>
            <CardContent>
              <PositionUpdateDepartments
                positionId={position.id}
                departments={position.departments}
              />
            </CardContent>
          </Card>
        </div>

        <Card>
          <CardHeader>
            <CardTitle>Информация</CardTitle>
          </CardHeader>
          <CardContent className="space-y-4">
            <div className="space-y-2">
              <div className="flex items-center gap-2 text-sm text-muted-foreground">
                <Calendar className="h-4 w-4" />
                <span>Создана:</span>
              </div>
              <p className="font-medium text-sm">
                {new Date(position.createdAt).toLocaleDateString()}
              </p>
            </div>

            <Separator />

            <div className="space-y-2">
              <div className="flex items-center gap-2 text-sm text-muted-foreground">
                <Calendar className="h-4 w-4" />
                <span>Обновлена:</span>
              </div>
              <p className="font-medium text-sm">
                {new Date(position.updatedAt).toLocaleDateString()}
              </p>
            </div>

            {position.deletedAt && (
              <>
                <Separator />
                <div className="space-y-2">
                  <div className="flex items-center gap-2 text-sm text-muted-foreground">
                    <Calendar className="h-4 w-4" />
                    <span>Удалена:</span>
                  </div>
                  <p className="font-medium text-sm">
                    {new Date(position.deletedAt).toLocaleDateString()}
                  </p>
                </div>
              </>
            )}
          </CardContent>
        </Card>
      </div>
    </div>
  );
};
