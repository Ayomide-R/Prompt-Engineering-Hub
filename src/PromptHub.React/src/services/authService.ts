import api from './api';
import { LoginRequest, AuthResponse } from '../types';

export const authService = {
  login: async (request: LoginRequest): Promise<AuthResponse> => {
    const response = await api.post<AuthResponse>('/Auth/login', request);
    return response.data;
  },

  logout: () => {
    localStorage.removeItem('authToken');
    localStorage.removeItem('userEmail');
  },

  isAuthenticated: () => {
    return !!localStorage.getItem('authToken');
  },
};
