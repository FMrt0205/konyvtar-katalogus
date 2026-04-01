using Konyvtar_katalogus.Models;

namespace Konyvtar_katalogus.Services
{
    public interface IBookService
    {
        bool AddBook(string title, string author, string isbn);
        IEnumerable<Book> GetAllBooks();
        IEnumerable<Book> SearchBooks(string searchTerm, bool sortByMatchCount = false);
        Task<(int importedCount, int skippedCount)> ImportBooksFromFileAsync(string filePath);
        bool DeleteBook(int bookId);
    }
}
