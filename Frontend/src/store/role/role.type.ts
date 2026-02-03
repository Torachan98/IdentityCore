import { RoleDTO } from "@/api/generated";
import { BaseState } from "@/store/base.types";

export interface RoleState extends BaseState {
  roles: RoleDTO[] | null;
  role: RoleDTO | null;
  pageSize: number;
  pageNum: number;
  totalItems: number;
  totalPages: number;
}
