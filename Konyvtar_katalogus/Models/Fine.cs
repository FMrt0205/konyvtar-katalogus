using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Konyvtar_katalogus.Models
{
    public class Fine
    {
        [Key]
        public int fineid { get; set; }
        [Required]
        public decimal amount { get; set; }
        [Required]
        public bool isPaid { get; set; }
        
        public int LoanId { get; set; }
        public Loan Loan { get; set; }



    }
}
