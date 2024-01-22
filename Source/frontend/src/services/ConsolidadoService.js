import api from "./api";

class ConsolidadoService {
  /**
   * Obtém os detalhes consolidados para um ID específico.
   * @param {string} id - O ID para o qual se deseja obter os detalhes consolidados.
   * @returns {Promise} Uma Promise que resolve com os detalhes consolidados.
   */
  async obterDetalhesConsolidados(id) {
    try {
      const response = await api.get(`/api/consolidado/Consolidado/${id}`);
      return response.data;
    } catch (error) {
      console.error("Erro ao obter detalhes consolidados:", error);
      throw error;
    }
  }
}

export default new ConsolidadoService();
