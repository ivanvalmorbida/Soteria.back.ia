using SistemaCadastro.API.DTOs;
using SistemaCadastro.API.Models;
using SistemaCadastro.API.Repositories;

namespace SistemaCadastro.API.Services;

public class PlanoContaService : IPlanoContaService
{
    private readonly IPlanoContaRepository _repository;

    public PlanoContaService(IPlanoContaRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<PlanoContaDto>> GetAllAsync()
    {
        var planos = await _repository.GetAllAsync();
        return planos.Select(Map);
    }

    public async Task<PlanoContaDto?> GetByIdAsync(int codigo)
    {
        var plano = await _repository.GetByIdAsync(codigo);
        return plano is null ? null : Map(plano);
    }

    public async Task<IEnumerable<PlanoContaDto>> GetByTipoAsync(string tipo)
    {
        var planos = await _repository.GetByTipoAsync(tipo);
        return planos.Select(Map);
    }

    public async Task<IEnumerable<PlanoContaDto>> GetByDescricaoAsync(string descricao)
    {
        var planos = await _repository.GetByDescricaoAsync(descricao);
        return planos.Select(Map);
    }

    public async Task<int> CreateAsync(PlanoContaCreateDto dto)
    {
        Validar(dto);

        var entidade = new PlanoConta
        {
            CodigoPai = dto.CodigoPai,
            Tipo = dto.Tipo.Trim(),
            Rotulo = dto.Rotulo.Trim(),
            Descricao = dto.Descricao.Trim()
        };

        return await _repository.CreateAsync(entidade);
    }

    public async Task<bool> UpdateAsync(PlanoContaUpdateDto dto)
    {
        Validar(dto);

        var entidade = new PlanoConta
        {
            Codigo = dto.Codigo,
            CodigoPai = dto.CodigoPai,
            Tipo = dto.Tipo.Trim(),
            Rotulo = dto.Rotulo.Trim(),
            Descricao = dto.Descricao.Trim()
        };

        return await _repository.UpdateAsync(entidade);
    }

    public Task<bool> DeleteAsync(int codigo) => _repository.DeleteAsync(codigo);

    private static void Validar(PlanoContaCreateDto dto)
    {
        ValidarCampos(dto.Tipo, dto.Rotulo, dto.Descricao);
    }

    private static void Validar(PlanoContaUpdateDto dto)
    {
        ValidarCampos(dto.Tipo, dto.Rotulo, dto.Descricao);
    }

    private static void ValidarCampos(string tipo, string rotulo, string descricao)
    {
        if (string.IsNullOrWhiteSpace(tipo))
            throw new ArgumentException("O tipo do plano de conta é obrigatório.");

        if (tipo.Trim().Length > 1)
            throw new ArgumentException("O tipo do plano de conta deve ter exatamente 1 caractere.");

        if (string.IsNullOrWhiteSpace(rotulo))
            throw new ArgumentException("O rótulo do plano de conta é obrigatório.");

        if (rotulo.Trim().Length > 30)
            throw new ArgumentException("O rótulo do plano de conta deve ter no máximo 30 caracteres.");

        if (string.IsNullOrWhiteSpace(descricao))
            throw new ArgumentException("A descrição do plano de conta é obrigatória.");

        if (descricao.Trim().Length > 50)
            throw new ArgumentException("A descrição do plano de conta deve ter no máximo 50 caracteres.");
    }

    private static PlanoContaDto Map(PlanoConta plano) => new()
    {
        Codigo = plano.Codigo,
        CodigoPai = plano.CodigoPai,
        CodigoPaiRotulo = string.Empty,
        Tipo = plano.Tipo,
        Rotulo = plano.Rotulo,
        Descricao = plano.Descricao
    };
}
