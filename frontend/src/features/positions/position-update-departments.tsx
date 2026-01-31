"use client";

import {
  DepartmentId,
  departmentIdsValidator,
  DepartmentShortDto,
} from "@/entities/departments/types";
import { usePositionAddDepartment } from "@/entities/positions/hooks/use-position-add-department";
import { usePositionRemoveDepartment } from "@/entities/positions/hooks/use-position-remove-department";
import { PositionId } from "@/entities/positions/types";
import {
  convertApiErrorsToValidationError,
  convertZodErrorToValidationError,
  EnvelopeError,
  isEnvelopeError,
} from "@/shared/api/errors";
import { Button } from "@/shared/components/ui/button";
import { Field, FieldError, FieldSet } from "@/shared/components/ui/field";
import { Spinner } from "@/shared/components/ui/spinner";
import { DepartmentSelect } from "@/widgets/departments/select/department-select";
import { useState } from "react";
import { toast } from "sonner";
import z from "zod";

const initialFormState = {
  departmentIds: [],
};

const formDataSchema = z.object({
  departmentIds: departmentIdsValidator,
});

type FormData = z.infer<typeof formDataSchema>;

interface PositionUpdateDepartmentsProps extends React.ComponentProps<"form"> {
  positionId: PositionId;
  departmentIds: DepartmentId[];
}

export const PositionUpdateDepartments = ({
  positionId,
  departmentIds: initialDepartmentIds,
  ...props
}: PositionUpdateDepartmentsProps) => {
  const {
    addDepartmentToPosition,
    error: addError,
    isPending: isAddPending,
  } = usePositionAddDepartment();
  const {
    removeDepartmentFromPosition,
    error: removeError,
    isPending: isRemovePending,
  } = usePositionRemoveDepartment();

  const [userFormData, setUserFormData] = useState<Partial<FormData>>({});

  const [showErrors, setShowErrors] = useState(false);

  const formData = {
    ...initialFormState,
    ...{ departmentIds: initialDepartmentIds },
    ...userFormData,
  };

  const isPending = isAddPending || isRemovePending;
  const error = addError || removeError;

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

  const handleSubmit = async (e: React.FormEvent<HTMLFormElement>) => {
    e.preventDefault();

    const validationErrors = validate();
    if (validationErrors) {
      setShowErrors(true);
      return;
    }

    const departmentsToAdd = formData.departmentIds.filter(
      (id) => !initialDepartmentIds.includes(id),
    );

    const departmentsToRemove = initialDepartmentIds.filter(
      (id) => !formData.departmentIds.includes(id),
    );

    try {
      if (departmentsToRemove.length > 0) {
        await removeDepartmentFromPosition({
          id: positionId,
          departmentIds: departmentsToRemove,
        });
      }

      if (departmentsToAdd.length > 0) {
        await addDepartmentToPosition({
          id: positionId,
          departmentIds: departmentsToAdd,
        });
      }

      toast.info("Изменения успешно применены");
    } catch {
      setShowErrors(true);
    }
  };

  const handleReset = (e: React.FormEvent<HTMLFormElement>) => {
    e.preventDefault();
    setUserFormData(initialFormState);
  };

  return (
    <form onSubmit={handleSubmit} onReset={handleReset} {...props}>
      <FieldSet>
        <Field>
          <DepartmentSelect
            className="max-h-[50vh]"
            id="departmentIds"
            stateId={"multi-select-position-" + positionId}
            selectedDepartments={formData.departmentIds.map(
              (id) => ({ id }) as DepartmentShortDto,
            )}
            onChangeChecked={(deps) =>
              setUserFormData((l) => ({
                ...l,
                departmentIds: deps.map((x) => x.id),
              }))
            }
            aria-invalid={!!errors?.departmentIds}
            request={{ isActive: true }}
          />
          <FieldError errors={errors?.departmentIds?.errors} />
        </Field>
      </FieldSet>
      {!!apiGeneralErrors && (
        <FieldError className="mt-2" errors={apiGeneralErrors} />
      )}
      <Field className="mt-4 flex gap-2">
        <Button type="reset" variant="outline">
          Сбросить
        </Button>
        <Button type="submit" disabled={isPending}>
          {isPending && <Spinner />}
          Принять
        </Button>
      </Field>
    </form>
  );
};
