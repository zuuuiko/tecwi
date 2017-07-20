using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tecwi.DAL.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<Entities.Book> Books { get; }

        void Save();
    }
}
