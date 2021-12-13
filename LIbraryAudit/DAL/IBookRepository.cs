using LIbraryAudit.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LIbraryAudit.DAL
{
    public interface IBookRepository : IDisposable
    {

        Task<List<Book>> GetBooks();
        void AddBook(Book book);
        void DeleteBook(int bookId);
        void UpdateBook(Book book);
        void Save();

    }
}
