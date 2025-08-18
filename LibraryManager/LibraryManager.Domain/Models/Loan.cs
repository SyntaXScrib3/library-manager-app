namespace LibraryManager.Domain.Entities;

public class Loan
{
    public Book Book { get; }
    public DateTime LoanDate { get; }
    public DateTime DueDate { get; private set; }

    public Loan(Book book, DateTime dueDate) : this(book, DateTime.UtcNow, dueDate) { }
    public Loan(Book book, DateTime loanDate, DateTime dueDate)
    {
        if (book is null) throw new ArgumentNullException(nameof(book));

        var loanUtc = ToUtc(loanDate);
        var dueUtc = ToUtc(dueDate);

        if (dueUtc <= loanUtc)
            throw new ArgumentException("Due date must be after the loan date.", nameof(dueDate));

        Book = book;
        LoanDate = loanUtc;
        DueDate = dueUtc;
    }

    public bool IsOverdue(DateTime? now = null)
    {
        var nowUtc = ToUtc(now ?? DateTime.UtcNow);
        return nowUtc > DueDate;
    }

    public void ExtendDueDate(TimeSpan by)
    {
        if (by <= TimeSpan.Zero)
            throw new ArgumentOutOfRangeException(nameof(by), "Extension must be positive.");
        DueDate = DueDate.Add(by);
    }

    public void ExtendDueDate(int days)
    {
        if (days <= 0) throw new ArgumentOutOfRangeException(nameof(days), "Days must be positive.");
        ExtendDueDate(TimeSpan.FromDays(days));
    }

    private static DateTime ToUtc(DateTime dt)
    {
        if (dt.Kind == DateTimeKind.Unspecified)
            return DateTime.SpecifyKind(dt, DateTimeKind.Utc);

        return dt.ToUniversalTime();
    }

    public override string ToString() => $"{Book.Title} — due {DueDate:d}";
}