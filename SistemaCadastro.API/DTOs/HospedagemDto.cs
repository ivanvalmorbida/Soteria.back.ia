namespace SistemaCadastro.API.DTOs;

public class HospedagemDto
{
    public int Codigo { get; set; }
    public int Pessoa { get; set; }
    public string PessoaNome { get; set; } = string.Empty;
    public string Quarto { get; set; } = string.Empty;
    public DateTime Checkin { get; set; }
    public DateTime? Checkout { get; set; }
    public decimal Diaria { get; set; }
    public decimal? Total { get; set; }
    public string Status { get; set; } = "Ativa";
    public string? Observacoes { get; set; }
    public DateTime? Cadastrado { get; set; }
}

public class HospedagemCreateDto
{
    public int Pessoa { get; set; }
    public string Quarto { get; set; } = string.Empty;
    public DateTime Checkin { get; set; }
    public DateTime? Checkout { get; set; }
    public decimal Diaria { get; set; }
    public decimal? Total { get; set; }
    public string Status { get; set; } = "Ativa";
    public string? Observacoes { get; set; }
}

public class HospedagemUpdateDto
{
    public int Codigo { get; set; }
    public int Pessoa { get; set; }
    public string Quarto { get; set; } = string.Empty;
    public DateTime Checkin { get; set; }
    public DateTime? Checkout { get; set; }
    public decimal Diaria { get; set; }
    public decimal? Total { get; set; }
    public string Status { get; set; } = "Ativa";
    public string? Observacoes { get; set; }
}
