using LibraryManager.Domain.Entities;
using Microsoft.VisualBasic;
using System.Text;
namespace LibraryManager.Domain.Services;

public class LibraryService
{
    private List<User> _users;
    private Dictionary<Guid, BookStock> _books;

    private const int DefaultLoanDays = 14;

    public LibraryService()
    {
        _users = new List<User>();
        _books = new Dictionary<Guid, BookStock>();
    }

    #region User

    public User AddUser(string firstName, string lastName, DateTime birthDate, string personalId)
    {
        var candidate = new User(firstName.Trim(), lastName.Trim(), birthDate, personalId);

        if (FindUserByPersonalId(candidate.PersonalId) != null)
            throw new InvalidOperationException("A user with the same Personal ID already exists.");

        _users.Add(candidate);
        return candidate;
    }

    public bool RemoveUser(Guid userId)
    {
        if (userId == Guid.Empty)
            throw new ArgumentException("User ID cannot be empty.", nameof(userId));


        for (int i = 0; i < _users.Count; i++)
        {
            var user = _users[i];

            if (user.Id != userId) continue;

            if (user.Loans.Count > 0)
                throw new InvalidOperationException(
                    $"Cannot remove user {userId}: user has {user.Loans.Count} active loan(s).");

            _users.RemoveAt(i);
            return true;
        }

        return false;
    }

    public User? GetUser(string personalId)
    {
        if (string.IsNullOrWhiteSpace(personalId))
            throw new ArgumentException("Personal ID cannot be null or empty.", nameof(personalId));

        var pid = personalId.Trim();

        return FindUserByPersonalId(pid);
    }

    // ?? TryGetUser ??

    public IReadOnlyList<User> GetAllUsers() => _users.AsReadOnly();

    #endregion

    #region Book

    public Guid AddBook(string title, string author, int year, int totalCopies)
    {
        foreach (var inventory in _books)
        {
            var book = inventory.Value.Book;

            if (string.Equals(book.Title, title.Trim(), StringComparison.CurrentCultureIgnoreCase) &&
             string.Equals(book.Author, author.Trim(), StringComparison.CurrentCultureIgnoreCase) &&
             book.Year == year)
                throw new InvalidOperationException("This book is already in the inventory.");
        }

        var newBook = new Book(title, author, year);
        _books[newBook.Id] = new BookStock(newBook, totalCopies);

        return newBook.Id;
    }

    public bool RemoveBook(Guid bookId)
    {
        if (bookId == Guid.Empty)
            throw new ArgumentException("Book ID cannot be empty.", nameof(bookId));

        if (!_books.TryGetValue(bookId, out var stock))
            return false;

        int usersWithLoan = 0;
        for (int i = 0; i < _users.Count; i++)
        {
            if (_users[i].HasBook(bookId))
                usersWithLoan++;
        }

        if (stock.BorrowedCopies > 0 || usersWithLoan > 0)
        {
            throw new InvalidOperationException(
                $"Cannot remove book \"{stock.Book.Title}\" ({bookId}). " +
                $"Active loans detected: users={usersWithLoan}, stock.BorrowedCopies={stock.BorrowedCopies}.");
        }

        return _books.Remove(bookId);
    }

    public BookStock? GetBook(Guid bookId)
    {
        if (bookId == Guid.Empty)
            throw new ArgumentException("Book ID cannot be empty.", nameof(bookId));

        BookStock stock;
        return _books.TryGetValue(bookId, out stock) ? stock : null;
    }
    public IReadOnlyList<BookStock> GetAllBooks()
    {
        var list = new List<BookStock>(_books.Count);

        foreach (var book in _books)
            list.Add(book.Value);

        return list.AsReadOnly();
    }

    // ?? TryGetBook ??

    public IEnumerable<BookStock> SearchBooks(string? title = null, string? author = null, int? year = null)
    {
        foreach (var item in _books)
        {
            if (CheckIfBookMatchesTheCondition(item.Value.Book, title, author, year))
            {
                yield return item.Value;
            }
        }
    }

    /*
     Enter title:  hobbit
     Enter author: tolkien
     Enter year:   1937
    */

    #endregion

    #region Loan

    public bool BorrowBook(User user, Guid bookId)
    {
        if (user is null)
            throw new ArgumentNullException(nameof(user));
        if (bookId == Guid.Empty)
            throw new ArgumentException("Book ID cannot be empty.", nameof(bookId));

        if (!_books.TryGetValue(bookId, out var stock))
            return false;

        var book = stock.Book;
        if (user.HasBook(bookId))
            throw new InvalidOperationException($"User {user.FullName} already has this book on loan.");

        if (stock.AvailableCopies <= 0)
            throw new InvalidOperationException($"No available copies of \"{stock.Book.Title}\" to borrow.");

        stock.BorrowCopy();
        try
        {
            var due = DateTime.UtcNow.AddDays(DefaultLoanDays);
            user.BorrowBook(stock.Book, due);
            return true;
        }
        catch
        {
            stock.ReturnCopy();
            throw;
        }
    }

    public bool ReturnBook(User user, Guid bookId)
    {
        if (user is null)
            throw new ArgumentNullException(nameof(user));
        if (bookId == Guid.Empty)
            throw new ArgumentException("Book ID cannot be empty.", nameof(bookId));

        if (!_books.TryGetValue(bookId, out var stock))
            return false;

        if (!user.HasBook(bookId))
            throw new InvalidOperationException($"User {user.FullName} does not have this book on loan.");

        user.ReturnBook(bookId);
        stock.ReturnCopy();
        return true;
    }

    public bool GiveBook(Guid fromId, Guid toId, Guid bookId)
    {
        if (fromId == Guid.Empty || toId == Guid.Empty || bookId == Guid.Empty)
            throw new ArgumentException("IDs cannot be empty GUIDs");

        var fromUser = _users.FirstOrDefault(u => u.Id == fromId);
        var toUser = _users.FirstOrDefault(u => u.Id == toId);

        if (fromUser is null || toUser is null)
            return false;

        if (!_books.TryGetValue(bookId, out var inventory))
            return false;

        var book = inventory.Book;

        var loan = fromUser.Loans.FirstOrDefault(l => l.Book.Id == bookId);

        if (loan is null)
            return false;

        fromUser.ReturnBook(bookId);
        toUser.BorrowBook(book, loan.DueDate);
        return true;
    }

    #endregion

    #region Import/Export

    public void Save(string directoryPath)
    {
        ValidatePath(directoryPath);
        string userSavePath = Path.Combine(directoryPath, "users.dat");
        string bookSavePath = Path.Combine(directoryPath, "books.dat");

        using Stream userStream = new FileStream(userSavePath, FileMode.Create, FileAccess.Write);
        using Stream bookStream = new FileStream(bookSavePath, FileMode.Create, FileAccess.Write);

        Save(userStream, bookStream);
    }
    public void Save(Stream userStream, Stream bookStream)
    {
        using BinaryWriter userWiter = new BinaryWriter(userStream, Encoding.UTF8, leaveOpen: true);
        using BinaryWriter bookWriter = new BinaryWriter(bookStream, Encoding.UTF8, leaveOpen: true);

        SaveBooks(bookWriter);
        SaveUsers(userWiter);
    }

    public void Load(string directoryPath)
    {
        ValidatePath(directoryPath);

        string userSavePath = Path.Combine(directoryPath, "users.dat");
        string bookSavePath = Path.Combine(directoryPath, "books.dat");

        using Stream userStream = new FileStream(userSavePath, FileMode.Open, FileAccess.Read);
        using Stream bookStream = new FileStream(bookSavePath, FileMode.Open, FileAccess.Read);

        Load(userStream, bookStream);
    }
    public void Load(Stream userStream, Stream bookStream)
    {
        using BinaryReader userReader = new BinaryReader(userStream, Encoding.UTF8, leaveOpen: true);
        using BinaryReader bookReader = new BinaryReader(bookStream, Encoding.UTF8, leaveOpen: true);

        BookLoad(bookReader);
        UserLoad(userReader);
    }

    private void SaveBooks(BinaryWriter writer)
    {
        writer.Write(_books.Count);
        foreach (var stock in _books.Values)
        {
            var book = stock.Book;

            writer.Write(book.Id.ToByteArray());
            writer.Write(book.Title);
            writer.Write(book.Author);
            writer.Write(book.Year);
            writer.Write(stock.TotalCopies);
            writer.Write(stock.BorrowedCopies);
        }
    }
    private void SaveUsers(BinaryWriter writer)
    {
        writer.Write(_users.Count);

        foreach (var user in _users)
        {
            writer.Write(user.Id.ToByteArray());
            writer.Write(user.PersonalId);
            writer.Write(user.FirstName);
            writer.Write(user.LastName);
            writer.Write(user.Birthdate.ToBinary());

            writer.Write(user.Loans.Count);
            foreach (var loan in user.Loans)
            {
                writer.Write(loan.Book.Id.ToByteArray());
                writer.Write(loan.DueDate.ToBinary());
            }
        }
    }

    private void UserLoad(BinaryReader userReader)
    {
        _users.Clear();
        int userCount = userReader.ReadInt32();
        for (int i = 0; i < userCount; i++)
        {
            var id = new Guid(userReader.ReadBytes(16));
            var personalId = userReader.ReadString();
            var firstName = userReader.ReadString();
            var lastName = userReader.ReadString();
            var birthdate = DateTime.FromBinary(userReader.ReadInt64());

            var user = new User(
                id: id,
                firstName: firstName,
                lastName: lastName,
                birthdate: birthdate,
                personalId: personalId,
                loans: null
            );

            int loanCount = userReader.ReadInt32();

            for (int j = 0; j < loanCount; j++)
            {
                var bookId = new Guid(userReader.ReadBytes(16));
                var dueDate = DateTime.FromBinary(userReader.ReadInt64());
                var book = _books[bookId].Book;
                user.BorrowBook(book, dueDate);
            }

            _users.Add(user);
        }
    }
    private void BookLoad(BinaryReader bookReader)
    {
        _books.Clear();
        int bookCount = bookReader.ReadInt32();
        
        for (int i = 0; i < bookCount; i++)
        {
            var id = new Guid(bookReader.ReadBytes(16));
            var title = bookReader.ReadString();
            var author = bookReader.ReadString();
            var year = bookReader.ReadInt32();
            int totalCopies = bookReader.ReadInt32();
            int borrowedCopies = bookReader.ReadInt32();

            var book = new Book(id, title, author, year);
            var stock = new BookStock(book, totalCopies);

            stock.BorrowedCopies = borrowedCopies;
            _books[book.Id] = stock;
        }
    }
    
    #endregion

    #region Helpers

    private static void ValidatePath(string directoryPath)
    {
        if (string.IsNullOrWhiteSpace(directoryPath))
            throw new ArgumentException("Directory path cannot be empty.", nameof(directoryPath));
        Directory.CreateDirectory(directoryPath);
    }

    private static void ValidateUserInput(string firstName, string lastName, string personalId, DateTime birthDate)
    {
        if (personalId == null)
            throw new ArgumentNullException(nameof(personalId), "Personal ID cannot be null");
        if (string.IsNullOrEmpty(firstName))
            throw new ArgumentException("First name cannot be null or empty", nameof(firstName));
        if (string.IsNullOrEmpty(lastName))
            throw new ArgumentException("Last name cannot be null or empty", nameof(lastName));
        if (birthDate == default)
            throw new ArgumentException("Birth date cannot be default value", nameof(birthDate));
    }

    private User? FindUserByPersonalId(string pid)
    {
        for (int i = 0; i < _users.Count; i++)
        {
            if (string.Equals(_users[i].PersonalId, pid, StringComparison.Ordinal))
                return _users[i];
        }
        return null;
    }

    private bool CheckIfBookMatchesTheCondition(Book book, string? title, string? author, int? year)
    {
        if ((!string.IsNullOrEmpty(author) && !book.Author.Contains(author, StringComparison.CurrentCultureIgnoreCase)) ||
            (!string.IsNullOrEmpty(title) && !book.Title.Contains(title, StringComparison.CurrentCultureIgnoreCase)) ||
            (year is not null && book.Year != year))
        {
            return false;
        }

        return true;
    }
    
    #endregion
}