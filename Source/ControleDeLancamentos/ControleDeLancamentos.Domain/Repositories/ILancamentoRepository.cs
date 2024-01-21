using ControleDeLancamentos.Domain.Entities;

namespace ControleDeLancamentos.Domain.Repositories
{
    public interface ILancamentoRepository
    {
        Task<IEnumerable<Lancamento>> ObterLancamentosDaContaAsync(Guid contaId);
        Task AdicionarLancamentoAsync(Lancamento lancamento);
        Task<decimal> CalcularSaldoAsync(Guid contaId);
    }
}
