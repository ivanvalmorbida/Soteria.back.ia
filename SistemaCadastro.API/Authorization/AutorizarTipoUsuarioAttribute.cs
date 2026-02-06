using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace SistemaCadastro.API.Authorization;

/// <summary>
/// Atributo para autorização baseada em tipo de usuário
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class AutorizarTipoUsuarioAttribute : Attribute, IAuthorizationFilter
{
    private readonly int[] _tiposPermitidos;

    public AutorizarTipoUsuarioAttribute(params int[] tiposPermitidos)
    {
        _tiposPermitidos = tiposPermitidos;
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        // Verificar se usuário está autenticado
        var user = context.HttpContext.User;
        if (!user.Identity?.IsAuthenticated ?? true)
        {
            context.Result = new UnauthorizedObjectResult(new { message = "Não autenticado" });
            return;
        }

        // Obter tipo do usuário do claim
        var tipoUsuarioClaim = user.FindFirst(ClaimTypes.Role)?.Value;
        
        if (string.IsNullOrEmpty(tipoUsuarioClaim) || !int.TryParse(tipoUsuarioClaim, out int tipoUsuario))
        {
            context.Result = new ForbidResult();
            return;
        }

        // Verificar se o tipo está entre os permitidos
        if (!_tiposPermitidos.Contains(tipoUsuario))
        {
            context.Result = new ObjectResult(new 
            { 
                message = "Acesso negado. Você não tem permissão para realizar esta ação.",
                tipoRequerido = string.Join(", ", _tiposPermitidos.Select(t => ObterNomeTipo(t)))
            })
            {
                StatusCode = 403
            };
        }
    }

    private string ObterNomeTipo(int tipo)
    {
        return tipo switch
        {
            1 => "Administrador",
            2 => "Usuário",
            3 => "Convidado",
            _ => $"Tipo {tipo}"
        };
    }
}

/// <summary>
/// Constantes para tipos de usuário
/// </summary>
public static class TipoUsuario
{
    public const int Administrador = 1;
    public const int Usuario = 2;
    public const int Convidado = 3;
}
