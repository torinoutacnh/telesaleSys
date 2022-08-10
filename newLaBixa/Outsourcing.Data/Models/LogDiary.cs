using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Outsourcing.Data.Models
{
    public class Logdiary :BaseEntity
    {
        public Logdiary()
        {
            DateCreated = new DateTime();
            LastEditedTime = new DateTime();
            TotalCallAgain = TotalCallSucceed = TotalNotCall = 0;
        }
        public string CodeLogDiary { get; set; }
        public string LogName { get; set; }
        public string LogNameEng { get; set; }
        public string Title { get; set; }
        public string Key_Cloudphone { get; set; }
        public string TitleEng { get; set; }
        public string Content { get; set; }
        public string ContentEng { get; set; }
        public string Noted { get; set; }
        public int TotalCallSucceed { get; set; }
        public int TotalNotCall { get; set; }
        public int TotalCallAgain { get; set; }
        public int TotalCall_Second { get; set; }
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
        public string TimeCall { get; set; }
        public string StatusCall { get; set; }
        public Nullable<int> TotalStopCall { get; set; }
        public string LinkFile { get; set; }
        public string PhoneReceived { get; set; }
        public Nullable<int> LevelId { get; set; }
        public string NameLevel { get; set; }
        public string TeleId { get; set; }
        public string TeleName { get; set; }
        public Nullable<double> Price_1 { get; set; }
        public Nullable<double> Price_2 { get; set; }
        public Nullable<double> Price_3 { get; set; }
        public double? TotalPayment { get; set; }
        public string Store_Code { get; set; }
        public int UserDataId { get; set; }

        [ForeignKey("UserDataId")]
        public virtual DataUser DataUser { get; set; }

        public virtual ICollection<OrderItem> OrderItems { get; set; }

    }
}
