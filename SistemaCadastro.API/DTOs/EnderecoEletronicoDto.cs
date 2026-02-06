namespace SistemaCadastro.API.DTOs;

/// <summary>
/// DTO para endereços eletrônicos (emails, redes sociais, sites)
/// </summary>
public class EnderecoEletronicoDto
{
    public int? Codigo { get; set; }
    public string? Endereco { get; set; }
    public int? Tipo { get; set; }
    public string? TipoDescricao { get; set; }
    public string? Descricao { get; set; }
}

/// <summary>
/// DTO para criar/atualizar endereço eletrônico
/// </summary>
public class EnderecoEletronicoCreateDto
{
    public string? Endereco { get; set; }
    public int? Tipo { get; set; }
    public string? Descricao { get; set; }
}
