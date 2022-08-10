using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Outsourcing.Data.Models
{
    public class Configuation : BaseEntity
    {
        public string KPI_DayCall { get; set; }
        public string KPI_WeekCall { get; set; }
        public string KPI_IncomeWeek { get; set; }

        public string KPI_IncomeMonth { get; set; }

        public double KPI_L1PL0 { get; set; }

        public double KPI_L2_L1 { get; set; }

        public double KPI_L3_L1 { get; set; }

        public bool IsDelete { get; set; }
        public bool IsActive { get; set; }
    }
}
