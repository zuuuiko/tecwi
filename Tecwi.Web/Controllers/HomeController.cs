using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tecwi.BLL.DTO;
using Tecwi.BLL.Interfaces;
using Tecwi.Web.Models;

namespace Tecwi.Web.Controllers
{
    public class HomeController : Controller
    {
        IDataService dataService;

        public HomeController(IDataService ds)
        {
            dataService = ds;
        }
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult GetBooks(string title, string authorName)
        {
            var books = SelectBooks(title, authorName);
            return PartialView(books);
        }

        public ActionResult AddBook(BookViewModel book)
        {
            Mapper.Initialize(cfg => cfg.CreateMap<BookViewModel, BookDTO>());
            var b = Mapper.Map<BookViewModel, BookDTO>(book);
            dataService.CreateBook(b);
            var books = SelectBooks(null, null);
            return PartialView("GetBooks", books);
        }

        public void EditBook(BookViewModel book)
        {
            Mapper.Initialize(cfg => cfg.CreateMap<BookViewModel, BookDTO>());
            var b = Mapper.Map<BookViewModel, BookDTO>(book);
            dataService.UpdateBook(b);
        }

        public ActionResult DeleteBook(int id)
        {
            dataService.DeleteBook(id);

            var books = SelectBooks(null, null);
            return PartialView("GetBooks", books);
        }

        private List<BookViewModel> SelectBooks(string title, string authorName)
        {
            var bookDTOs = dataService.GetBooks(title, authorName);
            Mapper.Initialize(cfg => cfg.CreateMap<BookDTO, BookViewModel>());
            return Mapper.Map<List<BookDTO>, List<BookViewModel>>(bookDTOs);
        }

        protected override void Dispose(bool disposing)
        {
            dataService.Dispose();
            base.Dispose(disposing);
        }
    }
}