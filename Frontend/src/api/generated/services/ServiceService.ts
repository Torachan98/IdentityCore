/* generated using openapi-typescript-codegen -- do not edit */
/* istanbul ignore file */
/* tslint:disable */
/* eslint-disable */
import type { CreateOrUpdateServiceRequest } from '../models/CreateOrUpdateServiceRequest';
import type { ServiceDTOApiResponse } from '../models/ServiceDTOApiResponse';
import type { ServiceDTOPaginationItemsApiResponse } from '../models/ServiceDTOPaginationItemsApiResponse';
import type { CancelablePromise } from '../core/CancelablePromise';
import { OpenAPI } from '../core/OpenAPI';
import { request as __request } from '../core/request';
export class ServiceService {
    /**
     * @returns ServiceDTOPaginationItemsApiResponse OK
     * @throws ApiError
     */
    public static getApiService({
        keyword,
        pageSize,
        pageNum,
    }: {
        keyword?: string,
        pageSize?: string,
        pageNum?: string,
    }): CancelablePromise<ServiceDTOPaginationItemsApiResponse> {
        return __request(OpenAPI, {
            method: 'GET',
            url: '/api/Service',
            query: {
                'Keyword': keyword,
                'PageSize': pageSize,
                'PageNum': pageNum,
            },
        });
    }
    /**
     * @returns ServiceDTOApiResponse OK
     * @throws ApiError
     */
    public static postApiService({
        requestBody,
    }: {
        requestBody?: CreateOrUpdateServiceRequest,
    }): CancelablePromise<ServiceDTOApiResponse> {
        return __request(OpenAPI, {
            method: 'POST',
            url: '/api/Service',
            body: requestBody,
            mediaType: 'application/json-patch+json',
        });
    }
    /**
     * @returns ServiceDTOApiResponse OK
     * @throws ApiError
     */
    public static putApiService({
        requestBody,
    }: {
        requestBody?: CreateOrUpdateServiceRequest,
    }): CancelablePromise<ServiceDTOApiResponse> {
        return __request(OpenAPI, {
            method: 'PUT',
            url: '/api/Service',
            body: requestBody,
            mediaType: 'application/json-patch+json',
        });
    }
    /**
     * @returns any OK
     * @throws ApiError
     */
    public static deleteApiService({
        guid,
    }: {
        guid?: string,
    }): CancelablePromise<any> {
        return __request(OpenAPI, {
            method: 'DELETE',
            url: '/api/Service',
            query: {
                'guid': guid,
            },
        });
    }
}
