/* generated using openapi-typescript-codegen -- do not edit */
/* istanbul ignore file */
/* tslint:disable */
/* eslint-disable */
import type { AuthenticationTokenObjectResponseApiResponse } from '../models/AuthenticationTokenObjectResponseApiResponse';
import type { BooleanApiResponse } from '../models/BooleanApiResponse';
import type { CreateOrUpdateUserRequest } from '../models/CreateOrUpdateUserRequest';
import type { ForgotPasswordRequest } from '../models/ForgotPasswordRequest';
import type { OTPRequest } from '../models/OTPRequest';
import type { SignInRequest } from '../models/SignInRequest';
import type { StringApiResponse } from '../models/StringApiResponse';
import type { UserDTOApiResponse } from '../models/UserDTOApiResponse';
import type { CancelablePromise } from '../core/CancelablePromise';
import { OpenAPI } from '../core/OpenAPI';
import { request as __request } from '../core/request';
export class AuthService {
    /**
     * @returns UserDTOApiResponse OK
     * @throws ApiError
     */
    public static postApiAuthRegistration({
        requestBody,
    }: {
        requestBody?: CreateOrUpdateUserRequest,
    }): CancelablePromise<UserDTOApiResponse> {
        return __request(OpenAPI, {
            method: 'POST',
            url: '/api/auth/registration',
            body: requestBody,
            mediaType: 'application/json-patch+json',
        });
    }
    /**
     * @returns any OK
     * @throws ApiError
     */
    public static postApiAuthConfirmOtp({
        requestBody,
    }: {
        requestBody?: OTPRequest,
    }): CancelablePromise<any> {
        return __request(OpenAPI, {
            method: 'POST',
            url: '/api/auth/confirm-otp',
            body: requestBody,
            mediaType: 'application/json-patch+json',
        });
    }
    /**
     * @returns BooleanApiResponse OK
     * @throws ApiError
     */
    public static postApiAuthReSentOtp({
        requestBody,
    }: {
        requestBody?: string,
    }): CancelablePromise<BooleanApiResponse> {
        return __request(OpenAPI, {
            method: 'POST',
            url: '/api/auth/re-sent-otp',
            body: requestBody,
            mediaType: 'application/json-patch+json',
        });
    }
    /**
     * @returns any OK
     * @throws ApiError
     */
    public static postApiAuthForgotPassword({
        requestBody,
    }: {
        requestBody?: ForgotPasswordRequest,
    }): CancelablePromise<any> {
        return __request(OpenAPI, {
            method: 'POST',
            url: '/api/auth/forgot-password',
            body: requestBody,
            mediaType: 'application/json-patch+json',
        });
    }
    /**
     * @returns AuthenticationTokenObjectResponseApiResponse OK
     * @throws ApiError
     */
    public static postApiAuthSignin({
        requestBody,
    }: {
        requestBody?: SignInRequest,
    }): CancelablePromise<AuthenticationTokenObjectResponseApiResponse> {
        return __request(OpenAPI, {
            method: 'POST',
            url: '/api/auth/signin',
            body: requestBody,
            mediaType: 'application/json-patch+json',
        });
    }
    /**
     * @returns StringApiResponse OK
     * @throws ApiError
     */
    public static postApiAuthRenewToken(): CancelablePromise<StringApiResponse> {
        return __request(OpenAPI, {
            method: 'POST',
            url: '/api/auth/renew-token',
        });
    }
    /**
     * @returns any OK
     * @throws ApiError
     */
    public static postApiAuthResetEmail({
        emailAddress,
    }: {
        emailAddress?: string,
    }): CancelablePromise<any> {
        return __request(OpenAPI, {
            method: 'POST',
            url: '/api/auth/reset-email',
            query: {
                'emailAddress': emailAddress,
            },
        });
    }
    /**
     * @returns any OK
     * @throws ApiError
     */
    public static postApiAuthConfirmResetEmail({
        emailAddress,
        otpCode,
    }: {
        emailAddress?: string,
        otpCode?: string,
    }): CancelablePromise<any> {
        return __request(OpenAPI, {
            method: 'POST',
            url: '/api/auth/confirm-reset-email',
            query: {
                'emailAddress': emailAddress,
                'otpCode': otpCode,
            },
        });
    }
    /**
     * @returns any OK
     * @throws ApiError
     */
    public static postApiAuthResetPassword(): CancelablePromise<any> {
        return __request(OpenAPI, {
            method: 'POST',
            url: '/api/auth/reset-password',
        });
    }
    /**
     * @returns BooleanApiResponse OK
     * @throws ApiError
     */
    public static postApiAuthSignout(): CancelablePromise<BooleanApiResponse> {
        return __request(OpenAPI, {
            method: 'POST',
            url: '/api/auth/signout',
        });
    }
}
