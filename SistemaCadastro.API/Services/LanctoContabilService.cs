using SistemaCadastro.API.DTOs;
using SistemaCadastro.API.Models;
using SistemaCadastro.API.Repositories;

namespace SistemaCadastro.API.Services;

public class LanctoContabilService : ILanctoContabilService
{
    private readonly ILanctoContabilRepository _repository;

    public LanctoContabilService(ILanctoContabilRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<LanctoContabilDto>> GetAllAsync()
    {
        var lanctos = await _repository.GetAllAsync();
        return lanctos.Select(Map);
    }

    public async Task<LanctoContabilDto?> GetByIdAsync(int codigo)
    {
        var lancto = await _repository.GetByIdAsync(codigo);
        return lancto is null ? null : Map(lancto);
    }

    public async Task<int> CreateAsync(LanctoContabilCreateDto dto)
    {
        Validar(dto.Descricao, dto.Valor);

        var entidade = new LanctoContabil
        {
            Pessoa = dto.Pessoa,
            CentroCusto = dto.CentroCusto,
            Credito = dto.Credito,
            Debito = dto.Debito,
            Valor = dto.Valor,
            Data = dto.Data,
            HC = dto.HC,
            Descricao = dto.Descricao.Trim()
        };

        return await _repository.CreateAsync(entidade);
    }

    public async Task<bool> UpdateAsync(LanctoContabilUpdateDto dto)
    {
        Validar(dto.Descricao, dto.Valor);

        var entidade = new LanctoContabil
        {
            Codigo = dto.Codigo,
            Pessoa = dto.Pessoa,
            CentroCusto = dto.CentroCusto,
            Credito = dto.Credito,
            Debito = dto.Debito,
            Valor = dto.Valor,
            Data = dto.Data,
            HC = dto.HC,
            Descricao = dto.Descricao.Trim()
        };

        return await _repository.UpdateAsync(entidade);
    }

    public Task<bool> DeleteAsync(int codigo) => _repository.DeleteAsync(codigo);

    private static void Validar(string descricao, double valor)
    {
        if (string.IsNullOrWhiteSpace(descricao))
            throw new ArgumentException("A descrição do lançamento contábil é obrigatória.");

        if (descricao.Trim().Length > 50)
            throw new ArgumentException("A descrição do lançamento contábil deve ter no máximo 50 caracteres.");

        if (valor <= 0)
            throw new ArgumentException("O valor do lançamento contábil deve ser maior que zero.");
    }

    private static LanctoContabilDto Map(LanctoContabil lancto) => new()
    {
        Codigo = lancto.Codigo,
        Pessoa = lancto.Pessoa,
        PessoaNome = string.Empty,
        CentroCusto = lancto.CentroCusto,
        CentroCustoDescricao = string.Empty,
        Credito = lancto.Credito,
        CreditoDescricao = string.Empty,
        Debito = lancto.Debito,
        DebitoDescricao = string.Empty,
        Valor = lancto.Valor,
        Data = lancto.Data,
        HC = lancto.HC,
        Descricao = lancto.Descricao
    };
}
