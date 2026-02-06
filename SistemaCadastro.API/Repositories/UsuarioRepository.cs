using Dapper;
using MySql.Data.MySqlClient;
using SistemaCadastro.API.Configuration;
using SistemaCadastro.API.Models;

namespace SistemaCadastro.API.Repositories;

public interface IUsuarioRepository
{
    Task<int> CreateAsync(Usuario usuario);
    Task<Usuario?> GetByIdAsync(int codigo);
    Task<Usuario?> GetByUsuarioAsync(string usuario);
    Task<bool> AlterarSenhaAsync(int usuarioId, string novaSenhaHash);
    Task<IEnumerable<Usuario>> GetAllAsync();
}

public class UsuarioRepository : IUsuarioRepository
{
    private readonly DatabaseConfig _dbConfig;

    public UsuarioRepository(DatabaseConfig dbConfig)
    {
        _dbConfig = dbConfig;
    }

    public async Task<int> CreateAsync(Usuario usuario)
    {
        using var connection = new MySqlConnection(_dbConfig.ConnectionString);
        
        var sql = @"INSERT INTO tb_usuario (usuario, senha, tipo, pessoa, cadastro)
                    VALUES (@UsuarioLogin, @Senha, @Tipo, @Pessoa, @Cadastro);
                    SELECT LAST_INSERT_ID();";
        
        var id = await connection.ExecuteScalarAsync<int>(sql, usuario);
        return id;
    }

    public async Task<Usuario?> GetByIdAsync(int codigo)
    {
        using var connection = new MySqlConnection(_dbConfig.ConnectionString);
        
        var sql = "SELECT codigo, usuario as UsuarioLogin, senha, tipo, pessoa, cadastro FROM tb_usuario WHERE codigo = @Codigo";
        var usuario = await connection.QueryFirstOrDefaultAsync<Usuario>(sql, new { Codigo = codigo });
        
        return usuario;
    }

    public async Task<Usuario?> GetByUsuarioAsync(string usuario)
    {
        using var connection = new MySqlConnection(_dbConfig.ConnectionString);
        
        var sql = "SELECT codigo, usuario as UsuarioLogin, senha, tipo, pessoa, cadastro FROM tb_usuario WHERE usuario = @Usuario";
        var user = await connection.QueryFirstOrDefaultAsync<Usuario>(sql, new { Usuario = usuario });
        
        return user;
    }

    public async Task<bool> AlterarSenhaAsync(int usuarioId, string novaSenhaHash)
    {
        using var connection = new MySqlConnection(_dbConfig.ConnectionString);
        
        var sql = "UPDATE tb_usuario SET senha = @Senha WHERE codigo = @Codigo";
        var result = await connection.ExecuteAsync(sql, new { Senha = novaSenhaHash, Codigo = usuarioId });
        
        return result > 0;
    }

    public async Task<IEnumerable<Usuario>> GetAllAsync()
    {
        using var connection = new MySqlConnection(_dbConfig.ConnectionString);
        
        var sql = "SELECT codigo, usuario as UsuarioLogin, tipo, pessoa, cadastro FROM tb_usuario ORDER BY cadastro DESC";
        var usuarios = await connection.QueryAsync<Usuario>(sql);
        
        return usuarios;
    }
}
