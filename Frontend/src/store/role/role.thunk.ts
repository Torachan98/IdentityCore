import { createAsyncThunk } from "@reduxjs/toolkit";
import {
  CreateOrUpdateRoleRequest,
  PermissionDTOApiResponse,
  RoleDTOApiResponse,
  RoleDTOPaginationItemsApiResponse,
  RoleService,
} from "@/api/generated";
import { FetchParams } from "../base.types";

export const fetchRoles = createAsyncThunk<
  RoleDTOPaginationItemsApiResponse,
  FetchParams
>("role/getRoles", async (query: FetchParams) => {
  return await RoleService.getApiRole(query);
});

export const fetchRoleById = createAsyncThunk<RoleDTOApiResponse, string>(
  "role/getRoleById",
  async (id: string) => {
    return await RoleService.getRoleById({ id: id });
  },
);

export const createRole = createAsyncThunk<
  RoleDTOApiResponse,
  CreateOrUpdateRoleRequest
>("role/createRole", async (payload: CreateOrUpdateRoleRequest) => {
  return await RoleService.postApiRole({ requestBody: payload });
});

export const updateRole = createAsyncThunk<
  RoleDTOApiResponse,
  CreateOrUpdateRoleRequest
>("role/updateRole", async (payload: CreateOrUpdateRoleRequest) => {
  return await RoleService.putApiRole({ requestBody: payload });
});

export const deleteRole = createAsyncThunk<any, string>(
  "role/deleteRole",
  async (id: string) => {
    return await RoleService.deleteApiRole({ guid: id });
  },
);
