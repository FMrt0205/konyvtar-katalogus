using Konyvtar_katalogus.Models;
using Microsoft.EntityFrameworkCore;

namespace Konyvtar_katalogus.Data.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly LibraryDbContext _context;

        public BookRepository(LibraryDbContext context)
        {
            _context = context;
        }

        public void Add(Book book)
        {
            _context.Books.Add(book);
        }

        public Book? GetById(int id)
        {
            return _context.Books
                .Include(b => b.Copies)
                .FirstOrDefault(b => b.bookid == id);
        }

        public IEnumerable<Book> GetAll()
        {
            return _context.Books
                .Include(b => b.Copies)
                .ToList();
        }

        public IEnumerable<Book> Search(string searchTerm, bool sortByMatchCount = false)
        {
            
            var searchLower = searchTerm.ToLower();
            var matches = _context.Books
                .Include(b => b.Copies)
                .Where(b => b.title.ToLower().Contains(searchLower) ||
                            b.author.ToLower().Contains(searchLower) ||
                            b.isbn.Contains(searchTerm))
                .AsEnumerable()
                
                .Select(b => new
                {
                    Book = b,
                    Score = CalculateRelevanceScore(b, searchLower, searchTerm),
                    MatchCount = CountOccurrences(b.title, searchLower)
                                 + CountOccurrences(b.author, searchLower)
                });
            

            if (sortByMatchCount)
            {
                return matches
                    .OrderByDescending(x => x.MatchCount)
                    .ThenByDescending(x => x.Score)
                    .ThenBy(x => x.Book.title)
                    .Select(x => x.Book)
                    .ToList();
            }

            return matches
                .OrderByDescending(x => x.Score)
                .ThenByDescending(x => x.MatchCount)
                .ThenBy(x => x.Book.title)
                .Select(x => x.Book)
                .ToList();
        }
    
        private static int CalculateRelevanceScore(Book book, string searchLower, string searchOriginal)
        {
            var score = 0;
            

            if (book.isbn.Equals(searchOriginal, StringComparison.OrdinalIgnoreCase))
                score += 100;

            score += CountOccurrences(book.title, searchLower) * 10;
            score += CountOccurrences(book.author, searchLower) * 8;
            

            if (book.title.StartsWith(searchOriginal, StringComparison.OrdinalIgnoreCase))
                score += 5;
            if (book.author.StartsWith(searchOriginal, StringComparison.OrdinalIgnoreCase))
                score += 4;

            return score;
        }
        
        private static int CountOccurrences(string source, string term)
        {
            if (string.IsNullOrWhiteSpace(source) || string.IsNullOrWhiteSpace(term))
                return 0;
            var count = 0;
            var index = 0;
            while ((index = source.IndexOf(term, index, StringComparison.OrdinalIgnoreCase)) >= 0)
            {
                count++;
                index += term.Length;
            }

            return count;
        }

        public void Delete(Book book)
        {
            _context.Books.Remove(book);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}
