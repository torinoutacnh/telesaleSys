﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XProject.Contract.Repository.Models
{
    public class CallsHistoryEntity : Entity
    {
        public string ServiceName { get; set; }
        public string AuthUser { get; set; }
        public string AuthKey { get; set; }
        public string TypeGet { get; set; }
        public DateTimeOffset DateStart { get; set; }
        public DateTimeOffset DateEnd { get; set; }
        public string CallNum { get; set; }
        public string ReceiveNum { get; set; }
        public string Key { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }
}
