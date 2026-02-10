using Microsoft.AspNetCore.Mvc;
using SistemaCadastro.API.Models;
using SistemaCadastro.API.Repositories;

namespace SistemaCadastro.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EstadoController : ControllerBase
{
    private readonly IEstadoRepository _repository;

    public EstadoController(IEstadoRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Estado>>> GetAll()
    {
        var estados = await _repository.GetAllAsync();
        return Ok(estados);
    }

    [HttpGet("{codigo}")]
    public async Task<ActionResult<Estado>> GetById(int codigo)
    {
        var estado = await _repository.GetByIdAsync(codigo);
        
        if (estado == null)
            return NotFound();

        return Ok(estado);
    }
}

[ApiController]
[Route("api/[controller]")]
public class CidadeController : ControllerBase
{
    private readonly ICidadeRepository _repository;

    public CidadeController(ICidadeRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Cidade>>> GetAll()
    {
        var cidades = await _repository.GetAllAsync();
        return Ok(cidades);
    }

    [HttpGet("estado/{estadoId}")]
    public async Task<ActionResult<IEnumerable<Cidade>>> GetByEstado(int estadoId)
    {
        var cidades = await _repository.GetByEstadoAsync(estadoId);
        return Ok(cidades);
    }

    [HttpGet("{codigo}")]
    public async Task<ActionResult<Cidade>> GetById(int codigo)
    {
        var cidade = await _repository.GetByIdAsync(codigo);
        
        if (cidade == null)
            return NotFound();

        return Ok(cidade);
    }
}

[ApiController]
[Route("api/[controller]")]
public class CepController : ControllerBase
{
    private readonly ICepRepository _repository;

    public CepController(ICepRepository repository)
    {
        _repository = repository;
    }

    [HttpGet("{cep}")]
    public async Task<ActionResult<Cep>> GetByCep(string cep)
    {
        // Remove formatação do CEP
        cep = cep.Replace("-", "").Replace(".", "");

        var cepData = await _repository.GetByCepAsync(cep);
        
        if (cepData == null)
            return NotFound(new { message = "CEP não encontrado" });

        return Ok(cepData);
    }
}

[ApiController]
[Route("api/[controller]")]
public class CBOController : ControllerBase
{
    private readonly ICBORepository _repository;

    public CBOController(ICBORepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CBO>>> GetAll()
    {
        var cbos = await _repository.GetAllAsync();
        return Ok(cbos);
    }

    [HttpGet("{codigo}")]
    public async Task<ActionResult<CBO>> GetByCodigo(string codigo)
    {
        var cbo = await _repository.GetByCodigoAsync(codigo);
        
        if (cbo == null)
            return NotFound();

        return Ok(cbo);
    }
}

[ApiController]
[Route("api/[controller]")]
public class NacionalidadeController : ControllerBase
{
    private readonly INacionalidadeRepository _repository;

    public NacionalidadeController(INacionalidadeRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Nacionalidade>>> GetAll()
    {
        var nacionalidades = await _repository.GetAllAsync();
        return Ok(nacionalidades);
    }

    [HttpGet("{codigo}")]
    public async Task<ActionResult<Nacionalidade>> GetById(int codigo)
    {
        var nacionalidade = await _repository.GetByIdAsync(codigo);
        
        if (nacionalidade == null)
            return NotFound();

        return Ok(nacionalidade);
    }
}

[ApiController]
[Route("api/[controller]")]
public class AtividadeEconomicaController : ControllerBase
{
    private readonly IAtividadeEconomicaRepository _repository;

    public AtividadeEconomicaController(IAtividadeEconomicaRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<AtividadeEconomica>>> GetAll()
    {
        var atividades = await _repository.GetAllAsync();
        return Ok(atividades);
    }

    [HttpGet("{codigo}")]
    public async Task<ActionResult<AtividadeEconomica>> GetById(int codigo)
    {
        var atividade = await _repository.GetByIdAsync(codigo);
        
        if (atividade == null)
            return NotFound();

        return Ok(atividade);
    }

    [HttpGet("setor/{setor}")]
    public async Task<ActionResult<IEnumerable<AtividadeEconomica>>> GetBySetor(int setor)
    {
        var atividades = await _repository.GetBySetorAsync(setor);
        return Ok(atividades);
    }
}