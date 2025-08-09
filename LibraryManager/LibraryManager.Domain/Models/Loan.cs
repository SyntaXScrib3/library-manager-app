namespace LibraryManager.Domain.Entities;

public class Loan
{
    public Book Book { get; }
    public DateTime DueDate { get; }

    public Loan(Book book, DateTime dueDate)
    {
        Book = book ?? throw new ArgumentNullException(nameof(book));
        DueDate = dueDate;
    }
}