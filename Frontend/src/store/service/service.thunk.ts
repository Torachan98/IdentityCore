import { createAsyncThunk } from "@reduxjs/toolkit";
import {
  CreateOrUpdateServiceRequest,
  ServiceDTOApiResponse,
  ServiceDTOPaginationItemsApiResponse,
  ServiceService,
} from "@/api/generated";
import { FetchParams } from "../base.types";

export const fetchServices = createAsyncThunk<
  ServiceDTOPaginationItemsApiResponse,
  FetchParams
>("service/getServices", async (query: FetchParams) => {
  return await ServiceService.getApiService(query);
});

export const createService = createAsyncThunk<
  ServiceDTOApiResponse,
  CreateOrUpdateServiceRequest
>("service/createService", async (payload: CreateOrUpdateServiceRequest) => {
  return await ServiceService.postApiService({ requestBody: payload });
});

export const updateService = createAsyncThunk<
  ServiceDTOApiResponse,
  CreateOrUpdateServiceRequest
>("service/updateService", async (payload: CreateOrUpdateServiceRequest) => {
  return await ServiceService.putApiService({ requestBody: payload });
});

export const deleteService = createAsyncThunk<any, string>(
  "service/deleteService",
  async (id: string) => {
    return await ServiceService.deleteApiService({ guid: id });
  },
);
