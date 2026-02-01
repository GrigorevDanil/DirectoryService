import { useDepartmentSetLocations } from "@/entities/departments/hooks/use-department-set-locations";
import { DepartmentId } from "@/entities/departments/types";
import { LocationDto, locationIdsValidation } from "@/entities/locations/types";
import {
  convertZodErrorToValidationError,
  isEnvelopeError,
  convertApiErrorsToValidationError,
  EnvelopeError,
} from "@/shared/api/errors";
import { Button } from "@/shared/components/ui/button";
import { Field, FieldError } from "@/shared/components/ui/field";
import { Spinner } from "@/shared/components/ui/spinner";
import { LocationSelect } from "@/widgets/locations/select/location-select";
import { useState } from "react";
import { toast } from "sonner";
import z from "zod";

const initialFormState = {
  locationIds: [],
};

const formDataSchema = z.object({
  locationIds: locationIdsValidation,
});

type FormData = z.infer<typeof formDataSchema>;

interface DepartmentUpdateLocationsProps extends React.ComponentProps<"form"> {
  department: {
    id: DepartmentId;
    locations: LocationDto[];
    isActive: boolean;
  };
}

export const DepartmentUpdateLocations = ({
  department,
  ...props
}: DepartmentUpdateLocationsProps) => {
  const { departmentSetLocationsAsync, error, isPending } =
    useDepartmentSetLocations();

  const [userFormData, setUserFormData] = useState<Partial<FormData>>({});

  const [showErrors, setShowErrors] = useState(false);

  const formData = {
    ...initialFormState,
    ...{ locationIds: [...new Set(department.locations.map((x) => x.id))] },
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

  const handleSubmit = async (e: React.FormEvent<HTMLFormElement>) => {
    e.preventDefault();

    const validationErrors = validate();
    if (validationErrors) {
      setShowErrors(true);
      return;
    }

    try {
      await departmentSetLocationsAsync({
        id: department.id,
        locationIds: formData.locationIds,
      });

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
      <Field>
        <LocationSelect
          id="locationIds"
          stateId={"multi-select-department-" + department.id}
          selectedLocations={formData.locationIds.map(
            (id) => ({ id }) as LocationDto,
          )}
          onChangeChecked={(locs) =>
            setUserFormData((l) => ({
              ...l,
              locationIds: locs.map((x) => x.id),
            }))
          }
          aria-invalid={!!errors?.locationIds}
          request={{ isActive: true }}
        />
        <FieldError errors={errors?.locationIds?.errors} />
      </Field>
      {!!apiGeneralErrors && (
        <FieldError className="mt-2" errors={apiGeneralErrors} />
      )}
      <Field className="mt-4 flex gap-2">
        <Button type="reset" variant="outline">
          Сбросить
        </Button>
        <Button type="submit" disabled={isPending || !department.isActive}>
          {isPending && <Spinner />}
          Принять
        </Button>
      </Field>
    </form>
  );
};
