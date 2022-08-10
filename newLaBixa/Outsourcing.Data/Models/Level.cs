using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Outsourcing.Data.Models
{
    public class Level :BaseEntity
    {
        public Level()
        {
            DateCreated = new DateTime();
            LastEditedTime = new DateTime();
        }
        public string CodeLevel { get; set; }
        public string LevelName { get; set; }
        public string LevelNameEng { get; set; }
        public string Title { get; set; }
        public string TitleEng { get; set; }
        public string Content { get; set; }
        public string ContentEng { get; set; }
        public string Noted { get; set; }
        public System.DateTime DateCreated { get; set; }
        public System.DateTime LastEditedTime { get; set; }
        public bool IsActive { get; set; }
        public bool Deleted { get; set; }
        public int Position { get; set; }
        public string temp1 { get; set; }
        public string temp2 { get; set; }
        public string temp3 { get; set; }
        public string temp4 { get; set; }
        public string temp5 { get; set; }

        public int BrandId { get; set; }
        public string BrandCode { get; set; }
        public string BrandName { get; set; }



        public virtual ICollection<DataUser> DataUsers { get; set; }
    }
}
