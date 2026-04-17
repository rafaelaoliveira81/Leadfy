import { HTTPClient } from './client';

/**
 * Centraliza o mapeamento de erros da API para um formato padronizado.
 * @param {Error} error - Erro capturado do axios.
 * @returns {Error} Erro mapeado com tipo e mensagem consistentes.
 */
const mapApiError = (error) => {
    if (!error.response) {
        throw new Error('Erro de rede ou servidor indisponível');
    }

    const { status, data } = error.response;

    if (status === 400) {
        return Promise.reject({
            type: 'validation',
            message: data.message || 'Dados inválidos',
            errors: data.errors || null
        });
    }

    if (status === 404) {
        return Promise.reject({
            type: 'not_found',
            message: data.message || 'Recurso não encontrado'
        });
    }

    if (status === 500) {
        return Promise.reject({
            type: 'server',
            message: 'Erro interno. Tente novamente mais tarde.'
        });
    }

    return Promise.reject({
        type: 'unknown',
        message: 'Erro inesperado'
    });
};

const productAPI = {
    /**
     * Cria um novo produto.
     * @param {Object} productData - Dados do produto (name, description, price).
     * @returns {Promise<{id: number}>} ID do produto criado.
     */
    async Create(productData) {
        try {
            const response = await HTTPClient.post(`/products`, productData);
            return response.data;
        } catch (error) {
            return mapApiError(error);
        }
    },

    /**
     * Obtém um produto pelo ID.
     * @param {number} productId - ID do produto.
     * @returns {Promise<ProductResponse>} Dados do produto.
     */
    async GetById(productId) {
        try {
            const response = await HTTPClient.get(`/products/${productId}`);
            return response.data;
        } catch (error) {
            return mapApiError(error);
        }
    },

    /**
     * Lista produtos com filtros opcionais.
     * @param {Object} options - Opções de filtro.
     * @param {boolean} [options.isActive] - Filtrar por status de ativação (opcional).
     * @param {string} [options.name] - Filtrar por nome contendo o valor (opcional).
     * @returns {Promise<ProductResponse[]>} Lista de produtos.
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
            const url = queryString ? `/products?${queryString}` : `/products`;
            
            const response = await HTTPClient.get(url);
            return response.data;
        } catch (error) {
            return mapApiError(error);
        }
    },

    /**
     * Atualiza um produto existente.
     * @param {number} productId - ID do produto a atualizar.
     * @param {Object} productData - Dados atualizados (name, description, price, isActive).
     * @returns {Promise<void>} Sem conteúdo na resposta (204).
     */
    async Update(productId, productData) {
        try {
            await HTTPClient.put(`/products/${productId}`, productData);
            return;
        } catch (error) {
            return mapApiError(error);
        }
    },

    /**
     * Deleta um produto.
     * @param {number} productId - ID do produto a deletar.
     * @returns {Promise<void>} Sem conteúdo na resposta (204).
     */
    async Delete(productId) {
        try {
            await HTTPClient.delete(`/products/${productId}`);
            return;
        } catch (error) {
            return mapApiError(error);
        }
    },

    /**
     * Desativa um produto.
     * @param {number} productId - ID do produto a desativar.
     * @returns {Promise<void>} Sem conteúdo na resposta (204).
     */
    async Deactivate(productId) {
        try {
            await HTTPClient.patch(`/products/${productId}/deactivate`);
            return;
        } catch (error) {
            return mapApiError(error);
        }
    },

    /**
     * Ativa um produto.
     * @param {number} productId - ID do produto a ativar.
     * @returns {Promise<void>} Sem conteúdo na resposta (204).
     */
    async Activate(productId) {
        try {
            await HTTPClient.patch(`/products/${productId}/activate`);
            return;
        } catch (error) {
            return mapApiError(error);
        }
    }
};

export default productAPI;