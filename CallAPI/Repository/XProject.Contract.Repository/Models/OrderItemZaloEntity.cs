
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XProject.Contract.Repository.Models
{
    public  class OrderItemZaloEntity : Entity
    {
        public string Product_Id { get; set; }
        public string Product_code { get; set; }
        public string Product_name { get; set; }
        public string Product_image { get; set; }
        public int Quantity { get; set; }
        public int Price { get; set; }
        public int Discount_price { get; set; }
        public int Discount_percent { get; set; }
        public bool isActive { get; set; }
        public bool Deleted { get; set; }
    }
}
