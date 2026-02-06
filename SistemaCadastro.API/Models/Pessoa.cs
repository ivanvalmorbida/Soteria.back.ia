namespace SistemaCadastro.API.Models;

public class Pessoa
{
    public int Codigo { get; set; }
    public char? Tipo { get; set; }
    public string? Nome { get; set; }
    public string? Cep { get; set; }
    public int? Estado { get; set; }
    public int? Cidade { get; set; }
    public int? Bairro { get; set; }
    public int? Endereco { get; set; }
    public string? Numero { get; set; }
    public string? Complemento { get; set; }
    public string? Obs { get; set; }
    public DateTime? Cadastro { get; set; }
}
