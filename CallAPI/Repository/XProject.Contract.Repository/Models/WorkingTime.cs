using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XProject.Contract.Repository.Models
{
    public class WorkingTime : Entity
    {
        public DateTime WorkStart { get; set; }
        public DateTime WorkEnd { get; set; }
        public string Status { get; set; }
        public int Duration { get; set; }

        public string WorkingDailyId { get; set; }
        public virtual WorkingDaily WorkingDaily { get; set; }
    }
}
