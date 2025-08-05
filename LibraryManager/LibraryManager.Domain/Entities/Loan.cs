namespace LibraryManager.Domain.Entities;

public class Loan
{
    public int Id { get; }
    public int UserId { get; }
    public int BookId { get; }
    public DateTime DueAt { get; }

    public Loan(int userId, int bookId, DateTime dueAt)
    {
        Id = GenerateId();
        UserId = userId;
        BookId = bookId;
        DueAt = dueAt;
    }

    private int GenerateId()
    {
        throw new NotImplementedException();
    }
}