import { HTTPClient } from "./client";

const mapApiError = (error) => {
  if (!error.response) {
    throw { type: "network", message: "Erro de rede ou servidor indisponível" };
  }

  const { status, data } = error.response;

  if (status === 400) {
    throw {
      type: "validation",
      message: data?.message || "Dados inválidos",
      errors: data?.errors || null,
    };
  }
  if (status === 404) {
    throw {
      type: "not_found",
      message: data?.message || "Recurso não encontrado",
    };
  }
  if (status === 500) {
    throw {
      type: "server",
      message: data?.message || "Erro interno. Tente novamente mais tarde.",
    };
  }

  throw { type: "unknown", message: data?.message || "Erro inesperado" };
};

const aiConfigApi = {
  async Create(data) {
    try {
      const response = await HTTPClient.post("/ai-config", data);
      return response.data;
    } catch (error) {
      return mapApiError(error);
    }
  },

  async GetAll() {
    try {
      const response = await HTTPClient.get("/ai-config");
      return response.data;
    } catch (error) {
      return mapApiError(error);
    }
  },

  async GetById(id) {
    try {
      const response = await HTTPClient.get(`/ai-config/${id}`);
      return response.data;
    } catch (error) {
      return mapApiError(error);
    }
  },

  async GetActive() {
    try {
      const response = await HTTPClient.get("/ai-config/ativa");
      return response.data;
    } catch (error) {
      return mapApiError(error);
    }
  },

  async Update(id, data) {
    try {
      await HTTPClient.put(`/ai-config/${id}`, data);
    } catch (error) {
      return mapApiError(error);
    }
  },

  async Delete(id) {
    try {
      await HTTPClient.delete(`/ai-config/${id}`);
    } catch (error) {
      return mapApiError(error);
    }
  },

  async Activate(id) {
    try {
      await HTTPClient.patch(`/ai-config/${id}/activate`);
    } catch (error) {
      return mapApiError(error);
    }
  },

  async Deactivate(id) {
    try {
      await HTTPClient.patch(`/ai-config/${id}/deactivate`);
    } catch (error) {
      return mapApiError(error);
    }
  },

  async GetModels() {
    try {
      const response = await HTTPClient.get("/ai-config/models");
      return response.data;
    } catch (error) {
      return mapApiError(error);
    }
  },
};

export default aiConfigApi;
