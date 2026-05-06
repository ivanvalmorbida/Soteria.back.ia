using SistemaCadastro.API.DTOs;
using SistemaCadastro.API.Models;

namespace SistemaCadastro.API.Services;

public interface IHospedagemService
{
    Task<IEnumerable<HospedagemDto>> GetAllAsync();
    Task<HospedagemDto?> GetByIdAsync(int codigo);
    Task<IEnumerable<HospedagemDto>> GetByPessoaAsync(int pessoaId);
    Task<IEnumerable<HospedagemDto>> GetByStatusAsync(string status);
    Task<int> CreateAsync(HospedagemCreateDto dto);
    Task<bool> UpdateAsync(HospedagemUpdateDto dto);
    Task<bool> DeleteAsync(int codigo);
}
