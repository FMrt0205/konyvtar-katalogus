using Konyvtar_katalogus.Models;

namespace Konyvtar_katalogus.Data.Repositories
{
    public interface IFineRepository
    {
        Fine? GetById(int id);
        Fine? GetByLoanId(int loanId);
        IEnumerable<Fine> GetAll();
        IEnumerable<Fine> GetUnpaid();
        void Add(Fine fine);
        void SaveChanges();
    }
}
