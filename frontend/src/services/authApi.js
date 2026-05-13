import { HTTPClient } from './client';

function createApiError(message, type) {
  return Object.assign(new Error(message), { type });
}

const authApi = {
  async login(credentials) {
    try {
      const response = await HTTPClient.post('/auth/login', credentials);
      return response.data;
    } catch (error) {
      if (!error.response) {
        throw createApiError('Erro de rede ou servidor indisponível.', 'network');
      }

      const { status, data } = error.response;

      if (status === 400) {
        throw createApiError(data?.message || 'Dados inválidos.', 'validation');
      }

      if (status === 401) {
        throw createApiError(data?.message || 'E-mail ou senha inválidos.', 'unauthorized');
      }

      throw createApiError(
        data?.message || 'Não foi possível realizar o login agora.',
        'server'
      );
    }
  },
};

export default authApi;