using ConsolidadoDiario.Domain.Entities;
using Newtonsoft.Json;
using StackExchange.Redis; 

namespace ConsolidadoDiario.Domain.Services
{
    public class RedisCacheService : IRedisCacheService
    {
        private readonly IDatabase _database;

        public RedisCacheService(IConnectionMultiplexer redisConnectionMultiplexer)
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

       
        public async Task<IEnumerable<Lancamento>> ObterLancamentosPorContaIdEDataAsync(Guid contaId, DateTime dataInicio, DateTime dataFim)
        {
            var redisKey = $"ContaId:{contaId}";
            var hashEntries = await _database.HashGetAllAsync(redisKey);

            var lancamentos = hashEntries.Select(entry =>
            {
                var serializedLancamento = entry.Value;
                var deserializedLancamento = JsonConvert.DeserializeObject<string>(serializedLancamento);
                var lancamento = JsonConvert.DeserializeObject<Lancamento>(deserializedLancamento);
                 
                if (lancamento.Data >= dataInicio && lancamento.Data <= dataFim)
                {
                    return lancamento;
                }

                return null;  
            }).Where(l => l != null);  

            return lancamentos;
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


        public async Task<IEnumerable<ConsolidatedData>> CalcularValorConsolidadoPorDiaAsync(Guid contaId, DateTime dataInicio, DateTime dataFim)
        {
            var lancamentosPorData = await ObterLancamentosPorContaIdEDataAsync(contaId, dataInicio, dataFim);

            var consolidatedData = lancamentosPorData
                .GroupBy(l => new { l.Data.Date, l.Tipo }) 
                .Select(group => new ConsolidatedData
                {
                    Date = group.Key.Date, 
                    TotalValue = group.Sum(l => l.Valor),
                    Lancamentos = group.ToList()
                });

            return consolidatedData;
        }


    }
}
