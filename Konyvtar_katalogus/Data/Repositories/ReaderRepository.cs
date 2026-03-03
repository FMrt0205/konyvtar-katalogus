using Konyvtar_katalogus.Models;
using Microsoft.EntityFrameworkCore;

namespace Konyvtar_katalogus.Data.Repositories
{
    public class ReaderRepository : IReaderRepository
    {
        private readonly LibraryDbContext _context;

        public ReaderRepository(LibraryDbContext context)
        {
            _context = context;
        }

        public void Add(Reader reader)
        {
            _context.Readers.Add(reader);
        }

        public Reader? GetById(int id)
        {
            return _context.Readers
                .Include(r => r.Loans)
                .FirstOrDefault(r => r.readerid == id);
        }

        public IEnumerable<Reader> GetAll()
        {
            return _context.Readers
                .Include(r => r.Loans)
                .ToList();
        }

        public void Delete(Reader reader)
        {
            _context.Readers.Remove(reader);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}
