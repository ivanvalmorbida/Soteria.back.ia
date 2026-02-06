namespace SistemaCadastro.API.DTOs;

/// <summary>
/// DTO para telefones
/// </summary>
public class TelefoneDto
{
    public int? Codigo { get; set; }
    public string? Telefone { get; set; }
    public byte? Tipo { get; set; }
    public string? TipoDescricao { get; set; }
    public string? Descricao { get; set; }
}

/// <summary>
/// DTO para criar/atualizar telefone
/// Mantém compatibilidade com ContatoDto antigo através do campo Valor
/// </summary>
public class TelefoneCreateDto
{
    public string? Telefone { get; set; }
    public string? Valor { get; set; }  // Alias para Telefone (retrocompatibilidade)
    public byte? Tipo { get; set; }
    public string? Descricao { get; set; }

    /// <summary>
    /// Retorna o telefone do campo correto (Telefone ou Valor)
    /// </summary>
    public string? ObterTelefone() => !string.IsNullOrWhiteSpace(Telefone) ? Telefone : Valor;
}
