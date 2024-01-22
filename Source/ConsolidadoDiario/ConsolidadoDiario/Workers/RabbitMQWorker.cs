using ConsolidadoDiario.Domain.Entities;
using ConsolidadoDiario.Domain.Services;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

public class RabbitMQWorker : BackgroundService
{ 
    private readonly IConfiguration _configuration; 
    private readonly RedisCacheService _redisCacheService;

    public RabbitMQWorker(IConfiguration configuration, RedisCacheService redisCacheService)
    {
        _configuration = configuration;
        _redisCacheService = redisCacheService;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {

        var hostName = Environment.GetEnvironmentVariable("RABBITMQ_DEFAULT_HOST") ?? _configuration["RabbitMqSettings:HostName"];
        var userName = Environment.GetEnvironmentVariable("RABBITMQ_DEFAULT_USER") ?? _configuration["RabbitMqSettings:UserName"];
        var password = Environment.GetEnvironmentVariable("RABBITMQ_DEFAULT_PASS") ?? _configuration["RabbitMqSettings:Password"];
        var queueName = Environment.GetEnvironmentVariable("RABBITMQ_DEFAULT_QUEUE_NAME") ?? _configuration["RabbitMqSettings:QueueName"];

        var factory = new ConnectionFactory() { HostName = hostName, UserName = userName, Password = password, Port = 5672 };

        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();
        channel.QueueDeclare(queue: queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);

        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += (model, ea) =>
        {
            var body = ea.Body;
            var message = Encoding.UTF8.GetString(body);
            Console.WriteLine($" [x] Received from RabbitMQ: {message}");

            var lancamento = JsonConvert.DeserializeObject<Lancamento>(message);
            _redisCacheService.AdicionarLancamentoAoRedisAsync(lancamento);
            channel.BasicAck(ea.DeliveryTag, false);

        };

        channel.BasicConsume(queue: queueName, false, consumer: consumer);
        await Task.Delay(Timeout.Infinite, stoppingToken);
    }
}
 
