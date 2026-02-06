namespace SistemaCadastro.API.Authorization;

/// <summary>
/// Tipos de usuário do sistema
/// </summary>
public static class Roles
{
    public const string Administrador = "1";
    public const string Usuario = "2";
    public const string Convidado = "3";
    
    // Grupos de permissões
    public const string AdministradorOuUsuario = "1,2";
    public const string Todos = "1,2,3";
}

/// <summary>
/// Políticas de autorização
/// </summary>
public static class Policies
{
    public const string ApenasAdministrador = "ApenasAdministrador";
    public const string UsuarioOuSuperior = "UsuarioOuSuperior";
    public const string QualquerAutenticado = "QualquerAutenticado";
}

/// <summary>
/// Descrição dos tipos de usuário
/// </summary>
public static class RoleDescriptions
{
    public static string GetDescription(string role)
    {
        return role switch
        {
            Roles.Administrador => "Administrador - Acesso total ao sistema",
            Roles.Usuario => "Usuário - Pode criar e editar cadastros",
            Roles.Convidado => "Convidado - Apenas leitura",
            _ => "Desconhecido"
        };
    }
}
