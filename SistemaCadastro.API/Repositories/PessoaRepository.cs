using Dapper;
using MySql.Data.MySqlClient;
using SistemaCadastro.API.Configuration;
using SistemaCadastro.API.Models;

namespace SistemaCadastro.API.Repositories;

public class PessoaRepository : IPessoaRepository
{
    private readonly DatabaseConfig _dbConfig;

    public PessoaRepository(DatabaseConfig dbConfig)
    {
        _dbConfig = dbConfig;
    }

    public async Task<int> CreateAsync(Pessoa pessoa)
    {
        using var connection = new MySqlConnection(_dbConfig.ConnectionString);
        
        var sql = @"INSERT INTO tb_pessoa (tipo, nome, cep, estado, cidade, bairro, endereco, numero, complemento, obs, cadastro)
                    VALUES (@Tipo, @Nome, @Cep, @Estado, @Cidade, @Bairro, @Endereco, @Numero, @Complemento, @Obs, @Cadastro);
                    SELECT LAST_INSERT_ID();";
        
        pessoa.Cadastro = DateTime.Now;
        var id = await connection.ExecuteScalarAsync<int>(sql, pessoa);
        return id;
    }

    public async Task<Pessoa?> GetByIdAsync(int codigo)
    {
        using var connection = new MySqlConnection(_dbConfig.ConnectionString);
        
        var sql = "SELECT * FROM tb_pessoa WHERE codigo = @Codigo";
        var pessoa = await connection.QueryFirstOrDefaultAsync<Pessoa>(sql, new { Codigo = codigo });
        
        return pessoa;
    }

    public async Task<IEnumerable<Pessoa>> GetAllAsync()
    {
        using var connection = new MySqlConnection(_dbConfig.ConnectionString);
        
        var sql = "SELECT * FROM tb_pessoa ORDER BY cadastro DESC";
        var pessoas = await connection.QueryAsync<Pessoa>(sql);
        
        return pessoas;
    }

    public async Task<IEnumerable<Pessoa>> SearchAsync(string searchTerm)
    {
        using var connection = new MySqlConnection(_dbConfig.ConnectionString);
        
        var sql = @"SELECT p.* FROM tb_pessoa p
                    LEFT JOIN tb_pessoa_fisica pf ON p.codigo = pf.pessoa
                    LEFT JOIN tb_pessoa_juridica pj ON p.codigo = pj.Pessoa
                    WHERE p.nome LIKE @SearchTerm 
                    OR pf.cpf LIKE @SearchTerm 
                    OR pj.cnpj LIKE @SearchTerm
                    ORDER BY p.cadastro DESC";
        
        var pessoas = await connection.QueryAsync<Pessoa>(sql, new { SearchTerm = $"%{searchTerm}%" });
        
        return pessoas;
    }

    public async Task<bool> UpdateAsync(Pessoa pessoa)
    {
        using var connection = new MySqlConnection(_dbConfig.ConnectionString);
        
        var sql = @"UPDATE tb_pessoa 
                    SET tipo = @Tipo, nome = @Nome, cep = @Cep, estado = @Estado, 
                        cidade = @Cidade, bairro = @Bairro, endereco = @Endereco, 
                        numero = @Numero, complemento = @Complemento, obs = @Obs
                    WHERE codigo = @Codigo";
        
        var result = await connection.ExecuteAsync(sql, pessoa);
        return result > 0;
    }

    public async Task<bool> DeleteAsync(int codigo)
    {
        using var connection = new MySqlConnection(_dbConfig.ConnectionString);
        
        var sql = "DELETE FROM tb_pessoa WHERE codigo = @Codigo";
        var result = await connection.ExecuteAsync(sql, new { Codigo = codigo });
        
        return result > 0;
    }

    public async Task<IEnumerable<Pessoa>> GetByTipoAsync(char tipo)
    {
        using var connection = new MySqlConnection(_dbConfig.ConnectionString);
        
        var sql = "SELECT * FROM tb_pessoa WHERE tipo = @Tipo ORDER BY cadastro DESC";
        var pessoas = await connection.QueryAsync<Pessoa>(sql, new { Tipo = tipo });
        
        return pessoas;
    }
}

public class PessoaFisicaRepository : IPessoaFisicaRepository
{
    private readonly DatabaseConfig _dbConfig;

    public PessoaFisicaRepository(DatabaseConfig dbConfig)
    {
        _dbConfig = dbConfig;
    }

    public async Task<int> CreateAsync(PessoaFisica pessoaFisica)
    {
        using var connection = new MySqlConnection(_dbConfig.ConnectionString);
        
        var sql = @"INSERT INTO tb_pessoa_fisica 
                    (pessoa, nascimento, cidadenasc, ufnasc, nacionalidade, sexo, cpf, 
                     identidade, orgaoidentidade, ufidentidade, estadocivil, conjuge, profissao, ctps, pis)
                    VALUES 
                    (@Pessoa, @Nascimento, @CidadeNasc, @UfNasc, @Nacionalidade, @Sexo, @Cpf, 
                     @Identidade, @OrgaoIdentidade, @UfIdentidade, @EstadoCivil, @Conjuge, @Profissao, @Ctps, @Pis)";
        
        var result = await connection.ExecuteAsync(sql, pessoaFisica);
        return result;
    }

    public async Task<PessoaFisica?> GetByPessoaIdAsync(int pessoaId)
    {
        using var connection = new MySqlConnection(_dbConfig.ConnectionString);
        
        var sql = "SELECT * FROM tb_pessoa_fisica WHERE pessoa = @PessoaId";
        var pessoaFisica = await connection.QueryFirstOrDefaultAsync<PessoaFisica>(sql, new { PessoaId = pessoaId });
        
        return pessoaFisica;
    }

    public async Task<bool> UpdateAsync(PessoaFisica pessoaFisica)
    {
        using var connection = new MySqlConnection(_dbConfig.ConnectionString);
        
        var sql = @"UPDATE tb_pessoa_fisica 
                    SET nascimento = @Nascimento, cidadenasc = @CidadeNasc, ufnasc = @UfNasc, 
                        nacionalidade = @Nacionalidade, sexo = @Sexo, cpf = @Cpf, 
                        identidade = @Identidade, orgaoidentidade = @OrgaoIdentidade, 
                        ufidentidade = @UfIdentidade, estadocivil = @EstadoCivil, 
                        conjuge = @Conjuge, profissao = @Profissao, ctps = @Ctps, pis = @Pis
                    WHERE pessoa = @Pessoa";
        
        var result = await connection.ExecuteAsync(sql, pessoaFisica);
        return result > 0;
    }

    public async Task<bool> DeleteAsync(int pessoaId)
    {
        using var connection = new MySqlConnection(_dbConfig.ConnectionString);
        
        var sql = "DELETE FROM tb_pessoa_fisica WHERE pessoa = @PessoaId";
        var result = await connection.ExecuteAsync(sql, new { PessoaId = pessoaId });
        
        return result > 0;
    }
}

public class PessoaJuridicaRepository : IPessoaJuridicaRepository
{
    private readonly DatabaseConfig _dbConfig;

    public PessoaJuridicaRepository(DatabaseConfig dbConfig)
    {
        _dbConfig = dbConfig;
    }

    public async Task<int> CreateAsync(PessoaJuridica pessoaJuridica)
    {
        using var connection = new MySqlConnection(_dbConfig.ConnectionString);
        
        var sql = @"INSERT INTO tb_pessoa_juridica 
                    (Pessoa, razaosocial, cnpj, incricaoestadual, atividade, homepage, representante)
                    VALUES 
                    (@Pessoa, @RazaoSocial, @Cnpj, @InscricaoEstadual, @Atividade, @Homepage, @Representante)";
        
        var result = await connection.ExecuteAsync(sql, pessoaJuridica);
        return result;
    }

    public async Task<PessoaJuridica?> GetByPessoaIdAsync(int pessoaId)
    {
        using var connection = new MySqlConnection(_dbConfig.ConnectionString);
        
        var sql = "SELECT * FROM tb_pessoa_juridica WHERE Pessoa = @PessoaId";
        var pessoaJuridica = await connection.QueryFirstOrDefaultAsync<PessoaJuridica>(sql, new { PessoaId = pessoaId });
        
        return pessoaJuridica;
    }

    public async Task<bool> UpdateAsync(PessoaJuridica pessoaJuridica)
    {
        using var connection = new MySqlConnection(_dbConfig.ConnectionString);
        
        var sql = @"UPDATE tb_pessoa_juridica 
                    SET razaosocial = @RazaoSocial, cnpj = @Cnpj, 
                        incricaoestadual = @InscricaoEstadual, atividade = @Atividade, 
                        homepage = @Homepage, representante = @Representante
                    WHERE Pessoa = @Pessoa";
        
        var result = await connection.ExecuteAsync(sql, pessoaJuridica);
        return result > 0;
    }

    public async Task<bool> DeleteAsync(int pessoaId)
    {
        using var connection = new MySqlConnection(_dbConfig.ConnectionString);
        
        var sql = "DELETE FROM tb_pessoa_juridica WHERE Pessoa = @PessoaId";
        var result = await connection.ExecuteAsync(sql, new { PessoaId = pessoaId });
        
        return result > 0;
    }
}
