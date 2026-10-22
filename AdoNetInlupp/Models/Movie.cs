namespace AdoNetInlupp.Models;

public record Movie(int Id, string Title, int ReleaseYear, string GenreName)
{
    public override string ToString()
    {
        return $"{Title} ({ReleaseYear}) - {GenreName}";
    }
}