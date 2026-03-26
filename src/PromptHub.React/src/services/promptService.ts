import api from './api';
import { PromptTemplateDto, ExpandPromptResponse } from '../types';

export interface ExpandPromptRequest {
  originalInput: string;
  templateId?: string | null;
  provider: string;
  role?: number | null;
}

export const promptService = {
  getPublicTemplates: async (): Promise<PromptTemplateDto[]> => {
    const response = await api.get<PromptTemplateDto[]>('/Template/public');
    return response.data;
  },

  getTemplateById: async (id: string): Promise<PromptTemplateDto> => {
    const response = await api.get<PromptTemplateDto>(`/Template/${id}`);
    return response.data;
  },

  expandPrompt: async (request: ExpandPromptRequest): Promise<ExpandPromptResponse> => {
    const response = await api.post<ExpandPromptResponse>('/Prompt/expand', request);
    return response.data;
  },
};
