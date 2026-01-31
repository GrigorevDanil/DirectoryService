"use client";

import { useDepartmentCreate } from "@/entities/departments/hooks/use-department-create";
import {
  departmentNameValidation,
  DepartmentShortDto,
  identifierValidation,
  parentIdValidation,
} from "@/entities/departments/types";
import { LocationDto, locationIdsValidation } from "@/entities/locations/types";
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
import { DepartmentSelect } from "@/widgets/departments/select/department-select";
import { LocationSelect } from "@/widgets/locations/select/location-select";
import { useState } from "react";
import z from "zod";

const initialFormState = {
  name: "",
  identifier: "",
  parentId: null,
  locationIds: [],
};

const formDataSchema = z.object({
  name: departmentNameValidation,
  identifier: identifierValidation,
  parentId: parentIdValidation,
  locationIds: locationIdsValidation,
});

type FormData = z.infer<typeof formDataSchema>;

export const DepartmentCreateForm = ({
  onSuccess,
  ...props
}: React.ComponentProps<"form"> & { onSuccess?: () => void }) => {
  const { departmentCreateAsync, isPending, error } = useDepartmentCreate();

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
      await departmentCreateAsync(formData);
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

          <Field>
            <FieldLabel htmlFor="parentId">
              Родительское подразделение
            </FieldLabel>
            <DepartmentSelect
              className="max-h-75"
              id="parentId"
              stateId="select-create-department"
              selectedDepartments={
                formData.parentId
                  ? [{ id: formData.parentId } as DepartmentShortDto]
                  : []
              }
              onChangeChecked={(deps) =>
                setUserFormData((l) => ({
                  ...l,
                  parentId: deps[0]?.id ?? undefined,
                }))
              }
              aria-invalid={!!errors?.parent}
              request={{ isActive: true }}
              multiselect={false}
            />
            <FieldError errors={errors?.parentId?.errors} />
          </Field>

          <Field>
            <FieldLabel htmlFor="locationIds">Локации</FieldLabel>
            <LocationSelect
              className="max-h-75"
              id="locationIds"
              stateId="multi-select-create-department"
              selectedLocations={formData.locationIds.map(
                (id) => ({ id }) as LocationDto,
              )}
              onChangeChecked={(locs) =>
                setUserFormData((l) => ({
                  ...l,
                  locationIds: locs.map((x) => x.id),
                }))
              }
              aria-invalid={!!errors?.locations}
              request={{ isActive: true }}
            />
            <FieldError errors={errors?.locations?.errors} />
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
