using System;
using System.Linq;
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

        public IQueryable<ContaBancaria> ObterContasPorUserId(Guid userId)
        {
            return _dbContext.ContasBancarias
                .Where(c => c.UserId == userId);
        }

        public async Task AdicionarConta(ContaBancaria conta)
        {
            if (conta == null)
                throw new ArgumentNullException(nameof(conta));

            _dbContext.ContasBancarias.Add(conta);
            _dbContext.SaveChanges();
        }


        public async Task AtualizarConta(ContaBancaria conta)
        {
            if (conta == null)
                throw new ArgumentNullException(nameof(conta));

            var contaExistente = _dbContext.ContasBancarias.FirstOrDefault(c => c.Id == conta.Id);

            if (contaExistente == null)
            {
                throw new Exception("Conta bancária não encontrada.");
            }

            // Atualiza os dados da conta existente
            contaExistente.Nome = conta.Nome;
            contaExistente.Saldo = conta.Saldo;

            _dbContext.SaveChanges();
        }

    }
}
