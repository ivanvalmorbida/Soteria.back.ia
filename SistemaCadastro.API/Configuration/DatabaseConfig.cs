namespace SistemaCadastro.API.Configuration;

public class DatabaseConfig
{
    public string ConnectionString { get; }

    public DatabaseConfig(string connectionString)
    {
        ConnectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
    }
}
