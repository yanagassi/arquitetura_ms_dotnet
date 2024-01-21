using ControleDeLancamentos.Domain.Entities;
using ControleDeLancamentos.Domain.Repositories;
using ControleDeLancamentos.Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace ControleDeLancamentos.Infrastructure.Repositories
{
    public class LancamentoRepository : ILancamentoRepository
    {
        private readonly ControleLancamentosDbContext _dbContext;

        public LancamentoRepository(ControleLancamentosDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<IEnumerable<Lancamento>> ObterLancamentosDaContaAsync(Guid contaId)
        {
            return await _dbContext.Lancamentos
                .Where(l => l.ContaId == contaId)
                .OrderBy(l => l.Data)
                .ToListAsync();
        }

        public async Task AdicionarLancamentoAsync(Lancamento lancamento)
        {
            if (lancamento == null)
                throw new ArgumentNullException(nameof(lancamento));

            _dbContext.Lancamentos.Add(lancamento);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<decimal> CalcularSaldoAsync(Guid contaId)
        {
            var debitos = await _dbContext.Lancamentos
                .Where(l => l.ContaId == contaId && l.Tipo == TipoLancamento.Debito)
                .SumAsync(l => l.Valor);

            var creditos = await _dbContext.Lancamentos
                .Where(l => l.ContaId == contaId && l.Tipo == TipoLancamento.Credito)
                .SumAsync(l => l.Valor);

            return creditos - debitos;
        }
    }
}
