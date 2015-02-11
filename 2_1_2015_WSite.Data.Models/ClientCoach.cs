using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2_1_2015_WSite.Data.Models
{
    public class ClientCoach
    {
        [Key]
        public int ClientCoachId { get; set; }

        public string ClientId { get; set; }
        [ForeignKey("ClientId")]
        public virtual ApplicationUser Client { get; set; }

        public string CoachId { get; set; }
        [ForeignKey("CoachId")]
        public virtual ApplicationUser Coach { get; set; }
    }
}
