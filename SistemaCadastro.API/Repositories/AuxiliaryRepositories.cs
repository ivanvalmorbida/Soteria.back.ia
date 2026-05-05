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
        var sql = "SELECT * FROM tb_cidade where codigo in(select distinct cidade from tb_cep where estado=@Estado) ORDER BY nome";
        var cidades = await connection.QueryAsync<Cidade>(sql, new { Estado = estadoId });
        
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

    public async Task<IEnumerable<Bairro>> GetByNameAsync(string nome)
    {
        using var connection = new MySqlConnection(_dbConfig.ConnectionString);

        var sql = "SELECT * FROM tb_bairro WHERE nome like CONCAT('%',@Nome,'%')";
        var bairros = await connection.QueryAsync<Bairro>(sql, new { Nome = nome });
        return bairros;
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
    public async Task<IEnumerable<Endereco>> GetByNameAsync(string nome)
    {
        using var connection = new MySqlConnection(_dbConfig.ConnectionString);
        
        var sql = "SELECT * FROM tb_endereco WHERE nome like CONCAT('%',@Nome,'%')";
        var endereco = await connection.QueryAsync<Endereco>(sql, new { Nome = nome });
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

        var sql = @"SELECT c.cep AS CepCodigo, c.complemento AS Complemento,
                           c.endereco AS Endereco, e.Nome AS EnderecoNome,
                           c.bairro AS Bairro, b.Nome AS BairroNome,
                           c.cidade AS Cidade, c.estado AS Estado
                    FROM tb_cep c
                    LEFT JOIN tb_endereco e ON e.Codigo = c.endereco
                    LEFT JOIN tb_bairro b ON b.Codigo = c.bairro
                    WHERE c.cep = @Cep";
        var cepData = await connection.QueryFirstOrDefaultAsync<Cep>(sql, new { Cep = cep });

        return cepData;
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

    public async Task<IEnumerable<CBO>> GetByDescricaoAsync(string descricao)
    {
        using var connection = new MySqlConnection(_dbConfig.ConnectionString);

        var sql = "SELECT CBO as Codigo, Descricao FROM tb_cbo WHERE Descricao like CONCAT('%',@Descricao,'%') ORDER BY Descricao";
        var cbos = await connection.QueryAsync<CBO>(sql, new { Descricao = descricao });
        return cbos;
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

    public async Task<IEnumerable<AtividadeEconomica>> GetByDescricaoAsync(string descricao)
    {
        using var connection = new MySqlConnection(_dbConfig.ConnectionString);

        var sql = "SELECT * FROM tb_atividade_economica WHERE descricao like CONCAT('%',@Descricao,'%') ORDER BY descricao";
        var atividades = await connection.QueryAsync<AtividadeEconomica>(sql, new { Descricao = descricao });
        return atividades;
    }

    public async Task<IEnumerable<AtividadeEconomica>> GetBySetorAsync(int setor)
    {
        using var connection = new MySqlConnection(_dbConfig.ConnectionString);
        
        var sql = "SELECT * FROM tb_atividade_economica WHERE setor = @Setor ORDER BY descricao";
        var atividades = await connection.QueryAsync<AtividadeEconomica>(sql, new { Setor = setor });
        
        return atividades;
    }
}

public class AtividadeEconomicaSubsetorRepository : IAtividadeEconomicaSubsetorRepository
{
    private readonly DatabaseConfig _dbConfig;

    public AtividadeEconomicaSubsetorRepository(DatabaseConfig dbConfig)
    {
        _dbConfig = dbConfig;
    }

    public async Task<IEnumerable<AtividadeEconomicaSubsetor>> GetAllAsync()
    {
        using var connection = new MySqlConnection(_dbConfig.ConnectionString);

        var sql = "SELECT * FROM tb_atividade_economica_subsetor ORDER BY subsetor";
        var subsetores = await connection.QueryAsync<AtividadeEconomicaSubsetor>(sql);

        return subsetores;
    }

    public async Task<AtividadeEconomicaSubsetor?> GetByIdAsync(int codigo)
    {
        using var connection = new MySqlConnection(_dbConfig.ConnectionString);

        var sql = "SELECT * FROM tb_atividade_economica_subsetor WHERE codigo = @Codigo";
        var subsetor = await connection.QueryFirstOrDefaultAsync<AtividadeEconomicaSubsetor>(sql, new { Codigo = codigo });

        return subsetor;
    }

    public async Task<IEnumerable<AtividadeEconomicaSubsetor>> GetBySetorAsync(int setor)
    {
        using var connection = new MySqlConnection(_dbConfig.ConnectionString);

        var sql = "SELECT * FROM tb_atividade_economica_subsetor WHERE setor = @Setor ORDER BY descricao";
        var subsetores = await connection.QueryAsync<AtividadeEconomicaSubsetor>(sql, new { Setor = setor });

        return subsetores;
    }

    public async Task<IEnumerable<AtividadeEconomicaSubsetor>> GetBySubSetorAsync(string subsetor)
    {
        using var connection = new MySqlConnection(_dbConfig.ConnectionString);

        var sql = "SELECT * FROM tb_atividade_economica_subsetor WHERE subsetor like CONCAT('%',@Subsetor,'%') ORDER BY subsetor";
        var subsetores = await connection.QueryAsync<AtividadeEconomicaSubsetor>(sql, new { Subsetor = subsetor });
        return subsetores;
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

public class CentroCustoRepository : ICentroCustoRepository
{
    private readonly DatabaseConfig _dbConfig;

    public CentroCustoRepository(DatabaseConfig dbConfig)
    {
        _dbConfig = dbConfig;
    }

    public async Task<IEnumerable<CentroCusto>> GetAllAsync()
    {
        using var connection = new MySqlConnection(_dbConfig.ConnectionString);

        var sql = "SELECT Codigo, Descricao FROM tb_centro_custo ORDER BY Descricao";
        var centros = await connection.QueryAsync<CentroCusto>(sql);
        return centros;
    }

    public async Task<CentroCusto?> GetByIdAsync(int codigo)
    {
        using var connection = new MySqlConnection(_dbConfig.ConnectionString);

        var sql = "SELECT Codigo, Descricao FROM tb_centro_custo WHERE Codigo = @Codigo";
        return await connection.QueryFirstOrDefaultAsync<CentroCusto>(sql, new { Codigo = codigo });
    }

    public async Task<IEnumerable<CentroCusto>> GetByDescricaoAsync(string descricao)
    {
        using var connection = new MySqlConnection(_dbConfig.ConnectionString);

        var sql = @"SELECT Codigo, Descricao FROM tb_centro_custo
                    WHERE Descricao LIKE CONCAT('%',@Descricao,'%')
                    ORDER BY Descricao";
        return await connection.QueryAsync<CentroCusto>(sql, new { Descricao = descricao });
    }

    public async Task<int> CreateAsync(CentroCusto centroCusto)
    {
        using var connection = new MySqlConnection(_dbConfig.ConnectionString);

        var codigo = centroCusto.Codigo;
        if (codigo <= 0)
        {
            var nextSql = "SELECT COALESCE(MAX(Codigo), 0) + 1 FROM tb_centro_custo";
            codigo = await connection.ExecuteScalarAsync<int>(nextSql);
        }

        var sql = "INSERT INTO tb_centro_custo (Codigo, Descricao) VALUES (@Codigo, @Descricao)";
        await connection.ExecuteAsync(sql, new { Codigo = codigo, centroCusto.Descricao });
        return codigo;
    }

    public async Task<bool> UpdateAsync(CentroCusto centroCusto)
    {
        using var connection = new MySqlConnection(_dbConfig.ConnectionString);

        var sql = "UPDATE tb_centro_custo SET Descricao = @Descricao WHERE Codigo = @Codigo";
        var result = await connection.ExecuteAsync(sql, new { centroCusto.Codigo, centroCusto.Descricao });
        return result > 0;
    }

    public async Task<bool> DeleteAsync(int codigo)
    {
        using var connection = new MySqlConnection(_dbConfig.ConnectionString);

        var sql = "DELETE FROM tb_centro_custo WHERE Codigo = @Codigo";
        var result = await connection.ExecuteAsync(sql, new { Codigo = codigo });
        return result > 0;
    }
}

public class EstadoCivilRepository : IEstadoCivilRepository
{
    private readonly DatabaseConfig _dbConfig;

    public EstadoCivilRepository(DatabaseConfig dbConfig)
    {
        _dbConfig = dbConfig;
    }

    public async Task<IEnumerable<EstadoCivil>> GetAllAsync()
    {
        using var connection = new MySqlConnection(_dbConfig.ConnectionString);
        
        var sql = "SELECT * FROM tb_estado_civil ORDER BY Descricao";
        var EstadoCivils = await connection.QueryAsync<EstadoCivil>(sql);
        
        return EstadoCivils;
    }

    public async Task<EstadoCivil?> GetByIdAsync(int codigo)
    {
        using var connection = new MySqlConnection(_dbConfig.ConnectionString);
        
        var sql = "SELECT * FROM tb_estado_civil WHERE codigo = @Codigo";
        var EstadoCivils = await connection.QueryFirstOrDefaultAsync<EstadoCivil>(sql, new { Codigo = codigo });
        
        return EstadoCivils;
    }
}
