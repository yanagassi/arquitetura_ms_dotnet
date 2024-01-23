using Moq;
using StackExchange.Redis;
using ConsolidadoDiario.Domain.Entities;
using Newtonsoft.Json;

namespace ConsolidadoDiario.Domain.Services.Tests
{
    [TestClass]
    public class RedisCacheServiceTests
    {
        private Mock<IDatabase> _mockDatabase;
        private Mock<IConnectionMultiplexer> _mockConnectionMultiplexer;
        private RedisCacheService _redisCacheService;

        [TestInitialize]
        public void Setup()
        {
            _mockDatabase = new Mock<IDatabase>();
            _mockConnectionMultiplexer = new Mock<IConnectionMultiplexer>();
            _mockConnectionMultiplexer.Setup(x => x.GetDatabase(It.IsAny<int>(), It.IsAny<object>())).Returns(_mockDatabase.Object);
            _redisCacheService = new RedisCacheService(_mockConnectionMultiplexer.Object);
        }

        [TestMethod]
        /// <summary>
        /// Testa se o método GetAsync retorna corretamente um objeto desserializado quando a chave existe no Redis.
        /// </summary>
        public async Task GetAsync_ShouldReturnDeserializedObject_WhenKeyExists()
        { 
            var key = (RedisKey)"testKey";
            var expectedObject = new Lancamento { Id = Guid.NewGuid(), Valor = 100.00m };

            _mockDatabase.Setup(x => x.StringGetAsync(key, CommandFlags.None)).ReturnsAsync(new RedisValue(JsonConvert.SerializeObject(expectedObject)));
             
            var result = await _redisCacheService.GetAsync<Lancamento>(key);
             
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedObject.Id, result.Id);
            Assert.AreEqual(expectedObject.Valor, result.Valor);
        }

        [TestMethod]
        /// <summary>
        /// Testa se o método GetAsync retorna o valor padrão quando a chave não existe no Redis.
        /// </summary>
        public async Task GetAsync_ShouldReturnDefault_WhenKeyDoesNotExist()
        { 
            var key = "nonExistingKey";

            _mockDatabase.Setup(x => x.StringGetAsync(key, CommandFlags.None)).ReturnsAsync((RedisValue)"");
             
            var result = await _redisCacheService.GetAsync<Lancamento>(key);
             
            Assert.AreEqual(default(Lancamento), result);
        }


        [TestMethod]
        /// <summary>
        /// Testa se o método RemoveAsync chama corretamente o método KeyDeleteAsync do Redis.
        /// </summary>
        public async Task RemoveAsync_ShouldCallKeyDeleteAsync_WithCorrectParameters()
        { 
            var key = "testKey";
             
            await _redisCacheService.RemoveAsync(key);
             
            _mockDatabase.Verify(x => x.KeyDeleteAsync(key, CommandFlags.None));
        }

       

        [TestMethod]
        /// <summary>
        /// Testa se o método SetHashFieldAsync chama corretamente o método HashSetAsync do Redis.
        /// </summary>
        public async Task SetHashFieldAsync_ShouldCallHashSetAsync_WithCorrectParameters()
        { 
            var key = (RedisKey)"testHashKey";
            var field = "testField";
            var value = new Lancamento { Id = Guid.NewGuid(), Valor = 75.00m };
             
            await _redisCacheService.SetHashFieldAsync(key, field, value);
             
            _mockDatabase.Verify(x => x.HashSetAsync(key, field, JsonConvert.SerializeObject(value), When.Always,CommandFlags.None));
        }

        [TestMethod]
        /// <summary>
        /// Testa se o método AdicionarLancamentoAoRedisAsync chama corretamente o método HashSetAsync do Redis.
        /// </summary>
        public async Task AdicionarLancamentoAoRedisAsync_ShouldCallSetHashFieldAsync_WithCorrectParameters()
        { 
            var lancamento = new Lancamento { Id = Guid.NewGuid(), Valor = 120.00m }; 
            await _redisCacheService.AdicionarLancamentoAoRedisAsync(lancamento); 
            _mockDatabase.Verify(x => x.HashSetAsync(
                It.IsAny<RedisKey>(), 
                It.IsAny<RedisValue>(),  
                It.IsAny<RedisValue>(),  
                When.Always, CommandFlags.None));
        }
         

    }
}
