using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2_1_2015_WSite.Data.Models
{
    public class ApptTimeFrame
    {
        [Key]
        public int ApptTimeFrameId { get; set; }
        [Required]
        public string ApptTimeFrameDesc { get; set; }
    }
}
