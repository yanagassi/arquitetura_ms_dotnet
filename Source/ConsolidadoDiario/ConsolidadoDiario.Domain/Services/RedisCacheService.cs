using ConsolidadoDiario.Domain.Entities;
using Newtonsoft.Json;
using StackExchange.Redis; 

namespace ConsolidadoDiario.Domain.Services
{
    public class RedisCacheService
    {
        private readonly IDatabase _database;

        public RedisCacheService(ConnectionMultiplexer redisConnectionMultiplexer)
        {
            if (redisConnectionMultiplexer == null)
            {
                throw new ArgumentNullException(nameof(redisConnectionMultiplexer));
            }

            _database = redisConnectionMultiplexer.GetDatabase();
        }


        public async Task<T> GetAsync<T>(string key)
        {
            var value = await _database.StringGetAsync(key);

            if (value.HasValue)
            {
                return System.Text.Json.JsonSerializer.Deserialize<T>(value);
            }

            return default;
        }

        public async Task SetAsync<T>(string key, T value)
        {
            var serializedValue = System.Text.Json.JsonSerializer.Serialize(value);
            await _database.StringSetAsync(key, serializedValue, null);
        }

        public async Task<bool> RemoveAsync(string key)
        {
            return await _database.KeyDeleteAsync(key);
        }

        public async Task SetHashFieldAsync<T>(string key, string field, T value)
        {
            var serializedValue = System.Text.Json.JsonSerializer.Serialize(value);
            await _database.HashSetAsync(key, field, serializedValue);
        }

        public async Task AdicionarLancamentoAoRedisAsync(Lancamento lancamento)
        {
            var redisKey = $"ContaId:{lancamento.ContaId}";
            var field = $"Lancamento:{Guid.NewGuid()}";

            var serializedLancamento = JsonConvert.SerializeObject(lancamento);

            await SetHashFieldAsync(redisKey, field, serializedLancamento);
        }

        public async Task<IEnumerable<Lancamento>> ObterLancamentosPorContaIdAsync(Guid contaId)
        {
            var redisKey = $"ContaId:{contaId}";
            var hashEntries = await _database.HashGetAllAsync(redisKey);

            var lancamentos = hashEntries.Select(entry =>
            {
                var serializedLancamento = entry.Value;
                var serializeObj = JsonConvert.DeserializeObject<string>(serializedLancamento);
                return JsonConvert.DeserializeObject<Lancamento>(serializeObj); 
            });
         
            return lancamentos;
        }  
    }
}
