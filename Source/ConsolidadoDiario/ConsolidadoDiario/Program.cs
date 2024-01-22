
using ConsolidadoDiario.Domain.Services;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);
 
 
builder.Services.AddControllers();
 

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var connStrRedis = Environment.GetEnvironmentVariable("REDIS_HOST") ??
    builder.Configuration["RedisHost"];


var redisConnectionMultiplexer = ConnectionMultiplexer.Connect(connStrRedis);
builder.Services.AddSingleton(redisConnectionMultiplexer);

builder.Services.AddSingleton<RedisCacheService>();



builder.Services.AddHostedService<RabbitMQWorker>();


var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpLogging();



app.UseAuthorization();

app.MapControllers();
app.UseHttpsRedirection();
app.UseHsts();

app.Run();

