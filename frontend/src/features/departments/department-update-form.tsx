"use client";

import { useDepartmentUpdate } from "@/entities/departments/hooks/use-department-update";
import {
  DepartmentId,
  departmentNameValidation,
  identifierValidation,
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
import { Input } from "@/shared/components/ui/input";
import { Spinner } from "@/shared/components/ui/spinner";
import { useState } from "react";
import z from "zod";

const initialFormState = {
  name: "",
  identifier: "",
};

const formDataSchema = z.object({
  name: departmentNameValidation,
  identifier: identifierValidation,
});

type FormData = z.infer<typeof formDataSchema>;

export const DepartmentUpdateForm = ({
  onSuccess,
  deparment,
  ...props
}: React.ComponentProps<"form"> & {
  onSuccess?: () => void;
  deparment: {
    id: DepartmentId;
    name: string;
    identifier: string;
  };
}) => {
  const { departmentUpdateAsync, isPending, error } = useDepartmentUpdate();

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
      await departmentUpdateAsync({ ...formData, id: deparment.id });
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
            <FieldLabel htmlFor="identifier">Краткое наименование</FieldLabel>
            <Input
              id="identifier"
              value={formData.identifier}
              onChange={(e) =>
                setUserFormData((l) => ({
                  ...l,
                  identifier: e.target.value,
                }))
              }
              aria-invalid={!!errors?.identifier}
            />
            <FieldError errors={errors?.identifier?.errors} />
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
        <Button type="submit">{isPending && <Spinner />}Обновить</Button>
      </Field>
    </form>
  );
};
