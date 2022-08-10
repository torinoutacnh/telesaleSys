using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XProject.Core.Constants
{
    public class Endpoints
    {
        public static class TrackingEndpoint
        {
            public const string Area = "/tracking";

            public const string BaseEndpoint = "~" + Area;
            public const string GetByBrand = "~" + Area + "/getbybrand";
            public const string GetByExt = "~" + Area + "/getbyext";
        }
    }
}
