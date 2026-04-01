using Konyvtar_katalogus.Models;
using Microsoft.EntityFrameworkCore;

namespace Konyvtar_katalogus.Data.Repositories
{
    public class FineRepository : IFineRepository
    {
        private readonly LibraryDbContext _context;

        public FineRepository(LibraryDbContext context)
        {
            _context = context;
        }

        public Fine? GetById(int id)
        {
            return _context.Fines
                .Include(f => f.Loan)
                    .ThenInclude(l => l.Reader)
                .Include(f => f.Loan)
                    .ThenInclude(l => l.Copy)
                        .ThenInclude(c => c.Book)
                .FirstOrDefault(f => f.fineid == id);
        }

        public Fine? GetByLoanId(int loanId)
        {
            return _context.Fines.FirstOrDefault(f => f.LoanId == loanId);
        }

        public IEnumerable<Fine> GetAll()
        {
            return _context.Fines
                .Include(f => f.Loan)
                    .ThenInclude(l => l.Reader)
                .Include(f => f.Loan)
                    .ThenInclude(l => l.Copy)
                        .ThenInclude(c => c.Book)
                .OrderByDescending(f => f.fineid)
                .ToList();
        }

        public IEnumerable<Fine> GetUnpaid()
        {
            return _context.Fines
                .Include(f => f.Loan)
                    .ThenInclude(l => l.Reader)
                .Include(f => f.Loan)
                    .ThenInclude(l => l.Copy)
                        .ThenInclude(c => c.Book)
                .Where(f => !f.isPaid)
                .OrderByDescending(f => f.fineid)
                .ToList();
        }

        public void Add(Fine fine)
        {
            _context.Fines.Add(fine);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}
