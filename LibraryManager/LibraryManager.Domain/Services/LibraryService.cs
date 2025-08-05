using LibraryManager.Domain.Entities;
using System;

namespace LibraryManager.Domain.Services;

public static class LibraryService
{
    public static void AddBook(string title, string author, int year)
    {
        throw new NotImplementedException();
    }

    public static void AddBook(Book book)
    {
        throw new NotImplementedException();
    }

    public static void DeleteBook(Book book)
    {
        throw new NotImplementedException();
    }

    public static Book GetBook(string title, string author, int year)
    {
        throw new NotImplementedException();
    }

    public static IEnumerable<Book> SearchBooks(string? title = null, string? author = null, int? year = null)
    {
        throw new NotImplementedException();
    }

    public static void RegisterUser(string firstName, string lastName, string personalId)
    {
        throw new NotImplementedException();
    }

    public static void RegisterUser(User user)
    {
        throw new NotImplementedException();
    }

    public static void UnregisterUser(string firstName, string lastName, string personalId)
    {
        throw new NotImplementedException();
    }

    public static User GetUser(string personalId)
    {
        throw new NotImplementedException();
    }

    public static void UnregisterUser(User user)
    {
        throw new NotImplementedException();
    }

    public static void BorrowBook(int userId, int bookId)
    {
        throw new NotImplementedException();
    }

    public static void ReturnBook(int userId, int bookId)
    {
        throw new NotImplementedException();
    }

    public static IEnumerable<Loan> GetLoansForUser(int userIds)
    {
        throw new NotImplementedException();
    }

    public static void Save(string filePath)
    {
        throw new NotImplementedException();
    }
    public static void Save(Stream stream)
    {
        throw new NotImplementedException();
    }

    public static void Load(string filePath)
    {
        throw new NotImplementedException();
    }
    public static void Load(Stream stream)
    {
        throw new NotImplementedException();
    }
}