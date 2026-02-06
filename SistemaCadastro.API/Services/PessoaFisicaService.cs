using SistemaCadastro.API.DTOs;
using SistemaCadastro.API.Models;
using SistemaCadastro.API.Repositories;

namespace SistemaCadastro.API.Services;

public class PessoaFisicaService : IPessoaFisicaService
{
    private readonly IPessoaRepository _pessoaRepository;
    private readonly IPessoaFisicaRepository _pessoaFisicaRepository;
    private readonly IPessoaEnderecoEletronicoRepository _enderecoEletronicoRepository;
    private readonly IPessoaTelefoneRepository _telefoneRepository;
    private readonly IBairroRepository _bairroRepository;
    private readonly IEnderecoRepository _enderecoRepository;
    private readonly IEstadoRepository _estadoRepository;
    private readonly ICidadeRepository _cidadeRepository;

    public PessoaFisicaService(
        IPessoaRepository pessoaRepository,
        IPessoaFisicaRepository pessoaFisicaRepository,
        IPessoaEnderecoEletronicoRepository enderecoEletronicoRepository,
        IPessoaTelefoneRepository telefoneRepository,
        IBairroRepository bairroRepository,
        IEnderecoRepository enderecoRepository,
        IEstadoRepository estadoRepository,
        ICidadeRepository cidadeRepository)
    {
        _pessoaRepository = pessoaRepository;
        _pessoaFisicaRepository = pessoaFisicaRepository;
        _enderecoEletronicoRepository = enderecoEletronicoRepository;
        _telefoneRepository = telefoneRepository;
        _bairroRepository = bairroRepository;
        _enderecoRepository = enderecoRepository;
        _estadoRepository = estadoRepository;
        _cidadeRepository = cidadeRepository;
    }

    public async Task<int> CreateAsync(PessoaFisicaCreateDto dto)
    {
        int? bairroId = null;
        int? enderecoId = null;

        if (!string.IsNullOrWhiteSpace(dto.Bairro))
            bairroId = await _bairroRepository.GetOrCreateAsync(dto.Bairro);

        if (!string.IsNullOrWhiteSpace(dto.Endereco))
            enderecoId = await _enderecoRepository.GetOrCreateAsync(dto.Endereco);

        var pessoa = new Pessoa
        {
            Tipo = 'F',
            Nome = dto.Nome,
            Cep = dto.Cep,
            Estado = dto.Estado,
            Cidade = dto.Cidade,
            Bairro = bairroId,
            Endereco = enderecoId,
            Numero = dto.Numero,
            Complemento = dto.Complemento,
            Obs = dto.Obs
        };

        var pessoaId = await _pessoaRepository.CreateAsync(pessoa);

        var pessoaFisica = new PessoaFisica
        {
            Pessoa = pessoaId,
            Cpf = dto.Cpf,
            Identidade = dto.Identidade,
            OrgaoIdentidade = dto.OrgaoIdentidade,
            UfIdentidade = dto.UfIdentidade,
            Nascimento = dto.Nascimento,
            Sexo = dto.Sexo,
            EstadoCivil = dto.EstadoCivil,
            Nacionalidade = dto.Nacionalidade,
            Profissao = dto.Profissao,
            Ctps = dto.Ctps,
            Pis = dto.Pis,
            CidadeNasc = dto.CidadeNasc,
            UfNasc = dto.UfNasc,
            Conjuge = dto.Conjuge
        };

        await _pessoaFisicaRepository.CreateAsync(pessoaFisica);

        // Adicionar telefones
        if (dto.Telefones != null && dto.Telefones.Any())
        {
            foreach (var tel in dto.Telefones)
            {
                var numeroTel = tel.ObterTelefone();
                if (!string.IsNullOrWhiteSpace(numeroTel))
                {
                    await _telefoneRepository.CreateAsync(new PessoaTelefone
                    {
                        Pessoa = pessoaId,
                        Telefone = TipoTelefone.ParseTelefone(numeroTel),
                        Tipo = tel.Tipo ?? TipoTelefone.Celular,
                        Descricao = tel.Descricao
                    });
                }
            }
        }

        // Adicionar endereços eletrônicos
        if (dto.EnderecosEletronicos != null && dto.EnderecosEletronicos.Any())
        {
            foreach (var endereco in dto.EnderecosEletronicos)
            {
                if (!string.IsNullOrWhiteSpace(endereco.Endereco))
                {
                    await _enderecoEletronicoRepository.CreateAsync(new PessoaEnderecoEletronico
                    {
                        Pessoa = pessoaId,
                        Endereco = endereco.Endereco,
                        Tipo = endereco.Tipo ?? TipoEnderecoEletronico.Email,
                        Descricao = endereco.Descricao
                    });
                }
            }
        }

        return pessoaId;
    }

    public async Task<PessoaFisicaDto?> GetByIdAsync(int codigo)
    {
        var pessoa = await _pessoaRepository.GetByIdAsync(codigo);
        if (pessoa == null || pessoa.Tipo != 'F')
            return null;

        var pessoaFisica = await _pessoaFisicaRepository.GetByPessoaIdAsync(codigo);
        if (pessoaFisica == null)
            return null;

        var telefones = await _telefoneRepository.GetByPessoaIdAsync(codigo);
        var enderecosEletronicos = await _enderecoEletronicoRepository.GetByPessoaIdAsync(codigo);

        string? estadoNome = null;
        string? cidadeNome = null;
        string? bairroNome = null;
        string? enderecoNome = null;

        if (pessoa.Estado.HasValue)
        {
            var estado = await _estadoRepository.GetByIdAsync(pessoa.Estado.Value);
            estadoNome = estado?.Nome;
        }

        if (pessoa.Cidade.HasValue)
        {
            var cidade = await _cidadeRepository.GetByIdAsync(pessoa.Cidade.Value);
            cidadeNome = cidade?.Nome;
        }

        if (pessoa.Bairro.HasValue)
        {
            var bairro = await _bairroRepository.GetByIdAsync(pessoa.Bairro.Value);
            bairroNome = bairro?.Nome;
        }

        if (pessoa.Endereco.HasValue)
        {
            var endereco = await _enderecoRepository.GetByIdAsync(pessoa.Endereco.Value);
            enderecoNome = endereco?.Nome;
        }

        return new PessoaFisicaDto
        {
            Codigo = pessoa.Codigo,
            Nome = pessoa.Nome,
            Cpf = pessoaFisica.Cpf,
            Identidade = pessoaFisica.Identidade,
            OrgaoIdentidade = pessoaFisica.OrgaoIdentidade,
            UfIdentidade = pessoaFisica.UfIdentidade,
            Nascimento = pessoaFisica.Nascimento,
            Sexo = pessoaFisica.Sexo,
            EstadoCivil = pessoaFisica.EstadoCivil,
            Nacionalidade = pessoaFisica.Nacionalidade,
            Profissao = pessoaFisica.Profissao,
            Ctps = pessoaFisica.Ctps,
            Pis = pessoaFisica.Pis,
            CidadeNasc = pessoaFisica.CidadeNasc,
            UfNasc = pessoaFisica.UfNasc,
            Cep = pessoa.Cep,
            Estado = pessoa.Estado,
            EstadoNome = estadoNome,
            Cidade = pessoa.Cidade,
            CidadeNome = cidadeNome,
            BairroNome = bairroNome,
            EnderecoNome = enderecoNome,
            Numero = pessoa.Numero,
            Complemento = pessoa.Complemento,
            Telefones = telefones.Select(t => new TelefoneDto
            {
                Codigo = t.Codigo,
                Telefone = TipoTelefone.FormatarTelefone(t.Telefone),
                Tipo = t.Tipo,
                TipoDescricao = TipoTelefone.ObterDescricao(t.Tipo),
                Descricao = t.Descricao
            }).ToList(),
            EnderecosEletronicos = enderecosEletronicos.Select(e => new EnderecoEletronicoDto
            {
                Codigo = e.Codigo,
                Endereco = e.Endereco,
                Tipo = e.Tipo,
                TipoDescricao = e.Tipo.HasValue ? TipoEnderecoEletronico.ObterDescricao(e.Tipo.Value) : null,
                Descricao = e.Descricao
            }).ToList(),
            Obs = pessoa.Obs,
            Cadastro = pessoa.Cadastro
        };
    }

    public async Task<bool> UpdateAsync(PessoaFisicaUpdateDto dto)
    {
        var pessoa = await _pessoaRepository.GetByIdAsync(dto.Codigo);
        if (pessoa == null)
            return false;

        int? bairroId = null;
        int? enderecoId = null;

        if (!string.IsNullOrWhiteSpace(dto.Bairro))
            bairroId = await _bairroRepository.GetOrCreateAsync(dto.Bairro);

        if (!string.IsNullOrWhiteSpace(dto.Endereco))
            enderecoId = await _enderecoRepository.GetOrCreateAsync(dto.Endereco);

        pessoa.Nome = dto.Nome;
        pessoa.Cep = dto.Cep;
        pessoa.Estado = dto.Estado;
        pessoa.Cidade = dto.Cidade;
        pessoa.Bairro = bairroId;
        pessoa.Endereco = enderecoId;
        pessoa.Numero = dto.Numero;
        pessoa.Complemento = dto.Complemento;
        pessoa.Obs = dto.Obs;

        await _pessoaRepository.UpdateAsync(pessoa);

        var pessoaFisica = await _pessoaFisicaRepository.GetByPessoaIdAsync(dto.Codigo);
        if (pessoaFisica != null)
        {
            pessoaFisica.Cpf = dto.Cpf;
            pessoaFisica.Identidade = dto.Identidade;
            pessoaFisica.OrgaoIdentidade = dto.OrgaoIdentidade;
            pessoaFisica.UfIdentidade = dto.UfIdentidade;
            pessoaFisica.Nascimento = dto.Nascimento;
            pessoaFisica.Sexo = dto.Sexo;
            pessoaFisica.EstadoCivil = dto.EstadoCivil;
            pessoaFisica.Nacionalidade = dto.Nacionalidade;
            pessoaFisica.Profissao = dto.Profissao;
            pessoaFisica.Ctps = dto.Ctps;
            pessoaFisica.Pis = dto.Pis;
            pessoaFisica.CidadeNasc = dto.CidadeNasc;
            pessoaFisica.UfNasc = dto.UfNasc;
            pessoaFisica.Conjuge = dto.Conjuge;

            await _pessoaFisicaRepository.UpdateAsync(pessoaFisica);
        }

        // Atualizar telefones
        await _telefoneRepository.DeleteByPessoaIdAsync(dto.Codigo);
        if (dto.Telefones != null && dto.Telefones.Any())
        {
            foreach (var tel in dto.Telefones)
            {
                var numeroTel = tel.ObterTelefone();
                if (!string.IsNullOrWhiteSpace(numeroTel))
                {
                    await _telefoneRepository.CreateAsync(new PessoaTelefone
                    {
                        Pessoa = dto.Codigo,
                        Telefone = TipoTelefone.ParseTelefone(numeroTel),
                        Tipo = tel.Tipo ?? TipoTelefone.Celular,
                        Descricao = tel.Descricao
                    });
                }
            }
        }

        // Atualizar endereços eletrônicos
        await _enderecoEletronicoRepository.DeleteByPessoaIdAsync(dto.Codigo);
        if (dto.EnderecosEletronicos != null && dto.EnderecosEletronicos.Any())
        {
            foreach (var endereco in dto.EnderecosEletronicos)
            {
                if (!string.IsNullOrWhiteSpace(endereco.Endereco))
                {
                    await _enderecoEletronicoRepository.CreateAsync(new PessoaEnderecoEletronico
                    {
                        Pessoa = dto.Codigo,
                        Endereco = endereco.Endereco,
                        Tipo = endereco.Tipo ?? TipoEnderecoEletronico.Email,
                        Descricao = endereco.Descricao
                    });
                }
            }
        }

        return true;
    }

    public async Task<IEnumerable<PessoaFisicaDto>> GetAllAsync()
    {
        var pessoas = await _pessoaRepository.GetByTipoAsync('F');
        var result = new List<PessoaFisicaDto>();

        foreach (var pessoa in pessoas)
        {
            var dto = await GetByIdAsync(pessoa.Codigo);
            if (dto != null)
                result.Add(dto);
        }

        return result;
    }
}
