import { MediaAssetId, OwnerType } from "@/entities/files/type";
import { BaseFileUpload } from "./base-file-upload";
import z from "zod";
import { convertZodErrorToValidationError } from "@/shared/api/errors";
import { FileUploadProps } from "@/shared/components/ui/file-upload";
import { calculateFileSize } from "@/shared/lib/calculate-file-size";

const allowedTypes = [
  "video/mp4",
  "video/webm",
  "video/ogg",
  "video/quicktime",
  "video/x-msvideo",
  "video/mpeg",
  "video/x-matroska",
];

const MAX_VIDEO_SIZE = 2 * 1024 * 1024 * 1024; // 2GB

const videoSchema = z
  .instanceof(File, { message: "Ожидается файл" })
  .refine((file) => allowedTypes.includes(file.type), {
    message: "Неподдерживаемый формат видео",
  })
  .refine((file) => file.size <= MAX_VIDEO_SIZE, {
    message: `Размер видео не должен превышать ${calculateFileSize(MAX_VIDEO_SIZE)}`,
  });

export interface VideoUploadProps extends Omit<
  FileUploadProps,
  "onUpload" | "onFileValidate"
> {
  context: OwnerType;
  entityId: string;
  onSuccessUpload?: (id: MediaAssetId) => void;
}

export const VideoUpload = (props: VideoUploadProps) => {
  const handleFileValidate = (file: File) => {
    const res = videoSchema.safeParse(file);

    if (res.success) return undefined;

    return convertZodErrorToValidationError(res.error);
  };

  return (
    <BaseFileUpload
      accept="video/*"
      assetType="video"
      description={`Разрешены форматы: .mp4, .webm, .ogg, .mov, .avi, .mpeg, .mkv. Максимальный размер: ${calculateFileSize(MAX_VIDEO_SIZE)}.`}
      onFileValidate={handleFileValidate}
      {...props}
    />
  );
};
