using Dapper;
using MySql.Data.MySqlClient;
using SistemaCadastro.API.Configuration;
using SistemaCadastro.API.Models;

namespace SistemaCadastro.API.Repositories;

public class EstadoRepository : IEstadoRepository
{
    private readonly DatabaseConfig _dbConfig;

    public EstadoRepository(DatabaseConfig dbConfig)
    {
        _dbConfig = dbConfig;
    }

    public async Task<IEnumerable<Estado>> GetAllAsync()
    {
        using var connection = new MySqlConnection(_dbConfig.ConnectionString);
        
        var sql = "SELECT * FROM tb_estado ORDER BY nome";
        var estados = await connection.QueryAsync<Estado>(sql);
        
        return estados;
    }

    public async Task<Estado?> GetByIdAsync(int codigo)
    {
        using var connection = new MySqlConnection(_dbConfig.ConnectionString);
        
        var sql = "SELECT * FROM tb_estado WHERE codigo = @Codigo";
        var estado = await connection.QueryFirstOrDefaultAsync<Estado>(sql, new { Codigo = codigo });
        
        return estado;
    }
}

public class CidadeRepository : ICidadeRepository
{
    private readonly DatabaseConfig _dbConfig;

    public CidadeRepository(DatabaseConfig dbConfig)
    {
        _dbConfig = dbConfig;
    }

    public async Task<IEnumerable<Cidade>> GetAllAsync()
    {
        using var connection = new MySqlConnection(_dbConfig.ConnectionString);
        
        var sql = "SELECT * FROM tb_cidade ORDER BY nome";
        var cidades = await connection.QueryAsync<Cidade>(sql);
        
        return cidades;
    }

    public async Task<IEnumerable<Cidade>> GetByEstadoAsync(int estadoId)
    {
        using var connection = new MySqlConnection(_dbConfig.ConnectionString);
        
        // Nota: A tabela tb_cidade não tem relação direta com estado no schema fornecido
        // Em produção, seria necessário adicionar essa relação
        var sql = "SELECT * FROM tb_cidade ORDER BY nome";
        var cidades = await connection.QueryAsync<Cidade>(sql);
        
        return cidades;
    }

    public async Task<Cidade?> GetByIdAsync(int codigo)
    {
        using var connection = new MySqlConnection(_dbConfig.ConnectionString);
        
        var sql = "SELECT * FROM tb_cidade WHERE codigo = @Codigo";
        var cidade = await connection.QueryFirstOrDefaultAsync<Cidade>(sql, new { Codigo = codigo });
        
        return cidade;
    }
}

public class BairroRepository : IBairroRepository
{
    private readonly DatabaseConfig _dbConfig;

    public BairroRepository(DatabaseConfig dbConfig)
    {
        _dbConfig = dbConfig;
    }

    public async Task<int> GetOrCreateAsync(string nome)
    {
        using var connection = new MySqlConnection(_dbConfig.ConnectionString);
        
        // Tenta buscar o bairro existente
        var sql = "SELECT Codigo FROM tb_bairro WHERE Nome = @Nome";
        var codigo = await connection.QueryFirstOrDefaultAsync<int?>(sql, new { Nome = nome });
        
        if (codigo.HasValue && codigo.Value > 0)
        {
            return codigo.Value;
        }
        
        // Se não existe, cria um novo
        var insertSql = "INSERT INTO tb_bairro (Nome) VALUES (@Nome); SELECT LAST_INSERT_ID();";
        var newId = await connection.ExecuteScalarAsync<int>(insertSql, new { Nome = nome });
        
        return newId;
    }

    public async Task<Bairro?> GetByIdAsync(int codigo)
    {
        using var connection = new MySqlConnection(_dbConfig.ConnectionString);
        
        var sql = "SELECT * FROM tb_bairro WHERE Codigo = @Codigo";
        var bairro = await connection.QueryFirstOrDefaultAsync<Bairro>(sql, new { Codigo = codigo });
        
        return bairro;
    }
}

public class EnderecoRepository : IEnderecoRepository
{
    private readonly DatabaseConfig _dbConfig;

    public EnderecoRepository(DatabaseConfig dbConfig)
    {
        _dbConfig = dbConfig;
    }

    public async Task<int> GetOrCreateAsync(string nome)
    {
        using var connection = new MySqlConnection(_dbConfig.ConnectionString);
        
        // Tenta buscar o endereço existente
        var sql = "SELECT Codigo FROM tb_endereco WHERE Nome = @Nome";
        var codigo = await connection.QueryFirstOrDefaultAsync<int?>(sql, new { Nome = nome });
        
        if (codigo.HasValue && codigo.Value > 0)
        {
            return codigo.Value;
        }
        
        // Se não existe, cria um novo
        var insertSql = "INSERT INTO tb_endereco (Nome) VALUES (@Nome); SELECT LAST_INSERT_ID();";
        var newId = await connection.ExecuteScalarAsync<int>(insertSql, new { Nome = nome });
        
        return newId;
    }

    public async Task<Endereco?> GetByIdAsync(int codigo)
    {
        using var connection = new MySqlConnection(_dbConfig.ConnectionString);
        
        var sql = "SELECT * FROM tb_endereco WHERE Codigo = @Codigo";
        var endereco = await connection.QueryFirstOrDefaultAsync<Endereco>(sql, new { Codigo = codigo });
        
        return endereco;
    }
}

public class CepRepository : ICepRepository
{
    private readonly DatabaseConfig _dbConfig;

    public CepRepository(DatabaseConfig dbConfig)
    {
        _dbConfig = dbConfig;
    }

    public async Task<Cep?> GetByCepAsync(string cep)
    {
        using var connection = new MySqlConnection(_dbConfig.ConnectionString);
        
        var sql = "SELECT * FROM tb_cep WHERE cep = @Cep";
        var cepData = await connection.QueryFirstOrDefaultAsync<Cep>(sql, new { Cep = cep });
        
        return cepData;
    }
}

public class PessoaEmailRepository : IPessoaEmailRepository
{
    private readonly DatabaseConfig _dbConfig;

    public PessoaEmailRepository(DatabaseConfig dbConfig)
    {
        _dbConfig = dbConfig;
    }

    public async Task<int> CreateAsync(PessoaEmail email)
    {
        using var connection = new MySqlConnection(_dbConfig.ConnectionString);
        
        var sql = @"INSERT INTO tb_pessoa_email (pessoa, email, descricao)
                    VALUES (@Pessoa, @Email, @Descricao);
                    SELECT LAST_INSERT_ID();";
        
        var id = await connection.ExecuteScalarAsync<int>(sql, email);
        return id;
    }

    public async Task<IEnumerable<PessoaEmail>> GetByPessoaIdAsync(int pessoaId)
    {
        using var connection = new MySqlConnection(_dbConfig.ConnectionString);
        
        var sql = "SELECT * FROM tb_pessoa_email WHERE pessoa = @PessoaId";
        var emails = await connection.QueryAsync<PessoaEmail>(sql, new { PessoaId = pessoaId });
        
        return emails;
    }

    public async Task<bool> DeleteByPessoaIdAsync(int pessoaId)
    {
        using var connection = new MySqlConnection(_dbConfig.ConnectionString);
        
        var sql = "DELETE FROM tb_pessoa_email WHERE pessoa = @PessoaId";
        var result = await connection.ExecuteAsync(sql, new { PessoaId = pessoaId });
        
        return result > 0;
    }
}

public class PessoaFoneRepository : IPessoaFoneRepository
{
    private readonly DatabaseConfig _dbConfig;

    public PessoaFoneRepository(DatabaseConfig dbConfig)
    {
        _dbConfig = dbConfig;
    }

    public async Task<int> CreateAsync(PessoaFone fone)
    {
        using var connection = new MySqlConnection(_dbConfig.ConnectionString);
        
        var sql = @"INSERT INTO tb_pessoa_fone (pessoa, fone, descricao)
                    VALUES (@Pessoa, @Fone, @Descricao);
                    SELECT LAST_INSERT_ID();";
        
        var id = await connection.ExecuteScalarAsync<int>(sql, fone);
        return id;
    }

    public async Task<IEnumerable<PessoaFone>> GetByPessoaIdAsync(int pessoaId)
    {
        using var connection = new MySqlConnection(_dbConfig.ConnectionString);
        
        var sql = "SELECT * FROM tb_pessoa_fone WHERE pessoa = @PessoaId";
        var fones = await connection.QueryAsync<PessoaFone>(sql, new { PessoaId = pessoaId });
        
        return fones;
    }

    public async Task<bool> DeleteByPessoaIdAsync(int pessoaId)
    {
        using var connection = new MySqlConnection(_dbConfig.ConnectionString);
        
        var sql = "DELETE FROM tb_pessoa_fone WHERE pessoa = @PessoaId";
        var result = await connection.ExecuteAsync(sql, new { PessoaId = pessoaId });
        
        return result > 0;
    }
}

public class CBORepository : ICBORepository
{
    private readonly DatabaseConfig _dbConfig;

    public CBORepository(DatabaseConfig dbConfig)
    {
        _dbConfig = dbConfig;
    }

    public async Task<IEnumerable<CBO>> GetAllAsync()
    {
        using var connection = new MySqlConnection(_dbConfig.ConnectionString);
        
        var sql = "SELECT CBO as Codigo, Descricao FROM tb_cbo ORDER BY Descricao";
        var cbos = await connection.QueryAsync<CBO>(sql);
        
        return cbos;
    }

    public async Task<CBO?> GetByCodigoAsync(string codigo)
    {
        using var connection = new MySqlConnection(_dbConfig.ConnectionString);
        
        var sql = "SELECT CBO as Codigo, Descricao FROM tb_cbo WHERE CBO = @Codigo";
        var cbo = await connection.QueryFirstOrDefaultAsync<CBO>(sql, new { Codigo = codigo });
        
        return cbo;
    }
}

public class NacionalidadeRepository : INacionalidadeRepository
{
    private readonly DatabaseConfig _dbConfig;

    public NacionalidadeRepository(DatabaseConfig dbConfig)
    {
        _dbConfig = dbConfig;
    }

    public async Task<IEnumerable<Nacionalidade>> GetAllAsync()
    {
        using var connection = new MySqlConnection(_dbConfig.ConnectionString);
        
        var sql = "SELECT * FROM tb_nacionalidade ORDER BY pais";
        var nacionalidades = await connection.QueryAsync<Nacionalidade>(sql);
        
        return nacionalidades;
    }

    public async Task<Nacionalidade?> GetByIdAsync(int codigo)
    {
        using var connection = new MySqlConnection(_dbConfig.ConnectionString);
        
        var sql = "SELECT * FROM tb_nacionalidade WHERE codigo = @Codigo";
        var nacionalidade = await connection.QueryFirstOrDefaultAsync<Nacionalidade>(sql, new { Codigo = codigo });
        
        return nacionalidade;
    }
}

public class AtividadeEconomicaRepository : IAtividadeEconomicaRepository
{
    private readonly DatabaseConfig _dbConfig;

    public AtividadeEconomicaRepository(DatabaseConfig dbConfig)
    {
        _dbConfig = dbConfig;
    }

    public async Task<IEnumerable<AtividadeEconomica>> GetAllAsync()
    {
        using var connection = new MySqlConnection(_dbConfig.ConnectionString);
        
        var sql = "SELECT * FROM tb_atividade_economica ORDER BY descricao";
        var atividades = await connection.QueryAsync<AtividadeEconomica>(sql);
        
        return atividades;
    }

    public async Task<AtividadeEconomica?> GetByIdAsync(int codigo)
    {
        using var connection = new MySqlConnection(_dbConfig.ConnectionString);
        
        var sql = "SELECT * FROM tb_atividade_economica WHERE codigo = @Codigo";
        var atividade = await connection.QueryFirstOrDefaultAsync<AtividadeEconomica>(sql, new { Codigo = codigo });
        
        return atividade;
    }

    public async Task<IEnumerable<AtividadeEconomica>> GetBySetorAsync(int setor)
    {
        using var connection = new MySqlConnection(_dbConfig.ConnectionString);
        
        var sql = "SELECT * FROM tb_atividade_economica WHERE setor = @Setor ORDER BY descricao";
        var atividades = await connection.QueryAsync<AtividadeEconomica>(sql, new { Setor = setor });
        
        return atividades;
    }
}

public class PessoaEnderecoEletronicoRepository : IPessoaEnderecoEletronicoRepository
{
    private readonly DatabaseConfig _dbConfig;

    public PessoaEnderecoEletronicoRepository(DatabaseConfig dbConfig)
    {
        _dbConfig = dbConfig;
    }

    public async Task<int> CreateAsync(PessoaEnderecoEletronico endereco)
    {
        using var connection = new MySqlConnection(_dbConfig.ConnectionString);
        
        var sql = @"INSERT INTO tb_pessoa_endereco_eletronico (pessoa, endereco, tipo, descricao)
                    VALUES (@Pessoa, @Endereco, @Tipo, @Descricao);
                    SELECT LAST_INSERT_ID();";
        
        var id = await connection.ExecuteScalarAsync<int>(sql, endereco);
        return id;
    }

    public async Task<IEnumerable<PessoaEnderecoEletronico>> GetByPessoaIdAsync(int pessoaId)
    {
        using var connection = new MySqlConnection(_dbConfig.ConnectionString);
        
        var sql = "SELECT * FROM tb_pessoa_endereco_eletronico WHERE pessoa = @PessoaId ORDER BY tipo, codigo";
        var enderecos = await connection.QueryAsync<PessoaEnderecoEletronico>(sql, new { PessoaId = pessoaId });
        
        return enderecos;
    }

    public async Task<bool> DeleteByPessoaIdAsync(int pessoaId)
    {
        using var connection = new MySqlConnection(_dbConfig.ConnectionString);
        
        var sql = "DELETE FROM tb_pessoa_endereco_eletronico WHERE pessoa = @PessoaId";
        var result = await connection.ExecuteAsync(sql, new { PessoaId = pessoaId });
        
        return result > 0;
    }
}

public class PessoaTelefoneRepository : IPessoaTelefoneRepository
{
    private readonly DatabaseConfig _dbConfig;

    public PessoaTelefoneRepository(DatabaseConfig dbConfig)
    {
        _dbConfig = dbConfig;
    }

    public async Task<int> CreateAsync(PessoaTelefone telefone)
    {
        using var connection = new MySqlConnection(_dbConfig.ConnectionString);
        
        var sql = @"INSERT INTO tb_pessoa_telefone (pessoa, tipo, telefone, descricao)
                    VALUES (@Pessoa, @Tipo, @Telefone, @Descricao);
                    SELECT LAST_INSERT_ID();";
        
        var id = await connection.ExecuteScalarAsync<int>(sql, telefone);
        return id;
    }

    public async Task<IEnumerable<PessoaTelefone>> GetByPessoaIdAsync(int pessoaId)
    {
        using var connection = new MySqlConnection(_dbConfig.ConnectionString);
        
        var sql = "SELECT * FROM tb_pessoa_telefone WHERE pessoa = @PessoaId ORDER BY tipo, codigo";
        var telefones = await connection.QueryAsync<PessoaTelefone>(sql, new { PessoaId = pessoaId });
        
        return telefones;
    }

    public async Task<bool> DeleteByPessoaIdAsync(int pessoaId)
    {
        using var connection = new MySqlConnection(_dbConfig.ConnectionString);
        
        var sql = "DELETE FROM tb_pessoa_telefone WHERE pessoa = @PessoaId";
        var result = await connection.ExecuteAsync(sql, new { PessoaId = pessoaId });
        
        return result > 0;
    }
}

