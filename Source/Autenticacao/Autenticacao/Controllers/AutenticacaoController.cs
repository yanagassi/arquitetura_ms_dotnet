using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;

namespace Autenticacao.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AutenticacaoController : ControllerBase
    {
        private readonly IServicoAutenticacao _servicoAutenticacao;
        public AutenticacaoController(IServicoAutenticacao servicoAutenticacao)
        {
            _servicoAutenticacao = servicoAutenticacao;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return Ok("Running!");
        }



        [HttpPost]
        public async Task<IActionResult> LoginAsync([FromBody] LoginRequestDto request)
        {

            if (request == null)
            {
                return BadRequest("Solicitação inválida");
            }


            var usuario = await _servicoAutenticacao.LoginAsync(request.Email, request.Senha);

            if (usuario == null)
            {
                return Unauthorized("Credenciais inválidas");
            }

            var token = _servicoAutenticacao.GerarTokenJwt(usuario);
            return Ok(new { Token = token });

        }
    }
}

