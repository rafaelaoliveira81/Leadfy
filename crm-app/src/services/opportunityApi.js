import mapApiError from "../utils/MapApiError";
import { HTTPClient } from "./client";

const opportunityAPI = {
  async Create(opportunityData) {
    try {
      const response = await HTTPClient.post(`/opportunities`, opportunityData);
      return response.data;
    } catch (error) {
      return mapApiError(error);
    }
  },

  async GetById(opportunityId) {
    try {
      const response = await HTTPClient.get(`/opportunities/${opportunityId}`);
      return response.data;
    } catch (error) {
      return mapApiError(error);
    }
  },

  async GetAll(options = {}) {
    try {
      const params = new URLSearchParams();

      if (options.isActive !== undefined && options.isActive !== null) {
        params.append("isActive", options.isActive);
      }

      if (options.leadId !== undefined && options.leadId !== null) {
        params.append("leadId", options.leadId);
      }

      const queryString = params.toString();
      const url = queryString
        ? `/opportunities?${queryString}`
        : `/opportunities`;

      const response = await HTTPClient.get(url);
      return response.data;
    } catch (error) {
      return mapApiError(error);
    }
  },

  async GetByStage(stage) {
    try {
      const response = await HTTPClient.get(`/opportunities/stage/${stage}`);
      return response.data;
    } catch (error) {
      return mapApiError(error);
    }
  },

  async Update(opportunityId, opportunityData) {
    try {
      await HTTPClient.put(`/opportunities/${opportunityId}`, opportunityData);
      return;
    } catch (error) {
      return mapApiError(error);
    }
  },

  async Delete(opportunityId) {
    try {
      await HTTPClient.delete(`/opportunities/${opportunityId}`);
      return;
    } catch (error) {
      return mapApiError(error);
    }
  },

  async Deactivate(opportunityId) {
    try {
      await HTTPClient.patch(`/opportunities/${opportunityId}/deactivate`);
      return;
    } catch (error) {
      return mapApiError(error);
    }
  },

  async Activate(opportunityId) {
    try {
      await HTTPClient.patch(`/opportunities/${opportunityId}/activate`);
      return;
    } catch (error) {
      return mapApiError(error);
    }
  },

  async PatchStage(opportunityId, stage) {
    try {
      await HTTPClient.patch(`/opportunities/${opportunityId}/stage`, {
        stage,
      });
      return;
    } catch (error) {
      return mapApiError(error);
    }
  },

  async PatchSortOrder(items) {
    try {
      await HTTPClient.patch(`/opportunities/reorder`, { items });
      return;
    } catch (error) {
      return mapApiError(error);
    }
  },

  async GenerateActionPlan(opportunityId, configId) {
    try {
      const response = await HTTPClient.post(
        `/opportunities/${opportunityId}/generate-action-plan`,
        { configId },
      );
      return response.data;
    } catch (error) {
      return mapApiError(error);
    }
  },

  async GetActionPlans(opportunityId) {
    try {
      const response = await HTTPClient.get(
        `/opportunities/${opportunityId}/action-plans`,
      );
      return response.data;
    } catch (error) {
      return mapApiError(error);
    }
  },
};

export default opportunityAPI;
