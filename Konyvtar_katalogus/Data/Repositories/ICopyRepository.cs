using Konyvtar_katalogus.Models;

namespace Konyvtar_katalogus.Data.Repositories
{
    public interface ICopyRepository
    {
        void Add(Copy copy);
        Copy? GetById(int id);
        IEnumerable<Copy> GetAll();
        IEnumerable<Copy> GetAvailable();
        void Delete(Copy copy);
        void SaveChanges();
    }
}
