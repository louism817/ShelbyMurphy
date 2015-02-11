using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2_1_2015_WSite.Data.Models
{
    public class Appointment
    {
        [Key]
        public int AppointmentId { get; set; }
        [Required]

        public DateTime Date { get; set; }
        [Required]
        public string Reason { get; set; }
        public bool Accepted { get; set; }

        public int ApptTypeId { get; set; }
        [ForeignKey("ApptTypeId")]
        public virtual ApptType ApptType { get; set; }

        public int AppTimeFrameId { get; set; }
        [ForeignKey("AppTimeFrameId")]
        public virtual ApptTimeFrame TimeFrame { get; set; }
    }
}
