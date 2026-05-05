using Dapper;
using MySql.Data.MySqlClient;
using SistemaCadastro.API.Configuration;
using SistemaCadastro.API.Models;

namespace SistemaCadastro.API.Repositories;

public class LanctoContabilRepository : ILanctoContabilRepository
{
    private readonly DatabaseConfig _dbConfig;

    public LanctoContabilRepository(DatabaseConfig dbConfig)
    {
        _dbConfig = dbConfig;
    }

    public async Task<IEnumerable<LanctoContabil>> GetAllAsync()
    {
        using var connection = new MySqlConnection(_dbConfig.ConnectionString);

        var sql = "SELECT Codigo, Pessoa, CentroCusto, Credito, Debito, Valor, Data, HC, Descricao FROM tb_lancto_contabil ORDER BY Data DESC";
        return await connection.QueryAsync<LanctoContabil>(sql);
    }

    public async Task<LanctoContabil?> GetByIdAsync(int codigo)
    {
        using var connection = new MySqlConnection(_dbConfig.ConnectionString);

        var sql = "SELECT Codigo, Pessoa, CentroCusto, Credito, Debito, Valor, Data, HC, Descricao FROM tb_lancto_contabil WHERE Codigo = @Codigo";
        return await connection.QueryFirstOrDefaultAsync<LanctoContabil>(sql, new { Codigo = codigo });
    }

    public async Task<int> CreateAsync(LanctoContabil lancto)
    {
        using var connection = new MySqlConnection(_dbConfig.ConnectionString);

        var sql = @"INSERT INTO tb_lancto_contabil (Pessoa, CentroCusto, Credito, Debito, Valor, Data, HC, Descricao) 
                    VALUES (@Pessoa, @CentroCusto, @Credito, @Debito, @Valor, @Data, @HC, @Descricao);
                    SELECT LAST_INSERT_ID();";
        return await connection.ExecuteScalarAsync<int>(sql, new
        {
            lancto.Pessoa,
            lancto.CentroCusto,
            lancto.Credito,
            lancto.Debito,
            lancto.Valor,
            lancto.Data,
            lancto.HC,
            lancto.Descricao
        });
    }

    public async Task<bool> UpdateAsync(LanctoContabil lancto)
    {
        using var connection = new MySqlConnection(_dbConfig.ConnectionString);

        var sql = @"UPDATE tb_lancto_contabil 
                    SET Pessoa = @Pessoa, CentroCusto = @CentroCusto, Credito = @Credito, Debito = @Debito, 
                        Valor = @Valor, Data = @Data, HC = @HC, Descricao = @Descricao 
                    WHERE Codigo = @Codigo";
        var result = await connection.ExecuteAsync(sql, new
        {
            lancto.Codigo,
            lancto.Pessoa,
            lancto.CentroCusto,
            lancto.Credito,
            lancto.Debito,
            lancto.Valor,
            lancto.Data,
            lancto.HC,
            lancto.Descricao
        });
        return result > 0;
    }

    public async Task<bool> DeleteAsync(int codigo)
    {
        using var connection = new MySqlConnection(_dbConfig.ConnectionString);

        var sql = "DELETE FROM tb_lancto_contabil WHERE Codigo = @Codigo";
        var result = await connection.ExecuteAsync(sql, new { Codigo = codigo });
        return result > 0;
    }
}
