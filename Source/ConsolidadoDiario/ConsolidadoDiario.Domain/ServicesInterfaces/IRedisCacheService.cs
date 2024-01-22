using System;
using System.Threading.Tasks;
using ConsolidadoDiario.Domain.Entities;

namespace ConsolidadoDiario.Domain.Services
{
    public interface IRedisCacheService
    {
        Task SetAsync<T>(string key, T value);
        Task<T> GetAsync<T>(string key);
        Task<bool> RemoveAsync(string key);
        Task SetHashFieldAsync<T>(string key, string field, T value);
        Task AdicionarLancamentoAoRedisAsync(LancamentoDTO lancamento);
    }
}
