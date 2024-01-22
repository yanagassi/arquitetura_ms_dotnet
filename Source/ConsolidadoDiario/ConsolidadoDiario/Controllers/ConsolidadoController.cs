using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConsolidadoDiario.Domain.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

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
                // Log the exception
                return StatusCode(500, "Erro interno ao processar a solicitação.");
            }
        }
    }
}

