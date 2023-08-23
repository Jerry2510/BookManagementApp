using ConceptArchitect.BookManagement;
using Microsoft.AspNetCore.Mvc;



namespace BooksWeb02.Controllers
{
    public class BookController : Controller
    {
        IBookService bookService;



        public BookController(IBookService books)
        {
            this.bookService = books;
        }



        public async Task<ViewResult> Index()
        {
            var books = await bookService.GetAllBooks();



            return View(books);
        }



        public async Task<ViewResult> Details(string id)
        {
            var book = await bookService.GetBookById(id);



            return View(book);
        }

        [HttpGet]
        public ViewResult Delete()
        {
            return View();
        }






        [HttpGet]
        public ViewResult Add()
        {
            return View(new Book());
        }



        [HttpPost]
        public async Task<ActionResult> Add(Book book)
        {
            await bookService.AddBook(book);



            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task Delete(string bookId)
        {
            await bookService.DeleteBook(bookId);
            Index();
            RedirectToAction("Index");



        }
        [HttpGet]
        public async Task<ViewResult> Edit(string id)
        {
            var author = await bookService.GetBookById(id);
            return View(author);
        }



        [HttpPost]
        public async Task<ActionResult> Edit(Book book)
        {
            await bookService.UpdateBook(book);



            return RedirectToAction("Index");
        }

        



        public async Task<ActionResult> SaveV2(Book book)
        {
            await bookService.AddBook(book);



            return RedirectToAction("Index");
        }





        public Book SaveV1(string id, string title, string description, string authorId, string photourl)
        {
            var book = new Book()
            {
                Id = id,
                Title = title,
                Description = description,
                AuthorId = authorId,
                CoverPhoto = photourl
            };



            return book;
        }



        public Book SaveV0()
        {
            var book = new Book()
            {
                Id = Request.Form["id"],
                Title = Request.Form["title"],
                Description = Request.Form["description"],
                AuthorId = Request.Form["authorId"],
                CoverPhoto = Request.Form["photourl"]
            };



            return book;
        }





    }
}