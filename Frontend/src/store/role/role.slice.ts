import { createSlice, PayloadAction } from "@reduxjs/toolkit";
import { RoleState } from "./role.type";
import { fetchRoleById, fetchRoles, updateRole } from "./role.thunk";
import { RoleDTO } from "@/api/generated";

const initialState: RoleState = {
  roles: null,
  role: null,
  loading: false,
  error: null,
  pageSize: 1,
  pageNum: 10,
  totalItems: 0,
  totalPages: 0,
};

const roleSlice = createSlice({
  name: "role",
  initialState: initialState,
  reducers: {
    setRole: (state, action: PayloadAction<RoleDTO>) => {
      state.role = action.payload;
    },
  },
  extraReducers: (builder) => {
    builder
      .addCase(fetchRoles.pending, (state) => {
        state.loading = true;
        state.error = null;
      })
      .addCase(fetchRoles.fulfilled, (state, action) => {
        state.loading = false;
        if (action.payload.isSuccess) {
          state.roles = action.payload.data?.items ?? [];
          state.pageNum = action.payload.data?.pageNum ?? 1;
          state.pageSize = action.payload.data?.pageSize ?? 10;
          state.totalPages = action.payload.data?.totalPage ?? 0;
          state.totalItems = action.payload.data?.totalCount ?? 0;
        }

        state.error = action.payload.message ?? "";
      })
      .addCase(fetchRoles.rejected, (state) => {
        state.loading = false;
        state.error = "Cannot GET /api/Role";
      })

      .addCase(fetchRoleById.pending, (state) => {
        state.loading = true;
        state.error = null;
      })
      .addCase(fetchRoleById.fulfilled, (state, action) => {
        state.loading = false;
        if (action.payload.isSuccess) {
          state.role = action.payload.data ?? null;
        }

        state.error = action.payload.message ?? "";
      })

      .addCase(fetchRoleById.rejected, (state) => {
        state.loading = false;
        state.error = "Cannot GET /api/GetRoleById";
      })

      .addCase(updateRole.pending, (state) => {
        state.loading = true;
        state.error = null;
      })

      .addCase(updateRole.fulfilled, (state, action) => {
        state.loading = false;
        if (action.payload.isSuccess) {
          state.role = action.payload.data ?? null;
        }

        state.error = action.payload.message ?? "";
      })

      .addCase(updateRole.rejected, (state) => {
        state.loading = false;
        state.error = "Cannot PUT /api/Role";
      });
  },
});

export const { setRole } = roleSlice.actions;
export default roleSlice.reducer;
