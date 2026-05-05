using SistemaCadastro.API.DTOs;
using SistemaCadastro.API.Models;
using SistemaCadastro.API.Repositories;

namespace SistemaCadastro.API.Services;

public class CentroCustoService : ICentroCustoService
{
    private readonly ICentroCustoRepository _repository;

    public CentroCustoService(ICentroCustoRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<CentroCustoDto>> GetAllAsync()
    {
        var centros = await _repository.GetAllAsync();
        return centros.Select(Map);
    }

    public async Task<CentroCustoDto?> GetByIdAsync(int codigo)
    {
        var centro = await _repository.GetByIdAsync(codigo);
        return centro is null ? null : Map(centro);
    }

    public async Task<IEnumerable<CentroCustoDto>> GetByDescricaoAsync(string descricao)
    {
        var centros = await _repository.GetByDescricaoAsync(descricao);
        return centros.Select(Map);
    }

    public async Task<int> CreateAsync(CentroCustoCreateDto dto)
    {
        ValidarDescricao(dto.Descricao);

        if (dto.Codigo.HasValue && dto.Codigo.Value > 0)
        {
            var existente = await _repository.GetByIdAsync(dto.Codigo.Value);
            if (existente != null)
                throw new ArgumentException($"Já existe um centro de custo com o código {dto.Codigo.Value}.");
        }

        var entidade = new CentroCusto
        {
            Codigo = dto.Codigo ?? 0,
            Descricao = dto.Descricao.Trim()
        };

        return await _repository.CreateAsync(entidade);
    }

    public async Task<bool> UpdateAsync(CentroCustoUpdateDto dto)
    {
        ValidarDescricao(dto.Descricao);

        var entidade = new CentroCusto
        {
            Codigo = dto.Codigo,
            Descricao = dto.Descricao.Trim()
        };

        return await _repository.UpdateAsync(entidade);
    }

    public Task<bool> DeleteAsync(int codigo) => _repository.DeleteAsync(codigo);

    private static void ValidarDescricao(string descricao)
    {
        if (string.IsNullOrWhiteSpace(descricao))
            throw new ArgumentException("A descrição do centro de custo é obrigatória.");

        if (descricao.Trim().Length > 50)
            throw new ArgumentException("A descrição do centro de custo deve ter no máximo 50 caracteres.");
    }

    private static CentroCustoDto Map(CentroCusto centro) => new()
    {
        Codigo = centro.Codigo,
        Descricao = centro.Descricao
    };
}
