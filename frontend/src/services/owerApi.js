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

const owerAPI = {
    /**
     * Cria um novo ower.
     * @param {Object} owerData - Dados do ower (name, userId).
     * @returns {Promise<{id: number}>} ID do ower criado.
     */
    async Create(owerData) {
        try {
            const response = await HTTPClient.post(`/owers`, owerData);
            return response.data;
        } catch (error) {
            return mapApiError(error);
        }
    },

    /**
     * Obtém um ower pelo ID.
     * @param {number} owerId - ID do ower.
     * @returns {Promise<OwerResponse>} Dados do ower.
     */
    async GetById(owerId) {
        try {
            const response = await HTTPClient.get(`/owers/${owerId}`);
            return response.data;
        } catch (error) {
            return mapApiError(error);
        }
    },

    /**
     * Lista owers com filtros opcionais.
     * @param {Object} options - Opções de filtro.
     * @param {boolean} [options.isActive] - Filtrar por status de ativação (opcional).
     * @param {string} [options.name] - Filtrar por nome contendo o valor (opcional).
     * @returns {Promise<OwerResponse[]>} Lista de owers.
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
            const url = queryString ? `/owers?${queryString}` : `/owers`;
            
            const response = await HTTPClient.get(url);
            return response.data;
        } catch (error) {
            return mapApiError(error);
        }
    },

    /**
     * Atualiza um ower existente.
     * @param {number} owerId - ID do ower a atualizar.
     * @param {Object} owerData - Dados atualizados (name, userId, isActive).
     * @returns {Promise<void>} Sem conteúdo na resposta (204).
     */
    async Update(owerId, owerData) {
        try {
            await HTTPClient.put(`/owers/${owerId}`, owerData);
            return;
        } catch (error) {
            return mapApiError(error);
        }
    },

    /**
     * Deleta um ower.
     * @param {number} owerId - ID do ower a deletar.
     * @returns {Promise<void>} Sem conteúdo na resposta (204).
     */
    async Delete(owerId) {
        try {
            await HTTPClient.delete(`/owers/${owerId}`);
            return;
        } catch (error) {
            return mapApiError(error);
        }
    },

    /**
     * Desativa um ower.
     * @param {number} owerId - ID do ower a desativar.
     * @returns {Promise<void>} Sem conteúdo na resposta (204).
     */
    async Deactivate(owerId) {
        try {
            await HTTPClient.patch(`/owers/${owerId}/deactivate`);
            return;
        } catch (error) {
            return mapApiError(error);
        }
    },

    /**
     * Ativa um ower.
     * @param {number} owerId - ID do ower a ativar.
     * @returns {Promise<void>} Sem conteúdo na resposta (204).
     */
    async Activate(owerId) {
        try {
            await HTTPClient.patch(`/owers/${owerId}/activate`);
            return;
        } catch (error) {
            return mapApiError(error);
        }
    }
};

export default owerAPI;
