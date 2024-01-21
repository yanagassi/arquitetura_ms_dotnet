using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;

/// <summary>
/// Serviço responsável por autenticação e geração de tokens JWT.
/// </summary>
public class ServicoAutenticacao : IServicoAutenticacao
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IConfiguration _configuration;

    /// <summary>
    /// Inicializa uma nova instância do ServicoAutenticacao.
    /// </summary>
    /// <param name="usuarioRepository">Repositório de usuários.</param>
    /// <param name="configuration">Configuração da aplicação.</param>
    public ServicoAutenticacao(IUsuarioRepository usuarioRepository, IConfiguration configuration)
    {
        _usuarioRepository = usuarioRepository;
        _configuration = configuration;
    }

    /// <summary>
    /// Valida as credenciais do usuário.
    /// </summary>
    /// <param name="email">E-mail do usuário.</param>
    /// <param name="senha">Senha do usuário.</param>
    /// <returns>True se as credenciais forem válidas, caso contrário, false.</returns>
    public async Task<bool> ValidarCredenciaisAsync(string email, string senha)
    {
        var usuario = await _usuarioRepository.ObterPorEmailAsync(email);

        // Verificar se o usuário existe e a senha é válida
        return usuario != null && BCrypt.Net.BCrypt.Verify(senha, usuario.SenhaHash);
    }

    /// <summary>
    /// Gera um token JWT para o usuário especificado.
    /// </summary>
    /// <param name="usuario">Usuário para o qual o token será gerado.</param>
    /// <returns>Token JWT gerado.</returns>
    public string GerarTokenJwt(Usuario usuario)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
            new Claim(ClaimTypes.Name, usuario.Nome),
            new Claim(ClaimTypes.Email, usuario.Email)
        };

        // Obter a chave secreta do arquivo de configuração
        var keyBytes = Encoding.UTF8.GetBytes(_configuration["JwtConfig:Secret"]);
        var key = new SymmetricSecurityKey(keyBytes);

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _configuration["JwtConfig:Issuer"],
            audience: _configuration["JwtConfig:Audience"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(int.Parse(_configuration["JwtConfig:ExpirationInMinutes"])),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    /// <summary>
    /// Realiza o login do usuário com as credenciais fornecidas.
    /// </summary>
    /// <param name="email">E-mail do usuário.</param>
    /// <param name="senha">Senha do usuário.</param>
    /// <returns>Usuário autenticado ou null se as credenciais forem inválidas.</returns>
    public async Task<Usuario> LoginAsync(string email, string senha)
    {
        var usuario = await _usuarioRepository.ObterPorEmailAsync(email);

        // Verificar se o usuário existe e a senha é válida
        return usuario != null && BCrypt.Net.BCrypt.Verify(senha, usuario.SenhaHash) ? usuario : null;
    }
}
