using Konyvtar_katalogus.Models;

namespace Konyvtar_katalogus.Data.Repositories
{
    public interface IBookRepository
    {
        void Add(Book book);
        Book? GetById(int id);
        IEnumerable<Book> GetAll();
        IEnumerable<Book> Search(string searchTerm);
        void Delete(Book book);
        void SaveChanges();
    }
}
