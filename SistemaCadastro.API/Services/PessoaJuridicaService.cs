using SistemaCadastro.API.DTOs;
using SistemaCadastro.API.Models;
using SistemaCadastro.API.Repositories;
using SistemaCadastro.API.Utilities;

namespace SistemaCadastro.API.Services;

public class PessoaJuridicaService : IPessoaJuridicaService
{
    private readonly IPessoaRepository _pessoaRepository;
    private readonly IPessoaJuridicaRepository _pessoaJuridicaRepository;
    private readonly IPessoaEnderecoEletronicoRepository _enderecoEletronicoRepository;
    private readonly IPessoaTelefoneRepository _telefoneRepository;
    private readonly IBairroRepository _bairroRepository;
    private readonly IEnderecoRepository _enderecoRepository;
    private readonly IEstadoRepository _estadoRepository;
    private readonly ICidadeRepository _cidadeRepository;

    public PessoaJuridicaService(
        IPessoaRepository pessoaRepository,
        IPessoaJuridicaRepository pessoaJuridicaRepository,
        IPessoaEnderecoEletronicoRepository enderecoEletronicoRepository,
        IPessoaTelefoneRepository telefoneRepository,
        IBairroRepository bairroRepository,
        IEnderecoRepository enderecoRepository,
        IEstadoRepository estadoRepository,
        ICidadeRepository cidadeRepository)
    {
        _pessoaRepository = pessoaRepository;
        _pessoaJuridicaRepository = pessoaJuridicaRepository;
        _enderecoEletronicoRepository = enderecoEletronicoRepository;
        _telefoneRepository = telefoneRepository;
        _bairroRepository = bairroRepository;
        _enderecoRepository = enderecoRepository;
        _estadoRepository = estadoRepository;
        _cidadeRepository = cidadeRepository;
    }

    public async Task<int> CreateAsync(PessoaJuridicaCreateDto dto)
    {
        // Validar CNPJ alfanumérico se fornecido
        if (!string.IsNullOrWhiteSpace(dto.Cnpj))
        {
            if (!CnpjAlfanumericoValidator.Validar(dto.Cnpj))
                throw new ArgumentException("CNPJ alfanumérico inválido. Verifique os dígitos verificadores.");

            dto.Cnpj = dto.Cnpj.Trim().ToUpperInvariant().Replace(".", "").Replace("/", "").Replace("-", "");
        }

        int? bairroId = null;
        int? enderecoId = null;

        if (!string.IsNullOrWhiteSpace(dto.Bairro))
            bairroId = await _bairroRepository.GetOrCreateAsync(dto.Bairro);

        if (!string.IsNullOrWhiteSpace(dto.Endereco))
            enderecoId = await _enderecoRepository.GetOrCreateAsync(dto.Endereco);

        var pessoa = new Pessoa
        {
            Tipo = 'J',
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

        var pessoaJuridica = new PessoaJuridica
        {
            Pessoa = pessoaId,
            RazaoSocial = dto.RazaoSocial,
            Cnpj = dto.Cnpj,
            InscricaoEstadual = dto.InscricaoEstadual,
            Atividade = dto.Atividade,
            Homepage = dto.Homepage,
            Representante = dto.Representante
        };

        await _pessoaJuridicaRepository.CreateAsync(pessoaJuridica);

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

    public async Task<PessoaJuridicaDto?> GetByIdAsync(int codigo)
    {
        var pessoa = await _pessoaRepository.GetByIdAsync(codigo);
        if (pessoa == null || pessoa.Tipo != 'J')
            return null;

        var pessoaJuridica = await _pessoaJuridicaRepository.GetByPessoaIdAsync(codigo);
        if (pessoaJuridica == null)
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

        string? representanteNome = null;
        if (pessoaJuridica.Representante.HasValue)
        {
            var representante = await _pessoaRepository.GetByIdAsync(pessoaJuridica.Representante.Value);
            representanteNome = representante?.Nome;
        }

        return new PessoaJuridicaDto
        {
            Codigo = pessoa.Codigo,
            RazaoSocial = pessoaJuridica.RazaoSocial,
            Nome = pessoa.Nome,
            Cnpj = pessoaJuridica.Cnpj,
            InscricaoEstadual = pessoaJuridica.InscricaoEstadual,
            Atividade = pessoaJuridica.Atividade,
            Homepage = pessoaJuridica.Homepage,
            Representante = pessoaJuridica.Representante,
            RepresentanteNome = representanteNome,
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

    public async Task<bool> UpdateAsync(PessoaJuridicaUpdateDto dto)
    {
        // Validar CNPJ alfanumérico se fornecido
        if (!string.IsNullOrWhiteSpace(dto.Cnpj))
        {
            if (!CnpjAlfanumericoValidator.Validar(dto.Cnpj))
                throw new ArgumentException("CNPJ alfanumérico inválido. Verifique os dígitos verificadores.");

            dto.Cnpj = dto.Cnpj.Trim().ToUpperInvariant().Replace(".", "").Replace("/", "").Replace("-", "");
        }

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

        var pessoaJuridica = await _pessoaJuridicaRepository.GetByPessoaIdAsync(dto.Codigo);
        if (pessoaJuridica != null)
        {
            pessoaJuridica.RazaoSocial = dto.RazaoSocial;
            pessoaJuridica.Cnpj = dto.Cnpj;
            pessoaJuridica.InscricaoEstadual = dto.InscricaoEstadual;
            pessoaJuridica.Atividade = dto.Atividade;
            pessoaJuridica.Homepage = dto.Homepage;
            pessoaJuridica.Representante = dto.Representante;

            await _pessoaJuridicaRepository.UpdateAsync(pessoaJuridica);
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

    public async Task<IEnumerable<PessoaJuridicaDto>> GetAllAsync()
    {
        var pessoas = await _pessoaRepository.GetByTipoAsync('J');
        var result = new List<PessoaJuridicaDto>();

        foreach (var pessoa in pessoas)
        {
            var dto = await GetByIdAsync(pessoa.Codigo);
            if (dto != null)
                result.Add(dto);
        }

        return result;
    }
}
