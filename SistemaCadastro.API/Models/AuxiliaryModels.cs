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
    public int? Bairro { get; set; }
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
