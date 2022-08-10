using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Outsourcing.Data.Models
{
    public class StoreProductMapping : BaseEntity
    {
        public StoreProductMapping()
        {
            DateCreated = new DateTime();
            LastEditedTime = new DateTime();
        }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public string Address { get; set; }
        public System.DateTime DateCreated { get; set; }
        public System.DateTime LastEditedTime { get; set; }
        public string Description { get; set; }
        public string Code { get; set; }

        public string Temp1 { get; set; }
        public string Temp2 { get; set; }
        public string Temp3 { get; set; }
        public int ProductId { get; set; }
        public int StoreId { get; set; }
        [ForeignKey("StoreId")]
        public virtual Store Store{ get; set; }
        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }

    }
}
