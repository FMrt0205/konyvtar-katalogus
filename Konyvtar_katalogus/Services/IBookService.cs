using Konyvtar_katalogus.Models;

namespace Konyvtar_katalogus.Services
{
    public interface IBookService
    {
        bool AddBook(string title, string author, string isbn);
        IEnumerable<Book> GetAllBooks();
        IEnumerable<Book> SearchBooks(string searchTerm);
        bool DeleteBook(int bookId);
    }
}
