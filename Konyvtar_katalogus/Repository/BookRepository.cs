using Konyvtar_katalogus.Data;
using Konyvtar_katalogus.Models;
using SQLitePCL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace Konyvtar_katalogus.Repository
{
    public  class BookRepository : IBookRepository, IDisposable
    {
        private LibraryDbContext context;

        public BookRepository(LibraryDbContext _context)
        {
            this.context = _context;

        }
        public IEnumerable<Models.Book> GetAllBooks()
        {
            return context.Books.ToList();
        }
        public Models.Book GetBookById(int id)
        {
            return context.Books.Find(id);
        }
        public void InsertBook(Models.Book book)
        {
            context.Books.Add(book);
        }
        public void DeleteBook(int id)
        {
            Models.Book book = context.Books.Find(id);
            if (book != null)
            {
                context.Books.Remove(book);
            }
        }
        public void UpdateBook(Models.Book book)
        {
            context.Books.Update(book);
        }
        public void Save()
        {
            context.SaveChanges();
        }

        private bool diposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!this.diposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.diposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public IEnumerable<Book> SearchAndOrderBooks(string? title, string? authir, string? isbn) {
            return context.Books.ToList(); }
        }


    }
}
