import { DepartmentId } from "@/entities/departments/types";
import { PositionDetailPage } from "@/pages/position-detail";

export default async function PositionDetailAsyncPage({
  params,
}: {
  params: Promise<{ id: DepartmentId }>;
}) {
  const { id } = await params;

  return <PositionDetailPage id={id} />;
}
