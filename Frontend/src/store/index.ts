import { configureStore } from "@reduxjs/toolkit";
import userSlice from "./user/user.slice";
import authSlice from "./auth/auth.slice";
import permissionSlice from "./permission/permission.slice";
import serviceSlice from "./service/service.slice";
import roleSlice from "./role/role.slice";

export const store = configureStore({
  reducer: {
    auth: authSlice,
    user: userSlice,
    permission: permissionSlice,
    service: serviceSlice,
    role: roleSlice,
  },
});

export type RootState = ReturnType<typeof store.getState>;
export type AppDispatch = typeof store.dispatch;
