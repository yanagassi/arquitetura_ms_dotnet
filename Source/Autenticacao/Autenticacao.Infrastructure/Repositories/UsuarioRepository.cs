using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

public class UsuarioRepository : IUsuarioRepository
{
    private readonly AutenticacaoDbContext _dbContext;

    public UsuarioRepository(AutenticacaoDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Usuario> ObterPorEmailAsync(string email)
    {
        return await _dbContext.Set<Usuario>().FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task AdicionarAsync(Usuario usuario)
    {
        await _dbContext.Set<Usuario>().AddAsync(usuario);
        await _dbContext.SaveChangesAsync();
    }
}
