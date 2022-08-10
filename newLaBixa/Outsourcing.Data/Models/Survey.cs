using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Outsourcing.Data.Models
{
    public class Survey : BaseEntity
    {
        public Survey()
        {

        }
        public int LogdiaryId { get; set; }
        public int QuestionId { get; set; }
        public int Point { get; set; }
        public string Note { get; set; }
        [ForeignKey("LogdiaryId")]
        public virtual Logdiary Logdiary { get; set; }
        [ForeignKey("QuestionId")]
        public virtual Question Question { get; set; }

    }
}
