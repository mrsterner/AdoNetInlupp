using System.Data;
using AdoNetInlupp.Models;
using Microsoft.Data.SqlClient;

namespace AdoNetInlupp.Data;

public class MovieRepository
{
    private readonly string connectionString;

    public MovieRepository(string connectionString)
    {
        this.connectionString = connectionString;
    }

    public List<Movie> GetAll()
    {
        const string sql = @"
            SELECT Movie.MovieId, Movie.Title, Movie.ReleaseYear, Genre.Name AS GenreName
            FROM Movie
            INNER JOIN Genre ON Genre.GenreId = Movie.GenreId
            ORDER BY Movie.Title";

        using var connection = new SqlConnection(connectionString);
        using var command = new SqlCommand(sql, connection);
        connection.Open();
        using var reader = command.ExecuteReader();

        return ReadMovies(reader);
    }

    public List<Movie> GetByGenre(string genreName)
    {
        const string sql = @"
            SELECT Movie.MovieId, Movie.Title, Movie.ReleaseYear, Genre.Name AS GenreName
            FROM Movie
            INNER JOIN Genre ON Genre.GenreId = Movie.GenreId
            WHERE Genre.Name = @genreName
            ORDER BY Movie.Title";

        using var connection = new SqlConnection(connectionString);
        using var command = new SqlCommand(sql, connection);
        command.Parameters.Add(
            new SqlParameter("@genreName", SqlDbType.NVarChar, 50) { Value = genreName });
        connection.Open();
        using var reader = command.ExecuteReader();

        return ReadMovies(reader);
    }

    public List<Genre> GetGenres()
    {
        const string sql = "SELECT GenreId, Name FROM Genre ORDER BY Name";

        var genres = new List<Genre>();

        using var connection = new SqlConnection(connectionString);
        using var command = new SqlCommand(sql, connection);
        connection.Open();
        using var reader = command.ExecuteReader();

        while (reader.Read())
            genres.Add(new Genre(
                reader.GetInt32(reader.GetOrdinal("GenreId")),
                reader.GetString(reader.GetOrdinal("Name"))));

        return genres;
    }

    public bool Add(string title, int releaseYear, string genreName)
    {
        const string sql = @"
            INSERT INTO Movie (Title, ReleaseYear, GenreId)
            SELECT @title, @releaseYear, Genre.GenreId
            FROM Genre
            WHERE Genre.Name = @genreName";

        using var connection = new SqlConnection(connectionString);
        using var command = new SqlCommand(sql, connection);
        command.Parameters.Add("@title", SqlDbType.NVarChar, 200).Value = title;
        command.Parameters.Add("@releaseYear", SqlDbType.Int).Value = releaseYear;
        command.Parameters.Add("@genreName", SqlDbType.NVarChar, 50).Value = genreName;
        connection.Open();

        return command.ExecuteNonQuery() > 0;
    }

    public bool Delete(int movieId)
    {
        const string sql = "DELETE FROM Movie WHERE MovieId = @movieId";

        using var connection = new SqlConnection(connectionString);
        using var command = new SqlCommand(sql, connection);
        command.Parameters.Add("@movieId", SqlDbType.Int).Value = movieId;
        connection.Open();

        return command.ExecuteNonQuery() > 0;
    }

    private static List<Movie> ReadMovies(SqlDataReader reader)
    {
        var movies = new List<Movie>();

        while (reader.Read())
            movies.Add(new Movie(
                reader.GetInt32(reader.GetOrdinal("MovieId")),
                reader.GetString(reader.GetOrdinal("Title")),
                reader.GetInt32(reader.GetOrdinal("ReleaseYear")),
                reader.GetString(reader.GetOrdinal("GenreName"))));

        return movies;
    }
}