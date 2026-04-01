using Konyvtar_katalogus.Models;

namespace Konyvtar_katalogus.Services
{
    public interface IFineService
    {
        IEnumerable<Fine> GetAllFines();
        IEnumerable<Fine> GetUnpaidFines();
        bool PayFine(int fineId);
    }
}
