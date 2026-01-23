/* generated using openapi-typescript-codegen -- do not edit */
/* istanbul ignore file */
/* tslint:disable */
/* eslint-disable */
import type { CancelablePromise } from '../core/CancelablePromise';
import { OpenAPI } from '../core/OpenAPI';
import { request as __request } from '../core/request';
export class FileService {
    /**
     * @returns any OK
     * @throws ApiError
     */
    public static postApiFile({
        formData,
    }: {
        formData?: {
            FileType?: number;
            File?: Blob;
        },
    }): CancelablePromise<any> {
        return __request(OpenAPI, {
            method: 'POST',
            url: '/api/File',
            formData: formData,
            mediaType: 'multipart/form-data',
        });
    }
}
