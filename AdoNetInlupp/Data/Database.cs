using Microsoft.Data.SqlClient;

namespace AdoNetInlupp.Data;

public static class Database
{
    public static string ConnectionString =>
        new SqlConnectionStringBuilder
        {
            DataSource = "localhost,1433",
            InitialCatalog = "MovieRegistry",
            UserID = "sa",
            Password = Environment.GetEnvironmentVariable("MOVIEDB_PASSWORD")
                       ?? throw new InvalidOperationException("MOVIEDB_PASSWORD is not set."),
            TrustServerCertificate = true
        }.ConnectionString;
}