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
        public ActionResult Index()
        {

            return View(db.Books.ToList());
        }
        public IActionResult ShowAvailable()
        {
            var avBooks = db.Books.Where(b => b.Archived != true & b.Reserved != true).ToList();
            return RedirectToAction("Index");
        }
        public ActionResult Add(Book book)
        {


            return View(bookRecord);
        }

        [HttpPost]
        public ActionResult Create(Book book)
        {

            db.Books.Add(book);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        
        public ActionResult Update(int? id)
        {
            bookRecord = db.Books.FirstOrDefault(i => i.Id == id);
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
        public ActionResult Update(Book book)
        {
            if (ModelState.IsValid)
            {
                
                
                db.Books.Update(book);
                

                db.SaveChanges();

                
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
            bool reservationPushed = Convert.ToBoolean(HttpContext.Session.GetString("reservationPushed"));

            if (reservationPushed == false)
            {
                HttpContext.Session.SetString("reservationPushed", "true");
                return Json(new { data = await db.Books.Where(b => b.Reserved == true).ToListAsync() });

            }
            else
            {
                HttpContext.Session.SetString("reservationPushed", "false");
                return RedirectToAction("GetAll");

            }


        }
    }
}
