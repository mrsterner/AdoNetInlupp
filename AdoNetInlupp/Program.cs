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
            Console.WriteLine("2. Add a movie");
            Console.WriteLine("3. Delete a movie");
            Console.WriteLine("0. Exit");
            Console.Write("Choose: ");

            switch (Console.ReadLine())
            {
                case "1":
                    ListMoviesByGenre();
                    break;
                case "2":
                    AddMovie();
                    break;
                case "3":
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
        Console.WriteLine("Available Movies:");
        foreach (var movie in Movie.GetAllMovies())
        {
            Console.WriteLine(movie);
        }
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
        var title = Console.ReadLine();

        if (int.TryParse(title, out var res))
        {
            var name = Movie.GetAllMovies().Select(l => l.Id == res);
            if (Movie.DeleteMovie(res))
            {
                Console.WriteLine($"Deleted {name}.");
            }
            else
            {
                Console.WriteLine("No such movie, movie not deleted.");
            }
        }
        else
        {
            Console.WriteLine("Title cannot be empty.");
        }
    }
}