using System;

public class Usuario
{
    public Guid Id { get;   set; }
    public string Nome { get;   set; }
    public string Email { get;   set; }
    public string SenhaHash { get;   set; }

    public void DefinirSenha(string senha)
    {

        SenhaHash = HashSenha(senha);
    }

    private string HashSenha(string senha)
    {
        return BCrypt.Net.BCrypt.HashPassword(senha);
    }
}
