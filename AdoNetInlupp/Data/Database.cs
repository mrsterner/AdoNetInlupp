using Microsoft.Data.SqlClient;

namespace AdoNetInlupp.Data;

public static class Database
{
    private const string DefaultPassword = "YourStrong!Passw0rd";

    public static string ConnectionString
    {
        get
        {
            var custom = Environment.GetEnvironmentVariable("MOVIEDB_CONNECTION");

            if (!string.IsNullOrWhiteSpace(custom))
            {
                return custom;
            }

            if (OperatingSystem.IsWindows())
            {
                return new SqlConnectionStringBuilder
                {
                    DataSource = "localhost",
                    InitialCatalog = "MovieRegistry",
                    IntegratedSecurity = true,
                    TrustServerCertificate = true
                }.ConnectionString;
            }

            var password = Environment.GetEnvironmentVariable("MOVIEDB_PASSWORD");

            return new SqlConnectionStringBuilder
            {
                DataSource = "localhost,1433",
                InitialCatalog = "MovieRegistry",
                UserID = "sa",
                Password = string.IsNullOrWhiteSpace(password) ? DefaultPassword : password,
                TrustServerCertificate = true
            }.ConnectionString;
        }
    }
}