namespace LibraryManager.Domain.Entities;

public class BookStock
{
    public Book Book { get; }
    public int TotalCopies { get; set; }
    public int BorrowedCopies { get; set; }

    public BookStock(Book book, int totalCopies)
    {
        Book = book ?? throw new ArgumentNullException(nameof(book));

        if (totalCopies < 1)
            throw new ArgumentException("There must be at least 1 copy.", nameof(totalCopies));

        TotalCopies = totalCopies;
        BorrowedCopies = 0;
    }

    public int AvailableCopies => TotalCopies - BorrowedCopies;
    public bool IsAvailable => AvailableCopies > 0;
    public void BorrowCopy()
    {
        if (AvailableCopies <= 0)
            throw new InvalidOperationException("No available copies.");
        BorrowedCopies++;
    }

    public void ReturnCopy()
    {
        if (BorrowedCopies <= 0)
            throw new InvalidOperationException("No borrowed copies to return.");
        BorrowedCopies--;
    }
}
