using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaCadastro.API.DTOs;
using SistemaCadastro.API.Services;

namespace SistemaCadastro.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class LanctoContabilController : ControllerBase
{
    private readonly ILanctoContabilService _service;
    private readonly ILogger<LanctoContabilController> _logger;

    public LanctoContabilController(ILanctoContabilService service, ILogger<LanctoContabilController> logger)
    {
        _service = service;
        _logger = logger;
    }

    /// <summary>
    /// Lista todos os lançamentos contábeis
    /// Permissões: Todos os usuários autenticados
    /// </summary>
    [HttpGet]
    [Authorize(Policy = "QualquerAutenticado")]
    [ProducesResponseType(typeof(IEnumerable<LanctoContabilDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<LanctoContabilDto>>> GetAll()
    {
        try
        {
            var lanctos = await _service.GetAllAsync();
            return Ok(lanctos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao listar lançamentos contábeis");
            return StatusCode(500, new { message = "Erro ao listar lançamentos contábeis" });
        }
    }

    /// <summary>
    /// Busca um lançamento contábil por código
    /// Permissões: Todos os usuários autenticados
    /// </summary>
    [HttpGet("{codigo}")]
    [Authorize(Policy = "QualquerAutenticado")]
    [ProducesResponseType(typeof(LanctoContabilDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<LanctoContabilDto>> GetById(int codigo)
    {
        try
        {
            var lancto = await _service.GetByIdAsync(codigo);

            if (lancto == null)
                return NotFound(new { message = "Lançamento contábil não encontrado" });

            return Ok(lancto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar lançamento contábil {Codigo}", codigo);
            return StatusCode(500, new { message = "Erro ao buscar lançamento contábil" });
        }
    }

    /// <summary>
    /// Cria um novo lançamento contábil
    /// Permissões: Apenas Administradores e Usuários
    /// </summary>
    [HttpPost]
    [Authorize(Policy = "UsuarioOuSuperior")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult> Create([FromBody] LanctoContabilCreateDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var codigo = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { codigo }, new { codigo, message = "Lançamento contábil criado com sucesso" });
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Dados inválidos ao criar lançamento contábil");
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar lançamento contábil");
            return StatusCode(500, new { message = "Erro ao criar lançamento contábil" });
        }
    }

    /// <summary>
    /// Atualiza um lançamento contábil
    /// Permissões: Apenas Administradores e Usuários
    /// </summary>
    [HttpPut("{codigo}")]
    [Authorize(Policy = "UsuarioOuSuperior")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult> Update(int codigo, [FromBody] LanctoContabilUpdateDto dto)
    {
        try
        {
            if (codigo != dto.Codigo)
                return BadRequest(new { message = "Código informado não corresponde ao código da URL" });

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var success = await _service.UpdateAsync(dto);

            if (!success)
                return NotFound(new { message = "Lançamento contábil não encontrado" });

            return Ok(new { message = "Lançamento contábil atualizado com sucesso" });
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Dados inválidos ao atualizar lançamento contábil {Codigo}", codigo);
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao atualizar lançamento contábil {Codigo}", codigo);
            return StatusCode(500, new { message = "Erro ao atualizar lançamento contábil" });
        }
    }

    /// <summary>
    /// Exclui um lançamento contábil
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
                return NotFound(new { message = "Lançamento contábil não encontrado" });

            return Ok(new { message = "Lançamento contábil excluído com sucesso" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao excluir lançamento contábil {Codigo}", codigo);
            return StatusCode(500, new { message = "Erro ao excluir lançamento contábil" });
        }
    }
}
