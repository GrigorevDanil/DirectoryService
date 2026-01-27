export type DepartmentId = string;

export interface DepartmentShortDto {
  id: DepartmentId;
  name: string;
  identifier: string;
  isActive: boolean;
}
