import { HTTPClient } from './client';

const mapApiError = (error) => {
    if (!error.response) {
        throw { type: 'network', message: 'Erro de rede ou servidor indisponível' };
    }
    const { status, data } = error.response;
    if (status === 400) throw { type: 'validation', message: data?.message || 'Dados inválidos' };
    if (status === 404) throw { type: 'not_found', message: data?.message || 'Recurso não encontrado' };
    if (status === 500) throw { type: 'server', message: data?.message || 'Erro interno. Tente novamente mais tarde.' };
    throw { type: 'unknown', message: data?.message || 'Erro inesperado' };
};

const interactionApi = {
    async AddToOpportunity(opportunityId, data) {
        try {
            const response = await HTTPClient.post(`/opportunities/${opportunityId}/interactions`, data);
            return response.data;
        } catch (error) {
            return mapApiError(error);
        }
    },

    async GetByOpportunityId(opportunityId) {
        try {
            const response = await HTTPClient.get(`/opportunities/${opportunityId}/interactions`);
            return response.data;
        } catch (error) {
            return mapApiError(error);
        }
    },

    async GetById(id) {
        try {
            const response = await HTTPClient.get(`/interactions/${id}`);
            return response.data;
        } catch (error) {
            return mapApiError(error);
        }
    },

    async Delete(id) {
        try {
            await HTTPClient.delete(`/interactions/${id}`);
        } catch (error) {
            return mapApiError(error);
        }
    },
};

export default interactionApi;
