using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaCadastro.API.DTOs;
using SistemaCadastro.API.Services;

namespace SistemaCadastro.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PlanoContaController : ControllerBase
{
    private readonly IPlanoContaService _service;
    private readonly ILogger<PlanoContaController> _logger;

    public PlanoContaController(IPlanoContaService service, ILogger<PlanoContaController> logger)
    {
        _service = service;
        _logger = logger;
    }

    /// <summary>
    /// Lista todos os planos de conta
    /// Permissões: Todos os usuários autenticados
    /// </summary>
    [HttpGet]
    [Authorize(Policy = "QualquerAutenticado")]
    [ProducesResponseType(typeof(IEnumerable<PlanoContaDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<PlanoContaDto>>> GetAll()
    {
        try
        {
            var planos = await _service.GetAllAsync();
            return Ok(planos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao listar planos de conta");
            return StatusCode(500, new { message = "Erro ao listar planos de conta" });
        }
    }

    /// <summary>
    /// Busca um plano de conta por código
    /// Permissões: Todos os usuários autenticados
    /// </summary>
    [HttpGet("{codigo}")]
    [Authorize(Policy = "QualquerAutenticado")]
    [ProducesResponseType(typeof(PlanoContaDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PlanoContaDto>> GetById(int codigo)
    {
        try
        {
            var plano = await _service.GetByIdAsync(codigo);

            if (plano == null)
                return NotFound(new { message = "Plano de conta não encontrado" });

            return Ok(plano);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar plano de conta {Codigo}", codigo);
            return StatusCode(500, new { message = "Erro ao buscar plano de conta" });
        }
    }

    /// <summary>
    /// Busca planos de conta por tipo
    /// Permissões: Todos os usuários autenticados
    /// </summary>
    [HttpGet("tipo/{tipo}")]
    [Authorize(Policy = "QualquerAutenticado")]
    [ProducesResponseType(typeof(IEnumerable<PlanoContaDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<PlanoContaDto>>> GetByTipo(string tipo)
    {
        try
        {
            var planos = await _service.GetByTipoAsync(tipo);
            return Ok(planos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar planos de conta por tipo {Tipo}", tipo);
            return StatusCode(500, new { message = "Erro ao buscar planos de conta" });
        }
    }

    /// <summary>
    /// Busca planos de conta pela descrição (LIKE)
    /// Permissões: Todos os usuários autenticados
    /// </summary>
    [HttpGet("descricao/{descricao}")]
    [Authorize(Policy = "QualquerAutenticado")]
    [ProducesResponseType(typeof(IEnumerable<PlanoContaDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<PlanoContaDto>>> GetByDescricao(string descricao)
    {
        try
        {
            var planos = await _service.GetByDescricaoAsync(descricao);
            return Ok(planos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar planos de conta por descrição {Descricao}", descricao);
            return StatusCode(500, new { message = "Erro ao buscar planos de conta" });
        }
    }

    /// <summary>
    /// Cria um novo plano de conta
    /// Permissões: Apenas Administradores e Usuários
    /// </summary>
    [HttpPost]
    [Authorize(Policy = "UsuarioOuSuperior")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult> Create([FromBody] PlanoContaCreateDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var codigo = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { codigo }, new { codigo, message = "Plano de conta criado com sucesso" });
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Dados inválidos ao criar plano de conta");
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar plano de conta");
            return StatusCode(500, new { message = "Erro ao criar plano de conta" });
        }
    }

    /// <summary>
    /// Atualiza um plano de conta
    /// Permissões: Apenas Administradores e Usuários
    /// </summary>
    [HttpPut("{codigo}")]
    [Authorize(Policy = "UsuarioOuSuperior")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult> Update(int codigo, [FromBody] PlanoContaUpdateDto dto)
    {
        try
        {
            if (codigo != dto.Codigo)
                return BadRequest(new { message = "Código informado não corresponde ao código da URL" });

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var success = await _service.UpdateAsync(dto);

            if (!success)
                return NotFound(new { message = "Plano de conta não encontrado" });

            return Ok(new { message = "Plano de conta atualizado com sucesso" });
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Dados inválidos ao atualizar plano de conta {Codigo}", codigo);
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao atualizar plano de conta {Codigo}", codigo);
            return StatusCode(500, new { message = "Erro ao atualizar plano de conta" });
        }
    }

    /// <summary>
    /// Exclui um plano de conta
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
                return NotFound(new { message = "Plano de conta não encontrado" });

            return Ok(new { message = "Plano de conta excluído com sucesso" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao excluir plano de conta {Codigo}", codigo);
            return StatusCode(500, new { message = "Erro ao excluir plano de conta" });
        }
    }
}
