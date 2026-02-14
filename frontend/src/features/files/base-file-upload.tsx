import { useState } from "react";
import { useUploadFile } from "@/entities/files/hooks/use-upload-file";
import { AssetType, MediaAssetId, OwnerType } from "@/entities/files/type";
import {
  ValidationError,
  EnvelopeError,
  isEnvelopeError,
  convertApiErrorsToValidationError,
} from "@/shared/api/errors";
import { Button } from "@/shared/components/ui/button";
import {
  Field,
  FieldDescription,
  FieldError,
  FieldSet,
} from "@/shared/components/ui/field";
import {
  FileUpload,
  FileUploadDropzone,
  FileUploadItem,
  FileUploadItemDelete,
  FileUploadItemMetadata,
  FileUploadItemPreview,
  FileUploadItemProgress,
  FileUploadList,
  FileUploadProps,
  FileUploadTrigger,
} from "@/shared/components/ui/file-upload";
import { cn } from "@/shared/lib/utils";
import { Upload, X } from "lucide-react";

export interface BaseFileUploadProps extends Omit<
  FileUploadProps,
  "onUpload" | "onFileValidate"
> {
  context: OwnerType;
  entityId: string;
  assetType: AssetType;
  description?: string;
  onFileValidate?: (file: File) => ValidationError | undefined;
  onSuccessUpload?: (id: MediaAssetId) => void;
}

export const BaseFileUpload = ({
  context,
  entityId,
  assetType,
  description,
  onFileValidate,
  onSuccessUpload,
  className,
  ...props
}: BaseFileUploadProps) => {
  const [files, setFiles] = useState<File[]>([]);
  const [localValidationError, setLocalValidationError] = useState<
    ValidationError | undefined
  >(undefined);

  const { uploadFileAsync, error, cancel } = useUploadFile();

  const handleUpload: NonNullable<FileUploadProps["onUpload"]> = async (
    files,
    { onProgress, onSuccess, onError },
  ) => {
    await Promise.all(
      files.map(async (file) => {
        try {
          const id = await uploadFileAsync({
            file,
            assetType,
            context,
            entityId,
            onProgress: ({ progress }) => onProgress(file, progress),
          });

          onSuccess(file);
          onSuccessUpload?.(id);
        } catch (e) {
          onError(file, e as Error);
          cancel(({ progress }) => onProgress(file, progress));
        }
      }),
    );
  };

  const handleCancel = async () => await cancel();

  const validate = (file: File) => {
    if (!onFileValidate) return undefined;

    const validation = onFileValidate(file);

    if (!validation) {
      setLocalValidationError(undefined);
      return undefined;
    }

    setLocalValidationError(validation);

    const firstKey = Object.keys(validation)[0];
    return validation[firstKey]?.errors?.[0];
  };

  const apiValidationErrors =
    error && isEnvelopeError(error)
      ? convertApiErrorsToValidationError((error as EnvelopeError).errorList)
      : undefined;

  const errors = apiValidationErrors || localValidationError;

  const fileErrors = errors
    ? errors[Object.keys(errors)[0]]?.errors
    : undefined;

  return (
    <FieldSet>
      <Field>
        <FileUpload
          value={files}
          onValueChange={setFiles}
          onUpload={handleUpload}
          onFileValidate={validate}
          aria-invalid={!!fileErrors}
          className={cn("w-full", className)}
          {...props}
        >
          <FileUploadDropzone>
            <div className="flex flex-col items-center gap-1">
              <div className="flex items-center justify-center rounded-full border p-2.5">
                <Upload className="size-6 text-muted-foreground" />
              </div>
              <p className="font-medium text-sm">Перетяните файл сюда</p>
            </div>

            <FileUploadTrigger asChild>
              <Button variant="outline" size="sm" className="mt-2 w-fit">
                Выбрать файл
              </Button>
            </FileUploadTrigger>
          </FileUploadDropzone>

          <FileUploadList>
            {files.map((file, index) => (
              <FileUploadItem key={index} value={file} className="flex-col">
                <div className="flex w-full items-center gap-2">
                  <FileUploadItemPreview />
                  <FileUploadItemMetadata />
                  <FileUploadItemDelete asChild onClick={handleCancel}>
                    <Button variant="ghost" size="icon" className="size-7">
                      <X />
                    </Button>
                  </FileUploadItemDelete>
                </div>
                <FileUploadItemProgress />
              </FileUploadItem>
            ))}
          </FileUploadList>
        </FileUpload>
        <FieldDescription>{description}</FieldDescription>

        <FieldError errors={fileErrors} />
      </Field>
    </FieldSet>
  );
};
