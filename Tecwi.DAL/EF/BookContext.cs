namespace Tecwi.DAL.EF
{
    using System;
    using System.Data.Entity;
    using System.Linq;

    public class BookContext : DbContext
    {
        public BookContext(string connectionString)
                : base(connectionString)
        {
        }
        public virtual DbSet<Entities.Book> Books { get; set; }
    }
}