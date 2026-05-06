using Dapper;
using MySql.Data.MySqlClient;
using SistemaCadastro.API.Configuration;
using SistemaCadastro.API.Models;

namespace SistemaCadastro.API.Repositories;

public class HospedagemRepository : IHospedagemRepository
{
    private readonly DatabaseConfig _dbConfig;

    public HospedagemRepository(DatabaseConfig dbConfig)
    {
        _dbConfig = dbConfig;
    }

    public async Task<IEnumerable<Hospedagem>> GetAllAsync()
    {
        using var connection = new MySqlConnection(_dbConfig.ConnectionString);

        var sql = @"SELECT h.Codigo, h.Pessoa, p.Nome AS PessoaNome, h.Quarto, h.Checkin, h.Checkout, 
                           h.Diaria, h.Total, h.Status, h.Observacoes, h.Cadastrado 
                    FROM tb_hospedagens h 
                    LEFT JOIN tb_pessoa p ON p.Codigo = h.Pessoa 
                    ORDER BY h.Checkin DESC";
        return await connection.QueryAsync<Hospedagem>(sql);
    }

    public async Task<Hospedagem?> GetByIdAsync(int codigo)
    {
        using var connection = new MySqlConnection(_dbConfig.ConnectionString);

        var sql = @"SELECT h.Codigo, h.Pessoa, p.Nome AS PessoaNome, h.Quarto, h.Checkin, h.Checkout, 
                           h.Diaria, h.Total, h.Status, h.Observacoes, h.Cadastrado 
                    FROM tb_hospedagens h 
                    LEFT JOIN tb_pessoa p ON p.Codigo = h.Pessoa 
                    WHERE h.Codigo = @Codigo";
        return await connection.QueryFirstOrDefaultAsync<Hospedagem>(sql, new { Codigo = codigo });
    }

    public async Task<IEnumerable<Hospedagem>> GetByPessoaAsync(int pessoaId)
    {
        using var connection = new MySqlConnection(_dbConfig.ConnectionString);

        var sql = @"SELECT h.Codigo, h.Pessoa, p.Nome AS PessoaNome, h.Quarto, h.Checkin, h.Checkout, 
                           h.Diaria, h.Total, h.Status, h.Observacoes, h.Cadastrado 
                    FROM tb_hospedagens h 
                    LEFT JOIN tb_pessoa p ON p.Codigo = h.Pessoa 
                    WHERE h.Pessoa = @PessoaId ORDER BY h.Checkin DESC";
        return await connection.QueryAsync<Hospedagem>(sql, new { PessoaId = pessoaId });
    }

    public async Task<IEnumerable<Hospedagem>> GetByStatusAsync(string status)
    {
        using var connection = new MySqlConnection(_dbConfig.ConnectionString);

        var sql = @"SELECT h.Codigo, h.Pessoa, p.Nome AS PessoaNome, h.Quarto, h.Checkin, h.Checkout, 
                           h.Diaria, h.Total, h.Status, h.Observacoes, h.Cadastrado 
                    FROM tb_hospedagens h 
                    LEFT JOIN tb_pessoa p ON p.Codigo = h.Pessoa 
                    WHERE h.Status = @Status ORDER BY h.Checkin DESC";
        return await connection.QueryAsync<Hospedagem>(sql, new { Status = status });
    }

    public async Task<int> CreateAsync(Hospedagem hospedagem)
    {
        using var connection = new MySqlConnection(_dbConfig.ConnectionString);

        var sql = @"INSERT INTO tb_hospedagens (Pessoa, Quarto, Checkin, Checkout, Diaria, Total, Status, Observacoes) 
                    VALUES (@Pessoa, @Quarto, @Checkin, @Checkout, @Diaria, @Total, @Status, @Observacoes);
                    SELECT LAST_INSERT_ID();";
        return await connection.ExecuteScalarAsync<int>(sql, new
        {
            hospedagem.Pessoa,
            hospedagem.Quarto,
            hospedagem.Checkin,
            Checkout = hospedagem.Checkout.HasValue ? hospedagem.Checkout.Value : (DateTime?)null,
            hospedagem.Diaria,
            Total = hospedagem.Total.HasValue ? hospedagem.Total.Value : (decimal?)null,
            Status = hospedagem.Status ?? "Ativa",
            Observacoes = hospedagem.Observacoes ?? (string?)null
        });
    }

    public async Task<bool> UpdateAsync(Hospedagem hospedagem)
    {
        using var connection = new MySqlConnection(_dbConfig.ConnectionString);

        var sql = @"UPDATE tb_hospedagens 
                    SET Pessoa = @Pessoa, Quarto = @Quarto, Checkin = @Checkin, Checkout = @Checkout, 
                        Diaria = @Diaria, Total = @Total, Status = @Status, Observacoes = @Observacoes 
                    WHERE Codigo = @Codigo";
        var result = await connection.ExecuteAsync(sql, new
        {
            hospedagem.Codigo,
            hospedagem.Pessoa,
            hospedagem.Quarto,
            hospedagem.Checkin,
            Checkout = hospedagem.Checkout.HasValue ? hospedagem.Checkout.Value : (DateTime?)null,
            hospedagem.Diaria,
            Total = hospedagem.Total.HasValue ? hospedagem.Total.Value : (decimal?)null,
            Status = hospedagem.Status ?? "Ativa",
            Observacoes = hospedagem.Observacoes ?? (string?)null
        });
        return result > 0;
    }

    public async Task<bool> DeleteAsync(int codigo)
    {
        using var connection = new MySqlConnection(_dbConfig.ConnectionString);

        var sql = "DELETE FROM tb_hospedagens WHERE Codigo = @Codigo";
        var result = await connection.ExecuteAsync(sql, new { Codigo = codigo });
        return result > 0;
    }
}
