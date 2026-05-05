namespace SistemaCadastro.API.DTOs;

public class CentroCustoDto
{
    public int Codigo { get; set; }
    public string Descricao { get; set; } = string.Empty;
}

public class CentroCustoCreateDto
{
    public int? Codigo { get; set; }
    public string Descricao { get; set; } = string.Empty;
}

public class CentroCustoUpdateDto
{
    public int Codigo { get; set; }
    public string Descricao { get; set; } = string.Empty;
}
