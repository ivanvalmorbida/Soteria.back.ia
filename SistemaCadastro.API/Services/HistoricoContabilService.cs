using SistemaCadastro.API.DTOs;
using SistemaCadastro.API.Models;
using SistemaCadastro.API.Repositories;

namespace SistemaCadastro.API.Services;

public class HistoricoContabilService : IHistoricoContabilService
{
    private readonly IHistoricoContabilRepository _repository;

    public HistoricoContabilService(IHistoricoContabilRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<HistoricoContabilDto>> GetAllAsync()
    {
        var historicos = await _repository.GetAllAsync();
        return historicos.Select(Map);
    }

    public async Task<HistoricoContabilDto?> GetByIdAsync(int codigo)
    {
        var historico = await _repository.GetByIdAsync(codigo);
        return historico is null ? null : Map(historico);
    }

    public async Task<IEnumerable<HistoricoContabilDto>> GetByDescricaoAsync(string descricao)
    {
        var historicos = await _repository.GetByDescricaoAsync(descricao);
        return historicos.Select(Map);
    }

    public async Task<int> CreateAsync(HistoricoContabilCreateDto dto)
    {
        ValidarDescricao(dto.Descricao);

        var entidade = new HistoricoContabil
        {
            Descricao = dto.Descricao.Trim()
        };

        return await _repository.CreateAsync(entidade);
    }

    public async Task<bool> UpdateAsync(HistoricoContabilUpdateDto dto)
    {
        ValidarDescricao(dto.Descricao);

        var entidade = new HistoricoContabil
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
            throw new ArgumentException("A descrição do histórico contábil é obrigatória.");

        if (descricao.Trim().Length > 50)
            throw new ArgumentException("A descrição do histórico contábil deve ter no máximo 50 caracteres.");
    }

    private static HistoricoContabilDto Map(HistoricoContabil historico) => new()
    {
        Codigo = historico.Codigo,
        Descricao = historico.Descricao
    };
}
