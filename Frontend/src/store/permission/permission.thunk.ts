import { createAsyncThunk } from "@reduxjs/toolkit";
import {
  CreateOrUpdatePermissionRequest,
  PermissionDTOApiResponse,
  PermissionDTOPaginationItemsApiResponse,
  PermissionService,
} from "@/api/generated";
import { FetchParams } from "../base.types";

export const fetchPermissions = createAsyncThunk<
  PermissionDTOPaginationItemsApiResponse,
  FetchParams
>("permission/getPermissions", async (query: FetchParams) => {
  return await PermissionService.getApiPermission(query);
});

export const createPermissions = createAsyncThunk<
  PermissionDTOApiResponse,
  CreateOrUpdatePermissionRequest
>(
  "permission/createPermission",
  async (payload: CreateOrUpdatePermissionRequest) => {
    return await PermissionService.postApiPermission({ requestBody: payload });
  },
);

export const updatePermissions = createAsyncThunk<
  PermissionDTOApiResponse,
  CreateOrUpdatePermissionRequest
>(
  "permission/updatePermission",
  async (payload: CreateOrUpdatePermissionRequest) => {
    return await PermissionService.putApiPermission({ requestBody: payload });
  },
);

export const removePermissions = createAsyncThunk<any, string>(
  "permission/updatePermission",
  async (id: string) => {
    return await PermissionService.deleteApiPermission({ guid: id });
  },
);
