import React, { createContext, useContext, useState, useEffect } from "react";
import AuthService from "../services/AuthService";

const ApiContext = createContext();

export const ApiProvider = ({ children }) => {
  const [authService] = useState(() => new AuthService());
  const [user, setUser] = useState(null);

  const [isAuthenticated, setIsAuthenticated] = useState(
    authService.isAuthenticated()
  );

  useEffect(() => {
    setIsAuthenticated(authService.isAuthenticated());
    setUser(authService.getUser());
  }, [authService]);

  const login = async (email, senha) => {
    const success = await authService.login(email, senha);
    if (success) {
      setIsAuthenticated(true);
    }
    return success;
  };

  const logout = () => {
    authService.logout();
    setIsAuthenticated(false);
  };

  return (
    <ApiContext.Provider value={{ isAuthenticated, login, logout, user }}>
      {children}
    </ApiContext.Provider>
  );
};

export const useApi = () => {
  const context = useContext(ApiContext);
  if (!context) {
    throw new Error("useApi deve ser usado dentro de ApiProvider");
  }
  return context;
};
