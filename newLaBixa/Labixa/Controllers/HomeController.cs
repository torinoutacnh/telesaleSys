using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Labixa.Models;
using Outsourcing.Service;
using Outsourcing.Data.Models;
using PagedList;
using Labixa.ViewModels;
using Labixa.Helpers;
namespace Labixa.Controllers
{

    public class HomeController : BaseHomeController
    {
     


        public HomeController()
        {
           
        }
        public ActionResult Index()
        {
           
            return Redirect("/Admin/DataUser");
        }

        public ActionResult About()
        {
            return View();
        }


        #region
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult getHeader()
        {
            return View();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult about_us()
        {
            return View();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Tours()
        {
            return View();
        }
        public ActionResult DetailTour()
        {
            return View();
        }
        public ActionResult Contact()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ContactUs(string Name, string Yahoo, string Rename, string Phone, string Description)
        {

            return View();
        }


        #endregion
        #region[Multi Language]
        public ActionResult SetCulture(string slug)
        {
            // Validate input
            slug = CultureHelper.GetImplementedCulture(slug);
            // Save culture in a cookie
            HttpCookie cookie = Request.Cookies["_culture"];
            if (cookie != null)
                cookie.Value = slug;   // update cookie value
            else
            {
                cookie = new HttpCookie("_culture");
                cookie.Value = slug;
                cookie.Expires = DateTime.Now.AddYears(1);
            }
            Response.Cookies.Add(cookie);
            return RedirectToAction("Index");
        }
        #endregion
    }
}