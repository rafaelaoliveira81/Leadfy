import { HTTPClient } from './client';

/**
 * Centraliza o mapeamento de erros da API para um formato padronizado.
 * @param {Error} error - Erro capturado do axios.
 * @returns {Error} Erro mapeado com tipo e mensagem consistentes.
 */
const mapApiError = (error) => {
    if (!error.response) {
        throw {
            type: 'network',
            message: 'Erro de rede ou servidor indisponível'
        };
    }

    const { status, data } = error.response;

    if (status === 400) {
        throw {
            type: 'validation',
            message: data?.message || 'Dados inválidos',
            errors: data?.errors || null
        };
    }

    if (status === 404) {
        throw {
            type: 'not_found',
            message: data?.message || 'Recurso não encontrado'
        };
    }

    if (status === 500) {
        throw {
            type: 'server',
            message: data?.message || 'Erro interno. Tente novamente mais tarde.'
        };
    }

    throw {
        type: 'unknown',
        message: data?.message || 'Erro inesperado'
    };
};

const leadAPI = {
    /**
     * Cria um novo lead.
     * @param {Object} leadData - Dados do lead (name, email, phoneNumber).
     * @returns {Promise<{id: number}>} ID do lead criado.
     */
    async Create(leadData) {
        try {
            const response = await HTTPClient.post(`/leads`, leadData);
            return response.data;
        } catch (error) {
            return mapApiError(error);
        }
    },

    /**
     * Obtém um lead pelo ID.
     * @param {number} leadId - ID do lead.
     * @returns {Promise<LeadResponse>} Dados do lead.
     */
    async GetById(leadId) {
        try {
            const response = await HTTPClient.get(`/leads/${leadId}`);
            return response.data;
        } catch (error) {
            return mapApiError(error);
        }
    },

    /**
     * Lista leads com filtros opcionais.
     * @param {Object} options - Opções de filtro.
     * @param {boolean} [options.isActive] - Filtrar por status de ativação (opcional).
     * @param {string} [options.name] - Filtrar por nome contendo o valor (opcional).
     * @returns {Promise<LeadResponse[]>} Lista de leads.
     */
    async GetAll(options = {}) {
        try {
            const params = new URLSearchParams();
            
            if (options.isActive !== undefined && options.isActive !== null) {
                params.append('isActive', options.isActive);
            } else if (options.name) {
                params.append('name', options.name);
            }

            const queryString = params.toString();
            const url = queryString ? `/leads?${queryString}` : `/leads`;
            
            const response = await HTTPClient.get(url);
            return response.data;
        } catch (error) {
            return mapApiError(error);
        }
    },

    /**
     * Atualiza um lead existente.
     * @param {number} leadId - ID do lead a atualizar.
     * @param {Object} leadData - Dados atualizados (name, email, phoneNumber).
     * @returns {Promise<void>} Sem conteúdo na resposta (204).
     */
    async Update(leadId, leadData) {
        try {
            await HTTPClient.put(`/leads/${leadId}`, leadData);
            return;
        } catch (error) {
            return mapApiError(error);
        }
    },

    /**
     * Deleta um lead.
     * @param {number} leadId - ID do lead a deletar.
     * @returns {Promise<void>} Sem conteúdo na resposta (204).
     */
    async Delete(leadId) {
        try {
            await HTTPClient.delete(`/leads/${leadId}`);
            return;
        } catch (error) {
            return mapApiError(error);
        }
    },

    /**
     * Desativa um lead.
     * @param {number} leadId - ID do lead a desativar.
     * @returns {Promise<void>} Sem conteúdo na resposta (204).
     */
    async Deactivate(leadId) {
        try {
            await HTTPClient.patch(`/leads/${leadId}/deactivate`);
            return;
        } catch (error) {
            return mapApiError(error);
        }
    },

    /**
     * Ativa um lead.
     * @param {number} leadId - ID do lead a ativar.
     * @returns {Promise<void>} Sem conteúdo na resposta (204).
     */
    async Activate(leadId) {
        try {
            await HTTPClient.patch(`/leads/${leadId}/activate`);
            return;
        } catch (error) {
            return mapApiError(error);
        }
    }
};

export default leadAPI;
