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
    Task<IEnumerable<PessoaFisicaDto>> GetByNameAsync(string nome);
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

public interface ICentroCustoService
{
    Task<IEnumerable<CentroCustoDto>> GetAllAsync();
    Task<CentroCustoDto?> GetByIdAsync(int codigo);
    Task<IEnumerable<CentroCustoDto>> GetByDescricaoAsync(string descricao);
    Task<int> CreateAsync(CentroCustoCreateDto dto);
    Task<bool> UpdateAsync(CentroCustoUpdateDto dto);
    Task<bool> DeleteAsync(int codigo);
}

public interface IHistoricoContabilService
{
    Task<IEnumerable<HistoricoContabilDto>> GetAllAsync();
    Task<HistoricoContabilDto?> GetByIdAsync(int codigo);
    Task<IEnumerable<HistoricoContabilDto>> GetByDescricaoAsync(string descricao);
    Task<int> CreateAsync(HistoricoContabilCreateDto dto);
    Task<bool> UpdateAsync(HistoricoContabilUpdateDto dto);
    Task<bool> DeleteAsync(int codigo);
}

public interface ILanctoContabilService
{
    Task<IEnumerable<LanctoContabilDto>> GetAllAsync();
    Task<LanctoContabilDto?> GetByIdAsync(int codigo);
    Task<int> CreateAsync(LanctoContabilCreateDto dto);
    Task<bool> UpdateAsync(LanctoContabilUpdateDto dto);
    Task<bool> DeleteAsync(int codigo);
}

public interface IPlanoContaService
{
    Task<IEnumerable<PlanoContaDto>> GetAllAsync();
    Task<PlanoContaDto?> GetByIdAsync(int codigo);
    Task<IEnumerable<PlanoContaDto>> GetByTipoAsync(string tipo);
    Task<IEnumerable<PlanoContaDto>> GetByDescricaoAsync(string descricao);
    Task<int> CreateAsync(PlanoContaCreateDto dto);
    Task<bool> UpdateAsync(PlanoContaUpdateDto dto);
    Task<bool> DeleteAsync(int codigo);
}
