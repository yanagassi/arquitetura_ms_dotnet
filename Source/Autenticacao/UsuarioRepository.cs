using System.Threading.Tasks;

public class UsuarioRepository : IUsuarioRepository
{
    private readonly DbContext _dbContext;

    public UsuarioRepository(DbContext dbContext)
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
