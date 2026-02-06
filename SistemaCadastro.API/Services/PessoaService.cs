using SistemaCadastro.API.Models;
using SistemaCadastro.API.Repositories;

namespace SistemaCadastro.API.Services;

public class PessoaService : IPessoaService
{
    private readonly IPessoaRepository _pessoaRepository;
    private readonly IPessoaFisicaRepository _pessoaFisicaRepository;
    private readonly IPessoaJuridicaRepository _pessoaJuridicaRepository;
    private readonly IPessoaEnderecoEletronicoRepository _enderecoEletronicoRepository;
    private readonly IPessoaTelefoneRepository _telefoneRepository;

    public PessoaService(
        IPessoaRepository pessoaRepository,
        IPessoaFisicaRepository pessoaFisicaRepository,
        IPessoaJuridicaRepository pessoaJuridicaRepository,
        IPessoaEnderecoEletronicoRepository enderecoEletronicoRepository,
        IPessoaTelefoneRepository telefoneRepository)
    {
        _pessoaRepository = pessoaRepository;
        _pessoaFisicaRepository = pessoaFisicaRepository;
        _pessoaJuridicaRepository = pessoaJuridicaRepository;
        _enderecoEletronicoRepository = enderecoEletronicoRepository;
        _telefoneRepository = telefoneRepository;
    }

    public async Task<IEnumerable<Pessoa>> GetAllAsync()
    {
        return await _pessoaRepository.GetAllAsync();
    }

    public async Task<Pessoa?> GetByIdAsync(int codigo)
    {
        return await _pessoaRepository.GetByIdAsync(codigo);
    }

    public async Task<IEnumerable<Pessoa>> SearchAsync(string searchTerm)
    {
        return await _pessoaRepository.SearchAsync(searchTerm);
    }

    public async Task<bool> DeleteAsync(int codigo)
    {
        var pessoa = await _pessoaRepository.GetByIdAsync(codigo);
        if (pessoa == null)
            return false;

        // Deletar registros relacionados primeiro
        await _enderecoEletronicoRepository.DeleteByPessoaIdAsync(codigo);
        await _telefoneRepository.DeleteByPessoaIdAsync(codigo);

        if (pessoa.Tipo == 'F')
        {
            await _pessoaFisicaRepository.DeleteAsync(codigo);
        }
        else if (pessoa.Tipo == 'J')
        {
            await _pessoaJuridicaRepository.DeleteAsync(codigo);
        }

        // Deletar pessoa
        return await _pessoaRepository.DeleteAsync(codigo);
    }
}
