using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Outsourcing.Data.Models
{
    public class UserDataMapping :BaseEntity
    {
        public UserDataMapping()
        {
            DateCreated = new DateTime();
            LastEditedTime = new DateTime();
        }
        public int DataUserId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string NameEng { get; set; }
        public string ShortName { get; set; }
        public string Title { get; set; }
        public string TitleEng { get; set; }
        public string Content { get; set; }
        public string ContentEng { get; set; }
        public string Noted { get; set; }
        public System.DateTime DateCreated { get; set; }
        public System.DateTime LastEditedTime { get; set; }
        public bool IsActive { get; set; }
        public bool Deleted { get; set; }
        public int Position { get; set; }
        public string temp1 { get; set; }
        public string temp2 { get; set; }
        public string temp3 { get; set; }
        public string temp4 { get; set; }
        public string temp5 { get; set; }
        public string UserId { get; set; }
        public string noteSchedule { get; set; }
        public Nullable<System.DateTime> ScheduleCall { get; set; }
        [ForeignKey("UserId")]

        public virtual User User { get; set; }
        [ForeignKey("DataUserId")]

        public virtual DataUser DataUser { get; set; }
        public virtual ICollection<Schedule> Schedules { get; set; }
    }
}
