using System.Threading.Tasks;

public interface IUsuarioRepository
{
    Task<Usuario> ObterPorEmailAsync(string email);
    Task AdicionarAsync(Usuario usuario);
}
