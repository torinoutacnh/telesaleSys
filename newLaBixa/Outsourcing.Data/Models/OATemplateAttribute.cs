using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Outsourcing.Data.Models
{

    public class OATemplateAttribute : BaseEntity
    {
        public OATemplateAttribute()
        {
            DateCreated = DateTime.Now;
            isDeleted = false;
        }
        public string AttributeName { get; set; }
        public string Content { get; set; }
        public string Temp_1 { get; set; }
        public string Temp_5 { get; set; }
        public string Temp_4 { get; set; }
        public string Temp_3 { get; set; }
        public string Temp_2 { get; set; }
        public bool isDeleted { get; set; }
        public DateTime DateCreated { get; set; }
        public int OATemplateId { get; set; }
        [ForeignKey("OATemplateId")]
        public virtual OATemplate OATemplate { get; set; }

    }
}
