import { createAsyncThunk } from "@reduxjs/toolkit";
import {
  BooleanApiResponse,
  CreateOrUpdateUserRequest,
  FileService,
  UserDTOApiResponse,
  UserDTOPaginationItems,
  UserDTOPaginationItemsApiResponse,
  UserService,
} from "@/api/generated";
import { FetchParams } from "../base.types";
import { UploadAvatar } from "./user.types";

export const fetchUsers = createAsyncThunk<
  UserDTOPaginationItemsApiResponse,
  FetchParams
>("user/getUsers", async (query: FetchParams) => {
  return await UserService.getApiUser(query);
});

export const fetchUserById = createAsyncThunk<UserDTOApiResponse, string>(
  "user/getUserById",
  async (id: string) => {
    return await UserService.getUserById({ id: id });
  },
);

export const createUser = createAsyncThunk<
  UserDTOApiResponse,
  CreateOrUpdateUserRequest
>("user/createUser", async (payload: CreateOrUpdateUserRequest) => {
  return await UserService.postApiUser({ requestBody: payload });
});

export const updateUser = createAsyncThunk<
  UserDTOApiResponse,
  CreateOrUpdateUserRequest
>("user/updateUser", async (payload: CreateOrUpdateUserRequest) => {
  return await UserService.putApiUser({ requestBody: payload });
});

export const deleteUser = createAsyncThunk<BooleanApiResponse, string>(
  "user/deleteUser",
  async (id: string) => {
    return await UserService.deleteApiUser({ guid: id });
  },
);

// export const uploadFile = createAsyncThunk<any, UploadAvatar>(
//   "file/uploadFile",
//   async (formData: UploadAvatar) => {
//     return await FileService.postApiFile({
//       formData: { File: formData.file, FileType: formData.fileType },
//     });
//   },
// );
