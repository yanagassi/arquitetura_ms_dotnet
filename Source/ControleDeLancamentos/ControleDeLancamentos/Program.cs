﻿using ControleDeLancamentos.Domain.Repositories;
using ControleDeLancamentos.Domain.Services;
using ControleDeLancamentos.Infrastructure.DbContexts;
using ControleDeLancamentos.Infrastructure.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Polly;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

var connectionString = Environment.GetEnvironmentVariable("DB_STRING_CONNECTION") ??
    builder.Configuration.GetConnectionString("ControleDeLancamentos");

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());


builder.Services.AddDbContext<ControleLancamentosDbContext>(options =>
    options.UseNpgsql(connectionString, b => b.MigrationsAssembly("ControleDeLancamentos")));

# region Repositories
builder.Services.AddScoped<ILancamentoRepository, LancamentoRepository>();
builder.Services.AddScoped<IContaBancariaRepository, ContaBancariaRepository>();
# endregion

# region Services
builder.Services.AddScoped<IServicoControleLancamentos, ServicoControleLancamentos>();
builder.Services.AddScoped<IRabbitMqService, RabbitMqService>();
builder.Services.AddScoped<IContasBancariasService, ContasBancariasService>();
# endregion


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();


// Verifica se o argumento "--migrate" está presente e aplica as migrações
if (args.Contains("--migrate"))
{

    var retryPolicy = Policy
        .Handle<Exception>()
        .WaitAndRetry(5, retryAttempt => TimeSpan.FromSeconds(10));


    retryPolicy.Execute(() =>
    {
        var dbContext = app.Services.GetRequiredService<ControleLancamentosDbContext>();
        dbContext.Database.Migrate();
        Console.WriteLine("Migrations applied successfully.");
    });
}


app.Run();
