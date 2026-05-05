using SistemaCadastro.API.Models;

namespace SistemaCadastro.API.Repositories;

public interface IPessoaRepository
{
    Task<int> CreateAsync(Pessoa pessoa);
    Task<Pessoa?> GetByIdAsync(int codigo);
    Task<IEnumerable<Pessoa>> GetAllAsync();
    Task<IEnumerable<Pessoa>> SearchAsync(string searchTerm);
    Task<bool> UpdateAsync(Pessoa pessoa);
    Task<bool> DeleteAsync(int codigo);
    Task<IEnumerable<Pessoa>> GetByTipoAsync(char tipo);
}

public interface IPessoaFisicaRepository
{
    Task<int> CreateAsync(PessoaFisica pessoaFisica);
    Task<PessoaFisica?> GetByPessoaIdAsync(int pessoaId);
    Task<IEnumerable<Pessoa>> GetByNameAsync(string nome);
    Task<bool> UpdateAsync(PessoaFisica pessoaFisica);
    Task<bool> DeleteAsync(int pessoaId);
}

public interface IPessoaJuridicaRepository
{
    Task<int> CreateAsync(PessoaJuridica pessoaJuridica);
    Task<PessoaJuridica?> GetByPessoaIdAsync(int pessoaId);
    Task<bool> UpdateAsync(PessoaJuridica pessoaJuridica);
    Task<bool> DeleteAsync(int pessoaId);
}

public interface IPessoaTelefoneRepository
{
    Task<int> CreateAsync(PessoaTelefone telefone);
    Task<IEnumerable<PessoaTelefone>> GetByPessoaIdAsync(int pessoaId);
    Task<bool> DeleteByPessoaIdAsync(int pessoaId);
}

public interface IEstadoRepository
{
    Task<IEnumerable<Estado>> GetAllAsync();
    Task<Estado?> GetByIdAsync(int codigo);
}

public interface ICidadeRepository
{
    Task<IEnumerable<Cidade>> GetAllAsync();
    Task<IEnumerable<Cidade>> GetByEstadoAsync(int estadoId);
    Task<Cidade?> GetByIdAsync(int codigo);
}

public interface IBairroRepository
{
    Task<int> GetOrCreateAsync(string nome);
    Task<Bairro?> GetByIdAsync(int codigo);
    Task<IEnumerable<Bairro>> GetByNameAsync(string nome);
}

public interface IEnderecoRepository
{
    Task<int> GetOrCreateAsync(string nome);
    Task<Endereco?> GetByIdAsync(int codigo);
    Task<IEnumerable<Endereco>> GetByNameAsync(string nome);
    
}

public interface ICepRepository
{
    Task<Cep?> GetByCepAsync(string cep);
}

public interface ICBORepository
{
    Task<IEnumerable<CBO>> GetAllAsync();
    Task<CBO?> GetByCodigoAsync(string codigo);
    Task<IEnumerable<CBO>> GetByDescricaoAsync(string descricao);
}

public interface INacionalidadeRepository
{
    Task<IEnumerable<Nacionalidade>> GetAllAsync();
    Task<Nacionalidade?> GetByIdAsync(int codigo);
}

public interface IAtividadeEconomicaRepository
{
    Task<IEnumerable<AtividadeEconomica>> GetAllAsync();
    Task<AtividadeEconomica?> GetByIdAsync(int codigo);
    Task<IEnumerable<AtividadeEconomica>> GetBySetorAsync(int setor);
    Task<IEnumerable<AtividadeEconomica>> GetByDescricaoAsync(string descricao);
}

public interface IAtividadeEconomicaSubsetorRepository
{
    Task<IEnumerable<AtividadeEconomicaSubsetor>> GetAllAsync();
    Task<AtividadeEconomicaSubsetor?> GetByIdAsync(int codigo);
    Task<IEnumerable<AtividadeEconomicaSubsetor>> GetBySetorAsync(int setor);
    Task<IEnumerable<AtividadeEconomicaSubsetor>> GetBySubSetorAsync(string subsetor);
}

public interface IPessoaEnderecoEletronicoRepository
{
    Task<int> CreateAsync(PessoaEnderecoEletronico endereco);
    Task<IEnumerable<PessoaEnderecoEletronico>> GetByPessoaIdAsync(int pessoaId);
    Task<bool> DeleteByPessoaIdAsync(int pessoaId);
}

public interface IEstadoCivilRepository
{
    Task<IEnumerable<EstadoCivil>> GetAllAsync();
    Task<EstadoCivil?> GetByIdAsync(int codigo);
}

public interface ICentroCustoRepository
{
    Task<IEnumerable<CentroCusto>> GetAllAsync();
    Task<CentroCusto?> GetByIdAsync(int codigo);
    Task<IEnumerable<CentroCusto>> GetByDescricaoAsync(string descricao);
    Task<int> CreateAsync(CentroCusto centroCusto);
    Task<bool> UpdateAsync(CentroCusto centroCusto);
    Task<bool> DeleteAsync(int codigo);
}

public interface IHistoricoContabilRepository
{
    Task<IEnumerable<HistoricoContabil>> GetAllAsync();
    Task<HistoricoContabil?> GetByIdAsync(int codigo);
    Task<IEnumerable<HistoricoContabil>> GetByDescricaoAsync(string descricao);
    Task<int> CreateAsync(HistoricoContabil historico);
    Task<bool> UpdateAsync(HistoricoContabil historico);
    Task<bool> DeleteAsync(int codigo);
}

public interface ILanctoContabilRepository
{
    Task<IEnumerable<LanctoContabil>> GetAllAsync();
    Task<LanctoContabil?> GetByIdAsync(int codigo);
    Task<int> CreateAsync(LanctoContabil lancto);
    Task<bool> UpdateAsync(LanctoContabil lancto);
    Task<bool> DeleteAsync(int codigo);
}
