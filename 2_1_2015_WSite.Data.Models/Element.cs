using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2_1_2015_WSite.Data.Models
{
    public class Element
    {
        [Key]
        public int ElementId { get; set; }
        [Required]
        public string ElementBody { get; set; }
        [Required]
        public int ElementType { get; set; }
    }
}
