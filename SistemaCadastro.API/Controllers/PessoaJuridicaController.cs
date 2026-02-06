using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using SistemaCadastro.API.DTOs;
using SistemaCadastro.API.Services;

namespace SistemaCadastro.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize] // Requer autenticação
public class PessoaJuridicaController : ControllerBase
{
    private readonly IPessoaJuridicaService _service;
    private readonly ILogger<PessoaJuridicaController> _logger;

    public PessoaJuridicaController(IPessoaJuridicaService service, ILogger<PessoaJuridicaController> logger)
    {
        _service = service;
        _logger = logger;
    }

    /// <summary>
    /// Lista todas as pessoas jurídicas cadastradas
    /// Permissões: Todos os usuários autenticados
    /// </summary>
    [HttpGet]
    [Authorize(Policy = "QualquerAutenticado")]
    [ProducesResponseType(typeof(IEnumerable<PessoaJuridicaDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<PessoaJuridicaDto>>> GetAll()
    {
        try
        {
            var pessoas = await _service.GetAllAsync();
            return Ok(pessoas);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao listar pessoas jurídicas");
            return StatusCode(500, new { message = "Erro ao listar pessoas jurídicas" });
        }
    }

    /// <summary>
    /// Busca uma pessoa jurídica por código
    /// Permissões: Todos os usuários autenticados
    /// </summary>
    [HttpGet("{codigo}")]
    [Authorize(Policy = "QualquerAutenticado")]
    [ProducesResponseType(typeof(PessoaJuridicaDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PessoaJuridicaDto>> GetById(int codigo)
    {
        try
        {
            var pessoa = await _service.GetByIdAsync(codigo);
            
            if (pessoa == null)
                return NotFound(new { message = "Pessoa jurídica não encontrada" });

            return Ok(pessoa);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar pessoa jurídica {Codigo}", codigo);
            return StatusCode(500, new { message = "Erro ao buscar pessoa jurídica" });
        }
    }

    /// <summary>
    /// Cria um novo cadastro de pessoa jurídica
    /// Permissões: Apenas Administradores e Usuários
    /// </summary>
    [HttpPost]
    [Authorize(Policy = "UsuarioOuSuperior")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult> Create([FromBody] PessoaJuridicaCreateDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var codigo = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { codigo }, new { codigo, message = "Pessoa jurídica criada com sucesso" });
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Dados inválidos ao criar pessoa jurídica");
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar pessoa jurídica");
            return StatusCode(500, new { message = "Erro ao criar pessoa jurídica" });
        }
    }

    /// <summary>
    /// Atualiza um cadastro de pessoa jurídica
    /// Permissões: Apenas Administradores e Usuários
    /// </summary>
    [HttpPut("{codigo}")]
    [Authorize(Policy = "UsuarioOuSuperior")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult> Update(int codigo, [FromBody] PessoaJuridicaUpdateDto dto)
    {
        try
        {
            if (codigo != dto.Codigo)
                return BadRequest(new { message = "Código informado não corresponde ao código da URL" });

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var success = await _service.UpdateAsync(dto);
            
            if (!success)
                return NotFound(new { message = "Pessoa jurídica não encontrada" });

            return Ok(new { message = "Pessoa jurídica atualizada com sucesso" });
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Dados inválidos ao atualizar pessoa jurídica {Codigo}", codigo);
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao atualizar pessoa jurídica {Codigo}", codigo);
            return StatusCode(500, new { message = "Erro ao atualizar pessoa jurídica" });
        }
    }
}
