namespace SistemaCadastro.API.Models;

public class Estado
{
    public int Codigo { get; set; }
    public string? Sigla { get; set; }
    public string Nome { get; set; } = string.Empty;
}

public class Cidade
{
    public int Codigo { get; set; }
    public string? Nome { get; set; }
}

public class Bairro
{
    public int Codigo { get; set; }
    public string? Nome { get; set; }
}

public class Endereco
{
    public int Codigo { get; set; }
    public string? Nome { get; set; }
}

public class Cep
{
    public string CepCodigo { get; set; } = string.Empty;
    public string? Complemento { get; set; }
    public int? Endereco { get; set; }
    public string? EnderecoNome { get; set; }
    public int? Bairro { get; set; }
    public string? BairroNome { get; set; }
    public int? Cidade { get; set; }
    public int? Estado { get; set; }
}

public class EstadoCivil
{
    public int Codigo { get; set; }
    public string? Descricao { get; set; }
}

public class Nacionalidade
{
    public int Codigo { get; set; }
    public string? Pais { get; set; }
    public string? Capital { get; set; }
    public string? Moeda { get; set; }
    public string? Idioma { get; set; }
}

public class CBO
{
    public string? Codigo { get; set; }
    public string? Descricao { get; set; }
}

public class AtividadeEconomica
{
    public int Codigo { get; set; }
    public int? Setor { get; set; }
    public int? Subsetor { get; set; }
    public string? Atividade { get; set; }
    public string? Descricao { get; set; }
}

public class AtividadeEconomicaSubsetor
{
    public int Codigo { get; set; }
    public int? Setor { get; set; }
    public string? Subsetor { get; set; }
    public string? Descricao { get; set; }
}

public class CentroCusto
{
    public int Codigo { get; set; }
    public string Descricao { get; set; } = string.Empty;
}

public class HistoricoContabil
{
    public int Codigo { get; set; }
    public string Descricao { get; set; } = string.Empty;
}

public class LanctoContabil
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

public class PlanoConta
{
    public int Codigo { get; set; }
    public int? CodigoPai { get; set; }
    public string Tipo { get; set; } = string.Empty;
    public string Rotulo { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
}
