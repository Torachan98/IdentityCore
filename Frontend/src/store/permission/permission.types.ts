import { PermissionDTO } from "@/api/generated";
import { BaseState } from "../base.types";

export interface PermissionState extends BaseState {
  permissions: PermissionDTO[] | null;
  pageSize: number;
  pageNum: number;
  totalItems: number;
  totalPages: number;
}
