using Konyvtar_katalogus.Data;
using Konyvtar_katalogus.Models;
using Konyvtar_katalogus.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace Konyvtar_katalogus.Repository

{
    public class CopyRepository : ICopyRepository
    {
        private LibraryDbContext context;
        public CopyRepository(LibraryDbContext _context) {

            this.context = _context;  }

        public IEnumerable<Models.Copy> GetCopies()
        {
            return context.Copies.ToList();
        }
        public Models.Copy GetCopyById(int id)
        {
            return context.Copies.Find(id);
        }
        public void InsertCopy(Models.Copy copy)
        {
            context.Copies.Add(copy);
        }
        public void DeleteCopy (int id)
        {
            Models.Copy copy = context.Copies.Find(id);
            if (copy != null)
            {
                context.Copies.Remove(copy);
            }
        }
        public void UpdateCopy(Models.Copy copy)
        {
            context.Copies.Update(copy);
        }


    }

}
