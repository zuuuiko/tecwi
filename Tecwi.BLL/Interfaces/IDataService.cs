using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tecwi.BLL.DTO;

namespace Tecwi.BLL.Interfaces
{
    public interface IDataService
    {
        /// <summary>
        /// SELECT all Books when call without parameters (with null default values)
        /// or SELECT all Books by Title and/or AuthorName
        /// </summary>
        /// <param name="title">System.String - null value by default</param>
        /// <param name="authorName">System.String - null value by default</param>
        /// <returns>System.Collections.Generics.List-BooksDTO-></returns>
        List<BookDTO> GetBooks(string title = null, string authorName = null);
        void CreateBook(BookDTO book);
        void UpdateBook(BookDTO book);
        void DeleteBook(int bookId);
        void Dispose();
    }
}
