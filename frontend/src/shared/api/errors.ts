import z from "zod";

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

export interface ValidationError {
  [key: string]: {
    errors: string[];
  };
}

export const convertApiErrorsToValidationError = (
  errors: ApiError[]
): ValidationError => {
  return errors.reduce((acc, error) => {
    if (error.type !== "VALIDATION" || !error.invalidField) {
      return acc;
    }

    const key = error.invalidField.split(".").pop() || error.invalidField;

    acc[key] = acc[key] ?? { errors: [] };
    acc[key].errors.push(error.message);

    return acc;
  }, {} as ValidationError);
};

export const convertZodErrorToValidationError = (
  error: z.ZodError
): ValidationError => {
  return error.issues.reduce<ValidationError>((acc, issue) => {
    const key = issue.path[issue.path.length - 1] as string;

    acc[key] = acc[key] ?? { errors: [] };
    acc[key].errors.push(issue.message);

    return acc;
  }, {});
};
