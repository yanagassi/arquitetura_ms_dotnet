using System;
using System.Threading.Tasks;
using ControleDeLancamentos.Domain.Entities;

namespace ControleDeLancamentos.Domain.Repositories
{
    public interface IContaBancariaRepository
    {
        Task<ContaBancaria> ObterContaAsync(Guid contaId);
        Task AdicionarConta(ContaBancaria conta);
        IQueryable<ContaBancaria> ObterContasPorUserId(Guid userId);
        Task AtualizarConta(ContaBancaria conta);
    }
}
