import { createSlice } from "@reduxjs/toolkit";
import { ServiceState } from "./service.type";
import { fetchServices } from "./service.thunk";

const initialState: ServiceState = {
  services: null,
  loading: false,
  error: null,
  pageSize: 1,
  pageNum: 10,
  totalItems: 0,
  totalPages: 0,
};

const serviceSlice = createSlice({
  name: "service",
  initialState: initialState,
  reducers: {},
  extraReducers: (builder) => {
    builder
      .addCase(fetchServices.pending, (state) => {
        state.loading = true;
        state.error = null;
      })

      .addCase(fetchServices.fulfilled, (state, action) => {
        state.loading = false;
        if (action.payload.isSuccess) {
          state.services = action.payload.data?.items ?? [];
          state.pageNum = action.payload.data?.pageNum ?? 1;
          state.pageSize = action.payload.data?.pageSize ?? 10;
          state.totalPages = action.payload.data?.totalPage ?? 0;
          state.totalItems = action.payload.data?.totalCount ?? 0;
        }

        state.error = action.payload.message ?? "";
      })

      .addCase(fetchServices.rejected, (state) => {
        state.loading = false;
        state.error = "Cannot GET /api/Service";
      });

    // .addCase()
    // .addCase()
    // .addCase();
  },
});

export default serviceSlice.reducer;
