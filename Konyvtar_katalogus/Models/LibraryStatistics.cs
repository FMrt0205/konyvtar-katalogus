namespace Konyvtar_katalogus.Models
{
    public class LibraryStatistics
    {
        
        public int TotalBooks { get; set; }
        
        public int TotalCopies { get; set; }
        
        public int AvailableCopies { get; set; }
        
        public int TotalReaders { get; set; }
        
        public int ActiveLoans { get; set; }
        
        public int OverdueLoans { get; set; }
        
        public int UnpaidFines { get; set; }
    }
}
