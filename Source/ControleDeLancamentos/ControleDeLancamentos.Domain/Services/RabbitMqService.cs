using System.Text;
using ControleDeLancamentos.Domain.Entities;
using Newtonsoft.Json;
using RabbitMQ.Client;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client.Exceptions;

public class RabbitMqService : IRabbitMqService
{
    private readonly IConfiguration _configuration;
    public RabbitMqService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task EnviarMensagemAsync(Lancamento mensagem)
    {
        var hostName = Environment.GetEnvironmentVariable("RABBITMQ_DEFAULT_HOST") ?? _configuration["RabbitMqSettings:HostName"];
        var userName = Environment.GetEnvironmentVariable("RABBITMQ_DEFAULT_USER") ?? _configuration["RabbitMqSettings:UserName"];
        var password = Environment.GetEnvironmentVariable("RABBITMQ_DEFAULT_PASS") ?? _configuration["RabbitMqSettings:Password"];
        var queueName = Environment.GetEnvironmentVariable("RABBITMQ_DEFAULT_QUEUE_NAME") ?? _configuration["RabbitMqSettings:QueueName"];

        var factory = new ConnectionFactory() { HostName = hostName, UserName = userName, Password = password, Port = 5672 };

        using (var connection = factory.CreateConnection())
        using (var channel = connection.CreateModel())
        {
            try
            {
                var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(mensagem));
                channel.BasicPublish(exchange: "", routingKey: queueName, basicProperties: null, body: body);
            }
            catch (OperationInterruptedException ex)
            {
                Console.WriteLine(ex);
            }
        }

        await Task.CompletedTask;
    }

}
