namespace SistemaCadastro.API.Models;

public class Usuario
{
    public int Codigo { get; set; }
    public string? UsuarioLogin { get; set; }
    public string? Senha { get; set; }
    public int? Tipo { get; set; }
    public int? Pessoa { get; set; }
    public DateTime? Cadastro { get; set; }
}

public class UsuarioTipo
{
    public int Codigo { get; set; }
    public string? Descricao { get; set; }
}
