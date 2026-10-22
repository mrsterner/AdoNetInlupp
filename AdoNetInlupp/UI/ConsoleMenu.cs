using AdoNetInlupp.Data;
using AdoNetInlupp.Models;
using Microsoft.Data.SqlClient;

namespace AdoNetInlupp.UI;

public class ConsoleMenu
{
    private readonly MovieRepository _repository;

    public ConsoleMenu(MovieRepository repository)
    {
        _repository = repository;
    }

    public void Run()
    {
        while (true)
        {
            PrintMenu();

            try
            {
                switch (Console.ReadLine())
                {
                    case "1":
                        ListMoviesByGenre();
                        break;
                    case "2":
                        ListAllMovies();
                        break;
                    case "3":
                        AddMovie();
                        break;
                    case "4":
                        DeleteMovie();
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Invalid choice.");
                        break;
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Database error: {ex.Message}");
            }
        }
    }

    private static void PrintMenu()
    {
        Console.WriteLine();
        Console.WriteLine("--- Filmregister ---");
        Console.WriteLine("1. List movies by genre");
        Console.WriteLine("2. List all movies");
        Console.WriteLine("3. Add a movie");
        Console.WriteLine("4. Delete a movie");
        Console.WriteLine("0. Exit");
        Console.Write("Choose: ");
    }

    private void PrintGenres()
    {
        Console.WriteLine("Available genres:");

        foreach (var genre in _repository.GetGenres()) Console.WriteLine(genre.Name);
    }

    private static void PrintMovieTable(List<Movie> movies)
    {
        if (movies.Count == 0)
        {
            Console.WriteLine("No movies in the register.");
            return;
        }

        var idWidth = Math.Max("Id".Length, movies.Max(m => m.Id.ToString().Length));
        var titleWidth = Math.Max("Title".Length, movies.Max(m => m.Title.Length));

        Console.WriteLine();
        Console.WriteLine($"{"Id".PadLeft(idWidth)}  {"Title".PadRight(titleWidth)}  Year  Genre");
        Console.WriteLine(new string('-', idWidth + titleWidth + 24));

        foreach (var movie in movies)
        {
            var id = movie.Id.ToString().PadLeft(idWidth);
            var title = movie.Title.PadRight(titleWidth);
            Console.WriteLine($"{id}  {title}  {movie.ReleaseYear}  {movie.GenreName}");
        }

        Console.WriteLine();
    }

    private void ListAllMovies()
    {
        PrintMovieTable(_repository.GetAll());
    }

    private void ListMoviesByGenre()
    {
        PrintGenres();
        Console.Write("Select a genre: ");
        var genre = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(genre))
        {
            Console.WriteLine("No genre entered.");
            return;
        }

        var movies = _repository.GetByGenre(genre);

        if (movies.Count == 0)
        {
            Console.WriteLine("No movies found.");
            return;
        }

        foreach (var movie in movies) Console.WriteLine(movie);

        Console.WriteLine($"Count: {movies.Count}");
    }

    private void AddMovie()
    {
        Console.Write("Title: ");
        var title = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(title))
        {
            Console.WriteLine("Title cannot be empty.");
            return;
        }

        Console.Write("Release year: ");

        if (!int.TryParse(Console.ReadLine(), out var year))
        {
            Console.WriteLine("Release year must be a number.");
            return;
        }

        PrintGenres();
        Console.Write("Genre: ");
        var genre = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(genre))
        {
            Console.WriteLine("No genre entered.");
            return;
        }

        if (_repository.Add(title, year, genre))
            Console.WriteLine($"Added {title} ({year}) to {genre}.");
        else
            Console.WriteLine("No such genre, movie not added.");
    }

    private void DeleteMovie()
    {
        var movies = _repository.GetAll();
        PrintMovieTable(movies);

        if (movies.Count == 0) return;

        Console.Write("Id: ");

        if (!int.TryParse(Console.ReadLine(), out var id))
        {
            Console.WriteLine("Id must be a number.");
            return;
        }

        var movie = movies.FirstOrDefault(m => m.Id == id);

        if (movie == null)
        {
            Console.WriteLine("No such movie, movie not deleted.");
            return;
        }

        if (_repository.Delete(id))
            Console.WriteLine($"Deleted {movie}.");
        else
            Console.WriteLine("Movie could not be deleted.");
    }
}