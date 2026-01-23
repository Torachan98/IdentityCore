import { createSlice } from "@reduxjs/toolkit";
import { renewToken, signIn } from "./auth.thunk";
import { AuthInformation, AuthState } from "./auth.types";
import { jwtDecode } from "jwt-decode";

const initialState: AuthState = {
  loading: false,
  token: "",
  error: null,
  step: 0,
  userToken: null,
};

const authSlice = createSlice({
  name: "auth",
  initialState: initialState,
  reducers: {
    // setToken: (state, action) => {
    //   state.token = action.payload;
    // },
    // logout: (state) => {
    //   state.token = "";
    // },
  },
  extraReducers: (builder) => {
    builder
      .addCase(signIn.pending, (state) => {
        state.loading = true;
        state.error = null;
      })

      .addCase(signIn.fulfilled, (state, action) => {
        state.loading = false;
        if (action.payload.isSuccess) {
          state.token = action.payload.data?.item?.accessToken ?? "";
          state.step = action.payload.data?.item?.step ?? 0;
          state.userToken = jwtDecode<AuthInformation>(
            action.payload.data?.item?.accessToken ?? "",
          );
        }

        state.error = action.payload.message ?? "";
      })

      .addCase(signIn.rejected, (state, action) => {
        state.loading = false;
        state.error = "Cannot POST /api/auth/signin";
      })

      .addCase(renewToken.pending, (state) => {
        state.loading = true;
        state.error = null;
      })

      .addCase(renewToken.fulfilled, (state, action) => {
        state.loading = false;
        if (action.payload.isSuccess) {
          state.token = action.payload.data ?? "";
          state.userToken = jwtDecode<AuthInformation>(
            action.payload.data ?? "",
          );
        }

        state.error = action.payload.message ?? "";
      })

      .addCase(renewToken.rejected, (state, action) => {
        state.loading = false;
        state.error = "Cannot POST /api/auth/renew-token";
      });

    // .addCase()
    // .addCase()
    // .addCase()

    // .addCase()
    // .addCase()
    // .addCase()
  },
});

// export const { setToken, logout } = authSlice.actions;
export default authSlice.reducer;
