import { HTTPClient } from "./client";
import mapApiError from "../utils/MapApiError";

const userAPI = {
  async Register(userData) {
    try {
      const response = await HTTPClient.post(`/users/register`, userData);
      return response.data;
    } catch (error) {
      return mapApiError(error);
    }
  },
  
  async Create(userData) {
    try {
      const response = await HTTPClient.post(`/users`, userData);
      return response.data;
    } catch (error) {
      return mapApiError(error);
    }
  },

  async GetById(userId) {
    try {
      const response = await HTTPClient.get(`/users/${userId}`);
      return response.data;
    } catch (error) {
      return mapApiError(error);
    }
  },

  async GetByEmail(email) {
    try {
      const response = await HTTPClient.get(`/users/email/${email}`);
      return response.data;
    } catch (error) {
      return mapApiError(error);
    }
  },

  async GetPaged(options = {}) {
    try {
      const params = new URLSearchParams();

      if (options.isActive !== undefined && options.isActive !== null) {
        params.append("isActive", options.isActive);
      }

      if (options.pagina !== undefined && options.pagina !== null) {
        params.append("pagina", options.pagina);
      }

      if (
        options.quantidadePorPagina !== undefined &&
        options.quantidadePorPagina !== null
      ) {
        params.append("quantidadePorPagina", options.quantidadePorPagina);
      }

      const queryString = params.toString();
      const url = queryString ? `/users?${queryString}` : `/users`;

      const response = await HTTPClient.get(url);
      return response.data;
    } catch (error) {
      return mapApiError(error);
    }
  },

  async Update(userId, userData) {
    try {
      await HTTPClient.put(`/users/${userId}`, userData);
      return;
    } catch (error) {
      return mapApiError(error);
    }
  },

  async UpdatePassword(userId, passwordData) {
    try {
      await HTTPClient.patch(`/users/${userId}/password`, passwordData);
      return;
    } catch (error) {
      return mapApiError(error);
    }
  },

  async Delete(userId) {
    try {
      await HTTPClient.delete(`/users/${userId}`);
      return;
    } catch (error) {
      return mapApiError(error);
    }
  },

  async Deactivate(userId) {
    try {
      await HTTPClient.patch(`/users/${userId}/deactivate`);
      return;
    } catch (error) {
      return mapApiError(error);
    }
  },

  async Activate(userId) {
    try {
      await HTTPClient.patch(`/users/${userId}/activate`);
      return;
    } catch (error) {
      return mapApiError(error);
    }
  },
};

export default userAPI;
