import axios from "axios";

export const HTTPClient = axios.create({
  baseURL: "https://localhost:7287/api",
});

HTTPClient.interceptors.request.use(
  (config) => {
    const token = localStorage.getItem("authToken");
    if (token) {
      config.headers.Authorization = `Bearer ${token}`;
    } else if (config.headers?.Authorization) {
      delete config.headers.Authorization;
    }

    return config;
  },
  (error) => Promise.reject(error),
);

export default HTTPClient;
