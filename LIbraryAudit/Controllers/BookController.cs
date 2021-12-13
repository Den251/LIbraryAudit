using LIbraryAudit.DAL;
using LIbraryAudit.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIbraryAudit.Controllers
{
    public class BookController : Controller
    {
        private LibraryContext db;
        public Book bookRecord { get; set; }
        
        public BookController(LibraryContext db)
        {
            this.db = db;
        }
        public async Task<IActionResult> Index()
        {

            return View(await db.Books.ToListAsync());
        }
        public async Task<IActionResult> ShowAvailable()
        {
            var avBooks = await db.Books.Where(b => b.Archived != true & b.Reserved != true).ToListAsync();
            return RedirectToAction("Index");
        }
        public ActionResult Add(Book book)
        {


            return View(bookRecord);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Book book)
        {

            db.Books.Add(book);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        
        public async Task<IActionResult> Update(int? id)
        {
            bookRecord = await db.Books.FirstOrDefaultAsync(i => i.Id == id);
            if (bookRecord == null)
            {
                return NotFound();
            }
            return View(bookRecord); 
        }
        public ActionResult Export()
        {
            List<Book> books = db.Books.ToList<Book>();
            StringBuilder sb = new StringBuilder();
            sb.Append("ID" + ',');
            sb.Append("Title" + ',');
            sb.Append("Author" + ',');
            sb.Append("\r\n");
            
            for (int i = 0; i < books.Count; i++)
            {
                 
                sb.Append(books[i].Id + ", "); //for some reason ',' doesn't work
                sb.Append(books[i].Title + ',');
                sb.Append(books[i].Author + ',');
                sb.Append("\r\n");
                
            }
            return File(Encoding.UTF8.GetBytes(sb.ToString()), "text/csv", "Library.csv");
        }
        [HttpPost]
        public async Task<IActionResult> Update(Book book)
        {
            if (ModelState.IsValid)
            {
                
                
                db.Books.Update(book);
                

                await db.SaveChangesAsync();

                
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> Reserve(int? id)
        {
            bookRecord = await db.Books.FirstOrDefaultAsync(i => i.Id == id);
            if (bookRecord.Reserved==false)
                bookRecord.Reserved = true;
            else
                bookRecord.Reserved = false;
            db.Books.Update(bookRecord);
            await db.SaveChangesAsync();
            return Json(new { success = true, message = "Reserved successful" });
        }
        
        [HttpPut]
        public async Task<IActionResult> Archive(int? id)
        {
            bookRecord = await db.Books.FirstOrDefaultAsync(i => i.Id == id);
            if (bookRecord.Archived == false)
                bookRecord.Archived = true;
            else
                bookRecord.Archived = false;
            db.Books.Update(bookRecord);
            await db.SaveChangesAsync();
            return Json(new { success = true, message = "Delete successful" });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var bookFromDb = await db.Books.FirstOrDefaultAsync(u => u.Id == id);
            if (bookFromDb == null)
            {
                return Json(new { success = false, message = "Error while Deleting" });
            }
            db.Books.Remove(bookFromDb);
            await db.SaveChangesAsync();
            return Json(new { success = true, message = "Delete successful" });
        }
       
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Json(new { data = await db.Books.Where(b => b.Archived != true).ToListAsync() });
        }
        public async Task<IActionResult> GetAllAlphabeticaly()
        {
            var info = await db.Books.Where(b => b.Archived != true).ToListAsync();
            
            return Json(new { data = info.OrderByDescending(a => a.Title) });
            
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAv(bool av)
        {
            bool availabilityPushed = Convert.ToBoolean(HttpContext.Session.GetString("availabilityPushed")); 
            
            if (availabilityPushed == false)
            {
                HttpContext.Session.SetString("availabilityPushed", "true");
                return Json(new { data = await db.Books.Where(b => b.Archived != true & b.Reserved != true).ToListAsync() });

            }
            else
            {
                HttpContext.Session.SetString("availabilityPushed", "false");
                return RedirectToAction("GetAll");
                
            }
            
            
        }
        public async Task<IActionResult> GetAllRes(bool res)
        {
            
            return Json(new { data = await db.Books.Where(b => b.Reserved == true).ToListAsync() });

            
        }
    }
}
