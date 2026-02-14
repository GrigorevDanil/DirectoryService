"use client";

import { Spinner } from "@/shared/components/ui/spinner";
import { Error } from "@/widgets/error";
import { Button } from "@/shared/components/ui/button";
import {
  Card,
  CardContent,
  CardHeader,
  CardTitle,
} from "@/shared/components/ui/card";
import {
  ArrowLeft,
  Calendar,
  Building,
  Tag,
  Route,
  MapPin,
  IndentIncrease,
  Users,
} from "lucide-react";
import { useRouter } from "next/navigation";
import { DepartmentId } from "@/entities/departments/types";
import { useDepartment } from "@/entities/departments/hooks/use-department";
import { ListLayout } from "@/shared/components/ui/list-layout";
import { DepartmentViewFilters } from "@/widgets/departments/view/department-view-filters";
import { DepartmentViewSearch } from "@/widgets/departments/view/department-view-search";
import { DepartmentView } from "@/widgets/departments/view/department-view";
import { DepartmentViewVariant } from "@/features/departments/view/department-view-variant";
import { DepartmentDeleteDialog } from "@/features/departments/department-delete-dialog";
import { CardInfo, CardInfoItemProps } from "@/widgets/card-info";
import { DepartmentUpdateDialog } from "@/widgets/departments/department-update-dialog";
import { DepartmentMoveDialog } from "@/widgets/departments/department-move-dialog";
import { DepartmentUpdateLocations } from "@/features/departments/department-update-locations";
import { PositionList } from "@/widgets/positions/position-list";
import { PositionListSearch } from "@/features/positions/list/position-list-search";
import { PositionListSortBy } from "@/features/positions/list/position-list-sort-by";
import { PositionListSortDirection } from "@/features/positions/list/position-list-sort-direction";
import { PositionListActive } from "@/features/positions/list/position-list-active";
import { PositionListPageSize } from "@/features/positions/list/position-list-page-size";
import { DepartmentVideoUpload } from "@/features/departments/department-video-upload";
import { VideoPlayer } from "@/widgets/video-player";

const { Container, Content, Header, Filters } = ListLayout;

export const DepartmentDetailPage = ({ id }: { id: DepartmentId }) => {
  const router = useRouter();
  const { error, isPending, department, refetch } = useDepartment(id);

  const departmentStateId = `list-${id}`;

  if (isPending) {
    return <Spinner />;
  }

  if (error) {
    return <Error error={error} reset={refetch} />;
  }

  if (department === undefined) {
    return (
      <div className="text-center py-12 text-muted-foreground">
        Подразделение не найдено
      </div>
    );
  }

  const info: CardInfoItemProps[] = [
    {
      icon: Tag,
      title: "Короткое название:",
      value: department.identifier,
    },
    {
      icon: Route,
      title: "Путь:",
      value: department.path,
    },
    {
      icon: IndentIncrease,
      title: "Глубина:",
      value: department.depth.toString(),
    },
    {
      icon: Calendar,
      title: "Создан:",
      value: new Date(department.createdAt).toLocaleDateString(),
    },
    {
      icon: Calendar,
      title: "Обновлен:",
      value: new Date(department.createdAt).toLocaleDateString(),
    },
    ...(department.deletedAt
      ? [
          {
            icon: Calendar,
            title: "Удален:",
            value: new Date(department.deletedAt).toLocaleDateString(),
          },
        ]
      : []),
  ];

  console.log(department);

  return (
    <div className="flex flex-col gap-6 p-2 mx-auto">
      <div className="flex items-center justify-between">
        <div className="flex items-center gap-4">
          <Button variant="outline" size="icon" onClick={() => router.back()}>
            <ArrowLeft className="h-4 w-4" />
          </Button>
          <div className="flex items-center gap-3">
            <div
              className={`h-3 w-3 rounded-full ${department.isActive ? "bg-green-500" : "bg-red-500"}`}
            />
          </div>
          <h1 className="text-3xl font-bold tracking-tight">
            {department.name}
          </h1>
        </div>
        <div className="flex gap-2">
          <DepartmentMoveDialog deparment={department} />
          <DepartmentUpdateDialog deparment={department} />
          <DepartmentDeleteDialog deparment={department} />
        </div>
      </div>

      <div className="grid grid-cols-1 lg:grid-cols-3 gap-6">
        <div className="lg:col-span-2 space-y-6">
          {department.videoUrl ? (
            <VideoPlayer className="h-1/4" url={department.videoUrl} />
          ) : (
            <DepartmentVideoUpload
              className="w-full h-1/4"
              departmentId={department.id}
            />
          )}

          <Card>
            <CardHeader>
              <div className="flex items-center justify-between">
                <div className="flex items-center gap-2">
                  <Building className="h-5 w-5 text-muted-foreground" />
                  <CardTitle>Дочерние подразделения</CardTitle>
                </div>
              </div>
            </CardHeader>
            <CardContent className="max-h-137.5 overflow-auto">
              <ListLayout>
                <Header>
                  <DepartmentViewSearch stateId={departmentStateId} />
                  <DepartmentViewVariant className="ml-auto" />
                </Header>
                <Container>
                  <Content>
                    <DepartmentView
                      parentId={department.id}
                      stateId={departmentStateId}
                    />
                  </Content>
                  <DepartmentViewFilters stateId={departmentStateId} />
                </Container>
              </ListLayout>
            </CardContent>
          </Card>

          <Card>
            <CardHeader>
              <div className="flex items-center justify-between">
                <div className="flex items-center gap-2">
                  <MapPin className="h-5 w-5 text-muted-foreground" />
                  <CardTitle>Локации</CardTitle>
                </div>
              </div>
            </CardHeader>
            <CardContent className="max-h-137.5 overflow-auto">
              <DepartmentUpdateLocations department={department} />
            </CardContent>
          </Card>

          <Card>
            <CardHeader>
              <div className="flex items-center justify-between">
                <div className="flex items-center gap-2">
                  <Users className="h-5 w-5 text-muted-foreground" />
                  <CardTitle>Позиции</CardTitle>
                </div>
              </div>
            </CardHeader>
            <CardContent className="max-h-137.5 overflow-auto">
              <ListLayout>
                <Header>
                  <PositionListSearch stateId={departmentStateId} />
                </Header>
                <Container>
                  <Content>
                    <PositionList
                      stateId={departmentStateId}
                      request={{ departmentIds: [department.id] }}
                    />
                  </Content>
                  <Filters>
                    <h3 className="font-medium text-sm mb-3">Фильтры</h3>
                    <div className="space-y-3">
                      <PositionListSortBy stateId={departmentStateId} />
                      <PositionListSortDirection stateId={departmentStateId} />
                      <PositionListActive stateId={departmentStateId} />
                      <PositionListPageSize stateId={departmentStateId} />
                    </div>
                  </Filters>
                </Container>
              </ListLayout>
            </CardContent>
          </Card>
        </div>

        <CardInfo items={info} />
      </div>
    </div>
  );
};
