using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Outsourcing.Data.Models
{
    public class District : BaseEntity
    {
        public District()
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
        public string Email { get; set; }
        public string Description { get; set; }
        public string Province { get; set; }
        public string Code { get; set; }

        public virtual ICollection<Store> Stores { get; set; }
    }
}
