using LibraryManager.Domain.Entities;
namespace LibraryManager.Domain.Services;

public class LibraryService
{
    private List<User> _users;
    private Dictionary<Guid, BookStock> _inventory;

    public LibraryService()
    {
        _users = new List<User>();
        _inventory = new Dictionary<Guid, BookStock>();
    }

    // USER METHODS
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

    // BOOK METHODS
    public Guid AddBook(string title, string author, int year, int totalCopies) => AddBook(new Book(title, author, year), totalCopies);

    public Guid AddBook(Book book, int totalCopies)
    {
        if (_inventory.ContainsKey(book.Id)) throw new ArgumentException($"The book with the ID: {book.Id}, is already in the inventory");

        _inventory[book.Id] = new BookStock(book, totalCopies);
        return book.Id;
    }

    public bool RemoveBook(Guid bookId)
    {
        if (!_inventory.ContainsKey(bookId)) throw new ArgumentException($"The book with the ID: {bookId}, is not in the inventory");

        return _inventory.Remove(bookId);
    }

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
    /*
      Enter title:  hobbit
      Enter author: tolkien
      Enter year:   1937
     */

    public BookStock? GetBookInventory(Guid bookId)
    {
        throw new NotImplementedException();
    }

    // LENDING METHODS
    public bool BorrowBook(User user, Guid bookId)
    {
        throw new NotImplementedException();
    }

    public bool ReturnBook(User user, Guid bookId)
    {
        throw new NotImplementedException();
    }

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
}