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
        public DbSet<Models.NotificationQueueItem> NotificationQueueItems { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        { if (!optionsBuilder.IsConfigured) 
            { optionsBuilder.UseSqlite("Data Source=library.db"); } }

        public LibraryDbContext(DbContextOptions<LibraryDbContext> options)
            : base(options)
        { }
        public LibraryDbContext() { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Models.Book>()
                .HasIndex(b => b.isbn)
                .IsUnique();

            modelBuilder.Entity<Models.Copy>()
                .HasIndex(c => c.InventoryNumber)
                .IsUnique();

            modelBuilder.Entity<Models.Fine>()
                .HasIndex(f => f.LoanId)
                .IsUnique();

            modelBuilder.Entity<Models.NotificationQueueItem>()
                .HasOne(n => n.Loan)
                .WithMany()
                .HasForeignKey(n => n.LoanId)
                .OnDelete(DeleteBehavior.Cascade);

            base.OnModelCreating(modelBuilder);
        }
    }
}
