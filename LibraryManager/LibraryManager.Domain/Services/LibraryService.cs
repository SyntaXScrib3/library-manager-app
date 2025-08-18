using LibraryManager.Domain.Entities;
namespace LibraryManager.Domain.Services;

public class LibraryService
{
    #region FIELDS
    private List<User> _users;
    private Dictionary<Guid, BookStock> _inventory;
    #endregion

    #region CONSTRUCTORS
    public LibraryService()
    {
        _users = new List<User>();
        _inventory = new Dictionary<Guid, BookStock>();
    }
    #endregion

    #region USER METHODS
    public User GetOrAddUser(string firstName, string lastName, string personalId)
    {
        throw new NotImplementedException();
    }

    public List<User> GetAllUsers()
    {
        throw new NotImplementedException();
    }

    public bool RemoveUser(Guid userId)
    {
        throw new NotImplementedException();
    }
    #endregion

    #region BOOK METHODS/SEARCH
    public Guid AddBook(string title, string author, int year, int quantity)
            => AddBook(new Book(title, author, year), quantity);

    public Guid AddBook(Guid id, string title, string author, int year, int quantity)
        => AddBook(new Book(id, title, author, year), quantity);

    public Guid AddBook(Book book, int quantity)
    {
        if (_inventory.ContainsKey(book.Id))
        {
            BookStock stock = GetBookStock(book.Id)!;
            stock.AddCopies(quantity);
        }
        else
        {
            _inventory[book.Id] = new BookStock(book, quantity);
        }

        return book.Id;
    }

    public void RemoveBooks(Guid bookId, int quantity)
    {
        if (!_inventory.ContainsKey(bookId))
            throw new ArgumentException($"The book with the ID: {bookId}, is not in the inventory");

        BookStock stock = GetBookStock(bookId)!;
        stock.RemoveCopies(quantity);

        if (stock.AvailableCopies == 0)
        {
            _inventory.Remove(bookId);
        }
    }

    public void RemoveAllBooks(Guid BookId) => RemoveBooks(BookId, GetBookStock(BookId)!.AvailableCopies);

    public IEnumerable<BookStock> SearchBooks(string? title = null, string? author = null, string? year = null)
    {
        foreach (var item in _inventory)
        {
            if (CheckIfBookMatchesTheCondition(item.Value.Book, title, author, year))
            {
                yield return item.Value;
            }
        }
    }

    public BookStock? GetBookStock(Guid bookId)
    {
        throw new NotImplementedException();
    }
    #endregion

    #region LENDING METHODS
    public bool BorrowBook(User user, Guid bookId)
    {
        throw new NotImplementedException();
    }

    public bool ReturnBook(User user, Guid bookId)
    {
        throw new NotImplementedException();
    }
    #endregion

    #region SAVE/LOAD METHODS
    public void Save(string filePath)
    {
        throw new NotImplementedException();
    }

    public void Save(Stream stream)
    {
        throw new NotImplementedException();
    }

    public void Load(string filePath)
    {
        throw new NotImplementedException();
    }

    public void Load(Stream stream)
    {
        throw new NotImplementedException();
    }
    #endregion

    #region PRIVATE HELPER METHODS
    private bool CheckIfBookMatchesTheCondition(Book book, string? title, string? author, string? year)
    {
        if ((!string.IsNullOrEmpty(author) && !book.Author.Contains(author, StringComparison.CurrentCultureIgnoreCase)) ||
            (!string.IsNullOrEmpty(title) && !book.Title.Contains(title, StringComparison.CurrentCultureIgnoreCase)) ||
            (!string.IsNullOrEmpty(year) && book.Year != int.Parse(year)))
        {
            return false;
        }

        return true;
    }
    #endregion
}

// FUTURE: We can implement a different search which matches the user's input with the first letters of the book field.
