using SistemaCadastro.API.Models;

namespace SistemaCadastro.API.Repositories;

public interface IHospedagemRepository
{
    Task<IEnumerable<Hospedagem>> GetAllAsync();
    Task<Hospedagem?> GetByIdAsync(int codigo);
    Task<IEnumerable<Hospedagem>> GetByPessoaAsync(int pessoaId);
    Task<IEnumerable<Hospedagem>> GetByStatusAsync(string status);
    Task<int> CreateAsync(Hospedagem hospedagem);
    Task<bool> UpdateAsync(Hospedagem hospedagem);
    Task<bool> DeleteAsync(int codigo);
}
