import { HTTPClient } from "./client";

const mapApiError = (error) => {
  if (!error.response) {
    throw {
      type: "network",
      message: "Erro de rede ou servidor indisponível",
    };
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

  throw {
    type: "unknown",
    message: data?.message || "Erro inesperado",
  };
};

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

  async GetAll(options = {}) {
    try {
      const params = new URLSearchParams();

      if (options.isActive !== undefined && options.isActive !== null) {
        params.append("isActive", options.isActive);
      } else if (options.name) {
        params.append("name", options.name);
      }

      const queryString = params.toString();
      const url = queryString ? `/leads?${queryString}` : `/leads`;

      const response = await HTTPClient.get(url);
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
