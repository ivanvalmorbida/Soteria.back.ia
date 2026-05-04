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
            new { Codigo = TipoEnderecoEletronico.Email, Descricao = TipoEnderecoEletronico.ObterDescricao(TipoEnderecoEletronico.Email), Icone = "📧" },
            new { Codigo = TipoEnderecoEletronico.Site, Descricao = TipoEnderecoEletronico.ObterDescricao(TipoEnderecoEletronico.Site), Icone = "🌐" },
            new { Codigo = TipoEnderecoEletronico.Facebook, Descricao = TipoEnderecoEletronico.ObterDescricao(TipoEnderecoEletronico.Facebook), Icone = "📘" },
            new { Codigo = TipoEnderecoEletronico.Instagram, Descricao = TipoEnderecoEletronico.ObterDescricao(TipoEnderecoEletronico.Instagram), Icone = "📷" },
            new { Codigo = TipoEnderecoEletronico.LinkedIn, Descricao = TipoEnderecoEletronico.ObterDescricao(TipoEnderecoEletronico.LinkedIn), Icone = "💼"},
            new { Codigo = TipoEnderecoEletronico.Twitter, Descricao = TipoEnderecoEletronico.ObterDescricao(TipoEnderecoEletronico.Twitter), Icone = "🐦" },
            new { Codigo = TipoEnderecoEletronico.YouTube, Descricao = TipoEnderecoEletronico.ObterDescricao(TipoEnderecoEletronico.YouTube), Icone = "📺" },
            new { Codigo = TipoEnderecoEletronico.TikTok, Descricao = TipoEnderecoEletronico.ObterDescricao(TipoEnderecoEletronico.TikTok), Icone = "🎵" },
            new { Codigo = TipoEnderecoEletronico.GitHub, Descricao = TipoEnderecoEletronico.ObterDescricao(TipoEnderecoEletronico.GitHub), Icone = "💻" },
            new { Codigo = TipoEnderecoEletronico.Outro, Descricao = TipoEnderecoEletronico.ObterDescricao(TipoEnderecoEletronico.Outro), Icone = "🔗" }
        };

        return Ok(tipos);
    }
}