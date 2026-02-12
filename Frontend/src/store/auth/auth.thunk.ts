import { createAsyncThunk } from "@reduxjs/toolkit";
import {
  AuthenticationTokenObjectResponseApiResponse,
  AuthService,
  BooleanApiResponse,
  SignInRequest,
  StringApiResponse,
} from "@/api/generated";
import { Response } from "./auth.types";

export const signIn = createAsyncThunk<
  AuthenticationTokenObjectResponseApiResponse,
  SignInRequest
>("auth/signIn", async (signInRequest: SignInRequest) => {
  return await AuthService.postApiAuthSignin({
    requestBody: signInRequest,
  });
});

export const signOut = createAsyncThunk<BooleanApiResponse>(
  "auth/signOut",
  async () => {
    return await AuthService.postApiAuthSignout();
  },
);

export const renewToken = createAsyncThunk<StringApiResponse, null>(
  "auth/renewToken",
  async () => {
    return await AuthService.postApiAuthRenewToken();
  },
);

export const resentOtp = createAsyncThunk<BooleanApiResponse, string>(
  "auth/re-sent-otp",
  async (guid: string) => {
    return await AuthService.postApiAuthReSentOtp({ requestBody: guid });
  },
);
