using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Outsourcing.Data.Models
{
    public class SchedulesStoreMapping : BaseEntity
    {
        public int IdStore { get; set; }
        public int IdSchedule { get; set; }

        //[ForeignKey("SchedulesId")]
        public virtual Schedule Schedules { get; set; }
        //[ForeignKey("StoreId")]
        public virtual Store Stores { get; set; }


    }
}

