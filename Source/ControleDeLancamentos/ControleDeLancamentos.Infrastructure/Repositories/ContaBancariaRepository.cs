using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using ControleDeLancamentos.Domain.Entities;
using ControleDeLancamentos.Domain.Repositories;
using ControleDeLancamentos.Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace ControleDeLancamentos.Infrastructure.Repositories
{
    public class ContaBancariaRepository : IContaBancariaRepository
    {
        private readonly ControleLancamentosDbContext _dbContext;

        public ContaBancariaRepository(ControleLancamentosDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<ContaBancaria> ObterContaAsync(Guid contaId)
        {
            return await _dbContext.ContasBancarias
                .Include(c => c.Lancamentos)
                .FirstOrDefaultAsync(c => c.Id == contaId);
        }

        public async Task<IQueryable<ContaBancaria>> ObterContasPorUserId(Guid userId)
        {
            return _dbContext.ContasBancarias
                .Where(c => c.UserId == userId);
        }

        public async Task AdicionarConta(ContaBancaria conta)
        {
            if (conta == null)
                throw new ArgumentNullException(nameof(conta));

            _dbContext.ContasBancarias.Add(conta);
            await _dbContext.SaveChangesAsync();
        }

        public async Task AtualizarConta(ContaBancaria conta)
        {
            if (conta == null)
                throw new ArgumentNullException(nameof(conta));

            var contaExistente = await _dbContext.ContasBancarias.FirstOrDefaultAsync(c => c.Id == conta.Id);

            if (contaExistente == null)
            {
                throw new Exception("Conta bancária não encontrada.");
            }


            contaExistente.Nome = conta.Nome;
            contaExistente.Saldo = conta.Saldo;

            await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<ContaBancaria>> ObterContasBancariasPorUserId(Guid userId)
        {
            return await _dbContext.ContasBancarias .Where(c => c.UserId == userId).ToArrayAsync();
        }
    }
}
