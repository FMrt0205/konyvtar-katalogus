using Konyvtar_katalogus.Data.Repositories;
using Konyvtar_katalogus.Models;

namespace Konyvtar_katalogus.Services
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;

        public BookService(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public bool AddBook(string title, string author, string isbn)
        {
            if (string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(author) || string.IsNullOrWhiteSpace(isbn))
                return false;

            if (isbn.Length != 13)
                return false;

            var book = new Book
            {
                title = title,
                author = author,
                isbn = isbn
            };

            _bookRepository.Add(book);
            _bookRepository.SaveChanges();
            return true;
        }

        public IEnumerable<Book> GetAllBooks()
        {
            return _bookRepository.GetAll();
        }

        public IEnumerable<Book> SearchBooks(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return Enumerable.Empty<Book>();

            return _bookRepository.Search(searchTerm);
        }

        public bool DeleteBook(int bookId)
        {
            var book = _bookRepository.GetById(bookId);
            if (book == null)
                return false;

            if (book.Copies?.Any() == true)
                return false;

            _bookRepository.Delete(book);
            _bookRepository.SaveChanges();
            return true;
        }
    }
}
