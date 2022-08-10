using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Outsourcing.Data.Models
{
    public class Campaign : BaseEntity
    {
        public string OwnId { get; set; }
        public string Name { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<int> Type { get; set; }
        public Nullable<int> Status { get; set; }
        public Nullable<System.DateTime> ExpiredDate { get; set; }
        public string Descriptions { get; set; }
        public Nullable<double> CostReal { get; set; }
        public Nullable<double> ExpectedRevenue { get; set; }
        public Nullable<double> RealRevenue { get; set; }
        public Nullable<double> RenewalRevenue { get; set; }
        public Nullable<double> CostExpect { get; set; }
        public string Code { get; set; }
        public Nullable<System.DateTime> EditDate { get; set; }
        public Nullable<double> Price_1 { get; set; }
        public Nullable<double> Price_2 { get; set; }
        public Nullable<double> Price_3 { get; set; }
        public string temp1 { get; set; }
        public string temp2 { get; set; }
        public bool IsDeleted { get; set; }
        public bool Active { get; set; }
        public virtual ICollection<CampaignDataMapping> CampaignDataMappings { get; set; }
    }
}
