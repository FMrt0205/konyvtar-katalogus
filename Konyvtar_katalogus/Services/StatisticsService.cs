using Konyvtar_katalogus.Data;
using Konyvtar_katalogus.Models;
using Microsoft.EntityFrameworkCore;

namespace Konyvtar_katalogus.Services
{
    public class StatisticsService : IStatisticsService
    {
        private readonly LibraryDbContext _context;

        public StatisticsService(LibraryDbContext context)
        {
            _context = context;
        }

        public async Task<LibraryStatistics> GenerateStatisticsAsync()
        {
            var books = await _context.Books.AsNoTracking().ToListAsync();
            var copies = await _context.Copies.AsNoTracking().ToListAsync();
            var readers = await _context.Readers.AsNoTracking().ToListAsync();
            var loans = await _context.Loans.AsNoTracking().ToListAsync();
            var fines = await _context.Fines.AsNoTracking().ToListAsync();

            var stats = new LibraryStatistics();

            await Task.Run(() =>
            {
                Parallel.Invoke(
                    () => stats.TotalBooks = books.Count,
                    () => stats.TotalCopies = copies.Count,
                    () => stats.AvailableCopies = copies.Count(c => c.isAvailable),
                    () => stats.TotalReaders = readers.Count,
                    () => stats.ActiveLoans = loans.Count(l => l.returnDate == DateTime.MinValue),
                    () => stats.OverdueLoans = loans.Count(l => l.returnDate == DateTime.MinValue && l.loanDate.AddDays(l.LoanPeriodDays) < DateTime.Now),
                    () => stats.UnpaidFines = fines.Count(f => !f.isPaid)
                );
            });

            return stats;
        }
    }
}
