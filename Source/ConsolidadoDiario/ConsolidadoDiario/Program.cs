using ConsolidadoDiario.Domain;
using ConsolidadoDiario.Domain.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Polly;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connStrRedis = Environment.GetEnvironmentVariable("REDIS_HOST") ?? builder.Configuration["RedisHost"];

var retryPolicy = Policy
    .Handle<RedisConnectionException>()
    .WaitAndRetry(new[]
    {
        TimeSpan.FromSeconds(10),
        TimeSpan.FromSeconds(30),
        TimeSpan.FromSeconds(60),
        TimeSpan.FromSeconds(120)
    });

builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
{
    var connStrRedis = Environment.GetEnvironmentVariable("REDIS_HOST") ?? builder.Configuration["RedisHost"];
    var retryPolicy = Policy
        .Handle<RedisConnectionException>()
        .WaitAndRetry(new[]
        {
            TimeSpan.FromSeconds(10),
            TimeSpan.FromSeconds(30),
            TimeSpan.FromSeconds(60),
            TimeSpan.FromSeconds(120)
        });

    return retryPolicy.Execute(() => ConnectionMultiplexer.Connect(connStrRedis));
});


var redisConnectionMultiplexer = retryPolicy.Execute(() => ConnectionMultiplexer.Connect(connStrRedis));
builder.Services.AddSingleton(redisConnectionMultiplexer);

builder.Services.AddSingleton<IRedisCacheService, RedisCacheService>();  
builder.Services.AddHostedService<RabbitMQWorker>();

// Remove the following line, as ConnectionMultiplexer is already registered as singleton
// builder.Services.AddScoped<IConnectionMultiplexer, ConnectionMultiplexer>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpLogging();

app.UseAuthorization();

app.MapControllers();
app.UseHttpsRedirection();
app.UseHsts();

app.Run();
