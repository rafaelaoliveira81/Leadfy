import mapApiError from "../utils/MapApiError";
import { HTTPClient } from "./client";

export const leadAPI = {
  async Create(leadData) {
    try {
      leadData.id = 0;
      const response = await HTTPClient.post(`/leads`, leadData);
      return response.data;
    } catch (error) {
      return mapApiError(error);
    }
  },

  async GetById(leadId) {
    try {
      const response = await HTTPClient.get(`/leads/${leadId}`);
      return response.data;
    } catch (error) {
      return mapApiError(error);
    }
  },

  async GetPaged(status = true, pagina = 1, quantidadePorPagina = 10) {
    try {
      const params = new URLSearchParams();

      if (status !== null && status !== undefined) {
        params.append("status", status);
      }
      params.append("pagina", pagina);
      params.append("quantidadePorPagina", quantidadePorPagina);

      const response = await HTTPClient.get(`/leads?${params.toString()}`);
      return response.data;
    } catch (error) {
      return mapApiError(error);
    }
  },

  async Update(leadData) {
    try {
      await HTTPClient.put("/leads", leadData);
      return;
    } catch (error) {
      return mapApiError(error);
    }
  },

  async Delete(leadId) {
    try {
      await HTTPClient.delete(`/leads/${leadId}`);
      return;
    } catch (error) {
      return mapApiError(error);
    }
  },

  async Deactivate(leadId) {
    try {
      await HTTPClient.patch(`/leads/${leadId}/deactivate`);
      return;
    } catch (error) {
      return mapApiError(error);
    }
  },

  async Activate(leadId) {
    try {
      await HTTPClient.patch(`/leads/${leadId}/activate`);
      return;
    } catch (error) {
      return mapApiError(error);
    }
  },
};
