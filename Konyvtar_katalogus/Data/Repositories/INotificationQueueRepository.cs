using Konyvtar_katalogus.Models;

namespace Konyvtar_katalogus.Data.Repositories
{
    public interface INotificationQueueRepository
    {
        IEnumerable<NotificationQueueItem> GetPending();
        bool HasPendingForLoan(int loanId);
        void Add(NotificationQueueItem item);
        void SaveChanges();
    }
}
