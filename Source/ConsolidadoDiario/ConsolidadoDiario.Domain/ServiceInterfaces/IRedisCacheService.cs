using ConsolidadoDiario.Domain.Entities;

namespace ConsolidadoDiario.Domain
{
    public interface IRedisCacheService
    {
        /// <summary>
        /// Obtém um objeto desserializado a partir do Redis com base na chave especificada.
        /// </summary>
        /// <typeparam name="T">Tipo do objeto a ser desserializado.</typeparam>
        /// <param name="key">Chave para recuperar o objeto no Redis.</param>
        /// <returns>Objeto desserializado ou o valor padrão se a chave não existir.</returns>
        Task<T> GetAsync<T>(string key);

        /// <summary>
        /// Armazena um objeto serializado no Redis com a chave especificada.
        /// </summary>
        /// <typeparam name="T">Tipo do objeto a ser armazenado.</typeparam>
        /// <param name="key">Chave para armazenar o objeto no Redis.</param>
        /// <param name="value">Objeto a ser armazenado.</param>
        /// <returns>Task representando a operação assíncrona.</returns>
        Task SetAsync<T>(string key, T value);

        /// <summary>
        /// Remove uma chave específica do Redis.
        /// </summary>
        /// <param name="key">Chave a ser removida.</param>
        /// <returns>True se a chave foi removida com sucesso; caso contrário, false.</returns>
        Task<bool> RemoveAsync(string key);

        /// <summary>
        /// Define o valor de um campo em um hash no Redis.
        /// </summary>
        /// <typeparam name="T">Tipo do objeto a ser armazenado no campo do hash.</typeparam>
        /// <param name="key">Chave do hash.</param>
        /// <param name="field">Campo do hash.</param>
        /// <param name="value">Valor a ser armazenado.</param>
        /// <returns>Task representando a operação assíncrona.</returns>
        Task SetHashFieldAsync<T>(string key, string field, T value);

        /// <summary>
        /// Adiciona um lançamento ao Redis como um campo em um hash específico.
        /// </summary>
        /// <param name="lancamento">Objeto Lancamento a ser adicionado.</param>
        /// <returns>Task representando a operação assíncrona.</returns>
        Task AdicionarLancamentoAoRedisAsync(Lancamento lancamento);

        /// <summary>
        /// Obtém uma coleção de lançamentos do Redis com base no ID da conta e no intervalo de datas.
        /// </summary>
        /// <param name="contaId">ID da conta.</param>
        /// <param name="dataInicio">Data de início do intervalo.</param>
        /// <param name="dataFim">Data de fim do intervalo.</param>
        /// <returns>Coleção de lançamentos filtrados pelo intervalo de datas.</returns>
        Task<IEnumerable<Lancamento>> ObterLancamentosPorContaIdEDataAsync(Guid contaId, DateTime dataInicio, DateTime dataFim);

        /// <summary>
        /// Obtém uma coleção de lançamentos do Redis com base no ID da conta.
        /// </summary>
        /// <param name="contaId">ID da conta.</param>
        /// <returns>Coleção de lançamentos associados ao ID da conta.</returns>
        Task<IEnumerable<Lancamento>> ObterLancamentosPorContaIdAsync(Guid contaId);

        /// <summary>
        /// Calcula o valor consolidado por dia para um determinado intervalo de datas.
        /// </summary>
        /// <param name="contaId">ID da conta.</param>
        /// <param name="dataInicio">Data de início do intervalo.</param>
        /// <param name="dataFim">Data de fim do intervalo.</param>
        /// <returns>Coleção de dados consolidados por dia.</returns>
        Task<IEnumerable<ConsolidatedData>> CalcularValorConsolidadoPorDiaAsync(Guid contaId, DateTime dataInicio, DateTime dataFim);
    }

}

