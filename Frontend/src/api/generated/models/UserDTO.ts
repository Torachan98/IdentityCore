/* generated using openapi-typescript-codegen -- do not edit */
/* istanbul ignore file */
/* tslint:disable */
/* eslint-disable */
import type { AssigningService } from './AssigningService';
import type { RoleResponse } from './RoleResponse';
export type UserDTO = {
    userId?: number;
    avatarUrl?: string;
    userName?: string;
    fullName?: string;
    email?: string;
    password?: string;
    phone?: string;
    phoneCode?: string;
    region?: string;
    guid?: string;
    dateCreated?: string;
    dateModified?: string;
    firstName?: string;
    middleName?: string;
    lastName?: string;
    isRequiredChangePassword?: boolean;
    isPasswordExpired?: boolean;
    isDeleted?: boolean;
    isLogin?: boolean;
    isActive?: boolean;
    otpCode?: string;
    attemptLogin?: number;
    locked?: string;
    lastLogin?: string;
    expirationDate?: string;
    otpLifeTime?: string;
    refreshToken?: string;
    fcmToken?: string;
    step?: number;
    permissions?: Array<string>;
    services?: Array<AssigningService>;
    roles?: Array<RoleResponse>;
};

