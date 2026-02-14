import { API_V1, httpClient } from "@/shared/api/http-client";
import { Envelope } from "@/shared/api/envelops";
import axios, { AxiosRequestConfig } from "axios";
import { AssetType, MediaAssetId, OwnerType } from "./type";

export type PartEtag = {
  partNumber: number;
  etag: string;
};

export type ChunkUploadUrl = {
  partNumber: number;
  uploadUrl: string;
};

export interface StartMultipartUploadRequest {
  fileName: string;
  contentType: string;
  fileSize: number;
  assetType: AssetType;
  context: OwnerType;
  entityId: string;
}

export interface StartMultipartUploadResponse {
  mediaAssetId: MediaAssetId;
  uploadId: string;
  chunkUploadUrls: ChunkUploadUrl[];
  chunkSize: number;
}

export interface UploadChunkRequest {
  uploadUrl: string;
  chunk: Blob;
  contentType: string;
}

export interface CompleteMultipartUploadRequest {
  mediaAssetId: MediaAssetId;
  uploadId: string;
  partEtags: PartEtag[];
}

export interface AbortMultipartUploadRequest {
  mediaAssetId: MediaAssetId;
  uploadId: string;
}

export const filesApi = {
  baseKey: "files",
  startMultipartUpload: async (
    request: StartMultipartUploadRequest,
    config?: AxiosRequestConfig,
  ): Promise<Envelope<StartMultipartUploadResponse>> => {
    const response = await httpClient.post<
      Envelope<StartMultipartUploadResponse>
    >(API_V1 + "/files/multipart/start", request, config);

    return response.data;
  },
  uploadChunk: async (
    { uploadUrl, chunk, contentType }: UploadChunkRequest,
    config?: AxiosRequestConfig,
  ): Promise<string> => {
    const response = await axios.put(uploadUrl, chunk, {
      headers: {
        "Content-Type": contentType,
      },
      ...config,
    });

    return response.headers["etag"]?.replace(/"/g, "") || "";
  },
  completeMultipartUpload: async (
    request: CompleteMultipartUploadRequest,
    config?: AxiosRequestConfig,
  ) => {
    await httpClient.post<Envelope<StartMultipartUploadResponse>>(
      API_V1 + "/files/multipart/complete",
      request,
      config,
    );
  },
  abortMultipartUpload: async (request: AbortMultipartUploadRequest) => {
    await httpClient.post(API_V1 + "/files/multipart/abort", request);
  },
};
