using Konyvtar_katalogus.Models;

namespace Konyvtar_katalogus.Data.Repositories
{
    public interface ILoanRepository
    {
        void Add(Loan loan);
        Loan? GetById(int id);
        IEnumerable<Loan> GetAll();
        IEnumerable<Loan> GetActive();
        void SaveChanges();
    }
}
