"use client";

import { useLocationAdd } from "@/entities/locations/hooks/use-location-add";
import {
  addressValidation,
  locationNameValidation,
  timezoneValidation,
} from "@/entities/locations/types";
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
  FieldLegend,
  FieldSet,
} from "@/shared/components/ui/field";
import { Input } from "@/shared/components/ui/input";
import { Spinner } from "@/shared/components/ui/spinner";
import { useState } from "react";
import z from "zod";

const initialFormState = {
  name: "",
  timezone: "",
  address: {
    country: "",
    postalCode: "",
    region: "",
    city: "",
    street: "",
    houseNumber: "",
  },
};

const formDataSchema = z.object({
  name: locationNameValidation,
  timezone: timezoneValidation,
  address: addressValidation,
});

type FormData = z.infer<typeof formDataSchema>;

export const LocationAddForm = ({
  onSuccess,
  ...props
}: React.ComponentProps<"form"> & { onSuccess?: () => void }) => {
  const { locationAddAsync, isPending, error } = useLocationAdd();

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
      await locationAddAsync(formData);
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
          <FieldGroup>
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
              <FieldLabel htmlFor="timezone">Часовой пояс *</FieldLabel>
              <Input
                id="timezone"
                value={formData.timezone}
                onChange={(e) =>
                  setUserFormData((l) => ({
                    ...l,
                    timezone: e.target.value,
                  }))
                }
                aria-invalid={!!errors?.timezone}
              />
              <FieldError errors={errors?.timezone?.errors} />
            </Field>
          </FieldGroup>
        </FieldSet>

        <FieldSet>
          <FieldLegend>Адрес</FieldLegend>
          <FieldGroup>
            <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
              <Field>
                <FieldLabel htmlFor="country">Страна *</FieldLabel>
                <Input
                  id="country"
                  value={formData.address.country}
                  onChange={(e) =>
                    setUserFormData((l) => ({
                      ...l,
                      address: {
                        ...(l.address || initialFormState.address),
                        country: e.target.value,
                      },
                    }))
                  }
                  aria-invalid={!!errors?.country}
                />
                <FieldError errors={errors?.country?.errors} />
              </Field>

              <Field>
                <FieldLabel htmlFor="postalCode">Почтовый индекс</FieldLabel>
                <Input
                  id="postalCode"
                  value={formData.address.postalCode}
                  onChange={(e) =>
                    setUserFormData((l) => ({
                      ...l,
                      address: {
                        ...(l.address || initialFormState.address),
                        postalCode: e.target.value,
                      },
                    }))
                  }
                  aria-invalid={!!errors?.postalCode}
                />
                <FieldError errors={errors?.postalCode?.errors} />
              </Field>
            </div>

            <Field>
              <FieldLabel htmlFor="region">Регион / Область</FieldLabel>
              <Input
                id="region"
                value={formData.address.region}
                onChange={(e) =>
                  setUserFormData((l) => ({
                    ...l,
                    address: {
                      ...(l.address || initialFormState.address),
                      region: e.target.value,
                    },
                  }))
                }
                aria-invalid={!!errors?.region}
              />
              <FieldError errors={errors?.region?.errors} />
            </Field>

            <Field>
              <FieldLabel htmlFor="city">Город *</FieldLabel>
              <Input
                id="city"
                value={formData.address.city}
                onChange={(e) =>
                  setUserFormData((l) => ({
                    ...l,
                    address: {
                      ...(l.address || initialFormState.address),
                      city: e.target.value,
                    },
                  }))
                }
                placeholder="Москва, Санкт-Петербург"
                aria-invalid={!!errors?.city}
              />
              <FieldError errors={errors?.city?.errors} />
            </Field>

            <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
              <Field>
                <FieldLabel htmlFor="street">Улица *</FieldLabel>
                <Input
                  id="street"
                  value={formData.address.street}
                  onChange={(e) =>
                    setUserFormData((l) => ({
                      ...l,
                      address: {
                        ...(l.address || initialFormState.address),
                        street: e.target.value,
                      },
                    }))
                  }
                  placeholder="Тверская улица, проспект Мира"
                  aria-invalid={!!errors?.street}
                />
                <FieldError errors={errors?.street?.errors} />
              </Field>

              <Field>
                <FieldLabel htmlFor="houseNumber">Номер дома *</FieldLabel>
                <Input
                  id="houseNumber"
                  value={formData.address.houseNumber}
                  onChange={(e) =>
                    setUserFormData((l) => ({
                      ...l,
                      address: {
                        ...(l.address || initialFormState.address),
                        houseNumber: e.target.value,
                      },
                    }))
                  }
                  aria-invalid={!!errors?.houseNumber}
                />
                <FieldError errors={errors?.houseNumber?.errors} />
              </Field>
            </div>
          </FieldGroup>
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
