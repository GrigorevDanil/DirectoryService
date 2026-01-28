import z from "zod";
import { DepartmentShortDto } from "../departments/types";

export type PositionId = string;

export interface PositionDto {
  id: PositionId;
  name: string;
  description: string;
  countDepartments: string;
  isActive: boolean;
  createdAt: string;
  updatedAt: string;
  deletedAt: string;
}

export const POSITION_NAME_MIN_LENGTH = 3;
export const POSITION_NAME_MAX_LENGTH = 100;

export const DESCRIPTION_MAX_LENGTH = 1000;

export const positionNameValidation = z
  .string()
  .min(
    POSITION_NAME_MIN_LENGTH,
    `Название должно содержать минимум ${POSITION_NAME_MIN_LENGTH} символа`,
  )
  .max(
    POSITION_NAME_MAX_LENGTH,
    `Название не должно превышать ${POSITION_NAME_MAX_LENGTH} символов`,
  );

export const descriptionValidation = z
  .string()
  .max(
    DESCRIPTION_MAX_LENGTH,
    `Описание не должно превышать ${DESCRIPTION_MAX_LENGTH} символов`,
  );

export interface PositionDetailDto {
  id: PositionId;
  name: string;
  description: string;
  departments: DepartmentShortDto[];
  countDepartments: string;
  isActive: boolean;
  createdAt: string;
  updatedAt: string;
  deletedAt: string;
}
