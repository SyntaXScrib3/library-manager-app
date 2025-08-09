namespace LibraryManager.Domain.Entities;

public class User
{
    public Guid Id { get; }
    public string PersonalId { get; }
    public string FirstName { get; }
    public string LastName { get; }
    public DateTime Birthdate { get; }

    private readonly List<Loan> _loans = new();
    public IReadOnlyList<Loan> Loans => _loans;

    public User(string firstName, string lastName, DateTime birthdate, string personalId)
    {
        Id = Guid.NewGuid();
        FirstName = firstName;
        LastName = lastName;
        Birthdate = birthdate;
        PersonalId = personalId;
    }

    public Loan? GetLoanFor(Book book) => _loans.FirstOrDefault(loan => loan.Book.Equals(book));

    public bool HasBook(Book book) => _loans.Any(loan => loan.Book.Equals(book));

    public void BorrowBook(Book book, DateTime dueDate)
    {
        throw new NotImplementedException();
    }

    public void ReturnBook(Book book)
    {
        throw new NotImplementedException();
    }
}

