using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaCadastro.API.DTOs;
using SistemaCadastro.API.Services;

namespace SistemaCadastro.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CentroCustoController : ControllerBase
{
    private readonly ICentroCustoService _service;
    private readonly ILogger<CentroCustoController> _logger;

    public CentroCustoController(ICentroCustoService service, ILogger<CentroCustoController> logger)
    {
        _service = service;
        _logger = logger;
    }

    /// <summary>
    /// Lista todos os centros de custo
    /// Permissões: Todos os usuários autenticados
    /// </summary>
    [HttpGet]
    [Authorize(Policy = "QualquerAutenticado")]
    [ProducesResponseType(typeof(IEnumerable<CentroCustoDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<CentroCustoDto>>> GetAll()
    {
        try
        {
            var centros = await _service.GetAllAsync();
            return Ok(centros);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao listar centros de custo");
            return StatusCode(500, new { message = "Erro ao listar centros de custo" });
        }
    }

    /// <summary>
    /// Busca um centro de custo por código
    /// Permissões: Todos os usuários autenticados
    /// </summary>
    [HttpGet("{codigo}")]
    [Authorize(Policy = "QualquerAutenticado")]
    [ProducesResponseType(typeof(CentroCustoDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CentroCustoDto>> GetById(int codigo)
    {
        try
        {
            var centro = await _service.GetByIdAsync(codigo);

            if (centro == null)
                return NotFound(new { message = "Centro de custo não encontrado" });

            return Ok(centro);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar centro de custo {Codigo}", codigo);
            return StatusCode(500, new { message = "Erro ao buscar centro de custo" });
        }
    }

    /// <summary>
    /// Busca centros de custo pela descrição (LIKE)
    /// Permissões: Todos os usuários autenticados
    /// </summary>
    [HttpGet("descricao/{descricao}")]
    [Authorize(Policy = "QualquerAutenticado")]
    [ProducesResponseType(typeof(IEnumerable<CentroCustoDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<CentroCustoDto>>> GetByDescricao(string descricao)
    {
        try
        {
            var centros = await _service.GetByDescricaoAsync(descricao);
            return Ok(centros);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar centros de custo por descrição {Descricao}", descricao);
            return StatusCode(500, new { message = "Erro ao buscar centros de custo" });
        }
    }

    /// <summary>
    /// Cria um novo centro de custo
    /// Permissões: Apenas Administradores e Usuários
    /// </summary>
    [HttpPost]
    [Authorize(Policy = "UsuarioOuSuperior")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult> Create([FromBody] CentroCustoCreateDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var codigo = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { codigo }, new { codigo, message = "Centro de custo criado com sucesso" });
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Dados inválidos ao criar centro de custo");
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar centro de custo");
            return StatusCode(500, new { message = "Erro ao criar centro de custo" });
        }
    }

    /// <summary>
    /// Atualiza um centro de custo
    /// Permissões: Apenas Administradores e Usuários
    /// </summary>
    [HttpPut("{codigo}")]
    [Authorize(Policy = "UsuarioOuSuperior")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult> Update(int codigo, [FromBody] CentroCustoUpdateDto dto)
    {
        try
        {
            if (codigo != dto.Codigo)
                return BadRequest(new { message = "Código informado não corresponde ao código da URL" });

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var success = await _service.UpdateAsync(dto);

            if (!success)
                return NotFound(new { message = "Centro de custo não encontrado" });

            return Ok(new { message = "Centro de custo atualizado com sucesso" });
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Dados inválidos ao atualizar centro de custo {Codigo}", codigo);
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao atualizar centro de custo {Codigo}", codigo);
            return StatusCode(500, new { message = "Erro ao atualizar centro de custo" });
        }
    }

    /// <summary>
    /// Exclui um centro de custo
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
                return NotFound(new { message = "Centro de custo não encontrado" });

            return Ok(new { message = "Centro de custo excluído com sucesso" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao excluir centro de custo {Codigo}", codigo);
            return StatusCode(500, new { message = "Erro ao excluir centro de custo" });
        }
    }
}
