﻿using ControleDeLancamentos.Domain.Entities;
using ControleDeLancamentos.Domain.Repositories;

namespace ControleDeLancamentos.Domain.Services
{
    public class ServicoControleLancamentos : IServicoControleLancamentos
    {
        private readonly ILancamentoRepository _lancamentoRepository;
        private readonly IContaBancariaRepository _contaBancariaRepository;
        private readonly IRabbitMqService _rabbitMqService;

        public ServicoControleLancamentos(
            ILancamentoRepository lancamentoRepository,
            IContaBancariaRepository contaBancariaRepository,
            IRabbitMqService rabbitMqService
            )
        {
            _lancamentoRepository = lancamentoRepository;
            _contaBancariaRepository = contaBancariaRepository;
            _rabbitMqService = rabbitMqService;
        }

        public async Task AdicionarLancamentoAsync(Lancamento lancamento)
        {
            var conta = await _contaBancariaRepository.ObterContaAsync(lancamento.ContaId);
            if (conta == null)
            {
                throw new Exception("Conta bancária não encontrada.");
            }

            if (lancamento.Tipo == TipoLancamento.Credito)
            {
                conta.Saldo += lancamento.Valor;
            }
            else if (lancamento.Tipo == TipoLancamento.Debito)
            {
                if (conta.Saldo < lancamento.Valor)
                {
                    throw new Exception("Saldo insuficiente para realizar o débito.");
                }

                conta.Saldo -= lancamento.Valor;
            }

            await _contaBancariaRepository.AtualizarConta(conta);
            await _lancamentoRepository.AdicionarLancamentoAsync(lancamento);


            lancamento.ContaBancaria = null;
            await _rabbitMqService.EnviarMensagemAsync(lancamento);
        }


        public async Task<IEnumerable<Lancamento>> ObterLancamentosDaContaAsync(Guid contaId)
        {
            return await _lancamentoRepository.ObterLancamentosDaContaAsync(contaId);
        }

        public async Task<decimal> CalcularSaldoAsync(Guid contaId)
        {
            return await _lancamentoRepository.CalcularSaldoAsync(contaId);
        }
    }
}
