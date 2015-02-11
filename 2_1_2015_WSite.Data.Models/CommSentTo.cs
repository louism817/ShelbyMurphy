using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2_1_2015_WSite.Data.Models
{
    public class CommSentTo
    {
        [Key]
        public int CommSentToId { get; set; }
        public DateTime PostedDate { get; set; }

        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual ApplicationUser SentTo { get; set; }

        public int CommunicationId { get; set; }
        [ForeignKey("CommunicationId")]
        public virtual Communication Communication { get; set; }
            
    }
}
