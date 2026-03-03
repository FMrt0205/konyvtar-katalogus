using Konyvtar_katalogus.Data.Repositories;
using Konyvtar_katalogus.Models;

namespace Konyvtar_katalogus.Services
{
    public class CopyService : ICopyService
    {
        private readonly ICopyRepository _copyRepository;
        private readonly IBookRepository _bookRepository;

        public CopyService(ICopyRepository copyRepository, IBookRepository bookRepository)
        {
            _copyRepository = copyRepository;
            _bookRepository = bookRepository;
        }

        public bool AddCopy(int bookId, string inventoryNumber)
        {
            var book = _bookRepository.GetById(bookId);
            if (book == null)
                return false;

            if (string.IsNullOrWhiteSpace(inventoryNumber))
                return false;

            var copy = new Copy
            {
                InventoryNumber = inventoryNumber,
                isAvailable = true,
                BookId = bookId
            };

            _copyRepository.Add(copy);
            _copyRepository.SaveChanges();
            return true;
        }

        public IEnumerable<Copy> GetAllCopies()
        {
            return _copyRepository.GetAll();
        }

        public IEnumerable<Copy> GetAvailableCopies()
        {
            return _copyRepository.GetAvailable();
        }

        public bool DeleteCopy(int copyId)
        {
            var copy = _copyRepository.GetById(copyId);
            if (copy == null)
                return false;

            if (!copy.isAvailable)
                return false;

            _copyRepository.Delete(copy);
            _copyRepository.SaveChanges();
            return true;
        }
    }
}
