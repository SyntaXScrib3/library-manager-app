namespace LibraryManager.Domain.Entities;

public class User
{   
    private readonly List<Loan> _loans;

    public Guid Id { get; }
    public string PersonalId { get; }
    public string FirstName { get; }
    public string LastName { get; }
    public DateTime Birthdate { get; }
    public IReadOnlyList<Loan> Loans => _loans;
    public string FullName => FirstName + " " + LastName;

    public User(string firstName, string lastName, DateTime birthdate, string personalId) : this(Guid.NewGuid(), firstName, lastName, birthdate, personalId, null) { }
    public User(Guid id, string firstName, string lastName, DateTime birthdate, string personalId, List<Loan>? loans)
    {
        if (id == Guid.Empty)
            throw new ArgumentException("Id cannot be empty.", nameof(id));
        if (string.IsNullOrWhiteSpace(firstName))
            throw new ArgumentException("First name cannot be empty.", nameof(firstName));
        if (string.IsNullOrWhiteSpace(lastName))
            throw new ArgumentException("Last name cannot be empty.", nameof(lastName));
        if (string.IsNullOrWhiteSpace(personalId))
            throw new ArgumentException("Personal ID cannot be empty.", nameof(personalId));
        if (birthdate.Date > DateTime.UtcNow.Date)
            throw new ArgumentOutOfRangeException(nameof(birthdate), "Birthdate cannot be in the future.");

        Id = id;
        FirstName = firstName.Trim();
        LastName = lastName.Trim();
        Birthdate = birthdate.Date;
        PersonalId = personalId.Trim();

        _loans = loans ?? new List<Loan>();
    }

    public Loan? GetLoanFor(Guid bookId)
    {
        for (int i = 0; i < _loans.Count; i++)
        {
            if (_loans[i].Book.Id == bookId)
                return _loans[i];
        }
        return null;
    }

    public Loan? GetLoanFor(Book book)
    {
        if (book is null) throw new ArgumentNullException(nameof(book));
        return GetLoanFor(book.Id);
    }

    public bool HasBook(Guid bookId) => GetLoanFor(bookId) != null;

    public bool HasBook(Book book)
    {
        if (book is null) throw new ArgumentNullException(nameof(book));
        return HasBook(book.Id);
    }

    public void BorrowBook(Book book, DateTime dueDate)
    {
        if (book is null)
            throw new ArgumentNullException(nameof(book));
        if (HasBook(book))
            throw new InvalidOperationException("User already has this book on loan.");
        if (dueDate <= DateTime.UtcNow)
            throw new ArgumentException("Due date must be in the future.", nameof(dueDate));

        _loans.Add(new Loan(book, dueDate));
    }

    public void ReturnBook(Guid bookId)
    {
        for (int i = 0; i < _loans.Count; i++)
        {
            if (_loans[i].Book.Id == bookId)
            {
                _loans.RemoveAt(i);
                return;
            }
        }
        throw new InvalidOperationException("User does not have this book on loan.");
    }
    public void ReturnBook(Book book)
    {
        if (book is null) throw new ArgumentNullException(nameof(book));
        ReturnBook(book.Id);
    }
}