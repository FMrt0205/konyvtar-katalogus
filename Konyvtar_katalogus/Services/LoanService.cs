using Konyvtar_katalogus.Data.Repositories;
using Konyvtar_katalogus.Models;

namespace Konyvtar_katalogus.Services
{
    public class LoanService : ILoanService
    {
        private readonly ILoanRepository _loanRepository;
        private readonly IReaderRepository _readerRepository;
        private readonly ICopyRepository _copyRepository;
        private readonly IFineRepository _fineRepository;
        private readonly INotificationQueueRepository _notificationQueueRepository;

        public LoanService(
            ILoanRepository loanRepository,
            IReaderRepository readerRepository,
            ICopyRepository copyRepository,
            IFineRepository fineRepository,
            INotificationQueueRepository notificationQueueRepository)
        {
            _loanRepository = loanRepository;
            _readerRepository = readerRepository;
            _copyRepository = copyRepository;
            _fineRepository = fineRepository;
            _notificationQueueRepository = notificationQueueRepository;
        }

        
        public bool CreateLoan(int readerId, int copyId)
        {
            var reader = _readerRepository.GetById(readerId);
            if (reader == null)
                return false;

            var copy = _copyRepository.GetById(copyId);
            if (copy == null || !copy.isAvailable)
                return false;

            var loan = new Loan
            {
                ReaderId = readerId,
                copyid = copyId,
                loanDate = DateTime.Now,
                returnDate = DateTime.MinValue
            };

            copy.isAvailable = false;

            _loanRepository.Add(loan);
            _loanRepository.SaveChanges();
            return true;
        }
 
        public bool ReturnLoan(int loanId)
        {
            var loan = _loanRepository.GetById(loanId);
            if (loan == null || loan.returnDate != DateTime.MinValue)
                return false;

            loan.returnDate = DateTime.Now;
            loan.Copy.isAvailable = true;

            var overdueDays = (loan.returnDate.Date - loan.dueDate.Date).Days;
            if (overdueDays > 0 && _fineRepository.GetByLoanId(loan.loanid) == null)
            {
                _fineRepository.Add(new Fine
                {
                    LoanId = loan.loanid,
                    amount = overdueDays * 100,
                    isPaid = false
                });
            }

            _loanRepository.SaveChanges();
            return true;
        }

        public IEnumerable<Loan> GetActiveLoans()
        {
            return _loanRepository.GetActive();
        }

        public IEnumerable<Loan> GetAllLoans()
        {
            return _loanRepository.GetAll();
        }

        public int QueueOverdueNotifications()
        {
            var activeLoans = _loanRepository.GetActive();
            var queued = 0;

            foreach (var loan in activeLoans)
            {
                if (DateTime.Now <= loan.dueDate)
                    continue;

                if (_notificationQueueRepository.HasPendingForLoan(loan.loanid))
                    continue;

                _notificationQueueRepository.Add(new NotificationQueueItem
                {
                    LoanId = loan.loanid,
                    Message = $"Késedelmes kölcsönzés: {loan.Reader.name} - {loan.Copy.Book.title} ({loan.Copy.InventoryNumber}), határidő: {loan.dueDate:yyyy-MM-dd}."
                });

                queued++;
            }

            if (queued > 0)
                _notificationQueueRepository.SaveChanges();

            return queued;
        }

        public int SendQueuedNotifications()
        {
            var pendingItems = _notificationQueueRepository.GetPending().ToList();
            if (pendingItems.Count == 0)
                return 0;

            foreach (var item in pendingItems)
            {
                Console.WriteLine($"[ÉRTESÍTÉS] {item.Message}");
                item.Status = "Sent";
                item.SentAt = DateTime.Now;
            }

            _notificationQueueRepository.SaveChanges();
            return pendingItems.Count;
        }
    }
}
