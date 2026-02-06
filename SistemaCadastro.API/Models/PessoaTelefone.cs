namespace SistemaCadastro.API.Models;

/// <summary>
/// Representa um telefone de uma pessoa
/// </summary>
public class PessoaTelefone
{
    public int Codigo { get; set; }
    public int Pessoa { get; set; }
    public byte Tipo { get; set; }
    public long? Telefone { get; set; }
    public string? Descricao { get; set; }
}

/// <summary>
/// Tipos de telefone
/// </summary>
public static class TipoTelefone
{
    public const byte Celular = 1;
    public const byte Fixo = 2;
    public const byte WhatsApp = 3;
    public const byte Telegram = 4;
    public const byte Comercial = 5;
    public const byte Residencial = 6;
    public const byte Recado = 7;
    public const byte Fax = 8;
    public const byte Outro = 99;

    public static string ObterDescricao(byte tipo)
    {
        return tipo switch
        {
            Celular => "Celular",
            Fixo => "Fixo",
            WhatsApp => "WhatsApp",
            Telegram => "Telegram",
            Comercial => "Comercial",
            Residencial => "Residencial",
            Recado => "Recado",
            Fax => "Fax",
            Outro => "Outro",
            _ => "Desconhecido"
        };
    }

    public static string ObterIcone(byte tipo)
    {
        return tipo switch
        {
            Celular => "📱",
            Fixo => "☎️",
            WhatsApp => "💬",
            Telegram => "✈️",
            Comercial => "🏢",
            Residencial => "🏠",
            Recado => "📞",
            Fax => "📠",
            _ => "📞"
        };
    }

    /// <summary>
    /// Formata o número de telefone
    /// </summary>
    public static string FormatarTelefone(long? telefone)
    {
        if (!telefone.HasValue) return string.Empty;

        var tel = telefone.Value.ToString();
        
        // Remove qualquer formatação prévia
        tel = new string(tel.Where(char.IsDigit).ToArray());

        // Celular: (XX) 9XXXX-XXXX
        if (tel.Length == 11)
        {
            return $"({tel.Substring(0, 2)}) {tel.Substring(2, 5)}-{tel.Substring(7, 4)}";
        }
        // Fixo: (XX) XXXX-XXXX
        else if (tel.Length == 10)
        {
            return $"({tel.Substring(0, 2)}) {tel.Substring(2, 4)}-{tel.Substring(6, 4)}";
        }
        // DDI + Celular: +XX (XX) 9XXXX-XXXX
        else if (tel.Length == 13)
        {
            return $"+{tel.Substring(0, 2)} ({tel.Substring(2, 2)}) {tel.Substring(4, 5)}-{tel.Substring(9, 4)}";
        }
        // DDI + Fixo: +XX (XX) XXXX-XXXX
        else if (tel.Length == 12)
        {
            return $"+{tel.Substring(0, 2)} ({tel.Substring(2, 2)}) {tel.Substring(4, 4)}-{tel.Substring(8, 4)}";
        }
        
        return tel;
    }

    /// <summary>
    /// Remove formatação do telefone e retorna apenas dígitos
    /// </summary>
    public static long? ParseTelefone(string? telefone)
    {
        if (string.IsNullOrWhiteSpace(telefone)) return null;

        var digitos = new string(telefone.Where(char.IsDigit).ToArray());
        
        if (string.IsNullOrEmpty(digitos)) return null;
        
        return long.TryParse(digitos, out var result) ? result : null;
    }
}
