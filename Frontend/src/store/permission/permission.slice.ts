import { createSlice } from "@reduxjs/toolkit";
import { PermissionState } from "./permission.types";
import { fetchPermissions } from "./permission.thunk";

const initialState: PermissionState = {
  permissions: null,
  loading: false,
  error: null,
  pageSize: 1,
  pageNum: 10,
  totalItems: 0,
  totalPages: 0,
};

const permissionSlice = createSlice({
  name: "permission",
  initialState: initialState,
  reducers: {},
  extraReducers: (builder) => {
    builder
      .addCase(fetchPermissions.pending, (state) => {
        state.loading = true;
        state.error = null;
      })

      .addCase(fetchPermissions.fulfilled, (state, action) => {
        state.loading = false;
        if (action.payload.isSuccess) {
          state.permissions = action.payload.data?.items ?? [];
          state.pageNum = action.payload.data?.pageNum ?? 1;
          state.pageSize = action.payload.data?.pageSize ?? 10;
          state.totalPages = action.payload.data?.totalPage ?? 0;
          state.totalItems = action.payload.data?.totalCount ?? 0;
        }

        state.error = action.payload.message ?? "";
      })

      .addCase(fetchPermissions.rejected, (state) => {
        state.loading = false;
        state.error = "Cannot GET /api/Permission";
      });
  },
});

export default permissionSlice.reducer;
