using ConsolidadoDiario.Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace ConsolidadoDiario.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ConsolidadoController : ControllerBase
    {
        private readonly RedisCacheService _redisCacheService;
        public ConsolidadoController(RedisCacheService redisCacheService)
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
    }
}

