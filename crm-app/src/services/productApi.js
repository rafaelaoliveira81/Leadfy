import mapApiError from "../utils/MapApiError";
import { HTTPClient } from "./client";

export const productAPI = {
  async Create(productData) {
    try {
      productData.id = 0;
      const response = await HTTPClient.post(`/products`, productData);
      return response.data;
    } catch (error) {
      return mapApiError(error);
    }
  },

  async GetById(productId) {
    try {
      const response = await HTTPClient.get(`/products/${productId}`);
      return response.data;
    } catch (error) {
      return mapApiError(error);
    }
  },

  async GetPaged(isActive = true, pagina = 1, quantidadePorPagina = 10) {
    try {
      const params = new URLSearchParams();

      if (isActive !== null && isActive !== undefined) {
        params.append("isActive", isActive);
      }
      params.append("pagina", pagina);
      params.append("quantidadePorPagina", quantidadePorPagina);

      const response = await HTTPClient.get(`/products?${params.toString()}`);
      return response.data;
    } catch (error) {
      return mapApiError(error);
    }
  },

  async Update(productData) {
    try {
      await HTTPClient.put("/products", productData);
      return;
    } catch (error) {
      return mapApiError(error);
    }
  },

  async Delete(productId) {
    try {
      await HTTPClient.delete(`/products/${productId}`);
      return;
    } catch (error) {
      return mapApiError(error);
    }
  },

  async Deactivate(productId) {
    try {
      await HTTPClient.patch(`/products/${productId}/deactivate`);
      return;
    } catch (error) {
      return mapApiError(error);
    }
  },

  async Activate(productId) {
    try {
      await HTTPClient.patch(`/products/${productId}/activate`);
      return;
    } catch (error) {
      return mapApiError(error);
    }
  },
};
