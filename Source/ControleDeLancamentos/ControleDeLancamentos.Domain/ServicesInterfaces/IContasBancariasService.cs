using ControleDeLancamentos.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ControleDeLancamentos.Domain.Services
{
    
    public interface IContasBancariasService
    {
        /// <summary>
        /// Retona todas as contas bancarias por ID da conta
        /// </summary>
        /// <param name="contaId">The ID of the bank account to retrieve.</param>
        /// <returns>The bank account with the specified ID.</returns>
        Task<ContaBancaria> ObterContaBancariaAsync(Guid contaId);

        // Add other methods related to account management as needed...

        /// <summary>
        /// Retona todas as contas bancarias por UserId.
        /// </summary>
        /// <returns>A collection of all bank accounts.</returns>
        Task<IEnumerable<ContaBancaria>> ObterContasPorUserId(Guid userId);

    }
}
