using Konyvtar_katalogus.Data.Repositories;
using Konyvtar_katalogus.Models;
using System.Collections.Concurrent;

namespace Konyvtar_katalogus.Services
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;

        public BookService(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        // import C:\Users\Felhasználó\Desktop\books_for_import_v2.csv

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

        public IEnumerable<Book> SearchBooks(string searchTerm, bool sortByMatchCount = false)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return Enumerable.Empty<Book>();

            return _bookRepository.Search(searchTerm, sortByMatchCount);
        }

        public async Task<(int importedCount, int skippedCount)> ImportBooksFromFileAsync(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath) || !File.Exists(filePath))
                return (0, 0);

            var lines = await File.ReadAllLinesAsync(filePath);
            if (lines.Length == 0)
                return (0, 0);

            var parsedBooks = new ConcurrentBag<Book>();

            Parallel.ForEach(lines, line =>
            {
                if (string.IsNullOrWhiteSpace(line))
                    return;

                var parts = line.Split(';');
                if (parts.Length < 3)
                    return;

                var title = parts[0].Trim();
                var author = parts[1].Trim();
                var isbn = parts[2].Trim();

                if (string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(author) || isbn.Length != 13)
                    return;

                parsedBooks.Add(new Book
                {
                    title = title,
                    author = author,
                    isbn = isbn
                });
            });

            var existingIsbns = _bookRepository.GetAll()
                .Select(b => b.isbn)
                .ToHashSet(StringComparer.OrdinalIgnoreCase);

            var uniqueBooks = parsedBooks
                .GroupBy(b => b.isbn, StringComparer.OrdinalIgnoreCase)
                .Select(g => g.First())
                .ToList();

            var importedCount = 0;
            foreach (var book in uniqueBooks)
            {
                if (existingIsbns.Contains(book.isbn))
                    continue;

                _bookRepository.Add(book);
                existingIsbns.Add(book.isbn);
                importedCount++;
            }

            if (importedCount > 0)
                _bookRepository.SaveChanges();

            var skippedCount = lines.Length - importedCount;
            return (importedCount, skippedCount);
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
