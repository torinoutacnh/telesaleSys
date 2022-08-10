using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XProject.Contract.Repository.Models
{
    public class SocialUserEntity : Entity
    {
        public string Oa_Id { get; set; }
        public string Phone_Number { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Zalo_Id { get; set; }
        public bool isFollow { get; set; }
        public bool Deleted { get; set; }
        public bool isActive { get; set; }
    }
}
