using System.Data;
using Microsoft.Data.SqlClient;

namespace AdoNetInlupp.Repository;

public record Movie(int Id, string Title, int ReleaseYear, string GenreName)
{
    
    private static string ConnectionString =>
        new SqlConnectionStringBuilder
        {
            DataSource = "localhost,1433",
            InitialCatalog = "MovieRegistry",
            UserID = "sa",
            Password = Environment.GetEnvironmentVariable("MOVIEDB_PASSWORD")
                       ?? throw new InvalidOperationException("MOVIEDB_PASSWORD is not set."),
            TrustServerCertificate = true
        }.ConnectionString;

    public static bool AddMovie(string title, int releaseYear, string genreName)
    {
        const string sql = @"
            INSERT INTO Movie (Title, ReleaseYear, GenreId)
            SELECT @title, @releaseYear, Genre.GenreId
            FROM Genre
            WHERE Genre.Name = @genreName";

        using var connection = new SqlConnection(ConnectionString);
        using var command = new SqlCommand(sql, connection);
        command.Parameters.Add("@title", SqlDbType.NVarChar, 200).Value = title;
        command.Parameters.Add("@releaseYear", SqlDbType.Int).Value = releaseYear;
        command.Parameters.Add("@genreName", SqlDbType.NVarChar, 50).Value = genreName;
        connection.Open();

        return command.ExecuteNonQuery() > 0;
    }
    
    public static bool DeleteMovie(int movieId)
    {
        const string sql = "DELETE FROM Movie WHERE MovieId = @movieId";

        using var connection = new SqlConnection(ConnectionString);
        using var command = new SqlCommand(sql, connection);
        command.Parameters.Add("@movieId", SqlDbType.Int).Value = movieId;
        connection.Open();

        return command.ExecuteNonQuery() > 0;
    }

    public static List<Movie> GetAllMovies()
    {
        const string sql = @"
            SELECT Movie.MovieId, Movie.Title, Movie.ReleaseYear, Genre.Name AS GenreName
            FROM Movie
            INNER JOIN Genre ON Genre.GenreId = Movie.GenreId
            ORDER BY Movie.Title";

        var movies = new List<Movie>();

        using var connection = new SqlConnection(ConnectionString);
        using var command = new SqlCommand(sql, connection);
        connection.Open();
        using var reader = command.ExecuteReader();

        while (reader.Read())
        {
            movies.Add(new Movie(
                reader.GetInt32(reader.GetOrdinal("MovieId")),
                reader.GetString(reader.GetOrdinal("Title")),
                reader.GetInt32(reader.GetOrdinal("ReleaseYear")),
                reader.GetString(reader.GetOrdinal("GenreName"))));
        }

        return movies;
    }
    
    public static List<Movie> GetAllMovies(string genreName)
    {
        const string sql = @"
            SELECT Movie.MovieId, Movie.Title, Movie.ReleaseYear, Genre.Name AS GenreName
            FROM Movie
            INNER JOIN Genre ON Genre.GenreId = Movie.GenreId
            WHERE Genre.Name = @genreName
            ORDER BY Movie.Title";

        var movies = new List<Movie>();

        using var connection = new SqlConnection(ConnectionString);
        using var command = new SqlCommand(sql, connection);
        command.Parameters.Add(
            new SqlParameter("@genreName", SqlDbType.NVarChar, 50) { Value = genreName});
        connection.Open();
        using var reader = command.ExecuteReader();

        while (reader.Read())
        {
            movies.Add(new Movie(
                reader.GetInt32(reader.GetOrdinal("MovieId")),
                reader.GetString(reader.GetOrdinal("Title")),
                reader.GetInt32(reader.GetOrdinal("ReleaseYear")),
                reader.GetString(reader.GetOrdinal("GenreName"))));
        }

        return movies;
    }
    
    public static List<string> GetAllGenres()
    {
        const string sql = @"
        SELECT Genre.Name
        FROM Genre
        ORDER BY Genre.Name";

        var genres = new List<string>();

        using var connection = new SqlConnection(ConnectionString);
        using var command = new SqlCommand(sql, connection);
        connection.Open();
        using var reader = command.ExecuteReader();

        while (reader.Read())
        {
            genres.Add(reader.GetString(reader.GetOrdinal("Name")));
        }

        return genres;
    }
}