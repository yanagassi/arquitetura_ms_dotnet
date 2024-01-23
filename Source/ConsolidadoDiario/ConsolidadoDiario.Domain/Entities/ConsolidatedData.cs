using ConsolidadoDiario.Domain.Entities;

public class ConsolidatedData
{
    public DateTime Date { get; set; }
    public decimal TotalValue { get; set; }
    public string Tipo { get; set; }
    public List<Lancamento> Lancamentos { get; set; }
}
