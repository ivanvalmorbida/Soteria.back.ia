using SistemaCadastro.API.DTOs;
using SistemaCadastro.API.Models;
using SistemaCadastro.API.Repositories;

namespace SistemaCadastro.API.Services;

public class HospedagemService : IHospedagemService
{
    private readonly IHospedagemRepository _repository;

    public HospedagemService(IHospedagemRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<HospedagemDto>> GetAllAsync()
    {
        var hospedagens = await _repository.GetAllAsync();
        return hospedagens.Select(Map);
    }

    public async Task<HospedagemDto?> GetByIdAsync(int codigo)
    {
        var hospedagem = await _repository.GetByIdAsync(codigo);
        return hospedagem is null ? null : Map(hospedagem);
    }

    public async Task<IEnumerable<HospedagemDto>> GetByPessoaAsync(int pessoaId)
    {
        var hospedagens = await _repository.GetByPessoaAsync(pessoaId);
        return hospedagens.Select(Map);
    }

    public async Task<IEnumerable<HospedagemDto>> GetByStatusAsync(string status)
    {
        var hospedagens = await _repository.GetByStatusAsync(status);
        return hospedagens.Select(Map);
    }

    public async Task<int> CreateAsync(HospedagemCreateDto dto)
    {
        Validar(dto.Quarto, dto.Diaria, dto.Checkin);

        var entidade = new Hospedagem
        {
            Pessoa = dto.Pessoa,
            Quarto = dto.Quarto.Trim(),
            Checkin = dto.Checkin,
            Checkout = dto.Checkout,
            Diaria = dto.Diaria,
            Total = dto.Total,
            Status = ValidarStatus(dto.Status),
            Observacoes = dto.Observacoes?.Trim()
        };

        return await _repository.CreateAsync(entidade);
    }

    public async Task<bool> UpdateAsync(HospedagemUpdateDto dto)
    {
        Validar(dto.Quarto, dto.Diaria, dto.Checkin);

        var entidade = new Hospedagem
        {
            Codigo = dto.Codigo,
            Pessoa = dto.Pessoa,
            Quarto = dto.Quarto.Trim(),
            Checkin = dto.Checkin,
            Checkout = dto.Checkout,
            Diaria = dto.Diaria,
            Total = dto.Total,
            Status = ValidarStatus(dto.Status),
            Observacoes = dto.Observacoes?.Trim()
        };

        return await _repository.UpdateAsync(entidade);
    }

    public Task<bool> DeleteAsync(int codigo) => _repository.DeleteAsync(codigo);

    private static void Validar(string quarto, decimal diaria, DateTime checkin)
    {
        if (string.IsNullOrWhiteSpace(quarto))
            throw new ArgumentException("O número do quarto é obrigatório.");

        if (quarto.Trim().Length > 10)
            throw new ArgumentException("O número do quarto deve ter no máximo 10 caracteres.");

        if (diaria <= 0)
            throw new ArgumentException("O valor da diária deve ser maior que zero.");

        if (checkin > DateTime.Now)
            throw new ArgumentException("A data de check-in não pode ser futura.");
    }

    private static string ValidarStatus(string? status)
    {
        var validos = new[] { "Ativa", "Finalizada", "Cancelada", "Reserva" };
        var s = string.IsNullOrWhiteSpace(status) ? "Ativa" : status.Trim();
        if (!validos.Contains(s))
            throw new ArgumentException($"Status inválido. Valores permitidos: {string.Join(", ", validos)}");
        return s;
    }

    private static HospedagemDto Map(Hospedagem hospedagem) => new()
    {
        Codigo = hospedagem.Codigo,
        Pessoa = hospedagem.Pessoa,
        PessoaNome = string.Empty,
        Quarto = hospedagem.Quarto,
        Checkin = hospedagem.Checkin,
        Checkout = hospedagem.Checkout,
        Diaria = hospedagem.Diaria,
        Total = hospedagem.Total,
        Status = hospedagem.Status ?? "Ativa",
        Observacoes = hospedagem.Observacoes,
        Cadastrado = hospedagem.Cadastrado
    };
}
