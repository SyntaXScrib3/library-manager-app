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
    public User GetOrAddUser(string firstName, string lastName, string personalId, DateTime birthDate)
    {
        ValidateUserInput(firstName, lastName, personalId, birthDate);
        // es abrunebs pirvel elements romelic akmayofilebs pirobas igives vaketebdi foreachit(es sxvaganac maqvs)
        var user = _users.FirstOrDefault(u => u.PersonalId == personalId);

        var newUser = new User(firstName, lastName, birthDate, personalId);
        _users.Add(newUser);
        return newUser;
    }

    private static void ValidateUserInput(string firstName, string lastName, string personalId, DateTime birthDate)
    {
        if(personalId == null)
            throw new ArgumentNullException(nameof(personalId), "Personal ID cannot be null");
        if (string.IsNullOrEmpty(firstName))
            throw new ArgumentException("First name cannot be null or empty", nameof(firstName));
        if (string.IsNullOrEmpty(lastName))
            throw new ArgumentException("Last name cannot be null or empty", nameof(lastName));
        if (birthDate == default)
            throw new ArgumentException("Birth date cannot be default value", nameof(birthDate));
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

    public bool GiveBook(Guid fromId, Guid toId, Guid bookId)
    {
        if(fromId == Guid.Empty || toId == Guid.Empty || bookId == Guid.Empty)
            throw new ArgumentException("IDs cannot be empty GUIDs");

        var fromUser = _users.FirstOrDefault(u => u.Id == fromId);
        var toUser = _users.FirstOrDefault(u => u.Id == toId);

        if (!_inventory.ContainsKey(bookId))
            return false;

        var inventory = _inventory[bookId];
        var book = inventory.Book;

        if (!fromUser.HasBook(book))
            return false;

        fromUser.ReturnBook(book);
        toUser.BorrowBook(book);

        return true;
    }

    // BOOK METHODS
    public Guid AddBook(string title, string author, int year, int totalCopies) => 
        AddBook(new Book(title, author, year), totalCopies);

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