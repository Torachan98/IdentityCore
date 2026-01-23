import { createAsyncThunk } from "@reduxjs/toolkit";
import {
  AuthenticationTokenObjectResponseApiResponse,
  AuthService,
  SignInRequest,
  StringApiResponse,
} from "../../api/generated";
import { Response } from "./auth.types";

export const signIn = createAsyncThunk<
  AuthenticationTokenObjectResponseApiResponse,
  SignInRequest
>("auth/signIn", async (signInRequest: SignInRequest) => {
  return (await AuthService.postApiAuthSignin({
    requestBody: signInRequest,
  })) as Response;
});

export const renewToken = createAsyncThunk<StringApiResponse, null>(
  "auth/renewToken",
  async () => {
    return await AuthService.postApiAuthRenewToken();
  },
);

export const resentOtp = createAsyncThunk<null, null>(
  "auth/resent-otp",
  async () => {},
);
