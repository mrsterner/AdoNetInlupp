using AdoNetInlupp.Data;
using AdoNetInlupp.UI;

namespace AdoNetInlupp;

internal class Program
{
    private static void Main(string[] args)
    {
        var repository = new MovieRepository(Database.ConnectionString);
        var menu = new ConsoleMenu(repository);
        menu.Run();
    }
}