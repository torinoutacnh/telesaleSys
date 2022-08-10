using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XProject.Contract.Repository.Models
{
    public class OrderZaloEntity : Entity
    {
        public string Customer_Id { get; set; }
        public string Contact_Phone { get; set; }
        public string Code { get; set; }
        public string Contact_Name { get; set; }
        public string Address { get; set; }
        public string Province_Name { get; set; }
        public string District_Name { get; set; }
        public string Ward_Name { get; set; }
        public int Total_discount_price { get; set; }
        public int Total_origin_price { get; set; }
        public DateTimeOffset Updated_at { get; set; }
        public bool isActive { get; set; }
        public bool Deleted { get; set; }
    }
}
