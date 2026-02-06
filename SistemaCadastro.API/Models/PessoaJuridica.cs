namespace SistemaCadastro.API.Models;

public class PessoaJuridica
{
    public int Pessoa { get; set; }
    public string? RazaoSocial { get; set; }
    public string? Cnpj { get; set; }
    public string? InscricaoEstadual { get; set; }
    public int? Atividade { get; set; }
    public string? Homepage { get; set; }
    public int? Representante { get; set; }
}
