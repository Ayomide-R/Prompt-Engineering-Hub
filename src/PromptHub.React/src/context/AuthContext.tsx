import React, { createContext, useContext, useState, useEffect } from 'react';
import { authService } from '../services/authService';
import { AuthResponse } from '../types';

interface AuthContextType {
  isAuthenticated: boolean;
  userEmail: string | null;
  login: (data: AuthResponse) => void;
  logout: () => void;
}

const AuthContext = createContext<AuthContextType | undefined>(undefined);

export const AuthProvider: React.FC<{ children: React.ReactNode }> = ({ children }) => {
  const [isAuthenticated, setIsAuthenticated] = useState(authService.isAuthenticated());
  const [userEmail, setUserEmail] = useState<string | null>(localStorage.getItem('userEmail'));

  const login = (data: AuthResponse) => {
    localStorage.setItem('authToken', data.token);
    localStorage.setItem('userEmail', data.email);
    setIsAuthenticated(true);
    setUserEmail(data.email);
  };

  const logout = () => {
    authService.logout();
    setIsAuthenticated(false);
    setUserEmail(null);
  };

  return (
    <AuthContext.Provider value={{ isAuthenticated, userEmail, login, logout }}>
      {children}
    </AuthContext.Provider>
  );
};

export const useAuth = () => {
  const context = useContext(AuthContext);
  if (context === undefined) {
    throw new Error('useAuth must be used within an AuthProvider');
  }
  return context;
};
