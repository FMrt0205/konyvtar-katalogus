using Konyvtar_katalogus.Models;
using Microsoft.EntityFrameworkCore;

namespace Konyvtar_katalogus.Data.Repositories
{
    public class NotificationQueueRepository : INotificationQueueRepository
    {
        private readonly LibraryDbContext _context;

        public NotificationQueueRepository(LibraryDbContext context)
        {
            _context = context;
        }

        public IEnumerable<NotificationQueueItem> GetPending()
        {
            return _context.NotificationQueueItems
                .Include(n => n.Loan)
                    .ThenInclude(l => l.Reader)
                .Include(n => n.Loan)
                    .ThenInclude(l => l.Copy)
                        .ThenInclude(c => c.Book)
                .Where(n => n.Status == "Pending")
                .OrderBy(n => n.CreatedAt)
                .ToList();
        }

        public bool HasPendingForLoan(int loanId)
        {
            return _context.NotificationQueueItems.Any(n => n.LoanId == loanId && n.Status == "Pending");
        }

        public void Add(NotificationQueueItem item)
        {
            _context.NotificationQueueItems.Add(item);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}
