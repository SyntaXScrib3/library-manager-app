
namespace LibraryManager.Domain.Entities;
public class Book
{
    public int Id { get; }
    public string Title { get; } = string.Empty;
    public string Author { get; } = string.Empty;
    public string Year { get; } = string.Empty;


    public Book(string title, string author, string year)
    {
        Id = GenerateId();
        Title = title;
        Author = author;
        Year = year;
    }

    private int GenerateId()
    {
        throw new NotImplementedException();
    }
}
