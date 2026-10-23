using AdoNetInlupp.Data;
using AdoNetInlupp.UI;
using Microsoft.Data.SqlClient;

namespace AdoNetInlupp;

internal class Program
{
    private static void Main(string[] args)
    {
        try
        {
            new DatabaseInitializer(Database.ConnectionString).Initialize();
        }
        catch (SqlException ex)
        {
            Console.WriteLine($"Could not connect to or set up the database: {ex.Message}");
            return;
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine(ex.Message);
            return;
        }

        var repository = new MovieRepository(Database.ConnectionString);
        var menu = new ConsoleMenu(repository);
        menu.Run();
    }
}