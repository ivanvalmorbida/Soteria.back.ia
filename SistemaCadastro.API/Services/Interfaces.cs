using SistemaCadastro.API.DTOs;
using SistemaCadastro.API.Models;

namespace SistemaCadastro.API.Services;

public interface IPessoaService
{
    Task<IEnumerable<Pessoa>> GetAllAsync();
    Task<Pessoa?> GetByIdAsync(int codigo);
    Task<IEnumerable<Pessoa>> SearchAsync(string searchTerm);
    Task<bool> DeleteAsync(int codigo);
}

public interface IPessoaFisicaService
{
    Task<int> CreateAsync(PessoaFisicaCreateDto dto);
    Task<PessoaFisicaDto?> GetByIdAsync(int codigo);
    Task<bool> UpdateAsync(PessoaFisicaUpdateDto dto);
    Task<IEnumerable<PessoaFisicaDto>> GetAllAsync();
}

public interface IPessoaJuridicaService
{
    Task<int> CreateAsync(PessoaJuridicaCreateDto dto);
    Task<PessoaJuridicaDto?> GetByIdAsync(int codigo);
    Task<bool> UpdateAsync(PessoaJuridicaUpdateDto dto);
    Task<IEnumerable<PessoaJuridicaDto>> GetAllAsync();
}
