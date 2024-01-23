
namespace ConsolidadoDiario.Domain.Entities
{
	public class Lancamento
    {

        public Guid Id { get; set; }
        public decimal Valor { get; set; }
        public string Tipo { get; set; }
        public string Descricao { get; set; }
        public DateTime Data { get; set; }
        public Guid ContaId { get; set; }
        public Guid UserId { get; set; }
  
	}
}

