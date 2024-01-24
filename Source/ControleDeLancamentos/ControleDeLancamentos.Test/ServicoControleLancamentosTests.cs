using System;
using ControleDeLancamentos.Domain.Entities;
using ControleDeLancamentos.Domain.Repositories;
using ControleDeLancamentos.Domain.Services;
using Moq;

namespace ControleDeLancamentos.Test
{
    [TestClass]
    public class ServicoControleLancamentosTests
    {
        private readonly Mock<ILancamentoRepository> _lancamentoRepositoryMock = new Mock<ILancamentoRepository>();
        private readonly Mock<IContaBancariaRepository> _contaBancariaRepositoryMock = new Mock<IContaBancariaRepository>();
        private readonly Mock<IRabbitMqService> _rabbitMqServiceMock = new Mock<IRabbitMqService>();

        [TestMethod]
        /// <summary>
        /// Deve adicionar um lançamento e atualizar o saldo de crédito na conta bancária.
        /// </summary>
        public async Task AdicionarLancamentoAsync_DeveAdicionarLancamentoEAtualizarSaldoCredito()
        { 
            var lancamento = new Lancamento { ContaId = Guid.NewGuid(), Tipo = TipoLancamento.Credito, Valor = 100 };

            var conta = new ContaBancaria { Id = lancamento.ContaId, Saldo = 500 };
            _contaBancariaRepositoryMock.Setup(repo => repo.ObterContaAsync(It.IsAny<Guid>())).ReturnsAsync(conta);

            var servicoControleLancamentos = new ServicoControleLancamentos(
                _lancamentoRepositoryMock.Object,
                _contaBancariaRepositoryMock.Object,
                _rabbitMqServiceMock.Object
            );
             
            await servicoControleLancamentos.AdicionarLancamentoAsync(lancamento);
             
            Assert.AreEqual(600, conta.Saldo);
            _contaBancariaRepositoryMock.Verify(repo => repo.AtualizarConta(It.IsAny<ContaBancaria>()), Times.Once);
            _lancamentoRepositoryMock.Verify(repo => repo.AdicionarLancamentoAsync(It.IsAny<Lancamento>()), Times.Once);
            _rabbitMqServiceMock.Verify(service => service.EnviarMensagemAsync(It.IsAny<Lancamento>()), Times.Once);
        }

        [TestMethod]
        /// <summary>
        /// Deve adicionar um lançamento e atualizar o saldo de débito na conta bancária.
        /// </summary>
        public async Task AdicionarLancamentoAsync_DeveAdicionarLancamentoEAtualizarSaldoDebito()
        { 
            var lancamento = new Lancamento { ContaId = Guid.NewGuid(), Tipo = TipoLancamento.Debito, Valor = 50 };

            var conta = new ContaBancaria { Id = lancamento.ContaId, Saldo = 100 };
            _contaBancariaRepositoryMock.Setup(repo => repo.ObterContaAsync(It.IsAny<Guid>())).ReturnsAsync(conta);

            var servicoControleLancamentos = new ServicoControleLancamentos(
                _lancamentoRepositoryMock.Object,
                _contaBancariaRepositoryMock.Object,
                _rabbitMqServiceMock.Object
            );
             
            await servicoControleLancamentos.AdicionarLancamentoAsync(lancamento);
             
            Assert.AreEqual(50, conta.Saldo);
            _contaBancariaRepositoryMock.Verify(repo => repo.AtualizarConta(It.IsAny<ContaBancaria>()), Times.Once);
            _lancamentoRepositoryMock.Verify(repo => repo.AdicionarLancamentoAsync(It.IsAny<Lancamento>()), Times.Once);
            _rabbitMqServiceMock.Verify(service => service.EnviarMensagemAsync(It.IsAny<Lancamento>()), Times.Once);
        }

        [TestMethod]
        /// <summary>
        /// Deve lançar uma exceção quando a conta bancária não é encontrada.
        /// </summary>
        public async Task AdicionarLancamentoAsync_DeveLancarExcecaoQuandoContaBancariaNaoEncontrada()
        { 
            var lancamento = new Lancamento { ContaId = Guid.NewGuid(), Tipo = TipoLancamento.Credito, Valor = 100 };
            _contaBancariaRepositoryMock.Setup(repo => repo.ObterContaAsync(It.IsAny<Guid>())).ReturnsAsync((ContaBancaria)null);

            var servicoControleLancamentos = new ServicoControleLancamentos(
                _lancamentoRepositoryMock.Object,
                _contaBancariaRepositoryMock.Object,
                _rabbitMqServiceMock.Object
            );
             
            await Assert.ThrowsExceptionAsync<Exception>(async () => await servicoControleLancamentos.AdicionarLancamentoAsync(lancamento));
            _contaBancariaRepositoryMock.Verify(repo => repo.AtualizarConta(It.IsAny<ContaBancaria>()), Times.Never);
            _lancamentoRepositoryMock.Verify(repo => repo.AdicionarLancamentoAsync(It.IsAny<Lancamento>()), Times.Never);
            _rabbitMqServiceMock.Verify(service => service.EnviarMensagemAsync(It.IsAny<Lancamento>()), Times.Never);
        }

        [TestMethod]
        /// <summary>
        /// Deve lançar uma exceção quando o saldo é insuficiente para um débito.
        /// </summary>
        public async Task AdicionarLancamentoAsync_DeveLancarExcecaoQuandoSaldoInsuficienteParaDebito()
        { 
            var lancamento = new Lancamento { ContaId = Guid.NewGuid(), Tipo = TipoLancamento.Debito, Valor = 150 };
            var conta = new ContaBancaria { Id = lancamento.ContaId, Saldo = 100 };
            _contaBancariaRepositoryMock.Setup(repo => repo.ObterContaAsync(It.IsAny<Guid>())).ReturnsAsync(conta);

            var servicoControleLancamentos = new ServicoControleLancamentos(
                _lancamentoRepositoryMock.Object,
                _contaBancariaRepositoryMock.Object,
                _rabbitMqServiceMock.Object
            );
             
            await Assert.ThrowsExceptionAsync<Exception>(async () => await servicoControleLancamentos.AdicionarLancamentoAsync(lancamento));
            _contaBancariaRepositoryMock.Verify(repo => repo.AtualizarConta(It.IsAny<ContaBancaria>()), Times.Never);
            _lancamentoRepositoryMock.Verify(repo => repo.AdicionarLancamentoAsync(It.IsAny<Lancamento>()), Times.Never);
            _rabbitMqServiceMock.Verify(service => service.EnviarMensagemAsync(It.IsAny<Lancamento>()), Times.Never);
        }

        [TestMethod]
        /// <summary>
        /// Deve retornar uma lista de lançamentos da conta.
        /// </summary>
        public async Task ObterLancamentosDaContaAsync_DeveRetornarListaDeLancamentos()
        { 
            var contaId = Guid.NewGuid();
            var lancamentos = new List<Lancamento>
                    {
                        new Lancamento { Id = Guid.NewGuid(), ContaId = contaId, Tipo = TipoLancamento.Credito, Valor = 100 },
                        new Lancamento { Id = Guid.NewGuid(), ContaId = contaId, Tipo = TipoLancamento.Debito, Valor = 50 },
                        new Lancamento { Id = Guid.NewGuid(), ContaId = contaId, Tipo = TipoLancamento.Credito, Valor = 75 },
                    };
            _lancamentoRepositoryMock.Setup(repo => repo.ObterLancamentosDaContaAsync(It.IsAny<Guid>())).ReturnsAsync(lancamentos);

            var servicoControleLancamentos = new ServicoControleLancamentos(
                _lancamentoRepositoryMock.Object,
                _contaBancariaRepositoryMock.Object,
                _rabbitMqServiceMock.Object
            );
             
            var resultado = await servicoControleLancamentos.ObterLancamentosDaContaAsync(contaId);
             
            Assert.IsNotNull(resultado);
            Assert.AreEqual(lancamentos.Count, resultado.Count());
            _lancamentoRepositoryMock.Verify(repo => repo.ObterLancamentosDaContaAsync(It.IsAny<Guid>()), Times.Once);
        }

        [TestMethod]
        /// <summary>
        /// Deve retornar o saldo correto da conta.
        /// </summary>
        public async Task CalcularSaldoAsync_DeveRetornarSaldoCorreto()
        { 
            var contaId = Guid.NewGuid();
            var saldoEsperado = 150;
            _lancamentoRepositoryMock.Setup(repo => repo.CalcularSaldoAsync(It.IsAny<Guid>())).ReturnsAsync(saldoEsperado);

            var servicoControleLancamentos = new ServicoControleLancamentos(
                _lancamentoRepositoryMock.Object,
                _contaBancariaRepositoryMock.Object,
                _rabbitMqServiceMock.Object
            );
             
            var resultado = await servicoControleLancamentos.CalcularSaldoAsync(contaId);
             
            Assert.AreEqual(saldoEsperado, resultado);
            _lancamentoRepositoryMock.Verify(repo => repo.CalcularSaldoAsync(It.IsAny<Guid>()), Times.Once);
        }

        [TestMethod]
        /// <summary>
        /// Deve lançar uma exceção ao adicionar um lançamento com valor zero.
        /// </summary>
        public async Task AdicionarLancamentoAsync_DeveLancarExcecaoAoAdicionarLancamentoComValorZero()
        {
            var lancamento = new Lancamento { ContaId = Guid.NewGuid(), Tipo = TipoLancamento.Credito, Valor = 0 };

            var servicoControleLancamentos = new ServicoControleLancamentos(
                _lancamentoRepositoryMock.Object,
                _contaBancariaRepositoryMock.Object,
                _rabbitMqServiceMock.Object
            );

            await Assert.ThrowsExceptionAsync<Exception>(async () => await servicoControleLancamentos.AdicionarLancamentoAsync(lancamento));
            _contaBancariaRepositoryMock.Verify(repo => repo.AtualizarConta(It.IsAny<ContaBancaria>()), Times.Never);
            _lancamentoRepositoryMock.Verify(repo => repo.AdicionarLancamentoAsync(It.IsAny<Lancamento>()), Times.Never);
            _rabbitMqServiceMock.Verify(service => service.EnviarMensagemAsync(It.IsAny<Lancamento>()), Times.Never);
        }

        [TestMethod]
        /// <summary>
        /// Deve lançar uma exceção ao adicionar um lançamento com valor negativo.
        /// </summary>
        public async Task AdicionarLancamentoAsync_DeveLancarExcecaoAoAdicionarLancamentoComValorNegativo()
        {
            var lancamento = new Lancamento { ContaId = Guid.NewGuid(), Tipo = TipoLancamento.Credito, Valor = -50 };

            var servicoControleLancamentos = new ServicoControleLancamentos(
                _lancamentoRepositoryMock.Object,
                _contaBancariaRepositoryMock.Object,
                _rabbitMqServiceMock.Object
            );

            await Assert.ThrowsExceptionAsync<Exception>(async () => await servicoControleLancamentos.AdicionarLancamentoAsync(lancamento));
            _contaBancariaRepositoryMock.Verify(repo => repo.AtualizarConta(It.IsAny<ContaBancaria>()), Times.Never);
            _lancamentoRepositoryMock.Verify(repo => repo.AdicionarLancamentoAsync(It.IsAny<Lancamento>()), Times.Never);
            _rabbitMqServiceMock.Verify(service => service.EnviarMensagemAsync(It.IsAny<Lancamento>()), Times.Never);
        }

        [TestMethod]
        /// <summary>
        /// Deve lançar uma exceção ao adicionar um lançamento com tipo inválido.
        /// </summary>
        public async Task AdicionarLancamentoAsync_DeveLancarExcecaoAoAdicionarLancamentoComTipoInvalido()
        {
            var lancamento = new Lancamento { ContaId = Guid.NewGuid(), Tipo = (TipoLancamento)99, Valor = 50 };

            var servicoControleLancamentos = new ServicoControleLancamentos(
                _lancamentoRepositoryMock.Object,
                _contaBancariaRepositoryMock.Object,
                _rabbitMqServiceMock.Object
            );

            await Assert.ThrowsExceptionAsync<Exception>(async () => await servicoControleLancamentos.AdicionarLancamentoAsync(lancamento));
            _contaBancariaRepositoryMock.Verify(repo => repo.AtualizarConta(It.IsAny<ContaBancaria>()), Times.Never);
            _lancamentoRepositoryMock.Verify(repo => repo.AdicionarLancamentoAsync(It.IsAny<Lancamento>()), Times.Never);
            _rabbitMqServiceMock.Verify(service => service.EnviarMensagemAsync(It.IsAny<Lancamento>()), Times.Never);
        }

  
 

    }

}

