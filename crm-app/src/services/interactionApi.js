import { HTTPClient } from "./client";
import mapApiError from "../utils/MapApiError";

const interactionApi = {
  async AddToOpportunity(opportunityId, data) {
    try {
      const response = await HTTPClient.post(
        `/opportunities/${opportunityId}/interactions`,
        data,
      );
      return response.data;
    } catch (error) {
      return mapApiError(error);
    }
  },

  async GetByOpportunityId(opportunityId) {
    try {
      const response = await HTTPClient.get(
        `/opportunities/${opportunityId}/interactions`,
      );
      return response.data;
    } catch (error) {
      return mapApiError(error);
    }
  },

  async GetById(id) {
    try {
      const response = await HTTPClient.get(`/interactions/${id}`);
      return response.data;
    } catch (error) {
      return mapApiError(error);
    }
  },

  async Delete(id) {
    try {
      await HTTPClient.delete(`/interactions/${id}`);
    } catch (error) {
      return mapApiError(error);
    }
  },
};

export default interactionApi;
