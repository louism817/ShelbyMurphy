using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2_1_2015_WSite.Data.Models
{
    public class Communication
    {
        [Key]
        public int CommunicationId { get; set; }
        [Required]
        public string Message { get; set; }
        [Required]
        public DateTime DateCreated { get; set; }
        public DateTime DateLastEdited { get; set; }
        public DateTime? DatePosted { get; set; }
        
        public DateTime? PubDate { get; set; }
        public DateTime? UnPubDate { get; set; }
        public bool Published { get; set; }
        public bool Deleted { get; set; }
        public string MessageTitle { get; set; }
        public string MessageLead { get; set; }
        [Required]
        public string CreatorId { get; set; }
        [ForeignKey("CreatorId")]
        public virtual ApplicationUser Creator { get; set; }

        public int CommunicationTypeId { get; set; }
        [ForeignKey("CommunicationTypeId")]
        public virtual CommunicationType CommunicationType { get; set; }
    }
}
