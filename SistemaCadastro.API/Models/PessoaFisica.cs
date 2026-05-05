namespace SistemaCadastro.API.Models;

public class PessoaFisica
{
    public int Pessoa { get; set; }
    public DateTime? Nascimento { get; set; }
    public int? CidadeNasc { get; set; }
    public int? UfNasc { get; set; }
    public int? Nacionalidade { get; set; }
    public char? Sexo { get; set; }
    public string? Cpf { get; set; }
    public int? EstadoCivil { get; set; }
    public int? Conjuge { get; set; }
    public int? Profissao { get; set; }
}
