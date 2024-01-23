using ControleDeLancamentos.Domain.Entities;


public interface IRabbitMqService
{
    /// <summary>
    /// Adiciona um lançamento, atualiza o saldo da conta e envia uma mensagem para o RabbitMQ.
    /// </summary>
    /// <param name="lancamento">Lançamento a ser adicionado.</param>
    Task EnviarMensagemAsync(Lancamento mensagem);
}
