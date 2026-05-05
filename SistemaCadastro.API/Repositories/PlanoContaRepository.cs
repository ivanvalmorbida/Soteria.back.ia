using Dapper;
using MySql.Data.MySqlClient;
using SistemaCadastro.API.Configuration;
using SistemaCadastro.API.Models;

namespace SistemaCadastro.API.Repositories;

public class PlanoContaRepository : IPlanoContaRepository
{
    private readonly DatabaseConfig _dbConfig;

    public PlanoContaRepository(DatabaseConfig dbConfig)
    {
        _dbConfig = dbConfig;
    }

    public async Task<IEnumerable<PlanoConta>> GetAllAsync()
    {
        using var connection = new MySqlConnection(_dbConfig.ConnectionString);

        var sql = "SELECT Codigo, CodigoPai, Tipo, Rotulo, Descricao FROM tb_plano_conta ORDER BY Rotulo";
        return await connection.QueryAsync<PlanoConta>(sql);
    }

    public async Task<PlanoConta?> GetByIdAsync(int codigo)
    {
        using var connection = new MySqlConnection(_dbConfig.ConnectionString);

        var sql = "SELECT Codigo, CodigoPai, Tipo, Rotulo, Descricao FROM tb_plano_conta WHERE Codigo = @Codigo";
        return await connection.QueryFirstOrDefaultAsync<PlanoConta>(sql, new { Codigo = codigo });
    }

    public async Task<IEnumerable<PlanoConta>> GetByTipoAsync(string tipo)
    {
        using var connection = new MySqlConnection(_dbConfig.ConnectionString);

        var sql = "SELECT Codigo, CodigoPai, Tipo, Rotulo, Descricao FROM tb_plano_conta WHERE Tipo = @Tipo ORDER BY Rotulo";
        return await connection.QueryAsync<PlanoConta>(sql, new { Tipo = tipo });
    }

    public async Task<IEnumerable<PlanoConta>> GetByDescricaoAsync(string descricao)
    {
        using var connection = new MySqlConnection(_dbConfig.ConnectionString);

        var sql = @"SELECT Codigo, CodigoPai, Tipo, Rotulo, Descricao FROM tb_plano_conta
                    WHERE Descricao LIKE CONCAT('%',@Descricao,'%')
                    ORDER BY Descricao";
        return await connection.QueryAsync<PlanoConta>(sql, new { Descricao = descricao });
    }

    public async Task<int> CreateAsync(PlanoConta planoConta)
    {
        using var connection = new MySqlConnection(_dbConfig.ConnectionString);

        var sql = @"INSERT INTO tb_plano_conta (CodigoPai, Tipo, Rotulo, Descricao) 
                    VALUES (@CodigoPai, @Tipo, @Rotulo, @Descricao);
                    SELECT LAST_INSERT_ID();";
        return await connection.ExecuteScalarAsync<int>(sql, new
        {
            CodigoPai = planoConta.CodigoPai.HasValue && planoConta.CodigoPai.Value > 0 ? planoConta.CodigoPai : (int?)null,
            planoConta.Tipo,
            planoConta.Rotulo,
            planoConta.Descricao
        });
    }

    public async Task<bool> UpdateAsync(PlanoConta planoConta)
    {
        using var connection = new MySqlConnection(_dbConfig.ConnectionString);

        var sql = @"UPDATE tb_plano_conta 
                    SET CodigoPai = @CodigoPai, Tipo = @Tipo, Rotulo = @Rotulo, Descricao = @Descricao 
                    WHERE Codigo = @Codigo";
        var result = await connection.ExecuteAsync(sql, new
        {
            planoConta.Codigo,
            CodigoPai = planoConta.CodigoPai.HasValue && planoConta.CodigoPai.Value > 0 ? planoConta.CodigoPai : (int?)null,
            planoConta.Tipo,
            planoConta.Rotulo,
            planoConta.Descricao
        });
        return result > 0;
    }

    public async Task<bool> DeleteAsync(int codigo)
    {
        using var connection = new MySqlConnection(_dbConfig.ConnectionString);

        var sql = "DELETE FROM tb_plano_conta WHERE Codigo = @Codigo";
        var result = await connection.ExecuteAsync(sql, new { Codigo = codigo });
        return result > 0;
    }
}
