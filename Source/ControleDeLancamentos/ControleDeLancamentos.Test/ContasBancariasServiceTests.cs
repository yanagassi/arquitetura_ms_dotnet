using System;
using ControleDeLancamentos.Domain.Entities;
using ControleDeLancamentos.Domain.Repositories;
using ControleDeLancamentos.Domain.Services;
using Moq;

namespace ControleDeLancamentos.Test
{
    [TestClass]
    public class ContasBancariasServiceTests
    {
        private readonly Mock<IContaBancariaRepository> _contaBancariaRepositoryMock = new Mock<IContaBancariaRepository>();


        [TestMethod]
        /// <summary>
        /// Testa o método <see cref="ContasBancariasService.ObterContaBancariaAsync(Guid)"/>.
        /// Deve retornar a conta bancária quando encontrada.
        /// </summary>
        public async Task ObterContaBancariaAsync_DeveRetornarContaBancariaQuandoEncontrada()
        { 
            var contaId = Guid.NewGuid();
            var contaBancaria = new ContaBancaria { Id = contaId, Nome = "Conta Teste", Saldo = 1000 };
            _contaBancariaRepositoryMock.Setup(repo => repo.ObterContaAsync(It.IsAny<Guid>())).ReturnsAsync(contaBancaria);

            var contasBancariasService = new ContasBancariasService(_contaBancariaRepositoryMock.Object);
             
            var resultado = await contasBancariasService.ObterContaBancariaAsync(contaId);
             
            Assert.IsNotNull(resultado);
            Assert.AreEqual(contaId, resultado.Id);
            Assert.AreEqual("Conta Teste", resultado.Nome);
            Assert.AreEqual(1000, resultado.Saldo);
            _contaBancariaRepositoryMock.Verify(repo => repo.ObterContaAsync(It.IsAny<Guid>()), Times.Once);
        }

        [TestMethod]
        /// <summary>
        /// Testa o método <see cref="ContasBancariasService.ObterContaBancariaAsync(Guid)"/>.
        /// Deve lançar exceção quando a conta bancária não é encontrada.
        /// </summary>
        public async Task ObterContaBancariaAsync_DeveLancarExcecaoQuandoContaNaoEncontrada()
        { 
            var contaId = Guid.NewGuid();
            _contaBancariaRepositoryMock.Setup(repo => repo.ObterContaAsync(It.IsAny<Guid>())).ReturnsAsync((ContaBancaria)null);

            var contasBancariasService = new ContasBancariasService(_contaBancariaRepositoryMock.Object);
             
            await Assert.ThrowsExceptionAsync<Exception>(() => contasBancariasService.ObterContaBancariaAsync(contaId));
            _contaBancariaRepositoryMock.Verify(repo => repo.ObterContaAsync(It.IsAny<Guid>()), Times.Once);
        }

        [TestMethod]
        /// <summary>
        /// Testa o método <see cref="ContasBancariasService.ObterContasPorUserId(Guid)"/>.
        /// Deve retornar uma lista de contas bancárias quando encontradas.
        /// </summary>
        public async Task ObterContasPorUserId_DeveRetornarListaDeContas()
        { 
            var userId = Guid.NewGuid();
            var contasBancarias = new List<ContaBancaria>
        {
            new ContaBancaria { Id = Guid.NewGuid(), Nome = "Conta1", Saldo = 500 },
            new ContaBancaria { Id = Guid.NewGuid(), Nome = "Conta2", Saldo = 1000 },
            new ContaBancaria { Id = Guid.NewGuid(), Nome = "Conta3", Saldo = 1500 },
        };
            _contaBancariaRepositoryMock.Setup(repo => repo.ObterContasBancariasPorUserId(It.IsAny<Guid>())).ReturnsAsync(contasBancarias);

            var contasBancariasService = new ContasBancariasService(_contaBancariaRepositoryMock.Object);
             
            var resultado = await contasBancariasService.ObterContasPorUserId(userId);
             
            Assert.IsNotNull(resultado);
            CollectionAssert.AreEqual(contasBancarias, (List<ContaBancaria>)resultado);
            _contaBancariaRepositoryMock.Verify(repo => repo.ObterContasBancariasPorUserId(It.IsAny<Guid>()), Times.Once);
        }

        [TestMethod]
        /// <summary>
        /// Testa o método <see cref="ContasBancariasService.ObterContasPorUserId(Guid)"/>.
        /// Deve retornar uma lista vazia quando não existem contas bancárias.
        /// </summary>
        public async Task ObterContasPorUserId_DeveRetornarListaVaziaQuandoNaoExistemContas()
        { 
            var userId = Guid.NewGuid();
            _contaBancariaRepositoryMock.Setup(repo => repo.ObterContasBancariasPorUserId(It.IsAny<Guid>())).ReturnsAsync(new List<ContaBancaria>());

            var contasBancariasService = new ContasBancariasService(_contaBancariaRepositoryMock.Object);
             
            var resultado = await contasBancariasService.ObterContasPorUserId(userId);
             
            Assert.IsNotNull(resultado);
            CollectionAssert.AreEqual(new List<ContaBancaria>(), (List<ContaBancaria>)resultado);
            _contaBancariaRepositoryMock.Verify(repo => repo.ObterContasBancariasPorUserId(It.IsAny<Guid>()), Times.Once);
        }
    }
}

