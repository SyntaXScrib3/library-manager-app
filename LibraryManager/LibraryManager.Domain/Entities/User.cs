namespace LibraryManager.Domain.Entities;

public class User
{
    public int Id { get; }
    public string PersonalId { get; } = string.Empty;
    public string FirstName { get; } = string.Empty;
    public string LastName { get; } = string.Empty;
    public DateTime BirthDate { get; }

    public User(string personalId, string firstName, string lastName, DateTime birthDate)
    {
        Id = GenerateID();
        PersonalId = personalId;
        FirstName = firstName;
        LastName = lastName;
        BirthDate = birthDate;
    }

    //Todo
    private int GenerateID()
    {
        throw new NotImplementedException();
    }

}
