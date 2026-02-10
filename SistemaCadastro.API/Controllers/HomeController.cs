using Microsoft.AspNetCore.Mvc;
using SistemaCadastro.API.Models;
using SistemaCadastro.API.Repositories;

namespace SistemaCadastro.API.Controllers
{
    public class HomeController : ControllerBase
    {
        private readonly IEstadoCivilRepository _repository;

        public HomeController(IEstadoCivilRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EstadoCivil>>> GetAll()
        {
            var estadocivil = await _repository.GetAllAsync();
            return Ok(estadocivil);
        }

        [HttpGet("{codigo}")]
        public async Task<ActionResult<EstadoCivil>> GetById(int codigo)
        {
            var estadocivil = await _repository.GetByIdAsync(codigo);

            if (estadocivil == null)
                return NotFound();

            return Ok(estadocivil);
        }
    }
}


