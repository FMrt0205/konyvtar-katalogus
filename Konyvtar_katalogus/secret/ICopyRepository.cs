using System;
using System.Collections.Generic;
using System.Text;
using Konyvtar_katalogus.Models;

namespace Konyvtar_katalogus.Repository
{
    public interface ICopyRepository
    {
        /*Alapvető CRUD Műveletek*/
        IEnumerable<Models.Copy> GetCopies();
        Models.Copy GetCopyById(int id);
        void InsertCopy(Models.Copy copy);
        void UpdateCopy(Models.Copy copy);
        void DeleteCopy(int id);
        
        /*IEnumerable<> egy olyan interfész ami biztosítsa hogy a bejárható objektumok listáján végig megy*/
        /* IEnumerable<Author> SearchAuthors(string? name); */
        /*típus? => nem kötelező értéket megadni.*/
        /* IEnumerable<Author> OrderAuthors(List<Author> authors, string? name);*/
        
    }
}
