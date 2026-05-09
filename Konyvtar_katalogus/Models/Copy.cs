using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Konyvtar_katalogus.Models
{
    public class Copy
    {
        [Key]
        public int copyid { get; set; }
        [Required]
        [MaxLength(5)]
        [RegularExpression(@"[1-9][1-9]-[1-9][1-9]")]
        
        public string InventoryNumber { get; set; }
        [Required]
        
        public bool isAvailable { get; set; }

        
        public int BookId { get; set; }

        public Book Book { get; set; }

        
        public ICollection<Loan> Loans { get; set; }
    }
}

