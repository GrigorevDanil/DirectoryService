import { useDepartmentAttachVideo } from "@/entities/departments/hooks/use-department-attach-video";
import { DepartmentId } from "@/entities/departments/types";
import { MediaAssetId } from "@/entities/files/type";
import { VideoUpload } from "../files/video-upload";

export interface DepartmentVideoUploadProps {
  className?: string;
  departmentId: DepartmentId;
}

export const DepartmentVideoUpload = ({
  departmentId,
  className,
}: DepartmentVideoUploadProps) => {
  const { departmentAttachVideoAsync } = useDepartmentAttachVideo();

  const handleSucess = async (videoId: MediaAssetId) => {
    await departmentAttachVideoAsync({ id: departmentId, videoId });
  };

  return (
    <VideoUpload
      className={className}
      context="department"
      entityId={departmentId}
      onSuccessUpload={handleSucess}
    />
  );
};
