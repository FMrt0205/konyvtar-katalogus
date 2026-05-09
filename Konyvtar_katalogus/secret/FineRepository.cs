using System;
using System.Collections.Generic;
using System.Text;
using Konyvtar_katalogus.Data;

namespace Konyvtar_katalogus.Repository
{
    public class FineRepository : IFineRepository
    {
        private LibraryDbContext context;
        public FineRepository(LibraryDbContext _context)
        {
            this.context = _context;
        }
        
        public IEnumerable<Models.Fine> GetFines()
        {
            return context.Fines.ToList();
        }
        public Models.Fine GetFineById(int id)
        {
            return context.Fines.Find(id);
        }
        public void InsertFine(Models.Fine fine)
        {
            context.Fines.Add(fine);
        }
        public void DeleteFine(int id)
        {
            Models.Fine fine = context.Fines.Find(id);
            if (fine != null)
            {
                context.Fines.Remove(fine);
            }
        }
        public void UpdateFine(Models.Fine fine)
        {
            context.Fines.Update(fine);
        }





    }
}
