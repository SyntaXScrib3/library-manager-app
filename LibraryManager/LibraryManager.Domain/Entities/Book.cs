namespace LibraryManager.Domain.Entities;
public class Book
{
    public Guid Id { get; }
    public string Title { get; }
    public string Author { get; }
    public int Year { get; }

    public Book(string title, string author, int year)
    {
        Id = Guid.NewGuid();
        Title = title;
        Author = author;
        Year = year;
    }

    public override bool Equals(object obj)
    {
        throw new NotImplementedException();
        //if (obj is not Book other) return false;
        //return Id == other.Id;
    }

    public override int GetHashCode()
    {
        throw new NotImplementedException();
        //return Id.GetHashCode();
    }
}