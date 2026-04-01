using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
namespace Konyvtar_katalogus.Models
{
    public class Reader
    {
        [Key]
        public int readerid { get; set; }
        [Required]
        public string name { get; set; }
        [MaxLength(200)]
        public string? email { get; set; }

        public ICollection<Loan> Loans { get; set; }

    }
}
