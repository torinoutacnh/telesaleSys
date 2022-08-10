using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XProject.Contract.Repository.Models
{
    public class Working_Time : Entity
    {
        public DateTime? WorkStart { get; set; }
        public DateTime? WorkEnd { get; set; }
        public string Status { get; set; }
        public int Duration { get; set; }

        public string Working_DailyId { get; set; }
        public virtual Working_Daily Working_Daily { get; set; }
    }
}
