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
    public Guid AddBook(string title, string author, int year, int totalCopies)
    {
        throw new NotImplementedException();
    }

    public bool RemoveBook(Guid bookId)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<BookStock> SearchBooks(string? title = null, string? author = null, int? year = null)
    {
        throw new NotImplementedException();
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
}