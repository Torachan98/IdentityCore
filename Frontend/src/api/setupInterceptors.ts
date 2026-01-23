import axios from "axios";

export const setupInterceptors = (
  getAccessToken: () => string | null,
  refreshToken: () => Promise<void>,
) => {
  axios.interceptors.request.use((config) => {
    const token = getAccessToken();
    if (token) {
      config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
  });

  axios.interceptors.response.use(
    (res) => res,
    async (error) => {
      if (error.response?.status === 400 || error.response?.status === 401) {
        await refreshToken();
        return axios(error.config);
      }
      return Promise.reject(error);
    },
  );
};
