using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XProject.Contract.Repository.Models
{
    public class BrandEntity : Entity
    {
        public string BrandId { get; set; }
        public string BrandName { get; set; }
        public string BrandCode { get; set; }
        public string ServiceName { get; set; }
        public string AuthUser { get; set; }
        public string AuthKey { get; set; }
        public string ZaloId { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public string AccessToken { get; set; }
        public string RefeshToken { get; set; }
        public bool isActive { get; set; }
        public bool Deleted { get; set; }
    }
}
