using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaCadastro.API.DTOs;
using SistemaCadastro.API.Services;

namespace SistemaCadastro.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class HistoricoContabilController : ControllerBase
{
    private readonly IHistoricoContabilService _service;
    private readonly ILogger<HistoricoContabilController> _logger;

    public HistoricoContabilController(IHistoricoContabilService service, ILogger<HistoricoContabilController> logger)
    {
        _service = service;
        _logger = logger;
    }

    /// <summary>
    /// Lista todos os históricos contábeis
    /// Permissões: Todos os usuários autenticados
    /// </summary>
    [HttpGet]
    [Authorize(Policy = "QualquerAutenticado")]
    [ProducesResponseType(typeof(IEnumerable<HistoricoContabilDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<HistoricoContabilDto>>> GetAll()
    {
        try
        {
            var historicos = await _service.GetAllAsync();
            return Ok(historicos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao listar históricos contábeis");
            return StatusCode(500, new { message = "Erro ao listar históricos contábeis" });
        }
    }

    /// <summary>
    /// Busca um histórico contábil por código
    /// Permissões: Todos os usuários autenticados
    /// </summary>
    [HttpGet("{codigo}")]
    [Authorize(Policy = "QualquerAutenticado")]
    [ProducesResponseType(typeof(HistoricoContabilDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<HistoricoContabilDto>> GetById(int codigo)
    {
        try
        {
            var historico = await _service.GetByIdAsync(codigo);

            if (historico == null)
                return NotFound(new { message = "Histórico contábil não encontrado" });

            return Ok(historico);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar histórico contábil {Codigo}", codigo);
            return StatusCode(500, new { message = "Erro ao buscar histórico contábil" });
        }
    }

    /// <summary>
    /// Busca históricos contábeis pela descrição (LIKE)
    /// Permissões: Todos os usuários autenticados
    /// </summary>
    [HttpGet("descricao/{descricao}")]
    [Authorize(Policy = "QualquerAutenticado")]
    [ProducesResponseType(typeof(IEnumerable<HistoricoContabilDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<HistoricoContabilDto>>> GetByDescricao(string descricao)
    {
        try
        {
            var historicos = await _service.GetByDescricaoAsync(descricao);
            return Ok(historicos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar históricos contábeis por descrição {Descricao}", descricao);
            return StatusCode(500, new { message = "Erro ao buscar históricos contábeis" });
        }
    }

    /// <summary>
    /// Cria um novo histórico contábil
    /// Permissões: Apenas Administradores e Usuários
    /// </summary>
    [HttpPost]
    [Authorize(Policy = "UsuarioOuSuperior")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult> Create([FromBody] HistoricoContabilCreateDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var codigo = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { codigo }, new { codigo, message = "Histórico contábil criado com sucesso" });
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Dados inválidos ao criar histórico contábil");
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar histórico contábil");
            return StatusCode(500, new { message = "Erro ao criar histórico contábil" });
        }
    }

    /// <summary>
    /// Atualiza um histórico contábil
    /// Permissões: Apenas Administradores e Usuários
    /// </summary>
    [HttpPut("{codigo}")]
    [Authorize(Policy = "UsuarioOuSuperior")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult> Update(int codigo, [FromBody] HistoricoContabilUpdateDto dto)
    {
        try
        {
            if (codigo != dto.Codigo)
                return BadRequest(new { message = "Código informado não corresponde ao código da URL" });

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var success = await _service.UpdateAsync(dto);

            if (!success)
                return NotFound(new { message = "Histórico contábil não encontrado" });

            return Ok(new { message = "Histórico contábil atualizado com sucesso" });
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Dados inválidos ao atualizar histórico contábil {Codigo}", codigo);
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao atualizar histórico contábil {Codigo}", codigo);
            return StatusCode(500, new { message = "Erro ao atualizar histórico contábil" });
        }
    }

    /// <summary>
    /// Exclui um histórico contábil
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
                return NotFound(new { message = "Histórico contábil não encontrado" });

            return Ok(new { message = "Histórico contábil excluído com sucesso" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao excluir histórico contábil {Codigo}", codigo);
            return StatusCode(500, new { message = "Erro ao excluir histórico contábil" });
        }
    }
}
