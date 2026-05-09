 using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Konyvtar_katalogus.Models
{
    public class Book
    {
        [Key]
        public int bookid { get; set; }
        [Required]
        public string title{ get; set; }
        [Required]
        public string author { get; set; }
        [Required]
        [MaxLength(13)]
        public string isbn { get; set; }
        public ICollection<Copy>? Copies { get; set; }

    }
}
