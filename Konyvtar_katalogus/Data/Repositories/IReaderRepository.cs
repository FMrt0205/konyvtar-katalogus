using Konyvtar_katalogus.Models;

namespace Konyvtar_katalogus.Data.Repositories
{
    public interface IReaderRepository
    {
        void Add(Reader reader);
        Reader? GetById(int id);
        IEnumerable<Reader> GetAll();
        void Delete(Reader reader);
        void SaveChanges();
    }
}
