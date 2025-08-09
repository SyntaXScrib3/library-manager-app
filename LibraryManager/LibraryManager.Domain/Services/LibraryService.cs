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
        ValidatePath(filePath);
        using (StreamWriter writer = new StreamWriter(filePath))
        {
            Save(writer);
        }
    }
    public void Save(StreamWriter writer)
    {
        foreach (var user in _users.Values)
        {
            writer.WriteLine($"{user.Id} {user.FirstName} {user.LastName} {user.PersonalId} {user.Birthdate}");
            foreach (var book in user.BorrowedBooks)
            {
                writer.WriteLine($"{book.Id} {book.Title} {book.Author} {book.Year}");
            }
        }
    }

    public List<User> Load(string filePath)
    {
        ValidatePath(filePath);
        using (StreamReader reader = new StreamReader(filePath))
        {
            return Load(reader);
        }
    }
    public List<User> Load(StreamReader reader)
    {
        List<User> users = new List<User>();
        while (!reader.EndOfStream)
        {
            var line = reader.ReadLine();
            if (line != null)
            {
                string[] parts = line.Split(' ');
                Guid userId = Guid.Parse(parts[0]);
                string firstName = parts[1];
                string lastName = parts[2];
                string personalId = parts[3];
                DateTime birthdate = DateTime.Parse(parts[4]);

                users.Add(new User(firstName, lastName, birthdate, personalId, userId));
            }
        }
        return users;
    }

    private static void ValidatePath(string filePath)
    {
        ArgumentException.ThrowIfNullOrEmpty(filePath, nameof(filePath));
        if (Directory.Exists(filePath))
        {
            throw new ArgumentException("File path must be a file, not a directory.", nameof(filePath));
        }
    }
}

//writer.WriteLine("{0, -3}| {1,-38} | {2,-15} | {3,-10} | {4,-20} | {5,-10}", "", "Id", "Name", "PID", "Birthday", "Borrowed Books");
//writer.WriteLine(new string('-', 100));

//int count = 1;
//foreach (var user in _users.Values)
//{
//    writer.WriteLine("{0, -3}| {1,-38} | {2,-15} | {3,-10} | {4,-10}",
//        count,
//        user.Id,
//        $"{user.FirstName} {user.LastName}",
//        user.PersonalId,
//        user.Birthdate);

//    foreach (var book in user.BorrowedBooks)
//    {
//        writer.WriteLine("{0, -3}| {1,-38} | {2,-15} | {3,-10} | {4,-10}",
//            "",
//            book.Id,
//            book.Title,
//            book.Author,
//            book.Year);
//    }

//    count++;
//}
