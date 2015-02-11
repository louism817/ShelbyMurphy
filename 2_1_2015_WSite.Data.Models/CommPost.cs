using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2_1_2015_WSite.Data.Models
{
    public class CommPost
    {
        [Key]
        public int CommPostId { get; set; }

        public int CommunicationId { get; set; }
        [ForeignKey("CommunicationId")]
        public virtual Communication Communication { get; set; }

        [Required]
        public DateTime PostedDate { get; set; }

    }
}
