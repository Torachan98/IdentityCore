import { RoleDTO } from "../../api/generated";
import { BaseState } from "../base.types";

export interface RoleState extends BaseState {
  roles: RoleDTO[] | null;
  role: RoleDTO | null;
  pageSize: number;
  pageNum: number;
  totalItems: number;
  totalPages: number;
}
