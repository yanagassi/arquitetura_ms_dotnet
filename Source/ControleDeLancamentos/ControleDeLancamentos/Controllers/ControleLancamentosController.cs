using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using ControleDeLancamentos.Domain.Services;
using ControleDeLancamentos.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace ControleDeLancamentos.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ControleLancamentosController : ControllerBase
    {
        private readonly IServicoControleLancamentos _servicoControleLancamentos;
        private readonly IMapper _mapper;


        public ControleLancamentosController(IServicoControleLancamentos servicoControleLancamentos, IMapper mapper)
        {
            _servicoControleLancamentos = servicoControleLancamentos;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return Ok();
        }

        [HttpGet("contas/{contaId}/lancamentos")]
        public async Task<IActionResult> ObterLancamentosDaConta(Guid contaId)
        {
            try
            {
                var lancamentos = await _servicoControleLancamentos.ObterLancamentosDaContaAsync(contaId);
                return Ok(lancamentos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno: {ex.Message}");
            }
        }

        [HttpPost("lancamentos")]
        public async Task<IActionResult> AdicionarLancamento([FromBody] LancamentoDTO lancamentoDTO)
        {
          
            try
            {
                await _servicoControleLancamentos.AdicionarLancamentoAsync(_mapper.Map<Domain.Entities.Lancamento>(lancamentoDTO));
                return Ok("Lançamento adicionado com sucesso");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno: {ex.Message}");
            }
        }

        [HttpGet("contas/{contaId}/saldo")]
        public async Task<IActionResult> CalcularSaldoDaConta(Guid contaId)
        {
            try
            {
                var saldo = await _servicoControleLancamentos.CalcularSaldoAsync(contaId);
                return Ok(new { Saldo = saldo });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno: {ex.Message}");
            }
        }

    }
}
