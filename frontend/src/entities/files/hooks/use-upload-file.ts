import { ChunkUploadUrl, filesApi, PartEtag } from "../api";
import { useMutation, useQueryClient } from "@tanstack/react-query";
import { toast } from "sonner";
import { AssetType, MediaAssetId, OwnerType } from "../type";
import { AxiosRequestConfig } from "axios";
import { useRef } from "react";

export interface UploadFileProps {
  file: File;
  assetType: AssetType;
  context: OwnerType;
  entityId: string;
  onProgress?: (progress: UploadProgress) => void;
}

export type UploadProgress = {
  status: "idle" | "uploading" | "completed";
  progress: number;
};

const initialUploadProgress: UploadProgress = {
  status: "idle",
  progress: 0,
};

export const useUploadFile = () => {
  const abortControllerRef = useRef<AbortController | null>(null);
  const currentUploadRef = useRef<{
    mediaAssetId: MediaAssetId;
    uploadId: string;
  } | null>(null);
  const queryClient = useQueryClient();

  const uploadFile = async ({
    assetType,
    context,
    entityId,
    file,
    onProgress = () => initialUploadProgress,
  }: UploadFileProps): Promise<MediaAssetId> => {
    onProgress?.({ status: "uploading", progress: 0 });
    abortControllerRef.current = new AbortController();
    const signal = abortControllerRef.current?.signal;

    const { result } = await filesApi.startMultipartUpload(
      {
        fileName: file.name,
        contentType: file.type,
        fileSize: file.size,
        assetType: assetType,
        context: context,
        entityId: entityId,
      },
      { signal },
    );

    if (result === null) {
      throw new Error("Ошибка начала загрузки файла");
    }

    const { chunkSize, chunkUploadUrls, uploadId, mediaAssetId } = result;

    currentUploadRef.current = {
      mediaAssetId,
      uploadId,
    };

    const partEtags: PartEtag[] = await uploadChunks(
      file,
      chunkUploadUrls,
      chunkSize,
      onProgress,
      { signal },
    );

    await filesApi.completeMultipartUpload(
      {
        mediaAssetId,
        uploadId,
        partEtags,
      },
      { signal },
    );

    onProgress({ status: "completed", progress: 100 });

    return mediaAssetId;
  };

  const cancel = async (onProgress?: (progress: UploadProgress) => void) => {
    abortControllerRef.current?.abort();

    const current = currentUploadRef.current;
    if (current) {
      try {
        await filesApi.abortMultipartUpload({
          mediaAssetId: current.mediaAssetId,
          uploadId: current.uploadId,
        });
      } catch {}
      currentUploadRef.current = null;
    }

    onProgress?.({ status: "idle", progress: 0 });
  };

  const uploadFileMutation = useMutation({
    mutationFn: uploadFile,
    async onSettled() {
      await queryClient.invalidateQueries({ queryKey: [filesApi.baseKey] });
    },
    onSuccess(_, variables) {
      toast.success(`Файл '${variables.file.name}' был успешно загружен`);
    },
  });

  return {
    uploadFileAsync: uploadFileMutation.mutateAsync,
    isPending: uploadFileMutation.isPending,
    error: uploadFileMutation.error,
    cancel,
  };
};

const uploadChunks = async (
  file: File,
  chunkUploadUrls: ChunkUploadUrl[],
  chunkSize: number,
  onProgress: (progress: UploadProgress) => void,
  config?: AxiosRequestConfig,
) => {
  const partEtags: PartEtag[] = [];

  await Promise.all(
    chunkUploadUrls.map(async (chunkUploadUrl) => {
      const start = (chunkUploadUrl.partNumber - 1) * chunkSize;

      const end = Math.min(start + chunkSize, file.size);

      const chunk = file.slice(start, end);

      const etag = await filesApi.uploadChunk(
        {
          uploadUrl: chunkUploadUrl.uploadUrl,
          chunk,
          contentType: file.type,
        },
        config,
      );

      partEtags.push({
        partNumber: chunkUploadUrl.partNumber,
        etag,
      });

      onProgress({
        status: "uploading",
        progress: Math.round(
          (chunkUploadUrl.partNumber / chunkUploadUrls.length) * 100,
        ),
      });
    }),
  );

  return partEtags;
};
