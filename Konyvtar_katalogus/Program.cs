using Konyvtar_katalogus.Data;
using Konyvtar_katalogus.Data.Repositories;
using Konyvtar_katalogus.Services;
using Konyvtar_katalogus.Presentation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

var serviceProvider = ConfigureServices();

using (var scope = serviceProvider.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<LibraryDbContext>();
    dbContext.Database.EnsureCreated();

    var menuManager = scope.ServiceProvider.GetRequiredService<MenuManager>();
    menuManager.Run();
}

static ServiceProvider ConfigureServices()
{
    var services = new ServiceCollection();

    services.AddDbContext<LibraryDbContext>(options =>
        options.UseSqlite("Data Source=library.db"));

    services.AddScoped<IBookRepository, BookRepository>();
    services.AddScoped<ICopyRepository, CopyRepository>();
    services.AddScoped<IReaderRepository, ReaderRepository>();
    services.AddScoped<ILoanRepository, LoanRepository>();

    services.AddScoped<IBookService, BookService>();
    services.AddScoped<ICopyService, CopyService>();
    services.AddScoped<IReaderService, ReaderService>();
    services.AddScoped<ILoanService, LoanService>();

    services.AddTransient<MenuManager>();

    return services.BuildServiceProvider();
}
