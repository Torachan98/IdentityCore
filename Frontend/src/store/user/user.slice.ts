import { createSlice, PayloadAction } from "@reduxjs/toolkit";
import { Step, UserState } from "./user.types";
import {
  createUser,
  deleteUser,
  fetchUserById,
  fetchUsers,
  updateUser,
} from "./user.thunk";
import { UserDTO } from "@/api/generated";

const initialState: UserState = {
  loading: false,
  users: null,
  user: null,
  step: Step.Init,
  error: null,
  pageSize: 10,
  pageNum: 1,
  totalItems: 0,
  totalPages: 0,
};

const userSlice = createSlice({
  name: "user",
  initialState: initialState,
  reducers: {
    setUser: (state, action: PayloadAction<UserDTO>) => {
      state.user = action.payload;
    },
    // setToken: (state, action) => {
    //   state.token = action.payload;
    // },
    // logout: (state) => {
    //   state.token = "";
    // },
  },
  extraReducers: (builder) => {
    builder
      .addCase(fetchUsers.pending, (state) => {
        state.loading = true;
        state.error = null;
      })

      .addCase(fetchUsers.fulfilled, (state, action) => {
        state.loading = false;

        if (action.payload.isSuccess) {
          state.users = action.payload.data?.items ?? [];
          state.pageNum = action.payload.data?.pageNum ?? 1;
          state.pageSize = action.payload.data?.pageSize ?? 10;
          state.totalPages = action.payload.data?.totalPage ?? 0;
          state.totalItems = action.payload.data?.totalCount ?? 0;
        }

        state.error = action.payload.message ?? "";
      })

      .addCase(fetchUsers.rejected, (state, action) => {
        state.loading = false;
        state.error = "Cannot GET /api/User";
      })

      .addCase(fetchUserById.pending, (state) => {
        state.loading = true;
        state.error = null;
      })

      .addCase(fetchUserById.fulfilled, (state, action) => {
        state.loading = false;
        if (action.payload.isSuccess) {
          state.user = action.payload.data ?? null;
        }

        state.error = action.payload.message ?? "";
      })

      .addCase(fetchUserById.rejected, (state) => {
        state.loading = false;
        state.error = "Cannot GET /api/GetUserById";
      })

      .addCase(createUser.pending, (state) => {
        state.loading = true;
        state.error = null;
      })

      .addCase(createUser.fulfilled, (state, action) => {
        state.loading = false;
        if (action.payload.isSuccess) {
          state.user = action.payload.data ?? null;
        }

        state.error = action.payload.message ?? "";
      })

      .addCase(createUser.rejected, (state) => {
        state.loading = false;
        state.error = "Cannot POST /api/CreateUser";
      })

      .addCase(updateUser.pending, (state) => {
        state.loading = true;
        state.error = null;
      })

      .addCase(updateUser.fulfilled, (state, action) => {
        state.loading = false;
        if (action.payload.isSuccess) {
          state.user = action.payload.data ?? null;
        }

        state.error = action.payload.message ?? "";
      })

      .addCase(updateUser.rejected, (state) => {
        state.loading = false;
        state.error = "Cannot PUT /api/UpdateUser";
      })

      .addCase(deleteUser.pending, (state) => {
        state.loading = true;
        state.error = null;
      })

      .addCase(deleteUser.fulfilled, (state, action) => {
        state.loading = true;
        // if (action.payload.isSuccess) {
        //   if (action.payload.data) {
        //     state.user = null;
        //   }
        // }

        state.error = action.payload.message ?? "";
      })

      .addCase(deleteUser.rejected, (state) => {
        state.loading = false;
        state.error = "Cannot DELETE /api/RemoveUser";
      });
  },
});

export const { setUser } = userSlice.actions;
export default userSlice.reducer;
