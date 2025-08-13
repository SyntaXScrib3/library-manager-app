namespace LibraryManager.Domain.Entities;

public class BookStock
{
    public Book Book { get; }
    public int TotalCopies { get; set; }
    public int BorrowedCopies { get; set; }

    public int AvailableCopies => TotalCopies - BorrowedCopies;
    public bool IsAvailable => AvailableCopies > 0;

    public BookStock(Book book, int totalCopies) : this(book, totalCopies, 0) { }

    public BookStock(Book book, int totalCopies, int borrowedCopies)
    {
        if (book is null)
            throw new ArgumentNullException(nameof(book));
        if (totalCopies < 1)
            throw new ArgumentException("There must be at least 1 copy.", nameof(totalCopies));
        if (borrowedCopies < 0 || borrowedCopies > totalCopies)
            throw new ArgumentOutOfRangeException(nameof(borrowedCopies), "Borrowed must be between 0 and total copies.");

        Book = book;
        TotalCopies = totalCopies;
        BorrowedCopies = borrowedCopies;
    }

    public void AddCopies(int count)
    {
        if (count <= 0)
            throw new ArgumentOutOfRangeException(nameof(count), "Count must be positive.");
        
        checked { TotalCopies += count; }
    }

    public void RemoveCopies(int count)
    {
        if (count <= 0)
            throw new ArgumentOutOfRangeException(nameof(count), "Count must be positive.");

        if (count > TotalCopies)
            throw new InvalidOperationException("Cannot remove more copies than exist in total.");

        if (TotalCopies - count < BorrowedCopies)
            throw new InvalidOperationException("Cannot reduce total below the number of borrowed copies.");

        TotalCopies -= count;
    }

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

    public override string ToString() =>
        $"{Book.Title} — Total: {TotalCopies}, Borrowed: {BorrowedCopies}, Available: {AvailableCopies}";
}
