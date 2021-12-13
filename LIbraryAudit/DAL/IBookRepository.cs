using LIbraryAudit.Models;
using System;
using System.Collections.Generic;


namespace LIbraryAudit.DAL
{
    public interface IBookRepository : IDisposable
    {

        IEnumerable<Book> GetBooks();
        void AddBook(Book book);
        void DeleteBook(int bookId);
        void UpdateBook(Book book);
        void Save();

    }
}
