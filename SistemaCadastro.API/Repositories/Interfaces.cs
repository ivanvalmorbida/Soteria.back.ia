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

public interface IPessoaEmailRepository
{
    Task<int> CreateAsync(PessoaEmail email);
    Task<IEnumerable<PessoaEmail>> GetByPessoaIdAsync(int pessoaId);
    Task<bool> DeleteByPessoaIdAsync(int pessoaId);
}

public interface IPessoaFoneRepository
{
    Task<int> CreateAsync(PessoaFone fone);
    Task<IEnumerable<PessoaFone>> GetByPessoaIdAsync(int pessoaId);
    Task<bool> DeleteByPessoaIdAsync(int pessoaId);
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
}

public interface IEnderecoRepository
{
    Task<int> GetOrCreateAsync(string nome);
    Task<Endereco?> GetByIdAsync(int codigo);
}

public interface ICepRepository
{
    Task<Cep?> GetByCepAsync(string cep);
}

public interface ICBORepository
{
    Task<IEnumerable<CBO>> GetAllAsync();
    Task<CBO?> GetByCodigoAsync(string codigo);
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
}

public interface IPessoaEnderecoEletronicoRepository
{
    Task<int> CreateAsync(PessoaEnderecoEletronico endereco);
    Task<IEnumerable<PessoaEnderecoEletronico>> GetByPessoaIdAsync(int pessoaId);
    Task<bool> DeleteByPessoaIdAsync(int pessoaId);
}
