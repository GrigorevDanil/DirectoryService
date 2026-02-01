import z from "zod";
import { LocationDto } from "../locations/types";
import { PositionDto } from "../positions/types";

export type DepartmentId = string;

export interface DepartmentShortDto {
  id: DepartmentId;
  name: string;
  identifier: string;
  path: string;
  isActive: boolean;
  createdAt: string;
  updatedAt: string;
  deletedAt: string;
}

export interface DepartmentDto {
  id: DepartmentId;
  name: string;
  identifier: string;
  parentId: DepartmentId;
  path: string;
  depth: number;
  isActive: boolean;
  children: DepartmentDto[];
  locations: LocationDto[];
  positions: PositionDto[];
  hasMoreChildren: boolean;
  createdAt: string;
  updatedAt: string;
  deletedAt: string;
}

export const DEPARTMENT_NAME_MIN_LENGTH = 3;
export const DEPARTMENT_NAME_MAX_LENGTH = 150;

export const departmentNameValidation = z
  .string()
  .min(
    DEPARTMENT_NAME_MIN_LENGTH,
    `Название должно содержать минимум ${DEPARTMENT_NAME_MIN_LENGTH} символа`,
  )
  .max(
    DEPARTMENT_NAME_MAX_LENGTH,
    `Название не должно превышать ${DEPARTMENT_NAME_MAX_LENGTH} символов`,
  );

export const IDENTIFIER_MIN_LENGTH = 3;
export const IDENTIFIER_MAX_LENGTH = 150;

export const identifierValidation = z
  .string()
  .regex(/^[a-zA-Z]+(?:-[a-zA-Z]+)*$/, {
    message: "Строка должна содержать только латиницу, разделённая дефисами",
  })
  .min(
    DEPARTMENT_NAME_MIN_LENGTH,
    `Название должно содержать минимум ${DEPARTMENT_NAME_MIN_LENGTH} символа`,
  )
  .max(
    DEPARTMENT_NAME_MAX_LENGTH,
    `Название не должно превышать ${DEPARTMENT_NAME_MAX_LENGTH} символов`,
  );

export const departmentIdsValidator = z
  .array(z.string())
  .min(1, "Выберите хотя бы одно подразделение")
  .refine(
    (arr) => new Set(arr.map((id) => id)).size === arr.length,
    "Подразделения не должны повторяться",
  );

export const parentIdValidation = z.string().nullable();
