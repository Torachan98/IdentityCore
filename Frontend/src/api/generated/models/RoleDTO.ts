/* generated using openapi-typescript-codegen -- do not edit */
/* istanbul ignore file */
/* tslint:disable */
/* eslint-disable */
import type { PermissionDTO } from './PermissionDTO';
import type { Role } from './Role';
export type RoleDTO = {
    guid?: string;
    name?: string;
    role?: Role;
    isLock?: boolean;
    description?: string;
    permissions?: Array<PermissionDTO>;
};

