import mapApiError from "../utils/MapApiError";
import { HTTPClient } from "./client";

export const promptAPI = {
  async Create(promptData) {
    try {
      const payload = { ...promptData, id: null };
      const response = await HTTPClient.post(`/prompts`, payload);
      return response.data;
    } catch (error) {
      return mapApiError(error);
    }
  },

  async GetById(promptId) {
    try {
      const response = await HTTPClient.get(`/prompts/${promptId}`);
      return response.data;
    } catch (error) {
      return mapApiError(error);
    }
  },

  async GetPaged(options = {}) {
    try {
      const params = new URLSearchParams();

      if (options.isActive !== null && options.isActive !== undefined) {
        params.append("isActive", options.isActive);
      }

      if (options.pagina !== null && options.pagina !== undefined) {
        params.append("pagina", options.pagina);
      }

      if (
        options.quantidadePorPagina !== null &&
        options.quantidadePorPagina !== undefined
      ) {
        params.append("quantidadePorPagina", options.quantidadePorPagina);
      }

      const queryString = params.toString();
      const url = queryString ? `/prompts?${queryString}` : `/prompts`;

      const response = await HTTPClient.get(url);
      return response.data;
    } catch (error) {
      return mapApiError(error);
    }
  },

  async Update(promptData) {
    try {
      await HTTPClient.put("/prompts", promptData);
      return;
    } catch (error) {
      return mapApiError(error);
    }
  },

  async Delete(promptId) {
    try {
      await HTTPClient.delete(`/prompts/${promptId}`);
      return;
    } catch (error) {
      return mapApiError(error);
    }
  },

  async Deactivate(promptId) {
    try {
      await HTTPClient.patch(`/prompts/${promptId}/deactivate`);
      return;
    } catch (error) {
      return mapApiError(error);
    }
  },

  async Activate(promptId) {
    try {
      await HTTPClient.patch(`/prompts/${promptId}/activate`);
      return;
    } catch (error) {
      return mapApiError(error);
    }
  },

  async OptimizePrompt(prompt) {
    try {
      const response = await HTTPClient.post(
        `/prompts/optimize`, prompt);
      return response.data;
    } catch (error) {
      return mapApiError(error);
    }
  },
};
