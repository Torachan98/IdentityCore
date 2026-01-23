/* generated using openapi-typescript-codegen -- do not edit */
/* istanbul ignore file */
/* tslint:disable */
/* eslint-disable */
import type { CreateOrUpdatePermissionRequest } from '../models/CreateOrUpdatePermissionRequest';
import type { PermissionDTOApiResponse } from '../models/PermissionDTOApiResponse';
import type { PermissionDTOPaginationItemsApiResponse } from '../models/PermissionDTOPaginationItemsApiResponse';
import type { CancelablePromise } from '../core/CancelablePromise';
import { OpenAPI } from '../core/OpenAPI';
import { request as __request } from '../core/request';
export class PermissionService {
    /**
     * @returns PermissionDTOPaginationItemsApiResponse OK
     * @throws ApiError
     */
    public static getApiPermission({
        keyword,
        pageSize,
        pageNum,
    }: {
        keyword?: string,
        pageSize?: string,
        pageNum?: string,
    }): CancelablePromise<PermissionDTOPaginationItemsApiResponse> {
        return __request(OpenAPI, {
            method: 'GET',
            url: '/api/Permission',
            query: {
                'Keyword': keyword,
                'PageSize': pageSize,
                'PageNum': pageNum,
            },
        });
    }
    /**
     * @returns PermissionDTOApiResponse OK
     * @throws ApiError
     */
    public static postApiPermission({
        requestBody,
    }: {
        requestBody?: CreateOrUpdatePermissionRequest,
    }): CancelablePromise<PermissionDTOApiResponse> {
        return __request(OpenAPI, {
            method: 'POST',
            url: '/api/Permission',
            body: requestBody,
            mediaType: 'application/json-patch+json',
        });
    }
    /**
     * @returns PermissionDTOApiResponse OK
     * @throws ApiError
     */
    public static putApiPermission({
        requestBody,
    }: {
        requestBody?: CreateOrUpdatePermissionRequest,
    }): CancelablePromise<PermissionDTOApiResponse> {
        return __request(OpenAPI, {
            method: 'PUT',
            url: '/api/Permission',
            body: requestBody,
            mediaType: 'application/json-patch+json',
        });
    }
    /**
     * @returns any OK
     * @throws ApiError
     */
    public static deleteApiPermission({
        guid,
    }: {
        guid?: string,
    }): CancelablePromise<any> {
        return __request(OpenAPI, {
            method: 'DELETE',
            url: '/api/Permission',
            query: {
                'guid': guid,
            },
        });
    }
}
