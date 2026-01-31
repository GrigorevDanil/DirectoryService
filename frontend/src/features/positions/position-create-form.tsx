"use client";

import {
  departmentIdsValidator,
  DepartmentShortDto,
} from "@/entities/departments/types";
import { usePositionCreate } from "@/entities/positions/hooks/use-position-create";
import {
  descriptionValidation,
  positionNameValidation,
} from "@/entities/positions/types";
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
import { Input } from "@/shared/components/ui/input";
import { Spinner } from "@/shared/components/ui/spinner";
import { Textarea } from "@/shared/components/ui/textarea";
import { DepartmentSelect } from "@/widgets/departments/select/department-select";
import { useState } from "react";
import z from "zod";

const initialFormState = {
  name: "",
  description: "",
  departmentIds: [],
};

const formDataSchema = z.object({
  name: positionNameValidation,
  description: descriptionValidation,
  departmentIds: departmentIdsValidator,
});

type FormData = z.infer<typeof formDataSchema>;

export const PositionCreateForm = ({
  onSuccess,
  ...props
}: React.ComponentProps<"form"> & { onSuccess?: () => void }) => {
  const { positionCreateAsync, isPending, error } = usePositionCreate();

  const [userFormData, setUserFormData] = useState<Partial<FormData>>({});

  const [showErrors, setShowErrors] = useState(false);

  const formData = {
    ...initialFormState,
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
      await positionCreateAsync(formData);
      onSuccess?.();
    } catch {
      setShowErrors(true);
      return;
    }
  };

  const handleReset = async (e: React.MouseEvent<HTMLFormElement>) => {
    e.preventDefault();

    setUserFormData(initialFormState);
  };

  return (
    <form onSubmit={handleSubmit} onReset={handleReset} {...props}>
      <FieldGroup>
        <FieldSet>
          <Field>
            <FieldLabel htmlFor="name">Название</FieldLabel>
            <Input
              id="name"
              value={formData.name}
              onChange={(e) =>
                setUserFormData((l) => ({
                  ...l,
                  name: e.target.value,
                }))
              }
              aria-invalid={!!errors?.name}
              autoFocus
            />
            <FieldError errors={errors?.name?.errors} />
          </Field>

          <Field>
            <FieldLabel htmlFor="description">Описание</FieldLabel>
            <Textarea
              id="description"
              value={formData.description}
              onChange={(e) =>
                setUserFormData((l) => ({
                  ...l,
                  description: e.target.value,
                }))
              }
              aria-invalid={!!errors?.description}
            />
            <FieldError errors={errors?.description?.errors} />
          </Field>

          <Field>
            <FieldLabel htmlFor="departmentIds">Подразделения</FieldLabel>
            <DepartmentSelect
              className="max-h-75"
              id="departmentIds"
              stateId="multi-select-create-position"
              selectedDepartments={formData.departmentIds.map(
                (id) => ({ id }) as DepartmentShortDto,
              )}
              onChangeChecked={(deps) =>
                setUserFormData((l) => ({
                  ...l,
                  departmentIds: deps.map((x) => x.id),
                }))
              }
              aria-invalid={!!errors?.departments}
              request={{ isActive: true }}
            />
            <FieldError errors={errors?.departments?.errors} />
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
        <Button type="submit">{isPending && <Spinner />}Добавить</Button>
      </Field>
    </form>
  );
};
