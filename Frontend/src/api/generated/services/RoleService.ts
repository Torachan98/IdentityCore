/* generated using openapi-typescript-codegen -- do not edit */
/* istanbul ignore file */
/* tslint:disable */
/* eslint-disable */
import type { CreateOrUpdateRoleRequest } from '../models/CreateOrUpdateRoleRequest';
import type { RoleDTOApiResponse } from '../models/RoleDTOApiResponse';
import type { RoleDTOPaginationItemsApiResponse } from '../models/RoleDTOPaginationItemsApiResponse';
import type { CancelablePromise } from '../core/CancelablePromise';
import { OpenAPI } from '../core/OpenAPI';
import { request as __request } from '../core/request';
export class RoleService {
    /**
     * @returns RoleDTOPaginationItemsApiResponse OK
     * @throws ApiError
     */
    public static getApiRole({
        isLock,
        keyword,
        pageSize,
        pageNum,
    }: {
        isLock?: boolean,
        keyword?: string,
        pageSize?: string,
        pageNum?: string,
    }): CancelablePromise<RoleDTOPaginationItemsApiResponse> {
        return __request(OpenAPI, {
            method: 'GET',
            url: '/api/Role',
            query: {
                'IsLock': isLock,
                'Keyword': keyword,
                'PageSize': pageSize,
                'PageNum': pageNum,
            },
        });
    }
    /**
     * @returns RoleDTOApiResponse OK
     * @throws ApiError
     */
    public static postApiRole({
        requestBody,
    }: {
        requestBody?: CreateOrUpdateRoleRequest,
    }): CancelablePromise<RoleDTOApiResponse> {
        return __request(OpenAPI, {
            method: 'POST',
            url: '/api/Role',
            body: requestBody,
            mediaType: 'application/json-patch+json',
        });
    }
    /**
     * @returns RoleDTOApiResponse OK
     * @throws ApiError
     */
    public static putApiRole({
        requestBody,
    }: {
        requestBody?: CreateOrUpdateRoleRequest,
    }): CancelablePromise<RoleDTOApiResponse> {
        return __request(OpenAPI, {
            method: 'PUT',
            url: '/api/Role',
            body: requestBody,
            mediaType: 'application/json-patch+json',
        });
    }
    /**
     * @returns any OK
     * @throws ApiError
     */
    public static deleteApiRole({
        guid,
    }: {
        guid?: string,
    }): CancelablePromise<any> {
        return __request(OpenAPI, {
            method: 'DELETE',
            url: '/api/Role',
            query: {
                'guid': guid,
            },
        });
    }
    /**
     * @returns RoleDTOApiResponse OK
     * @throws ApiError
     */
    public static getRoleById({
        id,
    }: {
        id: string,
    }): CancelablePromise<RoleDTOApiResponse> {
        return __request(OpenAPI, {
            method: 'GET',
            url: '/api/Role/{id}',
            path: {
                'id': id,
            },
        });
    }
}
