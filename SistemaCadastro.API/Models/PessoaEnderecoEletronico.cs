namespace SistemaCadastro.API.Models;

/// <summary>
/// Representa um endereço eletrônico de uma pessoa
/// (email, rede social, site, etc)
/// </summary>
public class PessoaEnderecoEletronico
{
    public int Codigo { get; set; }
    public int Pessoa { get; set; }
    public string? Endereco { get; set; }
    public int? Tipo { get; set; }
    public string? Descricao { get; set; }
}

/// <summary>
/// Tipos de endereços eletrônicos
/// </summary>
public static class TipoEnderecoEletronico
{
    public const int Email = 1;
    public const int Site = 2;
    public const int Facebook = 3;
    public const int Instagram = 4;
    public const int LinkedIn = 5;
    public const int Twitter = 6;
    public const int WhatsApp = 7;
    public const int Telegram = 8;
    public const int YouTube = 9;
    public const int TikTok = 10;
    public const int GitHub = 11;
    public const int Outro = 99;

    public static string ObterDescricao(int tipo)
    {
        return tipo switch
        {
            Email => "E-mail",
            Site => "Website",
            Facebook => "Facebook",
            Instagram => "Instagram",
            LinkedIn => "LinkedIn",
            Twitter => "Twitter/X",
            WhatsApp => "WhatsApp",
            Telegram => "Telegram",
            YouTube => "YouTube",
            TikTok => "TikTok",
            GitHub => "GitHub",
            Outro => "Outro",
            _ => "Desconhecido"
        };
    }

    public static string ObterIcone(int tipo)
    {
        return tipo switch
        {
            Email => "📧",
            Site => "🌐",
            Facebook => "📘",
            Instagram => "📷",
            LinkedIn => "💼",
            Twitter => "🐦",
            WhatsApp => "💬",
            Telegram => "✈️",
            YouTube => "📺",
            TikTok => "🎵",
            GitHub => "💻",
            _ => "🔗"
        };
    }
}
