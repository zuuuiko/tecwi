using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tecwi.BLL.DTO;
using Tecwi.DAL.Entities;
using Tecwi.DAL.Interfaces;

namespace Tecwi.BLL.Services
{
    public class DataService : Interfaces.IDataService
    {
        IUnitOfWork Database { get; set; }
        public DataService(IUnitOfWork unitOfWork)
        {
            Database = unitOfWork;
        }

        public void CreateBook(BookDTO book)
        {
            Mapper.Initialize(cfg => cfg.CreateMap<BookDTO, Book>());
            var b = Mapper.Map<BookDTO, Book>(book);
            try
            {
                Database.Books.Create(b);
                Database.Save();

            }
            catch (Exception ex)
            {
                //log ex
                throw;
            }
        }

        public void UpdateBook(BookDTO book)
        {
            Mapper.Initialize(cfg => cfg.CreateMap<BookDTO, Book>());
            var b = Mapper.Map<BookDTO, Book>(book);
            try
            {
                Database.Books.Update(b);
                Database.Save();

            }
            catch (Exception ex)
            {
                //log ex
                throw;
            }
        }

        public void DeleteBook(int bookId)
        {
            try
            {
                Database.Books.Delete(bookId);
                Database.Save();
            }
            catch (Exception ex)
            {
                //log ex
                throw;
            }
        }

        /// <summary>
        /// SELECT all Books when call without parameters (with null default values)
        /// or SELECT all Books by Title and/or AuthorName.
        /// </summary>
        /// <param name="title">System.String - null value by default</param>
        /// <param name="authorName">System.String - null value by default</param>
        /// <returns>System.Collections.Generics.List-BooksDTO-></returns>
        public List<BookDTO> GetBooks(string title = null, string authorName = null)
        {
            Mapper.Initialize(cfg => cfg.CreateMap<Book, BookDTO>());
            try
            {
                if (string.IsNullOrEmpty(title) && string.IsNullOrEmpty(authorName))
                {
                    return Mapper.Map<IEnumerable<Book>, List<BookDTO>>(Database.Books.GetAll());
                }
                else
                {
                    var books = Database.Books.Find(b => b.Title.Contains(title == null ? "" : title)
                                                      && b.AuthorName.Contains(authorName == null ? "" : authorName));
                    return Mapper.Map<IEnumerable<Book>, List<BookDTO>>(books);
                }
            }
            catch (Exception ex)
            {
                //log ex
                throw;
            }

        }

        public void Dispose()
        {
            Database.Dispose();
        }
    }
}
