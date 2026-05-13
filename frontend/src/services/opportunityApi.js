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

const opportunityAPI = {
  /**
   * Cria uma nova oportunidade.
   * @param {Object} opportunityData - Dados da oportunidade (title, leadId, ownerId, productId, stage, amount, expectedCloseDate).
   * @returns {Promise<{id: number}>} ID da oportunidade criada.
   */
  async Create(opportunityData) {
    try {
      const response = await HTTPClient.post(`/opportunities`, opportunityData);
      return response.data;
    } catch (error) {
      return mapApiError(error);
    }
  },

  /**
   * Obtém uma oportunidade pelo ID.
   * @param {number} opportunityId - ID da oportunidade.
   * @returns {Promise<OpportunityResponse>} Dados da oportunidade.
   */
  async GetById(opportunityId) {
    try {
      const response = await HTTPClient.get(`/opportunities/${opportunityId}`);
      return response.data;
    } catch (error) {
      return mapApiError(error);
    }
  },

  /**
   * Lista oportunidades com filtros opcionais.
   * @param {Object} options - Opções de filtro.
   * @param {boolean} [options.isActive] - Filtrar por status de ativação (opcional).
   * @param {string} [options.title] - Filtrar por título contendo o valor (opcional).
   * @returns {Promise<OpportunityResponse[]>} Lista de oportunidades.
   */
  async GetAll(options = {}) {
    try {
      const params = new URLSearchParams();

      if (options.isActive !== undefined && options.isActive !== null) {
        params.append("isActive", options.isActive);
      } else if (options.title) {
        params.append("title", options.title);
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

  /**
   * Atualiza uma oportunidade existente.
   * @param {number} opportunityId - ID da oportunidade a atualizar.
   * @param {Object} opportunityData - Dados atualizados (title, leadId, ownerId, productId, stage, amount, expectedCloseDate).
   * @returns {Promise<void>} Sem conteúdo na resposta (204).
   */
  async Update(opportunityId, opportunityData) {
    try {
      await HTTPClient.put(`/opportunities/${opportunityId}`, opportunityData);
      return;
    } catch (error) {
      return mapApiError(error);
    }
  },

  /**
   * Deleta uma oportunidade.
   * @param {number} opportunityId - ID da oportunidade a deletar.
   * @returns {Promise<void>} Sem conteúdo na resposta (204).
   */
  async Delete(opportunityId) {
    try {
      await HTTPClient.delete(`/opportunities/${opportunityId}`);
      return;
    } catch (error) {
      return mapApiError(error);
    }
  },

  /**
   * Desativa uma oportunidade.
   * @param {number} opportunityId - ID da oportunidade a desativar.
   * @returns {Promise<void>} Sem conteúdo na resposta (204).
   */
  async Deactivate(opportunityId) {
    try {
      await HTTPClient.patch(`/opportunities/${opportunityId}/deactivate`);
      return;
    } catch (error) {
      return mapApiError(error);
    }
  },

  /**
   * Ativa uma oportunidade.
   * @param {number} opportunityId - ID da oportunidade a ativar.
   * @returns {Promise<void>} Sem conteúdo na resposta (204).
   */
  async Activate(opportunityId) {
    try {
      await HTTPClient.patch(`/opportunities/${opportunityId}/activate`);
      return;
    } catch (error) {
      return mapApiError(error);
    }
  },

  /**
   * Altera o stage de uma oportunidade (usado pelo Kanban).
   * @param {number} opportunityId - ID da oportunidade.
   * @param {number} stage - Novo stage (int do enum backend).
   * @returns {Promise<void>}
   */
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

  /**
   * Atualiza a ordenação de múltiplas oportunidades em lote (Kanban reorder).
   * @param {Array<{id: number, stage: number, sortOrder: number}>} items
   * @returns {Promise<void>}
   */
  async PatchSortOrder(items) {
    try {
      await HTTPClient.patch(`/opportunities/reorder`, { items });
      return;
    } catch (error) {
      return mapApiError(error);
    }
  },

  /**
   * Gera o plano de ação para uma oportunidade usando a configuração de IA informada.
   * @param {number} opportunityId - ID da oportunidade.
   * @param {number} configId - ID da configuração de IA.
   * @returns {Promise<{opportunityId, configId, title, actionPlan, actionPlanGeneratedAt}>}
   */
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
};

export default opportunityAPI;
