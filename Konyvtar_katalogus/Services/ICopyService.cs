using Konyvtar_katalogus.Models;

namespace Konyvtar_katalogus.Services
{
    public interface ICopyService
    {
        bool AddCopy(int bookId, string inventoryNumber);
        IEnumerable<Copy> GetAllCopies();
        IEnumerable<Copy> GetAvailableCopies();
        bool DeleteCopy(int copyId);
    }
}
