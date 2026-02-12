using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
namespace Konyvtar_katalogus.Data
{
    public class LibraryDbContext : DbContext
    {
        public DbSet<Models.Book> Books { get; set; }
        public DbSet<Models.Copy> Copies { get; set; }
        public DbSet<Models.Reader> Readers { get; set; }
        public DbSet<Models.Loan> Loans { get; set; }
        public DbSet<Models.Fine> Fines { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        { if (!optionsBuilder.IsConfigured) 
            { optionsBuilder.UseSqlite("Data Source=library.db"); } }

        public LibraryDbContext(DbContextOptions<LibraryDbContext> options)
            : base(options)
        { }
        public LibraryDbContext() { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
