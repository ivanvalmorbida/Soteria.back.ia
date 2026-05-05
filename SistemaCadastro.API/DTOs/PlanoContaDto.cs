namespace SistemaCadastro.API.DTOs;

public class PlanoContaDto
{
    public int Codigo { get; set; }
    public int? CodigoPai { get; set; }
    public string? CodigoPaiRotulo { get; set; }
    public string Tipo { get; set; } = string.Empty;
    public string Rotulo { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
}

public class PlanoContaCreateDto
{
    public int? CodigoPai { get; set; }
    public string Tipo { get; set; } = string.Empty;
    public string Rotulo { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
}

public class PlanoContaUpdateDto
{
    public int Codigo { get; set; }
    public int? CodigoPai { get; set; }
    public string Tipo { get; set; } = string.Empty;
    public string Rotulo { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
}
