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

    internal void AddLoan(Loan loan)
    {
        if (loan == null)
            throw new ArgumentNullException(nameof(loan), "Loan cannot be null.");
        _loans.Add(loan);
    }

    //public User(Guid? id = null)
    //{
    //    Id = id ?? Guid.NewGuid();
    //}

    //public User(Guid? id, string firstName, string lastName, DateTime birthdate, string personalId) : this(id)
    //{
    //    FirstName = firstName;
    //    LastName = lastName;
    //    Birthdate = birthdate;
    //    PersonalId = personalId;
    //}
    public User(string firstName, string lastName, DateTime birthdate, string personalId, Guid? id = null)
    {
        Id = id ?? Guid.NewGuid();
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

