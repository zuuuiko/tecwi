using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Tecwi.BLL.Services;
using Tecwi.DAL.Interfaces;
using Tecwi.DAL.Entities;
using Tecwi.DAL.Repositories;
using System.Collections.Generic;
using Tecwi.BLL.DTO;
using System.Linq;

namespace Tecwi.BLL.Tests
{
    [TestClass]
    public class DataServiceTest
    {
        Mock<IRepository<Book>> mockRepo;
        Mock<IUnitOfWork> mockUnitOfWork;
        DataService dataService;
        Book bookSetup;
        List<Book> bookListSetup;

        [TestInitialize]
        public void Setup()
        {
            bookSetup = new Book
            {
                Id = 1,
                Title = "some Title1",
                AuthorName = "some Author's Name1",
                Date = DateTime.Now,
                Description = "some Description1"
            };
            bookListSetup = new List<Book>
            {
                bookSetup,
                new Book
                {
                    Id = 2,
                    Title="some Title2",
                    AuthorName = "some Author's Name2",
                    Date = DateTime.Now.AddDays(-1),
                    Description = "some Description2"
                },
                new Book
                {
                    Id = 3,
                    Title="some Title3",
                    AuthorName = "some Author's Name13",
                    Date = DateTime.Now.AddDays(-2),
                    Description = "some Description3"
                }
            };
            mockRepo = new Mock<IRepository<Book>>();

            mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.SetupGet(x => x.Books).Returns(mockRepo.Object);
            mockUnitOfWork.Setup(x => x.Books.GetAll()).Returns(bookListSetup);
            mockUnitOfWork.Setup(x => x.Books.Find(It.IsAny<Func<Book, bool>>()))
                .Returns((Func<Book, bool> f) => bookListSetup.Where(b => f(b)).ToList());
            mockUnitOfWork.Setup(x => x.Books.Get(It.IsAny<int>())).Returns(bookListSetup[0]);
            mockUnitOfWork.Setup(x => x.Books.Create(It.IsAny<Book>())).Callback((Book b) => bookListSetup.Add(b));
            mockUnitOfWork.Setup(x => x.Books.Delete(It.IsAny<int>())).Callback((int id) => bookListSetup.Where(b => b.Id != id));
            mockUnitOfWork.Setup(x => x.Books.Update(It.IsAny<Book>())).Callback((Book b) => bookSetup = b);

            dataService = new DataService(mockUnitOfWork.Object);
        }


        [TestMethod]
        public void GetAllBooksTest()
        {
            var allBooks = dataService.GetBooks();
            mockUnitOfWork.Verify(x => x.Books.GetAll(), Times.Once);
            Assert.IsInstanceOfType(allBooks, typeof(List<BookDTO>), "Return type is not correct");
            Assert.AreEqual(bookListSetup.Count, allBooks.Count, "Books count is not correct");
            var booksByTitle = dataService.GetBooks("1");
        }

        [TestMethod]
        public void GetBooksByTitleTest()
        {
            var booksByTitle = dataService.GetBooks("1");
            mockUnitOfWork.Verify(x => x.Books.Find(It.IsAny<Func<Book, bool>>()), Times.Once);
            Assert.AreEqual(bookListSetup.Where(b=>b.Title.Contains("1")).ToArray().Length, 
                booksByTitle.Count, "Book count is not correct");
        }
        [TestMethod]
        public void GetBooksByAuthorNameTest()
        {
            var booksByAuthorName = dataService.GetBooks(null, "1");
            mockUnitOfWork.Verify(x => x.Books.Find(It.IsAny<Func<Book, bool>>()), Times.Once);
            Assert.AreEqual(bookListSetup.Where(b => b.AuthorName.Contains("1")).ToArray().Length, 
                booksByAuthorName.Count, "Book count is not correct");
        }

        [TestMethod]
        public void GetBooksByTitleAndAuthorNameTest()
        {
            var booksByTitleAndAuthorName = dataService.GetBooks("1", "1");
            mockUnitOfWork.Verify(x => x.Books.Find(It.IsAny<Func<Book, bool>>()), Times.Once);
            Assert.AreEqual(bookListSetup.Where(b => b.Title.Contains("1") && b.AuthorName.Contains("1")).ToArray().Length, 
                booksByTitleAndAuthorName.Count, "Book count is not correct");
        }

        [TestMethod]
        public void CreateBookTest()
        {
            int id = 4;
            dataService.CreateBook(new BookDTO
            {
                Id = id,
                Title = "some Title4",
                AuthorName = "some Author's Name4",
                Date = DateTime.Now.AddDays(-4),
                Description = "some Description4"
            });
            mockUnitOfWork.Verify(x => x.Books.Create(It.IsAny<Book>()), Times.Once);
            Assert.IsNotNull(bookListSetup.FirstOrDefault(b => b.Id == id), "Book has not inserted");
        }

        [TestMethod]
        public void UpdateBookTest()
        {
            var bDTO = new BookDTO
            {
                Id = 1,
                Title = "some Title1 updated",
                AuthorName = "some Author's Name1 updated",
                Date = DateTime.Now.AddDays(-10),
                Description = "some Description1 updated"
            };
            dataService.UpdateBook(bDTO);
            mockUnitOfWork.Verify(x => x.Books.Update(It.IsAny<Book>()), Times.Once);
            Assert.AreEqual(bDTO.Title, bookSetup.Title, "Book Title is not correct");
            Assert.AreEqual(bDTO.AuthorName, bookSetup.AuthorName, "Book AuthorName is not correct");
            Assert.AreEqual(bDTO.Date, bookSetup.Date, "Book Date is not correct");
            Assert.AreEqual(bDTO.Description, bookSetup.Description, "Book Description is not correct");
        }

        [TestMethod]
        public void DeleteBookTest()
        {
            var b = new Book
            {
                Id = 1,
                Title = "some Title1",
                AuthorName = "some Author's Name1",
                Date = DateTime.Now,
                Description = "some Description1"
            };
            dataService.DeleteBook(1);
            mockUnitOfWork.Verify(x => x.Books.Delete(It.IsAny<int>()), Times.Once);
            CollectionAssert.DoesNotContain(bookListSetup, b, "Book has not deleted");
        }
    }
}
