using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using SistemaCadastro.API.DTOs;
using SistemaCadastro.API.Services;

namespace SistemaCadastro.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize] // Requer autenticação
public class PessoaFisicaController : ControllerBase
{
    private readonly IPessoaFisicaService _service;
    private readonly ILogger<PessoaFisicaController> _logger;

    public PessoaFisicaController(IPessoaFisicaService service, ILogger<PessoaFisicaController> logger)
    {
        _service = service;
        _logger = logger;
    }

    /// <summary>
    /// Lista todas as pessoas físicas cadastradas
    /// Permissões: Todos os usuários autenticados (Admin, Usuário, Convidado)
    /// </summary>
    [HttpGet]
    [Authorize(Policy = "QualquerAutenticado")]
    [ProducesResponseType(typeof(IEnumerable<PessoaFisicaDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<PessoaFisicaDto>>> GetAll()
    {
        try
        {
            var pessoas = await _service.GetAllAsync();
            return Ok(pessoas);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao listar pessoas físicas");
            return StatusCode(500, new { message = "Erro ao listar pessoas físicas" });
        }
    }

    /// <summary>
    /// Busca uma pessoa física por código
    /// Permissões: Todos os usuários autenticados
    /// </summary>
    [HttpGet("{codigo}")]
    [Authorize(Policy = "QualquerAutenticado")]
    [ProducesResponseType(typeof(PessoaFisicaDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PessoaFisicaDto>> GetById(int codigo)
    {
        try
        {
            var pessoa = await _service.GetByIdAsync(codigo);
            
            if (pessoa == null)
                return NotFound(new { message = "Pessoa física não encontrada" });

            return Ok(pessoa);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar pessoa física {Codigo}", codigo);
            return StatusCode(500, new { message = "Erro ao buscar pessoa física" });
        }
    }

    /// <summary>
    /// Cria um novo cadastro de pessoa física
    /// Permissões: Apenas Administradores e Usuários (Convidados NÃO podem)
    /// </summary>
    [HttpPost]
    [Authorize(Policy = "UsuarioOuSuperior")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult> Create([FromBody] PessoaFisicaCreateDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var codigo = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { codigo }, new { codigo, message = "Pessoa física criada com sucesso" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar pessoa física");
            return StatusCode(500, new { message = "Erro ao criar pessoa física" });
        }
    }

    /// <summary>
    /// Atualiza um cadastro de pessoa física
    /// Permissões: Apenas Administradores e Usuários
    /// </summary>
    [HttpPut("{codigo}")]
    [Authorize(Policy = "UsuarioOuSuperior")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult> Update(int codigo, [FromBody] PessoaFisicaUpdateDto dto)
    {
        try
        {
            if (codigo != dto.Codigo)
                return BadRequest(new { message = "Código informado não corresponde ao código da URL" });

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var success = await _service.UpdateAsync(dto);
            
            if (!success)
                return NotFound(new { message = "Pessoa física não encontrada" });

            return Ok(new { message = "Pessoa física atualizada com sucesso" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao atualizar pessoa física {Codigo}", codigo);
            return StatusCode(500, new { message = "Erro ao atualizar pessoa física" });
        }
    }
}
