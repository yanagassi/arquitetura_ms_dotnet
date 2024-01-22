using ControleDeLancamentos.Domain.Entities;

/// <summary>
/// Serviço responsável pelo controle de lançamentos e interações com contas bancárias.
/// </summary>
public interface IRabbitMqService
{
    /// <summary>
    /// Adiciona um lançamento, atualiza o saldo da conta e envia uma mensagem para o RabbitMQ.
    /// </summary>
    /// <param name="lancamento">Lançamento a ser adicionado.</param>
    Task EnviarMensagemAsync(Lancamento mensagem);
}
