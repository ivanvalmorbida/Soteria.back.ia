namespace SistemaCadastro.API.DTOs;

public class PessoaJuridicaCreateDto
{
    // Dados da Empresa
    public string RazaoSocial { get; set; } = string.Empty;
    public string Nome { get; set; } = string.Empty;
    public string? Cnpj { get; set; }
    public string? InscricaoEstadual { get; set; }
    public int? Atividade { get; set; }
    public string? Homepage { get; set; }
    public int? Representante { get; set; }

    // Endereço
    public string? Cep { get; set; }
    public int? Estado { get; set; }
    public int? Cidade { get; set; }
    public string? Bairro { get; set; }
    public string? Endereco { get; set; }
    public string? Numero { get; set; }
    public string? Complemento { get; set; }

    // Contatos
    public List<TelefoneCreateDto>? Telefones { get; set; }
    public List<EnderecoEletronicoDto>? EnderecosEletronicos { get; set; }

    // Observações
    public string? Obs { get; set; }
}

public class PessoaJuridicaUpdateDto
{
    public int Codigo { get; set; }
    public string RazaoSocial { get; set; } = string.Empty;
    public string Nome { get; set; } = string.Empty;
    public string? Cnpj { get; set; }
    public string? InscricaoEstadual { get; set; }
    public int? Atividade { get; set; }
    public string? Homepage { get; set; }
    public int? Representante { get; set; }
    public string? Cep { get; set; }
    public int? Estado { get; set; }
    public int? Cidade { get; set; }
    public string? Bairro { get; set; }
    public string? Endereco { get; set; }
    public string? Numero { get; set; }
    public string? Complemento { get; set; }
    public List<TelefoneCreateDto>? Telefones { get; set; }
    public List<EnderecoEletronicoDto>? EnderecosEletronicos { get; set; }
    public string? Obs { get; set; }
}

public class PessoaJuridicaDto
{
    public int Codigo { get; set; }
    public string? RazaoSocial { get; set; }
    public string? Nome { get; set; }
    public string? Cnpj { get; set; }
    public string? InscricaoEstadual { get; set; }
    public int? Atividade { get; set; }
    public string? AtividadeDescricao { get; set; }
    public string? Homepage { get; set; }
    public int? Representante { get; set; }
    public string? RepresentanteNome { get; set; }
    public string? Cep { get; set; }
    public int? Estado { get; set; }
    public string? EstadoNome { get; set; }
    public int? Cidade { get; set; }
    public string? CidadeNome { get; set; }
    public string? BairroNome { get; set; }
    public string? EnderecoNome { get; set; }
    public string? Numero { get; set; }
    public string? Complemento { get; set; }
    public List<TelefoneDto>? Telefones { get; set; }
    public List<EnderecoEletronicoDto>? EnderecosEletronicos { get; set; }
    public string? Obs { get; set; }
    public DateTime? Cadastro { get; set; }
}
