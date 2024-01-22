import api from "./api";

class ConsolidadoService {
  /**
   * Obtém detalhes consolidados para um ID específico.
   * @param {string} id - O ID para o qual se deseja obter os detalhes consolidados.
   * @returns {Promise} Uma Promise que resolve com os detalhes consolidados.
   */
  async obterDetalhesConsolidados(id) {
    try {
      const response = await api.get(`/api/consolidado/Consolidado/${id}`);
      return response.data;
    } catch (error) {
      console.error("Erro ao obter detalhes consolidados:", error);
      return [];
    }
  }

  /**
   * Obtém detalhes consolidados para um ID específico dentro de um intervalo de datas.
   * @param {string} id - O ID para o qual se deseja obter os detalhes consolidados.
   * @param {string} init - A data de início do intervalo de datas.
   * @param {string} end - A data de término do intervalo de datas.
   * @returns {Promise} Uma Promise que resolve com os detalhes consolidados.
   */
  async obterDetalhesConsolidadosPorData(id, init, end) {
    try {
      const response = await api.get(
        `/api/consolidado/Consolidado/${id}/${init}/${end}`
      );
      return response.data;
    } catch (error) {
      return [];
    }
  }

  /**
   * Obtém detalhes do saldo consolidado para um ID específico dentro de um intervalo de datas.
   * @param {string} id - O ID para o qual se deseja obter os detalhes do saldo consolidado.
   * @param {string} init - A data de início do intervalo de datas.
   * @param {string} end - A data de término do intervalo de datas.
   * @returns {Promise} Uma Promise que resolve com os detalhes do saldo consolidado.
   */
  async obterSaldoConsolidadoPorData(id, init, end) {
    try {
      const response = await api.get(
        `/api/consolidado/Consolidado/saldo/${id}/${init}/${end}`
      );
      return response.data;
    } catch (error) {
      return [];
    }
  }

  /**
   * Obtém contas bancárias para um usuário específico.
   * @param {string} userId - O ID do usuário para o qual as contas bancárias devem ser obtidas.
   * @returns {Promise} Uma Promise que resolve com as contas bancárias.
   */
  async ObterContasBancarias(userId) {
    try {
      const response = await api.get(
        `/api/lancamentos/ContaBancaria/usuario/${userId}`
      );
      return response.data;
    } catch (error) {
      return [];
    }
  }
}

export default new ConsolidadoService();
