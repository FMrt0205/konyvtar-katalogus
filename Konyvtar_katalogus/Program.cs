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
    EnsureLegacyDatabaseMigrationBaseline(dbContext);
    dbContext.Database.Migrate();

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
    services.AddScoped<IFineRepository, FineRepository>();
    services.AddScoped<INotificationQueueRepository, NotificationQueueRepository>();

    services.AddScoped<IBookService, BookService>();
    services.AddScoped<ICopyService, CopyService>();
    services.AddScoped<IReaderService, ReaderService>();
    services.AddScoped<ILoanService, LoanService>();
    services.AddScoped<IFineService, FineService>();
    services.AddScoped<IStatisticsService, StatisticsService>();

    services.AddTransient<MenuManager>();

    return services.BuildServiceProvider();
}

static void EnsureLegacyDatabaseMigrationBaseline(LibraryDbContext dbContext)
{
    var connection = dbContext.Database.GetDbConnection();
    if (connection.State != System.Data.ConnectionState.Open)
    {
        connection.Open();
    }

    try
    {
        var hasBooksTable = TableExists(connection, "Books");
        var hasHistoryTable = TableExists(connection, "__EFMigrationsHistory");

        if (!hasBooksTable || hasHistoryTable)
            return;

        dbContext.Database.ExecuteSqlRaw(@"
CREATE TABLE IF NOT EXISTS __EFMigrationsHistory (
    MigrationId TEXT NOT NULL CONSTRAINT PK___EFMigrationsHistory PRIMARY KEY,
    ProductVersion TEXT NOT NULL
);");

        dbContext.Database.ExecuteSqlRaw(@"
INSERT INTO __EFMigrationsHistory (MigrationId, ProductVersion)
SELECT '20260212094001_InitialCreate', '10.0.3'
WHERE NOT EXISTS (
    SELECT 1 FROM __EFMigrationsHistory WHERE MigrationId = '20260212094001_InitialCreate'
);");
    }
    finally
    {
        connection.Close();
    }
}

static bool TableExists(System.Data.Common.DbConnection connection, string tableName)
{
    using var command = connection.CreateCommand();
    command.CommandText = "SELECT COUNT(*) FROM sqlite_master WHERE type='table' AND name = $name";

    var parameter = command.CreateParameter();
    parameter.ParameterName = "$name";
    parameter.Value = tableName;
    command.Parameters.Add(parameter);

    var result = command.ExecuteScalar();
    return Convert.ToInt32(result) > 0;
}
