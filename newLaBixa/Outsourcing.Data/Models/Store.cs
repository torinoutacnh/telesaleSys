using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Outsourcing.Data.Models
{
    public class Store : BaseEntity
    {
        public Store()
        {
            DateCreated = new DateTime();
            LastEditedTime = new DateTime();
        }
        public string Name { get; set; }
        public string Store_Code { get; set; }
        public bool IsActive { get; set; }
        public string Address { get; set; }
        public System.DateTime DateCreated { get; set; }
        public System.DateTime LastEditedTime { get; set; }
        public string Email { get; set; }
        public Nullable<decimal> PhoneNumber { get; set; }
        public string Description { get; set; }
        public string District { get; set; }
        public string Province { get; set; }
        public string Code { get; set; }
        public int BrandId { get; set; }
        public bool IsDelete { get; set; }

        [ForeignKey("BrandId")]
        public virtual Brand Brand { get; set; }
        //public virtual ICollection<StoreProductMapping> StoreProductMappings { get; set; }
    }
}
