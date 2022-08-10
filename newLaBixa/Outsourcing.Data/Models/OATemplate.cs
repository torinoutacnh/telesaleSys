using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Outsourcing.Data.Models
{
    public class OATemplate : BaseEntity
    {
        public OATemplate()
        {
            DateCreated = DateTime.Now;
            LastEditedTime = DateTime.Now;
            IsActive = true;
            Deleted = false;
        }
        public string Template_Name { get; set; }
        public string Mode { get; set; }
        public string Phone { get; set; }
        public string TemplateId { get; set; }
        public string TrackingId { get; set; }
        public string Access_Token { get; set; }
        public string Address { get; set; }
        public string ContentEng { get; set; }
        public string Noted { get; set; }
        public System.DateTime DateCreated { get; set; }
        public System.DateTime LastEditedTime { get; set; }
        public bool IsActive { get; set; }
        public bool Deleted { get; set; }
        public string TeleId { get; set; }
        public string temp1 { get; set; }
        public string temp2 { get; set; }
        public string temp3 { get; set; }
        public string temp4 { get; set; }
        public string temp5 { get; set; }

        public Nullable<int> BrandId { get; set; }
        public string Brand_Name { get; set; }
        public string Brand_Code { get; set; }

        public string Balance { get; set; }
        public ICollection<OATemplateAttribute> OATemplateAttributes { get; set; }

    }
}
