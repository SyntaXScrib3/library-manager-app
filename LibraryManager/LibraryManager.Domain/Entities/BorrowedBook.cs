namespace LibraryManager.Domain.Entities;

public class BorrowedBook
{
    public Book Book { get; set; }
    public DateTime BorrowDate { get; set; }
    public DateTime DueDate { get; set; }
    public bool IsReturned { get; set; }

    public BorrowedBook(Book book, DateTime borrowDate, DateTime dueDate)
    {
        Book = book ?? throw new ArgumentNullException(nameof(book));
        BorrowDate = borrowDate;
        DueDate = dueDate;
        IsReturned = false;
    }

}
