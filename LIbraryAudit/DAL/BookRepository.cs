using LIbraryAudit.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIbraryAudit.DAL
{
    public class BookRepository : IBookRepository, IDisposable
    {
        private LibraryContext context;
        public BookRepository(LibraryContext context)
        {
            this.context = context;
        }
        public IEnumerable<Book> GetBooks()
        {
            return context.Books.ToList();
        }
        public void AddBook(Book book)
        {
            context.Books.Add(book);
        }

        public void DeleteBook(int bookId)
        {
            Book book = context.Books.Find(bookId);
            context.Books.Remove(book);
        }
        public void UpdateBook(Book book)
        {
            context.Entry(book).State = EntityState.Modified;
        }

        public void Save()
        {
            context.SaveChanges();
        }

        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
