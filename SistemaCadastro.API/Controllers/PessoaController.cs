using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using SistemaCadastro.API.Models;
using SistemaCadastro.API.Services;

namespace SistemaCadastro.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize] // Requer autenticação
public class PessoaController : ControllerBase
{
    private readonly IPessoaService _service;
    private readonly ILogger<PessoaController> _logger;

    public PessoaController(IPessoaService service, ILogger<PessoaController> logger)
    {
        _service = service;
        _logger = logger;
    }

    /// <summary>
    /// Lista todas as pessoas (físicas e jurídicas)
    /// Permissões: Todos os usuários autenticados
    /// </summary>
    [HttpGet]
    [Authorize(Policy = "QualquerAutenticado")]
    [ProducesResponseType(typeof(IEnumerable<Pessoa>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<Pessoa>>> GetAll()
    {
        try
        {
            var pessoas = await _service.GetAllAsync();
            return Ok(pessoas);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao listar pessoas");
            return StatusCode(500, new { message = "Erro ao listar pessoas" });
        }
    }

    /// <summary>
    /// Busca uma pessoa por código
    /// Permissões: Todos os usuários autenticados
    /// </summary>
    [HttpGet("{codigo}")]
    [Authorize(Policy = "QualquerAutenticado")]
    [ProducesResponseType(typeof(Pessoa), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Pessoa>> GetById(int codigo)
    {
        try
        {
            var pessoa = await _service.GetByIdAsync(codigo);
            
            if (pessoa == null)
                return NotFound(new { message = "Pessoa não encontrada" });

            return Ok(pessoa);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar pessoa {Codigo}", codigo);
            return StatusCode(500, new { message = "Erro ao buscar pessoa" });
        }
    }

    /// <summary>
    /// Pesquisa pessoas por termo (nome, CPF, CNPJ)
    /// Permissões: Todos os usuários autenticados
    /// </summary>
    [HttpGet("search")]
    [Authorize(Policy = "QualquerAutenticado")]
    [ProducesResponseType(typeof(IEnumerable<Pessoa>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<Pessoa>>> Search([FromQuery] string termo)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(termo))
                return BadRequest(new { message = "Termo de busca não pode ser vazio" });

            var pessoas = await _service.SearchAsync(termo);
            return Ok(pessoas);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao pesquisar pessoas com termo: {Termo}", termo);
            return StatusCode(500, new { message = "Erro ao pesquisar pessoas" });
        }
    }

    /// <summary>
    /// Exclui uma pessoa (física ou jurídica)
    /// Permissões: APENAS ADMINISTRADORES
    /// </summary>
    [HttpDelete("{codigo}")]
    [Authorize(Policy = "ApenasAdministrador")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult> Delete(int codigo)
    {
        try
        {
            var success = await _service.DeleteAsync(codigo);
            
            if (!success)
                return NotFound(new { message = "Pessoa não encontrada" });

            return Ok(new { message = "Pessoa excluída com sucesso" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao excluir pessoa {Codigo}", codigo);
            return StatusCode(500, new { message = "Erro ao excluir pessoa" });
        }
    }
}
