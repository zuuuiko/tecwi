using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tecwi.DAL.EF;
using Tecwi.DAL.Entities;
using Tecwi.DAL.Interfaces;

namespace Tecwi.DAL.Repositories
{
    public class BookRepository : IRepository<Book>
    {
        private BookContext db;

        public BookRepository(BookContext context)
        {
            this.db = context;
        }

        public List<Book> GetAll()
        {
            return db.Books.ToList();
        }

        public Book Get(int id)
        {
            return db.Books.FirstOrDefault(b => b.Id == id);
        }

        public void Create(Book book)
        {
            db.Books.Add(book);
        }

        public void Update(Book book)
        {
            db.Entry(book).State = EntityState.Modified;
        }

        public List<Book> Find(Func<Book, Boolean> predicate)
        {
            return db.Books.Where(predicate).ToList();
        }

        public void Delete(int id)
        {
            Book book = db.Books.FirstOrDefault(b => b.Id == id);
            if (book != null)
                db.Books.Remove(book);
        }
    }
}
