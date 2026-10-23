using System.Data;
using Microsoft.Data.SqlClient;

namespace AdoNetInlupp.Data;

public class DatabaseInitializer
{
    private readonly string connectionString;

    public DatabaseInitializer(string connectionString)
    {
        this.connectionString = connectionString;
    }

    public void Initialize()
    {
        var builder = new SqlConnectionStringBuilder(connectionString);
        var databaseName = builder.InitialCatalog;
        builder.InitialCatalog = "master";

        CreateDatabase(builder.ConnectionString, databaseName);
        CreateTablesAndSeed();
    }

    private static void CreateDatabase(string masterConnectionString, string databaseName)
    {
        const string sql = @"
            IF DB_ID(@databaseName) IS NULL
            BEGIN
                DECLARE @createSql NVARCHAR(MAX) = N'CREATE DATABASE ' + QUOTENAME(@databaseName);
                EXEC (@createSql);
            END";

        using var connection = new SqlConnection(masterConnectionString);
        using var command = new SqlCommand(sql, connection);
        command.Parameters.Add("@databaseName", SqlDbType.NVarChar, 128).Value = databaseName;
        connection.Open();
        command.ExecuteNonQuery();
    }

    private void CreateTablesAndSeed()
    {
        const string sql = @"
            IF OBJECT_ID('dbo.Genre', 'U') IS NULL
            BEGIN
                CREATE TABLE Genre
                (
                    GenreId INT IDENTITY (1,1) PRIMARY KEY,
                    Name    NVARCHAR(50) NOT NULL UNIQUE
                );
            END

            IF OBJECT_ID('dbo.Movie', 'U') IS NULL
            BEGIN
                CREATE TABLE Movie
                (
                    MovieId     INT IDENTITY (1,1) PRIMARY KEY,
                    Title       NVARCHAR(200) NOT NULL,
                    ReleaseYear INT           NOT NULL,
                    GenreId     INT           NOT NULL,
                    CONSTRAINT FK_Movie_Genre FOREIGN KEY (GenreId) REFERENCES Genre (GenreId)
                );
            END

            IF NOT EXISTS (SELECT 1 FROM Genre)
            BEGIN
                INSERT INTO Genre (Name)
                VALUES ('Sci-Fi'), ('Horror'), ('Action'), ('Comedy'), ('Drama');
            END

            IF NOT EXISTS (SELECT 1 FROM Movie)
            BEGIN
                INSERT INTO Movie (Title, ReleaseYear, GenreId)
                VALUES ('Alien', 1979, (SELECT GenreId FROM Genre WHERE Name = 'Sci-Fi')),
                       ('Blade Runner', 1982, (SELECT GenreId FROM Genre WHERE Name = 'Sci-Fi')),
                       ('The Shining', 1980, (SELECT GenreId FROM Genre WHERE Name = 'Horror')),
                       ('Die Hard', 1988, (SELECT GenreId FROM Genre WHERE Name = 'Action')),
                       ('Ghostbusters', 1984, (SELECT GenreId FROM Genre WHERE Name = 'Comedy')),
                       ('The Godfather', 1972, (SELECT GenreId FROM Genre WHERE Name = 'Drama'));
            END";

        using var connection = new SqlConnection(connectionString);
        using var command = new SqlCommand(sql, connection);
        connection.Open();
        command.ExecuteNonQuery();
    }
}