using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;
using Moq;

namespace Autenticacao.Tests
{

    [TestClass]
    public class ServicoAutenticacaoTests
    {
        private readonly Mock<IUsuarioRepository> _usuarioRepositoryMock = new Mock<IUsuarioRepository>();
        private readonly Mock<IConfiguration> _configurationMock = new Mock<IConfiguration>();

        [TestMethod]
        /// <summary>
        /// Deve retornar true quando as credenciais são válidas.
        /// </summary>
        public async Task ValidarCredenciaisAsync_DeveRetornarTrueQuandoCredenciaisValidas()
        { 
            var usuario = new Usuario { Email = "teste@teste.com", SenhaHash = BCrypt.Net.BCrypt.HashPassword("senha") };
            _usuarioRepositoryMock.Setup(repo => repo.ObterPorEmailAsync(It.IsAny<string>())).ReturnsAsync(usuario);

            var servicoAutenticacao = new ServicoAutenticacao(_usuarioRepositoryMock.Object, _configurationMock.Object);
             
            var resultado = await servicoAutenticacao.ValidarCredenciaisAsync("teste@teste.com", "senha");
             
            Assert.IsTrue(resultado);
        }


        [TestMethod]
        /// <summary>
        /// Deve retornar false quando o usuário não existe.
        /// </summary>
        public async Task ValidarCredenciaisAsync_DeveRetornarFalseQuandoUsuarioNaoExiste()
        { 
            _usuarioRepositoryMock.Setup(repo => repo.ObterPorEmailAsync(It.IsAny<string>())).ReturnsAsync((Usuario)null);

            var servicoAutenticacao = new ServicoAutenticacao(_usuarioRepositoryMock.Object, _configurationMock.Object);
             
            var resultado = await servicoAutenticacao.ValidarCredenciaisAsync("naoexistente@teste.com", "senha");
             
            Assert.IsFalse(resultado);
        }

        [TestMethod]
        /// <summary>
        /// Deve retornar false quando a senha é inválida.
        /// </summary>
        public async Task ValidarCredenciaisAsync_DeveRetornarFalseQuandoSenhaInvalida()
        { 
            var usuario = new Usuario { Email = "teste@teste.com", SenhaHash = BCrypt.Net.BCrypt.HashPassword("senha") };
            _usuarioRepositoryMock.Setup(repo => repo.ObterPorEmailAsync(It.IsAny<string>())).ReturnsAsync(usuario);

            var servicoAutenticacao = new ServicoAutenticacao(_usuarioRepositoryMock.Object, _configurationMock.Object);
             
            var resultado = await servicoAutenticacao.ValidarCredenciaisAsync("teste@teste.com", "senhaerrada");
             
            Assert.IsFalse(resultado);
        }

        [TestMethod]
        /// <summary>
        /// Deve retornar um token JWT válido.
        /// </summary>
        public async Task GerarTokenJwt_DeveRetornarTokenJwtValido()
        { 
            var usuario = new Usuario { Id = Guid.NewGuid(), Nome = "Teste", Email = "teste@teste.com" };
            _configurationMock.Setup(config => config["JwtConfig:Secret"]).Returns("seusegredojwtseusegredojwtseusegredojwt");
            _configurationMock.Setup(config => config["JwtConfig:Issuer"]).Returns("emissor");
            _configurationMock.Setup(config => config["JwtConfig:ExpirationInMinutes"]).Returns("60");

            var servicoAutenticacao = new ServicoAutenticacao(_usuarioRepositoryMock.Object, _configurationMock.Object);
             
            var tokenJwt = servicoAutenticacao.GerarTokenJwt(usuario);
             
            Assert.IsNotNull(tokenJwt);
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadToken(tokenJwt) as JwtSecurityToken;

            Assert.IsNotNull(jwtToken);
            Assert.AreEqual(usuario.Id.ToString(), jwtToken?.Claims?.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);
            Assert.AreEqual(usuario.Nome, jwtToken?.Claims?.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value);
            Assert.AreEqual(usuario.Email, jwtToken?.Claims?.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value);
        }

        [TestMethod]
        /// <summary>
        /// Deve retornar um usuário quando as credenciais são válidas.
        /// </summary>
        public async Task LoginAsync_DeveRetornarUsuarioQuandoCredenciaisSaoValidas()
        { 
            var usuario = new Usuario { Email = "teste@teste.com", SenhaHash = BCrypt.Net.BCrypt.HashPassword("senha") };
            _usuarioRepositoryMock.Setup(repo => repo.ObterPorEmailAsync(It.IsAny<string>())).ReturnsAsync(usuario);

            var servicoAutenticacao = new ServicoAutenticacao(_usuarioRepositoryMock.Object, _configurationMock.Object);
             
            var resultado = await servicoAutenticacao.LoginAsync("teste@teste.com", "senha");
             
            Assert.IsNotNull(resultado);
            Assert.AreEqual(usuario.Email, resultado.Email);
        }

        [TestMethod]
        /// <summary>
        /// Deve retornar null quando o usuário não existe.
        /// </summary>
        public async Task LoginAsync_DeveRetornarNullQuandoUsuarioNaoExiste()
        { 
            _usuarioRepositoryMock.Setup(repo => repo.ObterPorEmailAsync(It.IsAny<string>())).ReturnsAsync((Usuario)null);

            var servicoAutenticacao = new ServicoAutenticacao(_usuarioRepositoryMock.Object, _configurationMock.Object);
             
            var resultado = await servicoAutenticacao.LoginAsync("naoexistente@teste.com", "senha");
              
            Assert.IsNull(resultado);
        }

        [TestMethod]
        /// <summary>
        /// Deve retornar null quando a senha é inválida.
        /// </summary>
        public async Task LoginAsync_DeveRetornarNullQuandoSenhaInvalida()
        { 
            var usuario = new Usuario { Email = "teste@teste.com", SenhaHash = BCrypt.Net.BCrypt.HashPassword("senha") };
            _usuarioRepositoryMock.Setup(repo => repo.ObterPorEmailAsync(It.IsAny<string>())).ReturnsAsync(usuario);

            var servicoAutenticacao = new ServicoAutenticacao(_usuarioRepositoryMock.Object, _configurationMock.Object);
             
            var resultado = await servicoAutenticacao.LoginAsync("teste@teste.com", "senhaerrada");
             
            Assert.IsNull(resultado);
        }
    }

}

