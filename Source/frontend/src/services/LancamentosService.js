import api from "./api";

class LancamentoService {
  /**
   * Adiciona um novo lançamento.
   * @param {Object} lancamento - Objeto contendo os detalhes do lançamento.
   * @returns {Promise} Uma Promise que resolve com os detalhes do lançamento adicionado.
   */
  async adicionarLancamento(lancamento) {
    try {
      const response = await api.post(
        "/api/lancamentos/ControleLancamentos/lancamentos",
        lancamento
      );
      return response.data;
    } catch (error) {
      if (error.response && error.response.data) {
        throw error.response.data;
      }
      throw error.message;
    }
  }
}

export default new LancamentoService();
