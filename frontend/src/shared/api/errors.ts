export interface ApiError {
  code: string;
  message: string;
  type: ApiErrorType;
  invalidField?: string | null;
}

export type ApiErrorType =
  | "VALIDATION"
  | "NOT_FOUND"
  | "FAILURE"
  | "CONFLICT"
  | "FORBIDDEN";

export class EnvelopeError extends Error {
  public readonly errorList: ApiError[];

  constructor(errorList: ApiError[]) {
    const error = errorList[0];

    super(`[${error.type}] ${error.message}`);

    this.name = "EnvelopeError";
    this.errorList = errorList;

    Object.setPrototypeOf(this, EnvelopeError.prototype);
  }
}

export const isEnvelopeError = (error: unknown): error is EnvelopeError => {
  return error instanceof EnvelopeError;
};
