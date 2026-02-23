using Konyvtar_katalogus.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Konyvtar_katalogus.Repository
{
    public interface IBookRepository :IDisposable
    {
    /*Alapvető CRUD Műveletek*/
        IEnumerable<Book> GetBooks();
        void InsertBook(Book book);
        void UpdateBook(Book book);
        void DeleteBook(int bookid);
        void Save();


        /*IEnumerable<> egy olyan interfész ami biztosítsa hogy a bejárható objektumok listáján végig megy*/
       /* IEnumerable<Book> SearchBooks(string? title, string? author, string? isbn);*/
        /*típus? => nem kötelező értéket megadni.*/
       /* IEnumerable<Book> OrderBooks(List<Book> books, string? title, string? author, string? isbn);*/
        
        IEnumerable<Book> SearchAndOrderBooks(
    string? title,
    string? author,
    string? isbn,
    string? orderBy
);





    }
}
