using Konyvtar_katalogus.Data.Repositories;
using Konyvtar_katalogus.Models;

namespace Konyvtar_katalogus.Services
{
    public class FineService : IFineService
    {
        private readonly IFineRepository _fineRepository;

        public FineService(IFineRepository fineRepository)
        {
            _fineRepository = fineRepository;
        }

        public IEnumerable<Fine> GetAllFines()
        {
            return _fineRepository.GetAll();
        }

        public IEnumerable<Fine> GetUnpaidFines()
        {
            return _fineRepository.GetUnpaid();
        }

        public bool PayFine(int fineId)
        {
            var fine = _fineRepository.GetById(fineId);
            if (fine == null || fine.isPaid)
                return false;

            fine.isPaid = true;
            fine.paidAt = DateTime.Now;
            _fineRepository.SaveChanges();
            return true;
        }
    }
}
