using System.Net;

namespace LibraryManager.Domain.Entities;

public class BookInventory
{
    public int Id { get; }
    public int BookId { get; }
    public int AvailableQuantity { get; }

    public BookInventory(int bookId, int availableQuantity)
    {
        Id = GenerateId();
        BookId = bookId;
        AvailableQuantity = availableQuantity;
    }

    private int GenerateId()
    {
        throw new NotImplementedException();
    }
}
