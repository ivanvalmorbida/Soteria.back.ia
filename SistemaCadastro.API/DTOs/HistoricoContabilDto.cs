namespace SistemaCadastro.API.DTOs;

public class HistoricoContabilDto
{
    public int Codigo { get; set; }
    public string Descricao { get; set; } = string.Empty;
}

public class HistoricoContabilCreateDto
{
    public int? Codigo { get; set; }
    public string Descricao { get; set; } = string.Empty;
}

public class HistoricoContabilUpdateDto
{
    public int Codigo { get; set; }
    public string Descricao { get; set; } = string.Empty;
}
