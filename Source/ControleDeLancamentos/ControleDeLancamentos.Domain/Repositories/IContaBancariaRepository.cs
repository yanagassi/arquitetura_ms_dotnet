using System;
using System.Threading.Tasks;
using ControleDeLancamentos.Domain.Entities;

namespace ControleDeLancamentos.Domain.Repositories
{
    public interface IContaBancariaRepository
    {
        Task<ContaBancaria> ObterContaAsync(Guid contaId);
        Task AdicionarConta(ContaBancaria conta);
        Task<IEnumerable<ContaBancaria>> ObterContasBancariasPorUserId(Guid userId);
        Task AtualizarConta(ContaBancaria conta); 
    }
}
