import React, { createContext, useContext, useEffect, useState } from "react";
import { authAPI } from "../services/authApi";

const AuthContext = createContext();

const AUTH_STORAGE_KEY = "authToken";

export function AuthProvider({ children }) {
  const [token, setToken] = useState(null);
  const [claims, setClaims] = useState(null);

  useEffect(() => {
    const storedToken = localStorage.getItem(AUTH_STORAGE_KEY);

    if (!storedToken) {
      return;
    }

    try {
      const payload = storedToken.split(".")[1];

      const decodedPayload = atob(payload);

      const parsedClaims = JSON.parse(decodedPayload);

      if (parsedClaims.exp) {
        const expirationDate = parsedClaims.exp * 1000;

        if (expirationDate < Date.now()) {
          localStorage.removeItem(AUTH_STORAGE_KEY);
          return;
        }
      }

      setToken(storedToken);
      setClaims(parsedClaims);
    } catch (error) {
      console.error("Erro ao ler token:", error);

      localStorage.removeItem(AUTH_STORAGE_KEY);
    }
  }, []);

  const login = async (credentials) => {
    const response = await authAPI.Authentication(credentials);

    const newToken = response.token;

    if (!newToken) {
      throw new Error("Token não retornado pela API.");
    }

    try {
      const payload = newToken.split(".")[1];
      const decodedPayload = atob(payload);
      const parsedClaims = JSON.parse(decodedPayload);

      localStorage.setItem(AUTH_STORAGE_KEY, newToken);

      setToken(newToken);
      setClaims(parsedClaims);

      return response;
    } catch (error) {
      throw new Error("Erro ao processar token.");
    }
  };

  const logout = () => {
    localStorage.removeItem(AUTH_STORAGE_KEY);

    setToken(null);
    setClaims(null);
  };

  const isAuthenticated = !!token;

  return (
    <AuthContext.Provider
      value={{
        token,
        claims,
        isAuthenticated,
        login,
        logout,
      }}
    >
      {children}
    </AuthContext.Provider>
  );
}

export function useAuth() {
  return useContext(AuthContext);
}
