using Konyvtar_katalogus.Models;
using Microsoft.EntityFrameworkCore;

namespace Konyvtar_katalogus.Data.Repositories
{
    public class LoanRepository : ILoanRepository
    {
        private readonly LibraryDbContext _context;

        public LoanRepository(LibraryDbContext context)
        {
            _context = context;
        }

        public void Add(Loan loan)
        {
            _context.Loans.Add(loan);
        }

        public Loan? GetById(int id)
        {
            return _context.Loans
                .Include(l => l.Copy)
                .Include(l => l.Reader)
                .FirstOrDefault(l => l.loanid == id);
        }

        public IEnumerable<Loan> GetAll()
        {
            return _context.Loans
                .Include(l => l.Copy)
                    .ThenInclude(c => c.Book)
                .Include(l => l.Reader)
                .OrderByDescending(l => l.loanDate)
                .ToList();
        }

        public IEnumerable<Loan> GetActive()
        {
            return _context.Loans
                .Include(l => l.Copy)
                    .ThenInclude(c => c.Book)
                .Include(l => l.Reader)
                .Where(l => l.returnDate == DateTime.MinValue)
                .ToList();
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}
