using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Outsourcing.Data.Models
{
    public class CampaignDataMapping : BaseEntity
    {
        public int DataId { get; set; }
        public int CampaignId { get; set; }
        public string Description { get; set; }
        public Nullable<bool> isNew { get; set; }
        public bool IsDeleted { get; set; }
        public bool Active { get; set; }
        public virtual Campaign Campaign { get; set; }
        public virtual DataUser DataUser { get; set; }
    }
}
