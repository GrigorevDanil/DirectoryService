import { PositionId } from "@/entities/positions/types";
import { PositionDetailPage } from "@/pages/position-detail";

export default async function PositionDetailAsyncPage({
  params,
}: {
  params: Promise<{ id: PositionId }>;
}) {
  const { id } = await params;

  return <PositionDetailPage id={id} />;
}
