import api from "./api";
import { jwtDecode } from "jwt-decode";

const TOKEN_PREFIX = "AUTH_TOKEN_PLATFORM";

class AuthService {
  constructor() {
    this.token = localStorage.getItem(TOKEN_PREFIX);
  }

  /**
   * Autentica o usuário usando email e senha.
   * @param {string} email - O email do usuário.
   * @param {string} senha - A senha do usuário.
   * @returns {boolean} Retorna true se a autenticação for bem-sucedida; caso contrário, retorna false.
   */
  async login(email, senha) {
    try {
      const response = await api.post("/api/auth/autenticacao", {
        email,
        senha,
      });

      this.token = response.data.token;

      localStorage.setItem(TOKEN_PREFIX, this.token);

      return true;
    } catch (error) {
      console.error("Erro de autenticação:", error);
      return false;
    }
  }

  /**
   * Verifica se o usuário está autenticado com um token válido.
   * @returns {boolean} Retorna true se o usuário estiver autenticado; caso contrário, retorna false.
   */
  isAuthenticated() {
    return this.token !== null && this.token !== undefined;
  }

  /**
   * Desconecta o usuário, removendo o token de autenticação.
   */
  logout() {
    localStorage.removeItem(TOKEN_PREFIX);
    this.token = null;
  }

  removeClaimsKey(obj) {
    const pattern = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/";
    if (typeof obj !== "object" || obj === null) {
      return obj;
    }

    for (let key in obj) {
      if (obj.hasOwnProperty(key)) {
        if (key.includes(pattern)) {
          const newKey = key.replace(pattern, "");
          obj[newKey] = obj[key];
          delete obj[key];
        }
      }
    }

    return obj;
  }

  /**
   * Obtém o ID do usuário presente no token JWT.
   * @returns {string|null} Retorna o ID do usuário se presente; caso contrário, retorna null.
   */
  getUser() {
    try {
      const decodedToken = jwtDecode(this.token);

      return decodedToken ? this.removeClaimsKey(decodedToken) : null;
    } catch (error) {
      console.error("Erro ao obter ID do usuário:", error);
      return null;
    }
  }
}

export default AuthService;
