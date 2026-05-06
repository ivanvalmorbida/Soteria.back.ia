using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaCadastro.API.DTOs;
using SistemaCadastro.API.Services;

namespace SistemaCadastro.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class HospedagemController : ControllerBase
{
    private readonly IHospedagemService _service;
    private readonly ILogger<HospedagemController> _logger;

    public HospedagemController(IHospedagemService service, ILogger<HospedagemController> logger)
    {
        _service = service;
        _logger = logger;
    }

    [HttpGet]
    [Authorize(Policy = "QualquerAutenticado")]
    [ProducesResponseType(typeof(IEnumerable<HospedagemDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<HospedagemDto>>> GetAll()
    {
        try
        {
            var hospedagens = await _service.GetAllAsync();
            return Ok(hospedagens);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao listar hospedagens");
            return StatusCode(500, new { message = "Erro ao listar hospedagens" });
        }
    }

    [HttpGet("{codigo}")]
    [Authorize(Policy = "QualquerAutenticado")]
    [ProducesResponseType(typeof(HospedagemDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<HospedagemDto>> GetById(int codigo)
    {
        try
        {
            var hospedagem = await _service.GetByIdAsync(codigo);

            if (hospedagem == null)
                return NotFound(new { message = "Hospedagem não encontrada" });

            return Ok(hospedagem);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar hospedagem {Codigo}", codigo);
            return StatusCode(500, new { message = "Erro ao buscar hospedagem" });
        }
    }

    [HttpGet("pessoa/{pessoaId}")]
    [Authorize(Policy = "QualquerAutenticado")]
    [ProducesResponseType(typeof(IEnumerable<HospedagemDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<HospedagemDto>>> GetByPessoa(int pessoaId)
    {
        try
        {
            var hospedagens = await _service.GetByPessoaAsync(pessoaId);
            return Ok(hospedagens);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar hospedagens por pessoa {PessoaId}", pessoaId);
            return StatusCode(500, new { message = "Erro ao buscar hospedagens" });
        }
    }

    [HttpGet("status/{status}")]
    [Authorize(Policy = "QualquerAutenticado")]
    [ProducesResponseType(typeof(IEnumerable<HospedagemDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<HospedagemDto>>> GetByStatus(string status)
    {
        try
        {
            var hospedagens = await _service.GetByStatusAsync(status);
            return Ok(hospedagens);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar hospedagens por status {Status}", status);
            return StatusCode(500, new { message = "Erro ao buscar hospedagens" });
        }
    }

    [HttpPost]
    [Authorize(Policy = "UsuarioOuSuperior")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult> Create([FromBody] HospedagemCreateDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var codigo = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { codigo }, new { codigo, message = "Hospedagem criada com sucesso" });
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Dados inválidos ao criar hospedagem");
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar hospedagem");
            return StatusCode(500, new { message = "Erro ao criar hospedagem" });
        }
    }

    [HttpPut("{codigo}")]
    [Authorize(Policy = "UsuarioOuSuperior")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult> Update(int codigo, [FromBody] HospedagemUpdateDto dto)
    {
        try
        {
            if (codigo != dto.Codigo)
                return BadRequest(new { message = "Código informado não corresponde ao código da URL" });

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var success = await _service.UpdateAsync(dto);

            if (!success)
                return NotFound(new { message = "Hospedagem não encontrada" });

            return Ok(new { message = "Hospedagem atualizada com sucesso" });
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Dados inválidos ao atualizar hospedagem {Codigo}", codigo);
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao atualizar hospedagem {Codigo}", codigo);
            return StatusCode(500, new { message = "Erro ao atualizar hospedagem" });
        }
    }

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
                return NotFound(new { message = "Hospedagem não encontrada" });

            return Ok(new { message = "Hospedagem excluída com sucesso" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao excluir hospedagem {Codigo}", codigo);
            return StatusCode(500, new { message = "Erro ao excluir hospedagem" });
        }
    }
}
