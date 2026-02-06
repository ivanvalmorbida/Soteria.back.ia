using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using SistemaCadastro.API.Models;

namespace SistemaCadastro.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Policy = "QualquerAutenticado")]
public class TipoEnderecoEletronicoController : ControllerBase
{
    /// <summary>
    /// Lista todos os tipos de endereços eletrônicos disponíveis
    /// </summary>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<IEnumerable<object>> GetAll()
    {
        var tipos = new[]
        {
            new { Codigo = TipoEnderecoEletronico.Email, Descricao = TipoEnderecoEletronico.ObterDescricao(TipoEnderecoEletronico.Email), Icone = TipoEnderecoEletronico.ObterIcone(TipoEnderecoEletronico.Email) },
            new { Codigo = TipoEnderecoEletronico.Site, Descricao = TipoEnderecoEletronico.ObterDescricao(TipoEnderecoEletronico.Site), Icone = TipoEnderecoEletronico.ObterIcone(TipoEnderecoEletronico.Site) },
            new { Codigo = TipoEnderecoEletronico.Facebook, Descricao = TipoEnderecoEletronico.ObterDescricao(TipoEnderecoEletronico.Facebook), Icone = TipoEnderecoEletronico.ObterIcone(TipoEnderecoEletronico.Facebook) },
            new { Codigo = TipoEnderecoEletronico.Instagram, Descricao = TipoEnderecoEletronico.ObterDescricao(TipoEnderecoEletronico.Instagram), Icone = TipoEnderecoEletronico.ObterIcone(TipoEnderecoEletronico.Instagram) },
            new { Codigo = TipoEnderecoEletronico.LinkedIn, Descricao = TipoEnderecoEletronico.ObterDescricao(TipoEnderecoEletronico.LinkedIn), Icone = TipoEnderecoEletronico.ObterIcone(TipoEnderecoEletronico.LinkedIn) },
            new { Codigo = TipoEnderecoEletronico.Twitter, Descricao = TipoEnderecoEletronico.ObterDescricao(TipoEnderecoEletronico.Twitter), Icone = TipoEnderecoEletronico.ObterIcone(TipoEnderecoEletronico.Twitter) },
            new { Codigo = TipoEnderecoEletronico.WhatsApp, Descricao = TipoEnderecoEletronico.ObterDescricao(TipoEnderecoEletronico.WhatsApp), Icone = TipoEnderecoEletronico.ObterIcone(TipoEnderecoEletronico.WhatsApp) },
            new { Codigo = TipoEnderecoEletronico.Telegram, Descricao = TipoEnderecoEletronico.ObterDescricao(TipoEnderecoEletronico.Telegram), Icone = TipoEnderecoEletronico.ObterIcone(TipoEnderecoEletronico.Telegram) },
            new { Codigo = TipoEnderecoEletronico.YouTube, Descricao = TipoEnderecoEletronico.ObterDescricao(TipoEnderecoEletronico.YouTube), Icone = TipoEnderecoEletronico.ObterIcone(TipoEnderecoEletronico.YouTube) },
            new { Codigo = TipoEnderecoEletronico.TikTok, Descricao = TipoEnderecoEletronico.ObterDescricao(TipoEnderecoEletronico.TikTok), Icone = TipoEnderecoEletronico.ObterIcone(TipoEnderecoEletronico.TikTok) },
            new { Codigo = TipoEnderecoEletronico.GitHub, Descricao = TipoEnderecoEletronico.ObterDescricao(TipoEnderecoEletronico.GitHub), Icone = TipoEnderecoEletronico.ObterIcone(TipoEnderecoEletronico.GitHub) },
            new { Codigo = TipoEnderecoEletronico.Outro, Descricao = TipoEnderecoEletronico.ObterDescricao(TipoEnderecoEletronico.Outro), Icone = TipoEnderecoEletronico.ObterIcone(TipoEnderecoEletronico.Outro) }
        };

        return Ok(tipos);
    }
}
