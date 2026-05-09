using System;
using System.Collections.Generic;
using System.Text;

namespace Konyvtar_katalogus.Repository
{
    public interface IFineRepository
    {
            /*Alapvető CRUD Műveletek*/
            IEnumerable<Models.Fine> GetFines();
            Models.Fine GetFineById(int id);
            void InsertFine(Models.Fine fine);
            void UpdateFine(Models.Fine fine);
            void DeleteFine(int id);


    }
}
