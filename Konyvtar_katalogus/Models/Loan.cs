using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Konyvtar_katalogus.Models
{
    public class Loan
    {
        [Key]
        public int loanid { get; set; }
        [Required]
        public DateTime loanDate { get; set; }
        public DateTime returnDate { get; set; }
        public int copyid { get; set; }
        public Copy Copy { get; set; }
        public int ReaderId { get; set; }
        public Reader Reader { get; set; }
    }
}
