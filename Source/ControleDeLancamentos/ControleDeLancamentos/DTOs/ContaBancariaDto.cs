using System;

namespace ControleDeLancamentos.DTOs
{
    public class ContaBancariaDTO
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Nome { get; set; }
        public decimal Saldo { get; set; }
    }
}
