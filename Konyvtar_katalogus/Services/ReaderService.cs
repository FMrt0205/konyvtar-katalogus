using Konyvtar_katalogus.Data.Repositories;
using Konyvtar_katalogus.Models;

namespace Konyvtar_katalogus.Services
{
    public class ReaderService : IReaderService
    {
        private readonly IReaderRepository _readerRepository;

        public ReaderService(IReaderRepository readerRepository)
        {
            _readerRepository = readerRepository;
        }

        public bool AddReader(string name, string? email)
        {
            if (string.IsNullOrWhiteSpace(name))
                return false;

            var reader = new Reader
            {
                name = name,
                email = string.IsNullOrWhiteSpace(email) ? null : email.Trim()
            };
            _readerRepository.Add(reader);
            _readerRepository.SaveChanges();
            return true;
        }

        public IEnumerable<Reader> GetAllReaders()
        {
            return _readerRepository.GetAll();
        }

        public bool DeleteReader(int readerId)
        {
            var reader = _readerRepository.GetById(readerId);
            if (reader == null)
                return false;

            if (reader.Loans?.Any(l => l.returnDate == DateTime.MinValue) == true)
                return false;

            _readerRepository.Delete(reader);
            _readerRepository.SaveChanges();
            return true;
        }
    }
}
