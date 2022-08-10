using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Labixa.Areas.Admin.ViewModel
{
    public class CampaignFormModel
    {
        public int Id { get; set; }
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
    }
}