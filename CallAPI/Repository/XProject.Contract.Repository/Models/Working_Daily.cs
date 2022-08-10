using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XProject.Contract.Repository.Models
{
    public class Working_Daily : Entity
    {
        public string Brand_ID { get; set; }
        public string Brand_Name { get; set; }
        public string Brand_Code { get; set; }
        public string Ext { get; set; }
        public string Staff_Code { get; set; }
        public string Status { get; set; }
        public DateTime? DateStart { get; set; }
        public DateTime? DateEnd { get; set; }
        public int Duration { get; set; }
        public ICollection<Working_Time> Working_Times { get; set; }

    }
}
