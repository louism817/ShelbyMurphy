using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2_1_2015_WSite.Data.Models
{
    public class ApptType
    {
        [Key]
        public int ApptTypeId { get; set; }
        [Required]
        public string ApptTypeDesc { get; set; }
    }
}
