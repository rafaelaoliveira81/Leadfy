import { HTTPClient } from "./client";

/**
 * Centraliza o mapeamento de erros da API para um formato padronizado.
 * @param {Error} error - Erro capturado do axios.
 * @returns {Error} Erro mapeado com tipo e mensagem consistentes.
 */
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

const ownerAPI = {
  /**
   * Cria um novo owner.
   * @param {Object} ownerData - Dados do owner (name, userId).
   * @returns {Promise<{id: number}>} ID do owner criado.
   */
  async Create(ownerData) {
    try {
      const response = await HTTPClient.post(`/owners`, ownerData);
      return response.data;
    } catch (error) {
      return mapApiError(error);
    }
  },

  /**
   * Obtém um owner pelo ID.
   * @param {number} ownerId - ID do owner.
   * @returns {Promise<OwnerResponse>} Dados do owner.
   */
  async GetById(ownerId) {
    try {
      const response = await HTTPClient.get(`/owners/${ownerId}`);
      return response.data;
    } catch (error) {
      return mapApiError(error);
    }
  },

  /**
   * Lista owners com filtros opcionais.
   * @param {Object} options - Opções de filtro.
   * @param {boolean} [options.isActive] - Filtrar por status de ativação (opcional).
   * @param {string} [options.name] - Filtrar por nome contendo o valor (opcional).
   * @returns {Promise<OwnerResponse[]>} Lista de owners.
   */
  async GetAll(options = {}) {
    try {
      const params = new URLSearchParams();

      if (options.isActive !== undefined && options.isActive !== null) {
        params.append("isActive", options.isActive);
      } else if (options.name) {
        params.append("name", options.name);
      }

      const queryString = params.toString();
      const url = queryString ? `/owners?${queryString}` : `/owners`;

      const response = await HTTPClient.get(url);
      return response.data;
    } catch (error) {
      return mapApiError(error);
    }
  },

  /**
   * Atualiza um owner existente.
   * @param {number} ownerId - ID do owner a atualizar.
   * @param {Object} ownerData - Dados atualizados (name, userId, isActive).
   * @returns {Promise<void>} Sem conteúdo na resposta (204).
   */
  async Update(ownerId, ownerData) {
    try {
      await HTTPClient.put(`/owners/${ownerId}`, ownerData);
      return;
    } catch (error) {
      return mapApiError(error);
    }
  },

  /**
   * Deleta um owner.
   * @param {number} ownerId - ID do owner a deletar.
   * @returns {Promise<void>} Sem conteúdo na resposta (204).
   */
  async Delete(ownerId) {
    try {
      await HTTPClient.delete(`/owners/${ownerId}`);
      return;
    } catch (error) {
      return mapApiError(error);
    }
  },

  /**
   * Desativa um owner.
   * @param {number} ownerId - ID do owner a desativar.
   * @returns {Promise<void>} Sem conteúdo na resposta (204).
   */
  async Deactivate(ownerId) {
    try {
      await HTTPClient.patch(`/owners/${ownerId}/deactivate`);
      return;
    } catch (error) {
      return mapApiError(error);
    }
  },

  /**
   * Ativa um owner.
   * @param {number} ownerId - ID do owner a ativar.
   * @returns {Promise<void>} Sem conteúdo na resposta (204).
   */
  async Activate(ownerId) {
    try {
      await HTTPClient.patch(`/owners/${ownerId}/activate`);
      return;
    } catch (error) {
      return mapApiError(error);
    }
  },
};

export default ownerAPI;
