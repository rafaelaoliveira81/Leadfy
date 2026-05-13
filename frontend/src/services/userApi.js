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

    if (status === 401) {
        throw {
            type: 'unauthorized',
            message: data?.message || 'Não autorizado'
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

const userAPI = {
    /**
     * Cria um novo usuário.
    * @param {Object} userData - Dados do usuário (name, email, password).
     * @returns {Promise<{id: number}>} ID do usuário criado.
     */
    async Create(userData) {
        try {
            const response = await HTTPClient.post(`/users`, userData);
            return response.data;
        } catch (error) {
            return mapApiError(error);
        }
    },

    /**
     * Obtém um usuário pelo ID.
     * @param {number} userId - ID do usuário.
     * @returns {Promise<UserResponse>} Dados do usuário.
     */
    async GetById(userId) {
        try {
            const response = await HTTPClient.get(`/users/${userId}`);
            return response.data;
        } catch (error) {
            return mapApiError(error);
        }
    },

    /**
     * Obtém um usuário pelo e-mail.
     * @param {string} email - E-mail do usuário.
     * @returns {Promise<UserResponse>} Dados do usuário.
     */
    async GetByEmail(email) {
        try {
            const response = await HTTPClient.get(`/users/email/${email}`);
            return response.data;
        } catch (error) {
            return mapApiError(error);
        }
    },

    /**
     * Lista usuários com filtros opcionais.
     * @param {Object} options - Opções de filtro.
     * @param {boolean} [options.isActive] - Filtrar por status de ativação (opcional).
     * @param {string} [options.name] - Filtrar por nome contendo o valor (opcional).
     * @returns {Promise<UserResponse[]>} Lista de usuários.
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
            const url = queryString ? `/users?${queryString}` : `/users`;
            
            const response = await HTTPClient.get(url);
            return response.data;
        } catch (error) {
            return mapApiError(error);
        }
    },

    /**
     * Atualiza um usuário existente.
     * @param {number} userId - ID do usuário a atualizar.
        * @param {Object} userData - Dados atualizados (name, email).
     * @returns {Promise<void>} Sem conteúdo na resposta (204).
     */
    async Update(userId, userData) {
        try {
            await HTTPClient.put(`/users/${userId}`, userData);
            return;
        } catch (error) {
            return mapApiError(error);
        }
    },

    /**
     * Atualiza a senha de um usuário.
     * @param {number} userId - ID do usuário.
     * @param {Object} passwordData - Dados de senha (currentPassword, newPassword).
     * @returns {Promise<void>} Sem conteúdo na resposta (204).
     */
    async UpdatePassword(userId, passwordData) {
        try {
            await HTTPClient.patch(`/users/${userId}/password`, passwordData);
            return;
        } catch (error) {
            return mapApiError(error);
        }
    },

    /**
     * Deleta um usuário.
     * @param {number} userId - ID do usuário a deletar.
     * @returns {Promise<void>} Sem conteúdo na resposta (204).
     */
    async Delete(userId) {
        try {
            await HTTPClient.delete(`/users/${userId}`);
            return;
        } catch (error) {
            return mapApiError(error);
        }
    },

    /**
     * Desativa um usuário.
     * @param {number} userId - ID do usuário a desativar.
     * @returns {Promise<void>} Sem conteúdo na resposta (204).
     */
    async Deactivate(userId) {
        try {
            await HTTPClient.patch(`/users/${userId}/deactivate`);
            return;
        } catch (error) {
            return mapApiError(error);
        }
    },

    /**
     * Ativa um usuário.
     * @param {number} userId - ID do usuário a ativar.
     * @returns {Promise<void>} Sem conteúdo na resposta (204).
     */
    async Activate(userId) {
        try {
            await HTTPClient.patch(`/users/${userId}/activate`);
            return;
        } catch (error) {
            return mapApiError(error);
        }
    }
};

export default userAPI;
