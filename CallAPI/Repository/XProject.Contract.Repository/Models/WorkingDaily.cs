using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XProject.Contract.Repository.Models
{
    public class WorkingDaily : Entity
    {
        public string BrandID { get; set; }
        public string BrandName { get; set; }
        public string BrandCode { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
        public int Duration { get; set; }
        public ICollection<WorkingTime> Working_Times { get; set; }

    }
}
