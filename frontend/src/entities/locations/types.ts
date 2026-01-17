import z from "zod";

export interface LocationDto {
  id: string;
  name: string;
  timezone: string;
  address: AddressDto;
  isActive: boolean;
  createdAt: string;
  updatedAt: string;
}

export const LOCATION_NAME_MIN_LENGTH = 3;
export const LOCATION_NAME_MAX_LENGTH = 120;

export const TIMEZONE_MIN_LENGTH = 3;
export const TIMEZONE_MAX_LENGTH = 50;

export const locationNameValidation = z
  .string()
  .min(
    LOCATION_NAME_MIN_LENGTH,
    `Название должно содержать минимум ${LOCATION_NAME_MIN_LENGTH} символа`
  )
  .max(
    LOCATION_NAME_MAX_LENGTH,
    `Название не должно превышать ${LOCATION_NAME_MAX_LENGTH} символов`
  );

export const timezoneValidation = z
  .string()
  .min(
    TIMEZONE_MIN_LENGTH,
    `Часовой пояс должен содержать минимум ${TIMEZONE_MIN_LENGTH} символа`
  )
  .max(
    LOCATION_NAME_MAX_LENGTH,
    `Часовой пояс не должен превышать ${LOCATION_NAME_MAX_LENGTH} символов`
  );

export interface AddressDto {
  country: string;
  postalCode: string;
  region: string;
  city: string;
  street: string;
  houseNumber: string;
}

export const COUNTRY_MIN_LENGTH = 3;
export const COUNTRY_MAX_LENGTH = 100;

export const POSTAL_CODE_LENGTH = 6;

export const REGION_MIN_LENGTH = 3;
export const REGION_MAX_LENGTH = 100;

export const CITY_MIN_LENGTH = 3;
export const CITY_MAX_LENGTH = 100;

export const STREET_MIN_LENGTH = 3;
export const STREET_MAX_LENGTH = 150;

export const HOUSE_NUMBER_MAX_LENGTH = 3;

export const addressValidation = z.object({
  country: z
    .string()
    .min(
      COUNTRY_MIN_LENGTH,
      `Страна должна содержать минимум ${COUNTRY_MIN_LENGTH} символа`
    )
    .max(
      COUNTRY_MAX_LENGTH,
      `Название страны не должно превышать ${COUNTRY_MAX_LENGTH} символов`
    )
    .nonempty("Страна обязательна для заполнения"),

  postalCode: z
    .string()
    .length(
      POSTAL_CODE_LENGTH,
      `Почтовый индекс должен содержать ровно ${POSTAL_CODE_LENGTH} символов`
    )
    .nonempty("Почтовый индекс обязателен"),

  region: z
    .string()
    .min(
      REGION_MIN_LENGTH,
      `Регион должен содержать минимум ${REGION_MIN_LENGTH} символа`
    )
    .max(
      REGION_MAX_LENGTH,
      `Название региона не должно превышать ${REGION_MAX_LENGTH} символов`
    )
    .nonempty("Регион обязателен для заполнения"),

  city: z
    .string()
    .min(
      CITY_MIN_LENGTH,
      `Город должен содержать минимум ${CITY_MIN_LENGTH} символа`
    )
    .max(
      CITY_MAX_LENGTH,
      `Название города не должно превышать ${CITY_MAX_LENGTH} символов`
    )
    .nonempty("Город обязателен для заполнения"),

  street: z
    .string()
    .min(
      STREET_MIN_LENGTH,
      `Улица должна содержать минимум ${STREET_MIN_LENGTH} символа`
    )
    .max(
      STREET_MAX_LENGTH,
      `Название улицы не должно превышать ${STREET_MAX_LENGTH} символов`
    )
    .nonempty("Улица обязательна для заполнения"),

  houseNumber: z
    .string()
    .min(1, "Номер дома обязателен")
    .max(
      HOUSE_NUMBER_MAX_LENGTH,
      `Номер дома не должен превышать ${HOUSE_NUMBER_MAX_LENGTH} символов`
    )
    .nonempty("Номер дома обязателен"),
});
