export interface PromptTemplateDto {
  id: string;
  title: string;
  content: string;
  category: string;
  isPublic: boolean;
}

export interface ExpandPromptRequest {
  prompt: string;
  variables: Record<string, string>;
  provider: string;
}

export interface ExpandPromptResponse {
  expandedPrompt: string;
  provider: string;
}

export interface LoginRequest {
  email: string;
  password: string;
}

export interface RegisterRequest {
  email: string;
  password: string;
  confirmPassword: string;
}

export interface AuthResponse {
  token: string;
  email: string;
  roles: string[];
}

export enum RoleType {
  GeneralAssistant = 0,
  SeniorDeveloper = 1,
  CreativeWriter = 2,
  LegalExpert = 3,
  AcademicResearcher = 4,
  MarketingSpecialist = 5,
  DataAnalyst = 6
}

