namespace LibraryManager.Domain.Entities;

public class User
{
    public Guid Id { get; }
    public string PersonalId { get; }
    public string FirstName { get; }
    public string LastName { get; }
    public DateTime Birthdate { get; }

    public HashSet<Book> BorrowedBooks { get; }

    public User(string firstName, string lastName, DateTime birthdate, string personalId)
    {
        Id = Guid.NewGuid();
        FirstName = firstName;
        LastName = lastName;
        Birthdate = birthdate;
        PersonalId = personalId;
        BorrowedBooks = new HashSet<Book>();
    }

    public bool HasBook(Book book) => BorrowedBooks.Contains(book);

    public void BorrowBook(Book book) => BorrowedBooks.Add(book);

    public void ReturnBook(Book book) => BorrowedBooks.Remove(book);
}