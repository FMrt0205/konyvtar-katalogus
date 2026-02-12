using Konyvtar_katalogus.Data;
using Microsoft.EntityFrameworkCore;

var options = new DbContextOptionsBuilder<LibraryDbContext>()
    .UseSqlite("Data Source=library.db")
    .Options;

using var db = new LibraryDbContext(options);

// adatbázis létrehozása, ha nem létezik
db.Database.EnsureCreated();

Console.WriteLine("Database ready.");
db.Database.EnsureCreated();