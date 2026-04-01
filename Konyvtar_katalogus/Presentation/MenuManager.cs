using Konyvtar_katalogus.Services;

namespace Konyvtar_katalogus.Presentation
{
    public class MenuManager
    {
        private readonly IBookService _bookService;
        private readonly ICopyService _copyService;
        private readonly IReaderService _readerService;
        private readonly ILoanService _loanService;
        private readonly IFineService _fineService;
        private readonly IStatisticsService _statisticsService;

        public MenuManager(
            IBookService bookService,
            ICopyService copyService,
            IReaderService readerService,
            ILoanService loanService,
            IFineService fineService,
            IStatisticsService statisticsService)
        {
            _bookService = bookService;
            _copyService = copyService;
            _readerService = readerService;
            _loanService = loanService;
            _fineService = fineService;
            _statisticsService = statisticsService;
        }

        public void Run()
        {
            Console.WriteLine("=== Könyvtár Katalógus Rendszer ===\n");

            bool running = true;
            while (running)
            {
                Console.WriteLine("\n--- FŐMENÜ ---");
                Console.WriteLine("1. Könyvek kezelése");
                Console.WriteLine("2. Példányok kezelése");
                Console.WriteLine("3. Olvasók kezelése");
                Console.WriteLine("4. Kölcsönzések kezelése");
                Console.WriteLine("5. Késedelmi díjak");
                Console.WriteLine("0. Kilépés");
                Console.Write("\nVálassz: ");

                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        ManageBooks();
                        break;
                    case "2":
                        ManageCopies();
                        break;
                    case "3":
                        ManageReaders();
                        break;
                    case "4":
                        ManageLoans();
                        break;
                    case "5":
                        ManageFines();
                        break;
                    case "0":
                        running = false;
                        Console.WriteLine("Viszlát!");
                        break;
                    default:
                        Console.WriteLine("Érvénytelen választás!");
                        break;
                }
            }
        }

        private void ManageBooks()
        {
            bool back = false;
            while (!back)
            {
                Console.WriteLine("\n--- KÖNYVEK KEZELÉSE ---");
                Console.WriteLine("1. Új könyv hozzáadása");
                Console.WriteLine("2. Könyvek listázása");
                Console.WriteLine("3. Könyv törlése");
                Console.WriteLine("4. Könyv keresése");
                Console.WriteLine("5. Tömeges import fájlból (TPL)");
                Console.WriteLine("6. Statisztikák generálása háttérben (TPL)");
                Console.WriteLine("0. Vissza");
                Console.Write("\nVálassz: ");

                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        AddBook();
                        break;
                    case "2":
                        ListBooks();
                        break;
                    case "3":
                        DeleteBook();
                        break;
                    case "4":
                        SearchBooks();
                        break;
                    case "5":
                        ImportBooks();
                        break;
                    case "6":
                        ShowStatistics();
                        break;
                    case "0":
                        back = true;
                        break;
                    default:
                        Console.WriteLine("Érvénytelen választás!");
                        break;
                }
            }
        }

        private void AddBook()
        {
            Console.WriteLine("\n--- ÚJ KÖNYV HOZZÁADÁSA ---");

            Console.Write("Cím: ");
            var title = Console.ReadLine();

            Console.Write("Szerző: ");
            var author = Console.ReadLine();

            Console.Write("ISBN (13 karakter): ");
            var isbn = Console.ReadLine();

            if (_bookService.AddBook(title, author, isbn))
            {
                Console.WriteLine("Könyv sikeresen hozzáadva!");
            }
            else
            {
                Console.WriteLine("Hiba történt! Ellenőrizd a mezőket (ISBN 13 karakter legyen).");
            }
        }

        private void ListBooks()
        {
            Console.WriteLine("\n--- KÖNYVEK LISTÁJA ---");

            var books = _bookService.GetAllBooks();

            if (!books.Any())
            {
                Console.WriteLine("Nincsenek könyvek az adatbázisban.");
                return;
            }

            foreach (var book in books)
            {
                var copyCount = book.Copies?.Count ?? 0;
                Console.WriteLine($"[{book.bookid}] {book.title} - {book.author} (ISBN: {book.isbn}) | Példányok: {copyCount}");
            }
        }

        private void DeleteBook()
        {
            Console.WriteLine("\n--- KÖNYV TÖRLÉSE ---");
            ListBooks();

            Console.Write("\nTörlendő könyv ID-ja: ");
            if (!int.TryParse(Console.ReadLine(), out int bookId))
            {
                Console.WriteLine("Érvénytelen ID!");
                return;
            }

            if (_bookService.DeleteBook(bookId))
            {
                Console.WriteLine("Könyv törölve!");
            }
            else
            {
                Console.WriteLine("Hiba! A könyvnek vannak példányai vagy nem található.");
            }
        }

        private void SearchBooks()
        {
            Console.WriteLine("\n--- KÖNYV KERESÉSE ---");
            Console.Write("Keresési kifejezés (cím/szerző/ISBN): ");
            var searchTerm = Console.ReadLine();

            Console.Write("Rendezés (1 = relevancia, 2 = találatszám): ");
            var sortChoice = Console.ReadLine();
            var sortByMatchCount = sortChoice == "2";

            var results = _bookService.SearchBooks(searchTerm, sortByMatchCount);

            if (!results.Any())
            {
                Console.WriteLine($"Nincs találat a(z) '{searchTerm}' keresésre.");
                return;
            }

            Console.WriteLine($"\n{results.Count()} találat:");
            foreach (var book in results)
            {
                var copyCount = book.Copies?.Count ?? 0;
                var availableCount = book.Copies?.Count(c => c.isAvailable) ?? 0;
                Console.WriteLine($"[{book.bookid}] {book.title} - {book.author} (ISBN: {book.isbn}) | Példányok: {copyCount} (Elérhető: {availableCount})");
            }
        }

        private void ManageCopies()
        {
            bool back = false;
            while (!back)
            {
                Console.WriteLine("\n--- PÉLDÁNYOK KEZELÉSE ---");
                Console.WriteLine("1. Új példány hozzáadása");
                Console.WriteLine("2. Példányok listázása");
                Console.WriteLine("3. Példány törlése");
                Console.WriteLine("0. Vissza");
                Console.Write("\nVálassz: ");

                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        AddCopy();
                        break;
                    case "2":
                        ListCopies();
                        break;
                    case "3":
                        DeleteCopy();
                        break;
                    case "0":
                        back = true;
                        break;
                    default:
                        Console.WriteLine("Érvénytelen választás!");
                        break;
                }
            }
        }

        private void AddCopy()
        {
            Console.WriteLine("\n--- ÚJ PÉLDÁNY HOZZÁADÁSA ---");
            ListBooks();

            Console.Write("\nKönyv ID-ja: ");
            if (!int.TryParse(Console.ReadLine(), out int bookId))
            {
                Console.WriteLine("Érvénytelen ID!");
                return;
            }

            Console.Write("Leltári szám (pl. 12-34): ");
            var inventoryNumber = Console.ReadLine();

            if (_copyService.AddCopy(bookId, inventoryNumber))
            {
                Console.WriteLine("Példány sikeresen hozzáadva!");
            }
            else
            {
                Console.WriteLine("Hiba történt! A könyv nem található vagy érvénytelen leltári szám.");
            }
        }

        private void ListCopies()
        {
            Console.WriteLine("\n--- PÉLDÁNYOK LISTÁJA ---");

            var copies = _copyService.GetAllCopies();

            if (!copies.Any())
            {
                Console.WriteLine("Nincsenek példányok az adatbázisban.");
                return;
            }

            foreach (var copy in copies)
            {
                var status = copy.isAvailable ? "Elérhető" : "Kölcsönözve";
                Console.WriteLine($"[{copy.copyid}] {copy.Book.title} | Leltári szám: {copy.InventoryNumber} | {status}");
            }
        }

        private void DeleteCopy()
        {
            Console.WriteLine("\n--- PÉLDÁNY TÖRLÉSE ---");
            ListCopies();

            Console.Write("\nTörlendő példány ID-ja: ");
            if (!int.TryParse(Console.ReadLine(), out int copyId))
            {
                Console.WriteLine("Érvénytelen ID!");
                return;
            }

            if (_copyService.DeleteCopy(copyId))
            {
                Console.WriteLine("Példány törölve!");
            }
            else
            {
                Console.WriteLine("Hiba! A példány kölcsönözve van vagy nem található.");
            }
        }

        private void ManageReaders()
        {
            bool back = false;
            while (!back)
            {
                Console.WriteLine("\n--- OLVASÓK KEZELÉSE ---");
                Console.WriteLine("1. Új olvasó hozzáadása");
                Console.WriteLine("2. Olvasók listázása");
                Console.WriteLine("3. Olvasó törlése");
                Console.WriteLine("0. Vissza");
                Console.Write("\nVálassz: ");

                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        AddReader();
                        break;
                    case "2":
                        ListReaders();
                        break;
                    case "3":
                        DeleteReader();
                        break;
                    case "0":
                        back = true;
                        break;
                    default:
                        Console.WriteLine("Érvénytelen választás!");
                        break;
                }
            }
        }

        private void AddReader()
        {
            Console.WriteLine("\n--- ÚJ OLVASÓ HOZZÁADÁSA ---");

            Console.Write("Név: ");
            var name = Console.ReadLine();

            Console.Write("Email (opcionális): ");
            var email = Console.ReadLine();

            if (_readerService.AddReader(name, email))
            {
                Console.WriteLine("Olvasó sikeresen hozzáadva!");
            }
            else
            {
                Console.WriteLine("Hiba! A név megadása kötelező.");
            }
        }

        private void ListReaders()
        {
            Console.WriteLine("\n--- OLVASÓK LISTÁJA ---");

            var readers = _readerService.GetAllReaders();

            if (!readers.Any())
            {
                Console.WriteLine("Nincsenek olvasók az adatbázisban.");
                return;
            }

            foreach (var reader in readers)
            {
                var loanCount = reader.Loans?.Count(l => l.returnDate == DateTime.MinValue) ?? 0;
                var email = string.IsNullOrWhiteSpace(reader.email) ? "-" : reader.email;
                Console.WriteLine($"[{reader.readerid}] {reader.name} | Email: {email} | Aktív kölcsönzések: {loanCount}");
            }
        }

        private void DeleteReader()
        {
            Console.WriteLine("\n--- OLVASÓ TÖRLÉSE ---");
            ListReaders();

            Console.Write("\nTörlendő olvasó ID-ja: ");
            if (!int.TryParse(Console.ReadLine(), out int readerId))
            {
                Console.WriteLine("Érvénytelen ID!");
                return;
            }

            if (_readerService.DeleteReader(readerId))
            {
                Console.WriteLine("Olvasó törölve!");
            }
            else
            {
                Console.WriteLine("Hiba! Az olvasónak aktív kölcsönzései vannak vagy nem található.");
            }
        }

        private void ManageLoans()
        {
            bool back = false;
            while (!back)
            {
                Console.WriteLine("\n--- KÖLCSÖNZÉSEK KEZELÉSE ---");
                Console.WriteLine("1. Új kölcsönzés");
                Console.WriteLine("2. Visszahozatal");
                Console.WriteLine("3. Aktív kölcsönzések listája");
                Console.WriteLine("4. Összes kölcsönzés listája");
                Console.WriteLine("5. Késedelmes értesítések sorba állítása");
                Console.WriteLine("6. Értesítési sor kiküldése (log)");
                Console.WriteLine("0. Vissza");
                Console.Write("\nVálassz: ");

                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        CreateLoan();
                        break;
                    case "2":
                        ReturnLoan();
                        break;
                    case "3":
                        ListActiveLoans();
                        break;
                    case "4":
                        ListAllLoans();
                        break;
                    case "5":
                        QueueOverdueNotifications();
                        break;
                    case "6":
                        SendQueuedNotifications();
                        break;
                    case "0":
                        back = true;
                        break;
                    default:
                        Console.WriteLine("Érvénytelen választás!");
                        break;
                }
            }
        }

        private void CreateLoan()
        {
            Console.WriteLine("\n--- ÚJ KÖLCSÖNZÉS ---");

            ListReaders();
            Console.Write("\nOlvasó ID-ja: ");
            if (!int.TryParse(Console.ReadLine(), out int readerId))
            {
                Console.WriteLine("Érvénytelen ID!");
                return;
            }

            Console.WriteLine("\nElérhető példányok:");
            var availableCopies = _copyService.GetAvailableCopies();

            if (!availableCopies.Any())
            {
                Console.WriteLine("Nincsenek elérhető példányok!");
                return;
            }

            foreach (var copy in availableCopies)
            {
                Console.WriteLine($"[{copy.copyid}] {copy.Book.title} - {copy.InventoryNumber}");
            }

            Console.Write("\nPéldány ID-ja: ");
            if (!int.TryParse(Console.ReadLine(), out int copyId))
            {
                Console.WriteLine("Érvénytelen ID!");
                return;
            }

            if (_loanService.CreateLoan(readerId, copyId))
            {
                Console.WriteLine("Kölcsönzés sikeresen rögzítve!");
            }
            else
            {
                Console.WriteLine("Hiba! Ellenőrizd az olvasó és példány ID-kat.");
            }
        }

        private void ReturnLoan()
        {
            Console.WriteLine("\n--- VISSZAHOZATAL ---");

            ListActiveLoans();

            Console.Write("\nKölcsönzés ID-ja: ");
            if (!int.TryParse(Console.ReadLine(), out int loanId))
            {
                Console.WriteLine("Érvénytelen ID!");
                return;
            }

            if (_loanService.ReturnLoan(loanId))
            {
                Console.WriteLine("Visszahozatal rögzítve!");
            }
            else
            {
                Console.WriteLine("Hiba! A kölcsönzés nem található vagy már visszahozva.");
            }
        }

        private void ListActiveLoans()
        {
            Console.WriteLine("\n--- AKTÍV KÖLCSÖNZÉSEK ---");

            var activeLoans = _loanService.GetActiveLoans();

            if (!activeLoans.Any())
            {
                Console.WriteLine("Nincsenek aktív kölcsönzések.");
                return;
            }

            foreach (var loan in activeLoans)
            {
                var days = (DateTime.Now - loan.loanDate).Days;
                var overdue = DateTime.Now > loan.dueDate ? " | KÉSEDELMES" : string.Empty;
                Console.WriteLine($"[{loan.loanid}] {loan.Reader.name} - {loan.Copy.Book.title} ({loan.Copy.InventoryNumber}) | {loan.loanDate:yyyy-MM-dd} ({days} napja) | Határidő: {loan.dueDate:yyyy-MM-dd}{overdue}");
            }
        }

        private void ListAllLoans()
        {
            Console.WriteLine("\n--- ÖSSZES KÖLCSÖNZÉS ---");

            var loans = _loanService.GetAllLoans();

            if (!loans.Any())
            {
                Console.WriteLine("Nincsenek kölcsönzések.");
                return;
            }

            foreach (var loan in loans)
            {
                var status = loan.returnDate == DateTime.MinValue ? "Aktív" : $"Visszahozva: {loan.returnDate:yyyy-MM-dd}";
                Console.WriteLine($"[{loan.loanid}] {loan.Reader.name} - {loan.Copy.Book.title} | Kikölcsönözve: {loan.loanDate:yyyy-MM-dd} | {status}");
            }
        }

        private void ImportBooks()
        {
            Console.WriteLine("\n--- TÖMEGES IMPORT ---");
            Console.Write("Fájl útvonal (formátum: Cím;Szerző;ISBN soronként): ");
            var path = Console.ReadLine();

            var result = _bookService.ImportBooksFromFileAsync(path ?? string.Empty).GetAwaiter().GetResult();
            Console.WriteLine($"Import kész. Sikeres: {result.importedCount}, kihagyott: {result.skippedCount}");
        }

        private void ShowStatistics()
        {
            Console.WriteLine("\n--- STATISZTIKÁK (HÁTTÉRBEN) ---");
            var stats = _statisticsService.GenerateStatisticsAsync().GetAwaiter().GetResult();

            Console.WriteLine($"Könyvek száma: {stats.TotalBooks}");
            Console.WriteLine($"Példányok száma: {stats.TotalCopies}");
            Console.WriteLine($"Elérhető példányok: {stats.AvailableCopies}");
            Console.WriteLine($"Olvasók száma: {stats.TotalReaders}");
            Console.WriteLine($"Aktív kölcsönzések: {stats.ActiveLoans}");
            Console.WriteLine($"Késedelmes kölcsönzések: {stats.OverdueLoans}");
            Console.WriteLine($"Fizetetlen díjak: {stats.UnpaidFines}");
        }

        private void QueueOverdueNotifications()
        {
            var count = _loanService.QueueOverdueNotifications();
            Console.WriteLine($"Sorba állított értesítések: {count}");
        }

        private void SendQueuedNotifications()
        {
            var sent = _loanService.SendQueuedNotifications();
            Console.WriteLine($"Kiküldött értesítések: {sent}");
        }

        private void ManageFines()
        {
            bool back = false;
            while (!back)
            {
                Console.WriteLine("\n--- KÉSEDELMI DÍJAK ---");
                Console.WriteLine("1. Fizetetlen díjak listája");
                Console.WriteLine("2. Összes díj listája");
                Console.WriteLine("3. Díj befizetése");
                Console.WriteLine("0. Vissza");
                Console.Write("\nVálassz: ");

                var choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        ListFines(unpaidOnly: true);
                        break;
                    case "2":
                        ListFines(unpaidOnly: false);
                        break;
                    case "3":
                        PayFine();
                        break;
                    case "0":
                        back = true;
                        break;
                    default:
                        Console.WriteLine("Érvénytelen választás!");
                        break;
                }
            }
        }

        private void ListFines(bool unpaidOnly)
        {
            var fines = unpaidOnly ? _fineService.GetUnpaidFines() : _fineService.GetAllFines();
            if (!fines.Any())
            {
                Console.WriteLine("Nincsenek megjeleníthető díjak.");
                return;
            }

            foreach (var fine in fines)
            {
                var status = fine.isPaid ? $"Fizetve: {fine.paidAt:yyyy-MM-dd HH:mm}" : "Fizetetlen";
                Console.WriteLine($"[{fine.fineid}] {fine.Loan.Reader.name} - {fine.Loan.Copy.Book.title} | Összeg: {fine.amount} Ft | {status}");
            }
        }

        private void PayFine()
        {
            ListFines(unpaidOnly: true);

            Console.Write("\nBefizetendő díj ID-ja: ");
            if (!int.TryParse(Console.ReadLine(), out int fineId))
            {
                Console.WriteLine("Érvénytelen ID!");
                return;
            }

            Console.WriteLine(_fineService.PayFine(fineId)
                ? "Díj sikeresen befizetve."
                : "A díj nem található vagy már rendezett.");
        }
    }
}
