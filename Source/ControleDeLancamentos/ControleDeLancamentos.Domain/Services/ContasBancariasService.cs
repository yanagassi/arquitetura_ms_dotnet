using ControleDeLancamentos.Domain.Entities;
using ControleDeLancamentos.Domain.Repositories;

namespace ControleDeLancamentos.Domain.Services
{
    public class ContasBancariasService : IContasBancariasService
    {
        private readonly IContaBancariaRepository _contaBancariaRepository;
         
        public ContasBancariasService(IContaBancariaRepository contaBancariaRepository)
        {
            _contaBancariaRepository = contaBancariaRepository;
        }

      
        public async Task<ContaBancaria> ObterContaBancariaAsync(Guid contaId)
        {
            var conta = await _contaBancariaRepository.ObterContaAsync(contaId);

            if (conta == null)
            {
                throw new Exception("Conta bancária não encontrada.");
            }

            return conta;
        }

       
        public async Task<IEnumerable<ContaBancaria>> ObterContasPorUserId(Guid userId)
        {
            return await  _contaBancariaRepository.ObterContasBancariasPorUserId(userId);
        }
    }
}
