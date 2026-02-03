/* generated using openapi-typescript-codegen -- do not edit */
/* istanbul ignore file */
/* tslint:disable */
/* eslint-disable */
import type { AssigningService } from './AssigningService';
import type { RoleResponse } from './RoleResponse';
export type CreateOrUpdateUserRequest = {
    guid?: string;
    password?: string;
    email?: string;
    avatarUrl?: string;
    userName?: string;
    firstName?: string;
    middleName?: string;
    lastName?: string;
    phone?: string;
    phoneCode?: string;
    region?: string;
    isRequiredChangePassword?: boolean;
    isPasswordExpired?: boolean;
    expirationDate?: string;
    locked?: string;
    attemptLogin?: number;
    fcmToken?: string;
    isLock?: boolean;
    permissions?: Array<string>;
    services?: Array<AssigningService>;
    roles?: Array<RoleResponse>;
};

