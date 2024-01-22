/// <summary>
/// Serviço responsável por autenticação e geração de tokens JWT.
/// </summary>
public interface IServicoAutenticacao
{

    /// <summary>
    /// Valida as credenciais do usuário.
    /// </summary>
    /// <param name="email">E-mail do usuário.</param>
    /// <param name="senha">Senha do usuário.</param>
    /// <returns>True se as credenciais forem válidas, caso contrário, false.</returns>
    Task<bool> ValidarCredenciaisAsync(string email, string senha);

    /// <summary>
    /// Gera um token JWT para o usuário especificado.
    /// </summary>
    /// <param name="usuario">Usuário para o qual o token será gerado.</param>
    /// <returns>Token JWT gerado.</returns>
    string GerarTokenJwt(Usuario usuario);

    /// <summary>
    /// Realiza o login do usuário com as credenciais fornecidas.
    /// </summary>
    /// <param name="email">E-mail do usuário.</param>
    /// <param name="senha">Senha do usuário.</param>
    /// <returns>Usuário autenticado ou null se as credenciais forem inválidas.</returns>
    Task<Usuario> LoginAsync(string email, string senha);
}

