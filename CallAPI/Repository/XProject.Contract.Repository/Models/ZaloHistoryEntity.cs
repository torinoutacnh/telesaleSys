using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XProject.Contract.Repository.Models
{
    public  class ZaloHistoryEntity : Entity
    {
        //Id ứng dụng 
        public string app_id { get; set; }
        // Id của OA
        public string oa_id { get; set; }
        //Trường này chỉ có khi app_id và oa_id trên đang liên kết với nhau.
        //Id này có thể sử dụng cho social_api
        public string user_id_by_app { get; set; }
        //ID bảng event
        public string event_id { get; set; }
        public string timestamp { get; set; }   
        //Id recipient 
        public string socialuser_id { get; set; }
        //“msg_ids”: Danh sách id của các tin nhắn đã được người dùng xem.
        public string message { get; set; }
        //“id”: Id của Official Account gửi tin nhắn
        public string sender { get; set; }
    }
}
