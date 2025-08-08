using LibraryManager.Domain.Entities;
namespace LibraryManager.Domain.Services;

public class LibraryService
{
    private List<User> _users;
    private Dictionary<Guid, BookInventory> _inventory;

    public LibraryService()
    {
        _users = new List<User>();
        _inventory = new Dictionary<Guid, BookInventory>();
    }

    // USER METHODS
    public User GetOrAddUser(string firstName, string lastName, string personalId, DateTime birthDate)
    {
        if(personalId == null)
            throw new ArgumentNullException(nameof(personalId), "Personal ID cannot be null");
        if (string.IsNullOrEmpty(firstName))
            throw new ArgumentException("First name cannot be null or empty", nameof(firstName));
        if (string.IsNullOrEmpty(lastName))
            throw new ArgumentException("Last name cannot be null or empty", nameof(lastName));
        if (birthDate == default)
            throw new ArgumentException("Birth date cannot be default value", nameof(birthDate));

        foreach (var user in _users)
        {
            if (user.PersonalId == personalId)
                return user;
        }
        var newUser = new User(firstName, lastName, birthDate, personalId);
        _users.Add(newUser);
        return newUser;
    }

    public IReadOnlyList<User> GetAllUsers()
    {
        return _users.AsReadOnly();
    }

    public bool RemoveUser(Guid userId)
    {
        for (int i = 0; i < _users.Count; i++)
        {
            if (_users[i].Id == userId)
            {
                _users.RemoveAt(i);
                return true;
            }
        }
        return false;
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

    public IEnumerable<BookInventory> SearchBooks(string? title = null, string? author = null, int? year = null)
    {
        throw new NotImplementedException();
    }
    /*
      Enter title:  hobbit
      Enter author: tolkien
      Enter year:   1937
     */

    public BookInventory? GetBookInventory(Guid bookId)
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