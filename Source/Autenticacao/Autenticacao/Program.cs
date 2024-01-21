using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Adiciona serviços ao contêiner.
builder.Services.AddControllers();

var connectionString = Environment.GetEnvironmentVariable("DB_STRING_CONNECTION") ??
    builder.Configuration.GetConnectionString("AutenticacaoDbConnection");


Console.WriteLine("Connection");

Console.WriteLine(connectionString);

builder.Services.AddDbContext<AutenticacaoDbContext>(options =>
    options.UseNpgsql(connectionString, b => b.MigrationsAssembly("Autenticacao")));

# region Repositories
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
# endregion

# region Services
builder.Services.AddScoped<IServicoAutenticacao, ServicoAutenticacao>();
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
    var dbContext = app.Services.GetRequiredService<AutenticacaoDbContext>();

    try
    {
        dbContext.Database.Migrate();
        Console.WriteLine("Migrations applied successfully.");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error applying migrations: {ex.Message}");
    }
}

app.Run();
