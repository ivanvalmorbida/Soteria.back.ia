namespace SistemaCadastro.API.Utilities;

/// <summary>
/// Validador de CNPJ alfanumérico conforme algoritmo publicado pelo SERPRO.
/// Módulo 11 com pesos de 2 a 9 aplicados da direita para a esquerda.
/// Entrada esperada: 14 caracteres sem pontuação (ex: "12ABC34501DE35").
/// </summary>
public static class CnpjAlfanumericoValidator
{
    /// <summary>
    /// Retorna true quando os dois dígitos verificadores do CNPJ são válidos.
    /// </summary>
    public static bool Validar(string? cnpj)
    {
        if (string.IsNullOrEmpty(cnpj)) return false;

        // Normalizar: sem espaços, maiúsculas, sem pontuação
        cnpj = cnpj.Trim().ToUpperInvariant().Replace(".", "").Replace("/", "").Replace("-", "");

        if (cnpj.Length != 14) return false;

        // Os dois últimos caracteres (DV) devem ser dígitos
        if (!char.IsDigit(cnpj[12]) || !char.IsDigit(cnpj[13])) return false;

        // Todos os caracteres devem ser alfanuméricos
        foreach (var ch in cnpj)
            if (!char.IsLetterOrDigit(ch)) return false;

        int dv1Informado = cnpj[12] - '0';
        int dv2Informado = cnpj[13] - '0';

        // 1º DV – calculado sobre os 12 primeiros caracteres
        int dv1Calculado = CalcularDV(cnpj[..12]);
        if (dv1Calculado != dv1Informado) return false;

        // 2º DV – calculado sobre os 13 primeiros caracteres (12 base + 1º DV)
        int dv2Calculado = CalcularDV(cnpj[..13]);
        return dv2Calculado == dv2Informado;
    }

    /// <summary>
    /// Formata um CNPJ alfanumérico sem pontuação no padrão XX.XXX.XXX/XXXX-DD.
    /// Retorna a string original se o comprimento não for 14.
    /// </summary>
    public static string Formatar(string? cnpj)
    {
        if (string.IsNullOrEmpty(cnpj)) return cnpj ?? "";
        cnpj = cnpj.Replace(".", "").Replace("/", "").Replace("-", "");
        if (cnpj.Length != 14) return cnpj;

        return $"{cnpj[..2]}.{cnpj[2..5]}.{cnpj[5..8]}/{cnpj[8..12]}-{cnpj[12..14]}";
    }

    // ------------------------------------------------------------------
    // Internos
    // ------------------------------------------------------------------

    /// <summary>
    /// Calcula um único dígito verificador sobre a sequência de caracteres fornecida.
    /// Retorna -1 se algum caractere for inválido.
    /// </summary>
    private static int CalcularDV(string caracteres)
    {
        int soma = 0;
        int peso = 2; // começa em 2 pela direita

        // Percorre da direita para a esquerda
        for (int i = caracteres.Length - 1; i >= 0; i--)
        {
            int valor = ValorCaractere(caracteres[i]);
            if (valor < 0) return -1; // caractere não reconhecido

            soma += valor * peso;

            peso++;
            if (peso > 9) peso = 2; // cicla entre 2 e 9
        }

        int resto = soma % 11;
        return (resto == 0 || resto == 1) ? 0 : 11 - resto;
    }

    /// <summary>
    /// Mapeia um caractere alfanumérico para seu valor numérico conforme tabela SERPRO.
    /// Dígitos 0-9  → valor  0-9   (ASCII - 48)
    /// Letras  A-Z  → valor 17-42  (ASCII - 48)
    /// </summary>
    private static int ValorCaractere(char ch)
    {
        ch = char.ToUpperInvariant(ch);

        if (ch is >= '0' and <= '9') return ch - '0';       // 48 → 0 … 57 → 9
        if (ch is >= 'A' and <= 'Z') return ch - 'A' + 17;  // 65 → 17 … 90 → 42  (equivalente a ASCII - 48)
        return -1;
    }
}
