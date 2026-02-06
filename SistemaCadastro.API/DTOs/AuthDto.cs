namespace SistemaCadastro.API.DTOs;

public class LoginDto
{
    public string Usuario { get; set; } = string.Empty;
    public string Senha { get; set; } = string.Empty;
}

public class LoginResponseDto
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public string? Token { get; set; }
    public UsuarioDto? Usuario { get; set; }
}

public class UsuarioDto
{
    public int Codigo { get; set; }
    public string? Usuario { get; set; }
    public int? Tipo { get; set; }
    public string? TipoDescricao { get; set; }
    public int? Pessoa { get; set; }
    public string? NomePessoa { get; set; }
    public DateTime? Cadastro { get; set; }
}

public class RegistrarUsuarioDto
{
    public string Usuario { get; set; } = string.Empty;
    public string Senha { get; set; } = string.Empty;
    public string ConfirmarSenha { get; set; } = string.Empty;
    public int Tipo { get; set; }
    public int? Pessoa { get; set; }
}

public class AlterarSenhaDto
{
    public string SenhaAtual { get; set; } = string.Empty;
    public string NovaSenha { get; set; } = string.Empty;
    public string ConfirmarNovaSenha { get; set; } = string.Empty;
}
