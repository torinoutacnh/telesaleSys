using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Outsourcing.Data.Models
{
    public class TrackingAttendence : BaseEntity
    {
        public TrackingAttendence()
        {
            DateCreated = new DateTime();
            LastEditedTime = new DateTime();
            TotalCall_Number = TotalCall_Minute = TotalCall_Second = 0;
        }
        public string TeleName { get; set; }
        public string TeleId { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public string Address { get; set; }
        public System.DateTime DateCreated { get; set; }
        public System.DateTime LastEditedTime { get; set; }
        public System.DateTime? DateStart { get; set; }
        public System.DateTime? DateEnd { get; set; }
        public System.DateTime? DurationDate { get; set; }
        public int TotalCall_Number { get; set; }
        public int TotalCall_Minute { get; set; }
        public int TotalCall_Second { get; set; }
        public string Email { get; set; }
        public string Description { get; set; }
        public string Province { get; set; }
        public string Code { get; set; }

    }
}
