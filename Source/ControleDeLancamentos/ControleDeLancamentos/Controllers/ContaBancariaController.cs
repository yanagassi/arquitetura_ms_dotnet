using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using ControleDeLancamentos.Domain.Entities;
using ControleDeLancamentos.Domain.Services;
using ControleDeLancamentos.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace ControleDeLancamentos.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ContaBancariaController : ControllerBase
    {
        private readonly IContasBancariasService _servicoContaBancaria;
        private readonly IMapper _mapper;

        public ContaBancariaController(IContasBancariasService servicoContaBancaria, IMapper mapper)
        {
            _servicoContaBancaria = servicoContaBancaria ?? throw new ArgumentNullException(nameof(servicoContaBancaria));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }


        [HttpGet("usuario/{userId}")]
        public async Task<IActionResult> ObterContasPorUsuario(Guid userId)
        {
            try
            {
                var contas = await _servicoContaBancaria.ObterContasPorUserId(userId);
                return Ok(contas);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"{ex.Message}");
            }
        }

    }
}
