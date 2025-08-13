
namespace LibraryManager.Domain.Entities;
public sealed class Book : IComparable<Book>
{
    public Guid Id { get; }
    public string Title { get; }
    public string Author { get; }
    public int Year { get; }


    public Book(string title, string author, int year) : this(Guid.NewGuid(), title, author, year) { }

    public Book(Guid id, string title, string author, int year)
    {
        if (id == Guid.Empty)
            throw new ArgumentException("Id cannot be empty.", nameof(id));
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Title cannot be empty.", nameof(title));
        if (string.IsNullOrWhiteSpace(author))
            throw new ArgumentException("Author cannot be empty.", nameof(author));

        var maxYear = DateTime.UtcNow.Year + 1;
        if (year < 1450 || year > maxYear)
            throw new ArgumentOutOfRangeException(nameof(year), $"Year must be between 1450 and {maxYear}.");


        Id = id;
        Title = title.Trim();
        Author = author.Trim();
        Year = year;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(this, obj)) return true;
        return obj is Book other && Id == other.Id;
    }

    public override int GetHashCode() => Id.GetHashCode();

    public int CompareTo(Book? other)
    {
        if (other is null) return 1;

        int c = string.Compare(Title, other.Title, StringComparison.OrdinalIgnoreCase);
        if (c != 0) return c;

        c = string.Compare(Author, other.Author, StringComparison.OrdinalIgnoreCase);
        if (c != 0) return c;

        c = Year.CompareTo(other.Year);
        if (c != 0) return c;

        return Id.CompareTo(other.Id);
    }
    public override string ToString() => $"{Title} by {Author} ({Year})";

}
