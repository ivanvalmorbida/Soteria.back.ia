namespace SistemaCadastro.API.DTOs;

public class LanctoContabilDto
{
    public int Codigo { get; set; }
    public int Pessoa { get; set; }
    public string PessoaNome { get; set; } = string.Empty;
    public int CentroCusto { get; set; }
    public string CentroCustoDescricao { get; set; } = string.Empty;
    public int Credito { get; set; }
    public string CreditoDescricao { get; set; } = string.Empty;
    public int Debito { get; set; }
    public string DebitoDescricao { get; set; } = string.Empty;
    public double Valor { get; set; }
    public DateTime Data { get; set; }
    public int HP { get; set; }
    public string Descricao { get; set; } = string.Empty;
}

public class LanctoContabilCreateDto
{
    public int? Codigo { get; set; }
    public int Pessoa { get; set; }
    public int CentroCusto { get; set; }
    public int Credito { get; set; }
    public int Debito { get; set; }
    public double Valor { get; set; }
    public DateTime Data { get; set; }
    public int HP { get; set; }
    public string Descricao { get; set; } = string.Empty;
}

public class LanctoContabilUpdateDto
{
    public int Codigo { get; set; }
    public int Pessoa { get; set; }
    public int CentroCusto { get; set; }
    public int Credito { get; set; }
    public int Debito { get; set; }
    public double Valor { get; set; }
    public DateTime Data { get; set; }
    public int HP { get; set; }
    public string Descricao { get; set; } = string.Empty;
}
