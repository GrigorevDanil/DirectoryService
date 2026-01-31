import { DepartmentId } from "@/entities/departments/types";
import { DepartmentDetailPage } from "@/pages/department-detail";

export default async function DepartmentDetailAsyncPage({
  params,
}: {
  params: Promise<{ id: DepartmentId }>;
}) {
  const { id } = await params;

  return <DepartmentDetailPage id={id} />;
}
