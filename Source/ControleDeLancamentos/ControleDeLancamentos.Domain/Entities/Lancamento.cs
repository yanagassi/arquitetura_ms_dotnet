
using System;

namespace ControleDeLancamentos.Domain.Entities
{
    public class Lancamento
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public decimal Valor { get; set; }
        public TipoLancamento Tipo { get; set; }
        public string Descricao { get; set; }
        public DateTime Data { get; set; }
        public Guid ContaId { get; set; }

        // Relacionamento com Conta Bancária
        public ContaBancaria ContaBancaria { get; set; }
    }

    public enum TipoLancamento
    {
        Debito,
        Credito
    }
}
