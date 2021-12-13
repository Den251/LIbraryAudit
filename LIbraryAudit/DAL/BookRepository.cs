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
        public async Task<List<Book>> GetBooks()
        {
            return await context.Books.Where(b => b.Archived != true).ToListAsync();
        }
        public async Task<List<Book>> ShowAvailable()
        {
            
            return await context.Books.Where(b => b.Archived != true & b.Reserved != true).ToListAsync();
        }
        public void AddBook(Book book)
        {
            context.Books.Add(book);
            
        }



        public async Task<Book> FindBook(int? id)
        {
            return await context.Books.FirstOrDefaultAsync(i => i.Id == id);
            
        }

        public async void DeleteBook(int bookId)
        {
            Book book = await context.Books.FindAsync(bookId);
            context.Books.Remove(book);
            await context.SaveChangesAsync();
            
        }
        public async void UpdateBook(Book book)
        {
            
            context.Books.Update(book);
            await context.SaveChangesAsync();
        }

        public async Task<List<Book>> GetAllAlphabeticaly()
        {
            

            return await context.Books.Where(b => b.Archived != true).ToListAsync();

        }
        public async Task<List<Book>> GetAllAvailable()
        {


            return await context.Books.Where(b => b.Archived != true & b.Reserved != true).ToListAsync();

        }
        public async Task<List<Book>> GetAllReserved()
        {


            return await context.Books.Where(b => b.Reserved == true).ToListAsync();

        }
        


        public async void Save()
        {
            await context.SaveChangesAsync();
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
