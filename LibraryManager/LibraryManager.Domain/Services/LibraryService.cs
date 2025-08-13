using LibraryManager.Domain.Entities;
using System.Text;
namespace LibraryManager.Domain.Services;

public class LibraryService
{
    private List<User> _users;
    private Dictionary<Guid, BookStock> _books;

    public LibraryService()
    {
        _users = new List<User>();
        _books = new Dictionary<Guid, BookStock>();
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
        ValidatePath(filePath);
        string userSavePath = Path.Combine(filePath, "/users.txt");
        string bookSavePath = Path.Combine(filePath, "/books.txt");

        using Stream userStream = new FileStream(userSavePath, FileMode.OpenOrCreate, FileAccess.Write);
        using Stream bookStream = new FileStream(bookSavePath, FileMode.OpenOrCreate, FileAccess.Write);

        Save(userStream, bookStream);
    }
    public void Save(Stream userStream, Stream bookStream)
    {
        using BinaryWriter userWiter = new BinaryWriter(userStream, Encoding.UTF8, leaveOpen: true);
        using BinaryWriter bookWriter = new BinaryWriter(bookStream, Encoding.UTF8, leaveOpen: true);

        SaveBooks(bookWriter);
        SaveUsers(userWiter);
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
    public void Load(string filePath)
    {
        ValidatePath(filePath);

        string userSavePath = Path.Combine(filePath, "/users.txt");
        string bookSavePath = Path.Combine(filePath, "/books.txt");
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

    private void UserLoad(BinaryReader userReader)
    {
        _users.Clear();
        int userCount = userReader.ReadInt32();
        for (int i = 0; i < userCount; i++)
        {
            var user = new User(
                firstName: userReader.ReadString(),
                lastName: userReader.ReadString(),
                birthdate: DateTime.FromBinary(userReader.ReadInt64()),
                personalId: userReader.ReadString(),
                id: new Guid(userReader.ReadBytes(16))
            );


            int loanCount = userReader.ReadInt32();
            for (int j = 0; j < loanCount; j++)
            {
                var book = _books[new Guid(userReader.ReadBytes(16))].Book;
                var dueDate = DateTime.FromBinary(userReader.ReadInt64());
                user.AddLoan(new Loan(book, dueDate));
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
            var book = new Book(
                title: bookReader.ReadString(),
                author: bookReader.ReadString(),
                year: bookReader.ReadInt32(),
                id: new Guid(bookReader.ReadBytes(16))
            );

            int totalCopies = bookReader.ReadInt32();
            var stock = new BookStock(book, totalCopies);

            stock.BorrowedCopies = bookReader.ReadInt32();
            _books[book.Id] = stock;
        }
    }

    private static void ValidatePath(string filePath)
    {
        ArgumentNullException.ThrowIfNull(filePath, nameof(filePath));
        if (Directory.Exists(filePath))
        {
            throw new ArgumentException("File path must be a file, not a directory.", nameof(filePath));
        }
    }

}

//stream.WriteLine("{0, -3}| {1,-38} | {2,-15} | {3,-10} | {4,-20} | {5,-10}", "", "Id", "Name", "PID", "Birthday", "Borrowed Books");
//stream.WriteLine(new string('-', 100));

//int count = 1;
//foreach (var stock in _users.Values)
//{
//    stream.WriteLine("{0, -3}| {1,-38} | {2,-15} | {3,-10} | {4,-10}",
//        count,
//        stock.Id,
//        $"{stock.FirstName} {stock.LastName}",
//        stock.PersonalId,
//        stock.Birthdate);

//    foreach (var loan in stock.BorrowedBooks)
//    {
//        stream.WriteLine("{0, -3}| {1,-38} | {2,-15} | {3,-10} | {4,-10}",
//            "",
//            loan.Id,
//            loan.Title,
//            loan.Author,
//            loan.Year);
//    }

//    count++;
//}
