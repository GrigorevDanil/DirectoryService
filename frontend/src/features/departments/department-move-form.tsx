"use client";

import { useDepartmentMove } from "@/entities/departments/hooks/use-department-move";
import {
  DepartmentId,
  DepartmentShortDto,
  parentIdValidation,
} from "@/entities/departments/types";
import {
  convertApiErrorsToValidationError,
  convertZodErrorToValidationError,
  EnvelopeError,
  isEnvelopeError,
} from "@/shared/api/errors";
import { Button } from "@/shared/components/ui/button";
import {
  Field,
  FieldError,
  FieldGroup,
  FieldLabel,
  FieldSet,
} from "@/shared/components/ui/field";
import { Spinner } from "@/shared/components/ui/spinner";
import { DepartmentSelect } from "@/widgets/departments/select/department-select";
import { useState } from "react";
import z from "zod";

const initialFormState = {
  parentId: null,
};

const formDataSchema = z.object({
  parentId: parentIdValidation,
});

type FormData = z.infer<typeof formDataSchema>;

export const DepartmentMoveForm = ({
  onSuccess,
  deparment,
  ...props
}: React.ComponentProps<"form"> & {
  onSuccess?: () => void;
  deparment: {
    id: DepartmentId;
    parentId: DepartmentId | null;
  };
}) => {
  const { departmentMoveAsync, isPending, error } = useDepartmentMove();

  const [userFormData, setUserFormData] = useState<Partial<FormData>>({});

  const [showErrors, setShowErrors] = useState(false);

  const formData = {
    ...initialFormState,
    ...deparment,
    ...userFormData,
  };

  const validate = () => {
    const res = formDataSchema.safeParse(formData);
    if (res.success) {
      return undefined;
    }

    return convertZodErrorToValidationError(res.error);
  };

  const apiValidationErrors =
    error && isEnvelopeError(error)
      ? convertApiErrorsToValidationError((error as EnvelopeError).errorList)
      : undefined;

  const apiGeneralErrors =
    error && isEnvelopeError(error)
      ? error.errorList.flatMap((err) => err.message)
      : undefined;

  const errors = showErrors ? apiValidationErrors || validate() : undefined;

  const handleSubmit = async (e: React.MouseEvent<HTMLFormElement>) => {
    e.preventDefault();

    const errors = validate();

    if (errors) {
      setShowErrors(true);
      return;
    }

    try {
      await departmentMoveAsync(formData);
      onSuccess?.();
    } catch {
      setShowErrors(true);
      return;
    }
  };

  const handleReset = async (e: React.MouseEvent<HTMLFormElement>) => {
    e.preventDefault();

    setUserFormData(deparment);
  };

  return (
    <form onSubmit={handleSubmit} onReset={handleReset} {...props}>
      <FieldGroup>
        <FieldSet>
          <Field>
            <FieldLabel htmlFor="parentId">
              Родительское подразделение
            </FieldLabel>
            <DepartmentSelect
              id="parentId"
              stateId="select-move-department"
              selectedDepartments={
                formData.parentId
                  ? [{ id: formData.parentId } as DepartmentShortDto]
                  : []
              }
              onChangeChecked={(deps) =>
                setUserFormData((l) => ({
                  ...l,
                  parentId: deps[0]?.id ?? null,
                }))
              }
              aria-invalid={!!errors?.parent}
              request={{ isActive: true }}
              multiselect={false}
              excludeIds={[formData.id]}
            />
            <FieldError errors={errors?.parentId?.errors} />
          </Field>
        </FieldSet>
      </FieldGroup>

      {!!errors && (
        <FieldError className="mt-2" errors={apiGeneralErrors}></FieldError>
      )}

      <Field className="mt-4">
        <Button type="reset" variant="outline">
          Очистить форму
        </Button>
        <Button type="submit">{isPending && <Spinner />}Переместить</Button>
      </Field>
    </form>
  );
};
