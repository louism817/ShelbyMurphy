using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2_1_2015_WSite.Data.Models
{
    public class CommSchedUser
    {
        [Key]
        public int ComSchedUsersId { get; set; }

        public int CommScheduleId { get; set; }
        [ForeignKey("CommScheduleId")]
        public virtual CommSchedule CommSchedule { get; set; }

        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; }
    }
}
