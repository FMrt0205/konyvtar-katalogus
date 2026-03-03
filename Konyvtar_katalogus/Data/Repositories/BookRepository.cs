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

        public IEnumerable<Book> Search(string searchTerm)
        {
            var searchLower = searchTerm.ToLower();
            return _context.Books
                .Include(b => b.Copies)
                .Where(b => b.title.ToLower().Contains(searchLower) ||
                            b.author.ToLower().Contains(searchLower) ||
                            b.isbn.Contains(searchTerm))
                .ToList();
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
