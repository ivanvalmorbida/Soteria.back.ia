namespace SistemaCadastro.API.DTOs;

public class PessoaFisicaCreateDto
{
    // Dados Pessoais
    public string Nome { get; set; } = string.Empty;
    public string? Cpf { get; set; }
    public string? Identidade { get; set; }
    public string? OrgaoIdentidade { get; set; }
    public int? UfIdentidade { get; set; }
    public DateTime? Nascimento { get; set; }
    public char? Sexo { get; set; }
    public int? EstadoCivil { get; set; }
    public int? Nacionalidade { get; set; }
    public int? Profissao { get; set; }
    public string? Ctps { get; set; }
    public string? Pis { get; set; }
    public int? CidadeNasc { get; set; }
    public int? UfNasc { get; set; }
    public int? Conjuge { get; set; }

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

public class PessoaFisicaUpdateDto
{
    public int Codigo { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string? Cpf { get; set; }
    public string? Identidade { get; set; }
    public string? OrgaoIdentidade { get; set; }
    public int? UfIdentidade { get; set; }
    public DateTime? Nascimento { get; set; }
    public char? Sexo { get; set; }
    public int? EstadoCivil { get; set; }
    public int? Nacionalidade { get; set; }
    public int? Profissao { get; set; }
    public string? Ctps { get; set; }
    public string? Pis { get; set; }
    public int? CidadeNasc { get; set; }
    public int? UfNasc { get; set; }
    public int? Conjuge { get; set; }
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

public class PessoaFisicaDto
{
    public int Codigo { get; set; }
    public string? Nome { get; set; }
    public string? Cpf { get; set; }
    public string? Identidade { get; set; }
    public string? OrgaoIdentidade { get; set; }
    public int? UfIdentidade { get; set; }
    public DateTime? Nascimento { get; set; }
    public char? Sexo { get; set; }
    public int? EstadoCivil { get; set; }
    public int? Nacionalidade { get; set; }
    public int? Profissao { get; set; }
    public string? Ctps { get; set; }
    public string? Pis { get; set; }
    public int? CidadeNasc { get; set; }
    public int? UfNasc { get; set; }
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
