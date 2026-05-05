using Dapper;
using MySql.Data.MySqlClient;
using SistemaCadastro.API.Configuration;
using SistemaCadastro.API.Models;

namespace SistemaCadastro.API.Repositories;

public class HistoricoContabilRepository : IHistoricoContabilRepository
{
    private readonly DatabaseConfig _dbConfig;

    public HistoricoContabilRepository(DatabaseConfig dbConfig)
    {
        _dbConfig = dbConfig;
    }

    public async Task<IEnumerable<HistoricoContabil>> GetAllAsync()
    {
        using var connection = new MySqlConnection(_dbConfig.ConnectionString);

        var sql = "SELECT Codigo, Descricao FROM tb_historico_contabil ORDER BY Descricao";
        return await connection.QueryAsync<HistoricoContabil>(sql);
    }

    public async Task<HistoricoContabil?> GetByIdAsync(int codigo)
    {
        using var connection = new MySqlConnection(_dbConfig.ConnectionString);

        var sql = "SELECT Codigo, Descricao FROM tb_historico_contabil WHERE Codigo = @Codigo";
        return await connection.QueryFirstOrDefaultAsync<HistoricoContabil>(sql, new { Codigo = codigo });
    }

    public async Task<IEnumerable<HistoricoContabil>> GetByDescricaoAsync(string descricao)
    {
        using var connection = new MySqlConnection(_dbConfig.ConnectionString);

        var sql = @"SELECT Codigo, Descricao FROM tb_historico_contabil
                    WHERE Descricao LIKE CONCAT('%',@Descricao,'%')
                    ORDER BY Descricao";
        return await connection.QueryAsync<HistoricoContabil>(sql, new { Descricao = descricao });
    }

    public async Task<int> CreateAsync(HistoricoContabil historico)
    {
        using var connection = new MySqlConnection(_dbConfig.ConnectionString);

        var codigo = historico.Codigo;
        if (codigo <= 0)
        {
            var nextSql = "SELECT COALESCE(MAX(Codigo), 0) + 1 FROM tb_historico_contabil";
            codigo = await connection.ExecuteScalarAsync<int>(nextSql);
        }

        var sql = "INSERT INTO tb_historico_contabil (Codigo, Descricao) VALUES (@Codigo, @Descricao)";
        await connection.ExecuteAsync(sql, new { Codigo = codigo, historico.Descricao });
        return codigo;
    }

    public async Task<bool> UpdateAsync(HistoricoContabil historico)
    {
        using var connection = new MySqlConnection(_dbConfig.ConnectionString);

        var sql = "UPDATE tb_historico_contabil SET Descricao = @Descricao WHERE Codigo = @Codigo";
        var result = await connection.ExecuteAsync(sql, new { historico.Codigo, historico.Descricao });
        return result > 0;
    }

    public async Task<bool> DeleteAsync(int codigo)
    {
        using var connection = new MySqlConnection(_dbConfig.ConnectionString);

        var sql = "DELETE FROM tb_historico_contabil WHERE Codigo = @Codigo";
        var result = await connection.ExecuteAsync(sql, new { Codigo = codigo });
        return result > 0;
    }
}
