using Konyvtar_katalogus.Services;

namespace Konyvtar_katalogus.Presentation
{
    // Ez az osztály kezeli az összes felhasználói menüt és interakciót
    public class MenuManager
    {
        // Privát mezők – ezek tartják a service-eket (üzleti logika réteg)
        // A "_" előtag jelöli, hogy osztályszintű privát mező
        private readonly IBookService _bookService;           // könyvek kezelése
        private readonly ICopyService _copyService;           // példányok kezelése
        private readonly IReaderService _readerService;       // olvasók kezelése
        private readonly ILoanService _loanService;           // kölcsönzések kezelése
        private readonly IFineService _fineService;           // késedelmi díjak kezelése
        private readonly IStatisticsService _statisticsService; // statisztikák generálása

        // Konstruktor – amikor létrejön a MenuManager, megkapja a szükséges service-eket
        // Ez az ún. Dependency Injection (DI): kívülről adjuk be a függőségeket
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

        // A program belépési pontja – ez indítja el a főmenüt
        public void Run()
        {
            Console.WriteLine("=== Könyvtár Katalógus Rendszer ===\n");

            bool running = true;
            // Addig fut a ciklus, amíg a felhasználó ki nem lép (0-t nem nyom)
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

                var choice = Console.ReadLine(); // beolvassa a felhasználó választását

                // A választás alapján meghívja a megfelelő almenüt
                switch (choice)
                {
                    case "1": ManageBooks(); break;
                    case "2": ManageCopies(); break;
                    case "3": ManageReaders(); break;
                    case "4": ManageLoans(); break;
                    case "5": ManageFines(); break;
                    case "0":
                        running = false; // kilép a while ciklusból
                        Console.WriteLine("Viszlát!");
                        break;
                    default:
                        Console.WriteLine("Érvénytelen választás!");
                        break;
                }
            }
        }

        // ───────────────────────────────────────────────
        // KÖNYVEK ALMENÜ
        // ───────────────────────────────────────────────

        private void ManageBooks()
        {
            bool back = false;
            while (!back) // addig fut, amíg a felhasználó vissza nem lép
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
                    case "1": AddBook(); break;
                    case "2": ListBooks(); break;
                    case "3": DeleteBook(); break;
                    case "4": SearchBooks(); break;
                    case "5": ImportBooks(); break;
                    case "6": ShowStatistics(); break;
                    case "0": back = true; break; // kilép a while ciklusból → visszatér a főmenübe
                    default: Console.WriteLine("Érvénytelen választás!"); break;
                }
            }
        }

        // Új könyv felvitele: bekéri az adatokat, majd átadja a service-nek
        private void AddBook()
        {
            Console.WriteLine("\n--- ÚJ KÖNYV HOZZÁADÁSA ---");

            Console.Write("Cím: ");
            var title = Console.ReadLine();

            Console.Write("Szerző: ");
            var author = Console.ReadLine();

            Console.Write("ISBN (13 karakter): ");
            var isbn = Console.ReadLine();

            // A service végzi az érvényesítést és mentést – visszaad true/false-t
            if (_bookService.AddBook(title, author, isbn))
                Console.WriteLine("Könyv sikeresen hozzáadva!");
            else
                Console.WriteLine("Hiba történt! Ellenőrizd a mezőket (ISBN 13 karakter legyen).");
        }

        // Az összes könyv kilistázása az adatbázisból
        private void ListBooks()
        {
            Console.WriteLine("\n--- KÖNYVEK LISTÁJA ---");

            var books = _bookService.GetAllBooks();

            if (!books.Any()) // ha üres a lista
            {
                Console.WriteLine("Nincsenek könyvek az adatbázisban.");
                return;
            }

            foreach (var book in books)
            {
                // ?. = null-safe hozzáférés: ha Copies null, nem dob hibát
                // ?? 0 = ha null, akkor 0-t használ
                var copyCount = book.Copies?.Count ?? 0;
                Console.WriteLine($"[{book.bookid}] {book.title} - {book.author} (ISBN: {book.isbn}) | Példányok: {copyCount}");
            }
        }

        // Könyv törlése ID alapján
        private void DeleteBook()
        {
            Console.WriteLine("\n--- KÖNYV TÖRLÉSE ---");
            ListBooks(); // először kilistázza, hogy látszódjanak az ID-k

            Console.Write("\nTörlendő könyv ID-ja: ");
            // int.TryParse: biztonságos szám-parse, ha nem szám, false-t ad vissza
            if (!int.TryParse(Console.ReadLine(), out int bookId))
            {
                Console.WriteLine("Érvénytelen ID!");
                return; // kilép a metódusból
            }

            if (_bookService.DeleteBook(bookId))
                Console.WriteLine("Könyv törölve!");
            else
                Console.WriteLine("Hiba! A könyvnek vannak példányai vagy nem található.");
        }

        // Könyv keresése cím/szerző/ISBN alapján, kétféle rendezéssel
        private void SearchBooks()
        {
            Console.WriteLine("\n--- KÖNYV KERESÉSE ---");
            Console.Write("Keresési kifejezés (cím/szerző/ISBN): ");
            var searchTerm = Console.ReadLine();

            Console.Write("Rendezés (1 = relevancia, 2 = találatszám): ");
            var sortChoice = Console.ReadLine();
            var sortByMatchCount = sortChoice == "2"; // bool: true ha "2"-t írt be

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
                // Lambda szűrés: csak az elérhető (isAvailable == true) példányokat számolja
                var availableCount = book.Copies?.Count(c => c.isAvailable) ?? 0;
                Console.WriteLine($"[{book.bookid}] {book.title} - {book.author} (ISBN: {book.isbn}) | Példányok: {copyCount} (Elérhető: {availableCount})");
            }
        }

        // ───────────────────────────────────────────────
        // PÉLDÁNYOK ALMENÜ
        // (Egy könyvből több fizikai példány is lehet)
        // ───────────────────────────────────────────────

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
                    case "1": AddCopy(); break;
                    case "2": ListCopies(); break;
                    case "3": DeleteCopy(); break;
                    case "0": back = true; break;
                    default: Console.WriteLine("Érvénytelen választás!"); break;
                }
            }
        }

        // Új fizikai példány hozzáadása egy meglévő könyvhöz
        private void AddCopy()
        {
            Console.WriteLine("\n--- ÚJ PÉLDÁNY HOZZÁADÁSA ---");
            ListBooks(); // megmutatja a könyveket, hogy tudja melyik ID-t adja meg

            Console.Write("\nKönyv ID-ja: ");
            if (!int.TryParse(Console.ReadLine(), out int bookId))
            {
                Console.WriteLine("Érvénytelen ID!");
                return;
            }

            Console.Write("Leltári szám (pl. 12-34): ");
            var inventoryNumber = Console.ReadLine();

            if (_copyService.AddCopy(bookId, inventoryNumber))
                Console.WriteLine("Példány sikeresen hozzáadva!");
            else
                Console.WriteLine("Hiba történt! A könyv nem található vagy érvénytelen leltári szám.");
        }

        // Összes példány listázása, státusszal (elérhető / kölcsönözve)
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
                // Ternáris operátor: ha igaz → "Elérhető", ha hamis → "Kölcsönözve"
                var status = copy.isAvailable ? "Elérhető" : "Kölcsönözve";
                Console.WriteLine($"[{copy.copyid}] {copy.Book.title} | Leltári szám: {copy.InventoryNumber} | {status}");
            }
        }

        // Példány törlése ID alapján (csak ha nincs aktív kölcsönzés rajta)
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
                Console.WriteLine("Példány törölve!");
            else
                Console.WriteLine("Hiba! A példány kölcsönözve van vagy nem található.");
        }

        // ───────────────────────────────────────────────
        // OLVASÓK ALMENÜ
        // ───────────────────────────────────────────────

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
                    case "1": AddReader(); break;
                    case "2": ListReaders(); break;
                    case "3": DeleteReader(); break;
                    case "0": back = true; break;
                    default: Console.WriteLine("Érvénytelen választás!"); break;
                }
            }
        }

        // Új könyvtári olvasó regisztrálása (email opcionális)
        private void AddReader()
        {
            Console.WriteLine("\n--- ÚJ OLVASÓ HOZZÁADÁSA ---");

            Console.Write("Név: ");
            var name = Console.ReadLine();

            Console.Write("Email (opcionális): ");
            var email = Console.ReadLine();

            if (_readerService.AddReader(name, email))
                Console.WriteLine("Olvasó sikeresen hozzáadva!");
            else
                Console.WriteLine("Hiba! A név megadása kötelező.");
        }

        // Olvasók listázása az aktív kölcsönzéseik számával együtt
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
                // Csak az aktív (még vissza nem hozott) kölcsönzéseket számolja
                // DateTime.MinValue = "nincs visszahozatal dátuma" = aktív kölcsönzés
                var loanCount = reader.Loans?.Count(l => l.returnDate == DateTime.MinValue) ?? 0;
                // Ha üres az email mező, "-" jelenik meg helyette
                var email = string.IsNullOrWhiteSpace(reader.email) ? "-" : reader.email;
                Console.WriteLine($"[{reader.readerid}] {reader.name} | Email: {email} | Aktív kölcsönzések: {loanCount}");
            }
        }

        // Olvasó törlése (csak ha nincs aktív kölcsönzése)
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
                Console.WriteLine("Olvasó törölve!");
            else
                Console.WriteLine("Hiba! Az olvasónak aktív kölcsönzései vannak vagy nem található.");
        }

        // ───────────────────────────────────────────────
        // KÖLCSÖNZÉSEK ALMENÜ
        // ───────────────────────────────────────────────

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
                Console.WriteLine("7. Kölcsönzés késedelmessé tétele (teszt)");
                Console.WriteLine("0. Vissza");
                Console.Write("\nVálassz: ");

                var choice = Console.ReadLine();
                switch (choice)
                {
                    case "1": CreateLoan(); break;
                    case "2": ReturnLoan(); break;
                    case "3": ListActiveLoans(); break;
                    case "4": ListAllLoans(); break;
                    case "5": QueueOverdueNotifications(); break;
                    case "6": SendQueuedNotifications(); break;
                    case "7": MarkLoanOverdue(); break;
                    case "0": back = true; break;
                    default: Console.WriteLine("Érvénytelen választás!"); break;
                }
            }
        }

        // Új kölcsönzés rögzítése: olvasóhoz rendelünk egy szabad példányt
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
            // Csak az isAvailable == true példányokat kéri le
            var availableCopies = _copyService.GetAvailableCopies();

            if (!availableCopies.Any())
            {
                Console.WriteLine("Nincsenek elérhető példányok!");
                return;
            }

            foreach (var copy in availableCopies)
                Console.WriteLine($"[{copy.copyid}] {copy.Book.title} - {copy.InventoryNumber}");

            Console.Write("\nPéldány ID-ja: ");
            if (!int.TryParse(Console.ReadLine(), out int copyId))
            {
                Console.WriteLine("Érvénytelen ID!");
                return;
            }

            if (_loanService.CreateLoan(readerId, copyId))
                Console.WriteLine("Kölcsönzés sikeresen rögzítve!");
            else
                Console.WriteLine("Hiba! Ellenőrizd az olvasó és példány ID-kat.");
        }

        // Visszahozatal rögzítése: megadja a kölcsönzés ID-ját
        private void ReturnLoan()
        {
            Console.WriteLine("\n--- VISSZAHOZATAL ---");
            ListActiveLoans(); // segít megtalálni a helyes ID-t

            Console.Write("\nKölcsönzés ID-ja: ");
            if (!int.TryParse(Console.ReadLine(), out int loanId))
            {
                Console.WriteLine("Érvénytelen ID!");
                return;
            }

            if (_loanService.ReturnLoan(loanId))
                Console.WriteLine("Visszahozatal rögzítve!");
            else
                Console.WriteLine("Hiba! A kölcsönzés nem található vagy már visszahozva.");
        }

        // Csak a jelenleg aktív (vissza nem hozott) kölcsönzések megjelenítése
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
                // Kiszámolja hány napja van kint a könyv
                var days = (DateTime.Now - loan.loanDate).Days;
                // Ha lejárt a határidő, kiírja a figyelmeztetést, különben üres string
                var overdue = DateTime.Now > loan.dueDate ? " | KÉSEDELMES" : string.Empty;
                Console.WriteLine($"[{loan.loanid}] {loan.Reader.name} - {loan.Copy.Book.title} ({loan.Copy.InventoryNumber}) | {loan.loanDate:yyyy-MM-dd} ({days} napja) | Határidő: {loan.dueDate:yyyy-MM-dd}{overdue}");
            }
        }

        // Az összes kölcsönzés listázása (aktív + lezárt egyaránt)
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
                // Ha returnDate == DateTime.MinValue → még nem hozták vissza → "Aktív"
                var status = loan.returnDate == DateTime.MinValue
                    ? "Aktív"
                    : $"Visszahozva: {loan.returnDate:yyyy-MM-dd}";
                Console.WriteLine($"[{loan.loanid}] {loan.Reader.name} - {loan.Copy.Book.title} | Kikölcsönözve: {loan.loanDate:yyyy-MM-dd} | {status}");
            }
        }

        // Fájlból olvas be könyveket soronként (Cím;Szerző;ISBN formátum)
        // .GetAwaiter().GetResult() = async metódust szinkron módon hív meg
        private void ImportBooks()
        {
            Console.WriteLine("\n--- TÖMEGES IMPORT ---");
            Console.Write("Fájl útvonal (formátum: Cím;Szerző;ISBN soronként): ");
            var path = Console.ReadLine();

            var result = _bookService.ImportBooksFromFileAsync(path ?? string.Empty).GetAwaiter().GetResult();
            Console.WriteLine($"Import kész. Sikeres: {result.importedCount}, kihagyott: {result.skippedCount}");
        }

        // Statisztikák lekérése és megjelenítése (szintén async → szinkron hívás)
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

        // Késedelmes kölcsönzőknek értesítéseket állít sorba (még nem küldi ki)
        private void QueueOverdueNotifications()
        {
            var count = _loanService.QueueOverdueNotifications();
            Console.WriteLine($"Sorba állított értesítések: {count}");
        }

        // A sorba állított értesítéseket ténylegesen kiküldi (logban jelenik meg)
        private void SendQueuedNotifications()
        {
            var sent = _loanService.SendQueuedNotifications();
            Console.WriteLine($"Kiküldött értesítések: {sent}");
        }

        private void MarkLoanOverdue()
        {
            Console.WriteLine("\n--- KÖLCSÖNZÉS KÉSEDELMESSÉ TÉTELE (TESZT) ---");
            ListActiveLoans();

            Console.Write("\nKölcsönzés ID-ja: ");
            if (!int.TryParse(Console.ReadLine(), out int loanId))
            {
                Console.WriteLine("Érvénytelen ID!");
                return;
            }

            Console.Write("Hány nappal legyen korábbra állítva a kölcsönzés dátuma? (pl. 15): ");
            if (!int.TryParse(Console.ReadLine(), out int daysEarlier) || daysEarlier <= 0)
            {
                Console.WriteLine("Érvénytelen napok száma!");
                return;
            }

            if (_loanService.MarkLoanOverdue(loanId, daysEarlier))
                Console.WriteLine("A kölcsönzés dátuma vissza lett állítva.");
            else
                Console.WriteLine("Hiba! A kölcsönzés nem található vagy már lezárt.");
        }

        // ───────────────────────────────────────────────
        // KÉSEDELMI DÍJAK ALMENÜ
        // ───────────────────────────────────────────────

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
                    case "1": ListFines(unpaidOnly: true); break;  // csak fizetetlenek
                    case "2": ListFines(unpaidOnly: false); break; // összes
                    case "3": PayFine(); break;
                    case "0": back = true; break;
                    default: Console.WriteLine("Érvénytelen választás!"); break;
                }
            }
        }

        // Díjak listázása – egy metódus két viselkedéssel, paraméter alapján
        private void ListFines(bool unpaidOnly)
        {
            // Ternáris operátorral dönti el melyik service-metódust hívja
            var fines = unpaidOnly ? _fineService.GetUnpaidFines() : _fineService.GetAllFines();

            if (!fines.Any())
            {
                Console.WriteLine("Nincsenek megjeleníthető díjak.");
                return;
            }

            foreach (var fine in fines)
            {
                var status = fine.isPaid
                    ? $"Fizetve: {fine.paidAt:yyyy-MM-dd HH:mm}" // dátum+idő formátum
                    : "Fizetetlen";
                Console.WriteLine($"[{fine.fineid}] {fine.Loan.Reader.name} - {fine.Loan.Copy.Book.title} | Összeg: {fine.amount} Ft | {status}");
            }
        }

        // Egy konkrét díj befizetésének rögzítése ID alapján
        private void PayFine()
        {
            ListFines(unpaidOnly: true); // csak a fizetetleneket mutatja

            Console.Write("\nBefizetendő díj ID-ja: ");
            if (!int.TryParse(Console.ReadLine(), out int fineId))
            {
                Console.WriteLine("Érvénytelen ID!");
                return;
            }

            // Tömör ternáris kiírás: sikeres-e a befizetés?
            Console.WriteLine(_fineService.PayFine(fineId)
                ? "Díj sikeresen befizetve."
                : "A díj nem található vagy már rendezett.");
        }
    }
}