using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using SistemaCadastro.API.Models;

namespace SistemaCadastro.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Policy = "QualquerAutenticado")]
public class TipoTelefoneController : ControllerBase
{
    /// <summary>
    /// Lista todos os tipos de telefone disponíveis
    /// </summary>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<IEnumerable<object>> GetAll()
    {
        var tipos = new[]
        {
            new { Codigo = TipoTelefone.Celular, Descricao = TipoTelefone.ObterDescricao(TipoTelefone.Celular), Icone = TipoTelefone.ObterIcone(TipoTelefone.Celular) },
            new { Codigo = TipoTelefone.Fixo, Descricao = TipoTelefone.ObterDescricao(TipoTelefone.Fixo), Icone = TipoTelefone.ObterIcone(TipoTelefone.Fixo) },
            new { Codigo = TipoTelefone.WhatsApp, Descricao = TipoTelefone.ObterDescricao(TipoTelefone.WhatsApp), Icone = TipoTelefone.ObterIcone(TipoTelefone.WhatsApp) },
            new { Codigo = TipoTelefone.Telegram, Descricao = TipoTelefone.ObterDescricao(TipoTelefone.Telegram), Icone = TipoTelefone.ObterIcone(TipoTelefone.Telegram) },
            new { Codigo = TipoTelefone.Comercial, Descricao = TipoTelefone.ObterDescricao(TipoTelefone.Comercial), Icone = TipoTelefone.ObterIcone(TipoTelefone.Comercial) },
            new { Codigo = TipoTelefone.Residencial, Descricao = TipoTelefone.ObterDescricao(TipoTelefone.Residencial), Icone = TipoTelefone.ObterIcone(TipoTelefone.Residencial) },
            new { Codigo = TipoTelefone.Recado, Descricao = TipoTelefone.ObterDescricao(TipoTelefone.Recado), Icone = TipoTelefone.ObterIcone(TipoTelefone.Recado) },
            new { Codigo = TipoTelefone.Fax, Descricao = TipoTelefone.ObterDescricao(TipoTelefone.Fax), Icone = TipoTelefone.ObterIcone(TipoTelefone.Fax) },
            new { Codigo = TipoTelefone.Outro, Descricao = TipoTelefone.ObterDescricao(TipoTelefone.Outro), Icone = TipoTelefone.ObterIcone(TipoTelefone.Outro) }
        };

        return Ok(tipos);
    }
}
