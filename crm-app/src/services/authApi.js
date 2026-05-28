import mapApiError from "../utils/MapApiError";
import { HTTPClient } from "./client";

export const authAPI = {
  async Authentication(userData) {
    try {
      const response = await HTTPClient.post(`/auth/login`, userData);
      return response.data;
    } catch (error) {
      return mapApiError(error);
    }
  },
};
