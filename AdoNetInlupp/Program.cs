using AdoNetInlupp.Repository;
using Microsoft.Data.SqlClient;

namespace AdoNetInlupp;

class Program
{
    static void Main(string[] args)
    {
        while (true)
        {
            Console.WriteLine();
            Console.WriteLine("1. List movies by genre");
            Console.WriteLine("2. List all movies");
            Console.WriteLine("3. Add a movie");
            Console.WriteLine("4. Delete a movie");
            Console.WriteLine("0. Exit");
            Console.Write("Choose: ");

            switch (Console.ReadLine())
            {
                case "1":
                    ListMoviesByGenre();
                    break;
                case "2":
                    PrintMovies();
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
    }

    static void PrintGenres()
    {
        Console.WriteLine("Available Genres:");
        foreach (var genre in Movie.GetAllGenres())
        {
            Console.WriteLine(genre);
        }
    }
    
    static void PrintMovies()
    {
        var movies = Movie.GetAllMovies();

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

    static void ListMoviesByGenre()
    {
        PrintGenres();
        Console.Write("Select a Genre: ");
        var selected = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(selected))
        {
            Console.WriteLine("No genre entered.");
            return;
        }

        var movies = Movie.GetAllMovies(selected);

        if (movies.Count == 0)
        {
            Console.WriteLine("No movies found.");
            return;
        }

        foreach (var movie in movies)
        {
            Console.WriteLine($"{movie.Title} ({movie.ReleaseYear}) - {movie.GenreName}");
        }
        Console.WriteLine($"Count: {movies.Count}");
    }

    static void AddMovie()
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

        if (Movie.AddMovie(title, year, genre))
        {
            Console.WriteLine($"Added {title} ({year}) to {genre}.");
        }
        else
        {
            Console.WriteLine("No such genre, movie not added.");
        }
    }
    
    static void DeleteMovie()
    {
        PrintMovies();
        Console.Write("Id: ");

        if (!int.TryParse(Console.ReadLine(), out var id))
        {
            Console.WriteLine("Id must be a number.");
            return;
        }

        var movie = Movie.GetAllMovies().FirstOrDefault(m => m.Id == id);

        if (movie == null)
        {
            Console.WriteLine("No such movie, movie not deleted.");
            return;
        }

        if (Movie.DeleteMovie(id))
        {
            Console.WriteLine($"Deleted {movie}.");
        }
        else
        {
            Console.WriteLine("Movie could not be deleted.");
        }
    }
}