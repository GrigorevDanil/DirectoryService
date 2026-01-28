"use client";

import { DepartmentShortDto } from "@/entities/departments/types";
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
import { DepartmentMultiSelect } from "@/widgets/departments/multi-select";
import { useState } from "react";
import { toast } from "sonner";
import z from "zod";

const initialFormState = {
  departments: [],
};

const formDataSchema = z.object({
  departments: z
    .array(
      z.object({
        id: z.string(),
        name: z.string(),
        identifier: z.string(),
        isActive: z.boolean(),
      }),
    )
    .min(1, "Выберите хотя бы одно подразделение")
    .refine(
      (arr) => new Set(arr.map((dept) => dept.id)).size === arr.length,
      "Подразделения не должны повторяться",
    ),
});

type FormData = z.infer<typeof formDataSchema>;

interface PositionUpdateDepartmentsProps extends React.ComponentProps<"form"> {
  positionId: PositionId;
  departments: DepartmentShortDto[];
}

export const PositionUpdateDepartments = ({
  positionId,
  departments: initialDepartments,
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
    ...{ departments: initialDepartments },
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

    const currentDepartmentIds = formData.departments.map((dep) => dep.id);
    const initialDepartmentIds = initialDepartments.map((dep) => dep.id);

    const departmentsToAdd = currentDepartmentIds.filter(
      (id) => !initialDepartmentIds.includes(id),
    );

    const departmentsToRemove = initialDepartmentIds.filter(
      (id) => !currentDepartmentIds.includes(id),
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
    setUserFormData({ departments: initialDepartments });
  };

  return (
    <form onSubmit={handleSubmit} onReset={handleReset} {...props}>
      <FieldSet>
        <Field>
          <DepartmentMultiSelect
            className="max-h-[50vh]"
            id="departmentIds"
            stateId={"multi-select-position-" + positionId}
            selectedDepartments={formData.departments}
            onChangeChecked={(deps) =>
              setUserFormData((l) => ({
                ...l,
                departments: deps,
              }))
            }
            aria-invalid={!!errors?.departments}
            request={{ isActive: true }}
          />
          <FieldError errors={errors?.departments?.errors} />
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
