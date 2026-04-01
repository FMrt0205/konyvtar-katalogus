using Konyvtar_katalogus.Models;

namespace Konyvtar_katalogus.Services
{
    public interface ILoanService
    {
        bool CreateLoan(int readerId, int copyId);
        bool ReturnLoan(int loanId);
        IEnumerable<Loan> GetActiveLoans();
        IEnumerable<Loan> GetAllLoans();
        int QueueOverdueNotifications();
        int SendQueuedNotifications();
    }
}
