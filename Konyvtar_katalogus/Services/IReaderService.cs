using Konyvtar_katalogus.Models;

namespace Konyvtar_katalogus.Services
{
    public interface IReaderService
    {
        bool AddReader(string name);
        IEnumerable<Reader> GetAllReaders();
        bool DeleteReader(int readerId);
    }
}
