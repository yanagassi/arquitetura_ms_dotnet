using ConsolidadoDiario.Domain;
using ConsolidadoDiario.Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace ConsolidadoDiario.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ConsolidadoController : ControllerBase
    {
        private readonly IRedisCacheService _redisCacheService;
        public ConsolidadoController(IRedisCacheService redisCacheService)
        {
            _redisCacheService = redisCacheService; 
        }

        [HttpGet]
        public IActionResult Index()
        {
            return Ok("Running!");
        }

        [HttpGet("{contaId}")]
        public async Task<IActionResult> ObterLancamentosPorContaId(Guid contaId)
        {
            try
            {
                var lancamentos = await _redisCacheService.ObterLancamentosPorContaIdAsync(contaId);

                return Ok(lancamentos);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return StatusCode(500, "Erro interno ao processar a solicitação.");
            }
        }


        [HttpGet("saldo/{contaId}/{dataInicio}/{dataFim}")]
        public async Task<IActionResult> CalcularValorConsolidadoPorDiaAsync(Guid contaId, DateTime dataInicio, DateTime dataFim)
        {
            try
            {
                var lancamentos = await _redisCacheService.CalcularValorConsolidadoPorDiaAsync(contaId,dataInicio, dataFim);

                return Ok(lancamentos);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return StatusCode(500, "Erro interno ao processar a solicitação.");
            }
        }
        [HttpGet("{contaId}/{dataInicio}/{dataFim}")]
        public async Task<IActionResult> ObterLancamentosPorContaIdEData(Guid contaId, DateTime dataInicio, DateTime dataFim)
        {
            try
            {
                var lancamentos = await _redisCacheService.ObterLancamentosPorContaIdEDataAsync(contaId, dataInicio, dataFim);
                return Ok(lancamentos);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return StatusCode(500, "Erro interno ao processar a solicitação.");
            }
        }

    }
}

