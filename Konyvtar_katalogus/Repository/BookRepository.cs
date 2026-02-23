using Konyvtar_katalogus.Data;
using Konyvtar_katalogus.Models;
using SQLitePCL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using static Konyvtar_katalogus.Service.ScoreCalculated;
using Konyvtar_katalogus.Service;

namespace Konyvtar_katalogus.Repository
{
    public  class BookRepository : IBookRepository, IDisposable
    {
        private LibraryDbContext context;
        private ScoreCalculated score;

        public BookRepository(LibraryDbContext _context)
        {
            this.context = _context;

        }
        public IEnumerable<Models.Book> GetBooks()
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

        public List<Book> SearchAndOrderBooks(string? title, string? author, string? isbn) { 


            IQueryable<Book> query = context.Books
    .Where(b => (title == null || b.title.Contains(title))
             && (author == null || b.author.Contains(author))
             && (isbn == null || b.isbn.Contains(isbn)));

            var filteredBooks = context.Books
    .Where(b => (title == null || b.title.Contains(title))
             && (author == null || b.author.Contains(author))
             && (isbn == null || b.isbn.Contains(isbn)))
    .AsEnumerable() // most már LINQ to Objects
    .Select(b => new
    {
        Book = b,
        Score = score.scoreCalculateScore(b, title, author, isbn)
    })
    .OrderByDescending(x => x.Score)
    .Select(x => x.Book)
    .ToList();
            
        return filteredBooks;


        }
        }


    }

