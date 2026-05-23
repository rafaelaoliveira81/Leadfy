import axios from 'axios';
import { getAuthSession } from './authStorage';

export const HTTPClient = axios.create({
  baseURL: 'https://localhost:7287/api',
  headers: {
    'Content-Type': 'application/json;charset=UTF-8',
  },
});

HTTPClient.interceptors.request.use((config) => {
  const session = getAuthSession();

  if (session?.token) {
    config.headers.Authorization = `Bearer ${session.token}`;
  }

  return config;
});
