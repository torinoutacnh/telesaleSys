using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Outsourcing.Data.Models
{
    public class Brand : BaseEntity
    {
        public Brand()
        {
            DateCreated = new DateTime();
            LastEditedTime = new DateTime();
            Num1 = Num2 = Num3 = 0;
            Price1 = Price2 = Price3 = 0;
        }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public string Address { get; set; }
        public System.DateTime DateCreated { get; set; }
        public System.DateTime LastEditedTime { get; set; }
        public string Description { get; set; }
        public string Code { get; set; }
        public string ManagerName { get; set; }
        public string PhoneContact { get; set; }
        public string Email { get; set; }
        public string Website { get; set; }
        
        public int Num1 { get; set; }
        public int Num2 { get; set; }
        public int Num3 { get; set; }
        public double Price1 { get; set; }
        public double Price2 { get; set; }
        public double Price3 { get; set; }
        public string Temp1 { get; set; }
        public string Temp2 { get; set; }
        public string Temp3 { get; set; }
        //public bool Deleted { get; set; }

        public virtual ICollection<Store> Store { get; set; }
    }
}
