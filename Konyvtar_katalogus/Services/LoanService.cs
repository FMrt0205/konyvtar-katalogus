using Konyvtar_katalogus.Data.Repositories;
using Konyvtar_katalogus.Models;

namespace Konyvtar_katalogus.Services
{
    public class LoanService : ILoanService
    {
        private readonly ILoanRepository _loanRepository;
        private readonly IReaderRepository _readerRepository;
        private readonly ICopyRepository _copyRepository;

        public LoanService(ILoanRepository loanRepository, IReaderRepository readerRepository, ICopyRepository copyRepository)
        {
            _loanRepository = loanRepository;
            _readerRepository = readerRepository;
            _copyRepository = copyRepository;
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
    }
}
