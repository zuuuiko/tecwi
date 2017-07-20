using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tecwi.DAL.EF;
using Tecwi.DAL.Entities;
using Tecwi.DAL.Interfaces;

namespace Tecwi.DAL.Repositories
{
    public class EFUnitOfWork : IUnitOfWork
    {
        private BookContext db;
        private BookRepository bookRepository;

        public EFUnitOfWork(string connectionString)
        {
            db = new BookContext(connectionString);
        }
        public IRepository<Book> Books
        {
            get
            {
                if(bookRepository == null)
                    bookRepository = new BookRepository(db);
                return bookRepository;
            }
        }

        public void Save()
        {
            db.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    db.Dispose();
                }
                this.disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
