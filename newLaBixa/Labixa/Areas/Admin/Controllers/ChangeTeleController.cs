using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Outsourcing.Data.Models;
using Outsourcing.Service;
using Excel = Microsoft.Office.Interop.Excel;
using System.IO;
using System.Data.OleDb;
using OfficeOpenXml;
using Labixa.Areas.Admin.ViewModel;
using AutoMapper;
using Outsourcing.Core.Extensions;
using System.Web.Script.Serialization;
using Labixa.Areas.Admin.Models;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using Outsourcing.Core.Common;
using System.Text.RegularExpressions;
using Labixa.Areas.Report.Controllers;
using System;

namespace Labixa.Areas.Admin.Controllers
{
    public class ChangeTeleController : Controller
    {
        readonly ILogdiaryService logDiaryService;
        readonly IProductService productService;
        readonly IDataUserService service;
        readonly IUserDataMappingService userDataMappingService;


        int _BRAND_ID = 0;
        string _BRAND_CODE = "THT";
        string _BRAND_NAME = "Toàn Hệ Thống";
        public void GetCookie()
        {
            HttpCookie brandIdCookie = Request.Cookies["BrandId"];
            HttpCookie brandNameCookie = Request.Cookies["BrandCode"];
            HttpCookie brandCodeCookie = Request.Cookies["BrandName"];
            if (brandIdCookie != null)
            {
                _BRAND_ID = int.Parse(brandIdCookie.Value);
                _BRAND_CODE = brandNameCookie.Value;
                _BRAND_NAME = brandCodeCookie.Value;
            }
        }

        public ChangeTeleController(ILogdiaryService logDiaryService, IProductService productService, IDataUserService service)
        {
            this.logDiaryService = logDiaryService;
            this.productService = productService;
            this.service = service;
        }
        //
        // GET: /Admin/ChangeTele/
        public ActionResult Index()
        {
            GetCookie();
            var model = logDiaryService.GetLogdiarys().Where(p => p.UserDataId == p.DataUser.Id);
            return View(model);
        }
	}
}