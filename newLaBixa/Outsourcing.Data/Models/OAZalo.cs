using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Outsourcing.Data.Models
{
    public class OAZalo : BaseEntity
    {
        public OAZalo()
        {
            DateCreated = DateTime.Now;
            Deleted = false;
        }
        public string OA_Id { get; set; }
        public string SecretKey { get; set; }
        public string AccessToken { get; set; }
        public string RefeshToken { get; set; }
        public DateTime DateCreated { get; set; }
        public string CodeChallege { get; set; }
        public string CodeVerify { get; set; }
        public int Brand_Id { get; set; }
        public string Brand_Code { get; set; }
        public string Brand_Name { get; set; }
        public string Phone { get; set; }
        public string Ath_Code { get; set; }
        public DateTime Expire_Datetime { get; set; }
        public bool isPublic { get; set; }
        public bool Deleted { get; set; }
    }
}
