using Konyvtar_katalogus.Models;
using Microsoft.EntityFrameworkCore;

namespace Konyvtar_katalogus.Data.Repositories
{
    public class CopyRepository : ICopyRepository
    {
        private readonly LibraryDbContext _context;

        public CopyRepository(LibraryDbContext context)
        {
            _context = context;
        }

        public void Add(Copy copy)
        {
            _context.Copies.Add(copy);
        }

        public Copy? GetById(int id)
        {
            return _context.Copies
                .Include(c => c.Book)
                .FirstOrDefault(c => c.copyid == id);
        }

        public IEnumerable<Copy> GetAll()
        {
            return _context.Copies
                .Include(c => c.Book)
                .ToList();
        }

        public IEnumerable<Copy> GetAvailable()
        {
            return _context.Copies
                .Include(c => c.Book)
                .Where(c => c.isAvailable)
                .ToList();
        }

        public void Delete(Copy copy)
        {
            _context.Copies.Remove(copy);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}
