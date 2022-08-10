using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XProject.Contract.Repository.Models
{
    public class EventZaloEntity : Entity
    {
        public string Event_Name { get; set; }
        public string Description { get; set; }
        public string Event_Code { get; set; }
        public string Data_Json { get; set; }
        public string isActive { get; set; }
        public bool Deleted { get; set; }
    }
}
