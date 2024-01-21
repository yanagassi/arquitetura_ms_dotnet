public interface IServicoAutenticacao
{
    Task<bool> ValidarCredenciaisAsync(string email, string senha);
    string GerarTokenJwt(Usuario usuario);
    Task<Usuario> LoginAsync(string email, string senha);
}

