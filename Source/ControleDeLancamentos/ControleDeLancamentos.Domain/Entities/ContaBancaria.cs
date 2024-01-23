
namespace ControleDeLancamentos.Domain.Entities
{
    public class ContaBancaria
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Nome { get; set; }
        public decimal Saldo { get; set; } = 0;

        // Relacionamento com Lançamentos
        public List<Lancamento>? Lancamentos { get; set; }
    }
}
