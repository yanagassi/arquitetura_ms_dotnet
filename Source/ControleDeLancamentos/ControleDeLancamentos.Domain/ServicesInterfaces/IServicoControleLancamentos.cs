
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ControleDeLancamentos.Domain.Entities;

namespace ControleDeLancamentos.Domain.Services
{
    public interface IServicoControleLancamentos
    {
        /// <summary>
        /// Adiciona um novo lançamento, atualizando o saldo da conta bancária associada.
        /// </summary>
        /// <param name="lancamento">O lançamento a ser adicionado.</param>
        /// <returns>Uma tarefa que representa a operação assíncrona.</returns>
        /// <exception cref="Exception">Lançada quando a conta bancária associada não é encontrada ou não há saldo suficiente para débito.</exception>
        Task AdicionarLancamentoAsync(Lancamento lancamento);

        /// <summary>
        /// Obtém os lançamentos associados a uma conta bancária.
        /// </summary>
        /// <param name="contaId">O identificador da conta bancária.</param>
        /// <returns>Uma coleção de lançamentos associados à conta bancária.</returns>
        Task<IEnumerable<Lancamento>> ObterLancamentosDaContaAsync(Guid contaId);

        /// <summary>
        /// Calcula o saldo atual de uma conta bancária com base nos lançamentos associados.
        /// </summary>
        /// <param name="contaId">O identificador da conta bancária.</param>
        /// <returns>O saldo atual da conta bancária.</returns>
        Task<decimal> CalcularSaldoAsync(Guid contaId);
    }
}
