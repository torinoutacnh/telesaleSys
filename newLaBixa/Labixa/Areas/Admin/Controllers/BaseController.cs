using Labixa.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace Labixa.Areas.Admin.Controllers
{
    public static class paramet
    {
        public static int _BRAND_ID { get; set; }
        public static string _BRAND_CODE { get; set; }
        public static string _BRAND_NAME { get; set; }
    }
    [Authorize]
    public class BaseController : Controller
    {
        protected override IAsyncResult BeginExecuteCore(AsyncCallback callback, object state)
        {
            string cultureName = null;
            cultureName = "vi";
            // Validate culture name
            HttpCookie cultureCookie = Request.Cookies["_culture"];
            if (cultureCookie != null)
            {                //cultureName = cultureCookie.Value;
                cultureName = "vi";
            }
            //else
            //    cultureName = Request.UserLanguages != null && Request.UserLanguages.Length > 0 ?
            //            Request.UserLanguages[0] :  // obtain it from HTTP header AcceptLanguages
            //            null;
            // Validate culture name
            cultureName = CultureHelper.GetImplementedCulture(cultureName); // This is safe

            cultureName = "vi";
            // Modify current thread's cultures            
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("vi");
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;
            GetCookie();
            return base.BeginExecuteCore(callback, state);
        }
        #region thông báo message
        protected void SetAlert(string message, string type)
        {
            TempData["AlertMessage"] = message;
            if (type == "success")
            {
                TempData["AlertType"] = "alert-success";
            }else if (type == "warning")
            {
                TempData["AlertType"] = "alert-warning";
            }
            else
            {
                TempData["AlertType"] = "alert-danger";
            }
        }
        #endregion
        public void GetCookie()
        {
            HttpCookie brandIdCookie = Request.Cookies["BrandId"];
            HttpCookie brandNameCookie = Request.Cookies["BrandCode"];
            HttpCookie brandCodeCookie = Request.Cookies["BrandName"];
            if (brandIdCookie != null)
            {
                paramet._BRAND_ID = int.Parse(brandIdCookie.Value);
                paramet._BRAND_CODE = brandNameCookie.Value;
                paramet._BRAND_NAME = brandCodeCookie.Value;
            }
        }
    }
}