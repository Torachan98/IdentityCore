/* generated using openapi-typescript-codegen -- do not edit */
/* istanbul ignore file */
/* tslint:disable */
/* eslint-disable */
import type { BooleanApiResponse } from '../models/BooleanApiResponse';
import type { CreateOrUpdateUserRequest } from '../models/CreateOrUpdateUserRequest';
import type { UserDTOApiResponse } from '../models/UserDTOApiResponse';
import type { UserDTOPaginationItemsApiResponse } from '../models/UserDTOPaginationItemsApiResponse';
import type { CancelablePromise } from '../core/CancelablePromise';
import { OpenAPI } from '../core/OpenAPI';
import { request as __request } from '../core/request';
export class UserService {
    /**
     * @returns UserDTOPaginationItemsApiResponse OK
     * @throws ApiError
     */
    public static getApiUser({
        regions,
        keyword,
        pageSize,
        pageNum,
    }: {
        regions?: Array<string>,
        keyword?: string,
        pageSize?: string,
        pageNum?: string,
    }): CancelablePromise<UserDTOPaginationItemsApiResponse> {
        return __request(OpenAPI, {
            method: 'GET',
            url: '/api/User',
            query: {
                'Regions': regions,
                'Keyword': keyword,
                'PageSize': pageSize,
                'PageNum': pageNum,
            },
        });
    }
    /**
     * @returns UserDTOApiResponse OK
     * @throws ApiError
     */
    public static postApiUser({
        requestBody,
    }: {
        requestBody?: CreateOrUpdateUserRequest,
    }): CancelablePromise<UserDTOApiResponse> {
        return __request(OpenAPI, {
            method: 'POST',
            url: '/api/User',
            body: requestBody,
            mediaType: 'application/json-patch+json',
        });
    }
    /**
     * @returns UserDTOApiResponse OK
     * @throws ApiError
     */
    public static putApiUser({
        requestBody,
    }: {
        requestBody?: CreateOrUpdateUserRequest,
    }): CancelablePromise<UserDTOApiResponse> {
        return __request(OpenAPI, {
            method: 'PUT',
            url: '/api/User',
            body: requestBody,
            mediaType: 'application/json-patch+json',
        });
    }
    /**
     * @returns BooleanApiResponse OK
     * @throws ApiError
     */
    public static deleteApiUser({
        guid,
    }: {
        guid?: string,
    }): CancelablePromise<BooleanApiResponse> {
        return __request(OpenAPI, {
            method: 'DELETE',
            url: '/api/User',
            query: {
                'guid': guid,
            },
        });
    }
    /**
     * @returns UserDTOApiResponse OK
     * @throws ApiError
     */
    public static getUserById({
        id,
    }: {
        id: string,
    }): CancelablePromise<UserDTOApiResponse> {
        return __request(OpenAPI, {
            method: 'GET',
            url: '/api/User/{id}',
            path: {
                'id': id,
            },
        });
    }
}
