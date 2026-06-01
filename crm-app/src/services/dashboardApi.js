import mapApiError from "../utils/MapApiError";
import { HTTPClient } from "./client";

export const dashboardAPI = {
  async GetData() {
    try {
      const response = await HTTPClient.get("/dashboard");
      return response.data;
    } catch (error) {
      return mapApiError(error);
    }
  },
};
