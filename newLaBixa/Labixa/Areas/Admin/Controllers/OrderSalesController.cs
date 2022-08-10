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
using System.Collections.Generic;

namespace Labixa.Areas.Admin.Controllers
{
    public class OrderSalesController : Controller
    {
        readonly ILogdiaryService logDiaryService;
        readonly IProductService productService;
        readonly IDataUserService service;


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

        public OrderSalesController(ILogdiaryService logDiaryService, IProductService productService, IDataUserService service)
        {
            this.logDiaryService = logDiaryService;
            this.productService = productService;
            this.service = service;
        }
        //
        // GET: /Admin/OrderSales/
        public ActionResult Index(string lastDate)
        {
            GetCookie();
            //var listProduct = productService.GetAllProducts().Where(p => p.BrandId == _BRAND_ID).ToList();
            //ViewBag.ListProduct = listProduct;
            //var day = dateFrom.Day;
            //var month = dateFrom.Month;
            
            var from = lastDate.Split(' ')[0];
            var dateFrom = DateTime.Parse(from.Split('/')[1] + "/" + from.Split('/')[0] + "/" + from.Split('/')[2]);
            var day = dateFrom.Date;
            //var listDiary = new List<Logdiary>();
            //var listUser = service.GetDataUsers().Where(p => p.BrandId == _BRAND_ID);
            //var model = logDiaryService.GetLogdiarys().Where(p => p.DateCreated.Date.Equals(dateFrom.Date) && p.StatusCall.Equals("BUY PRODUCT") && p.IsActive == true && p.Deleted != true && p.DataUser.BrandId == _BRAND_ID && p.OrderItems.Any()).ToList();
            //listDiary = logDiaryService.GetLogdiarys().Where(p => p.DateCreated.Month == dateFrom.Month && p.DateCreated.Year == dateFrom.Year && p.StatusCall != null).ToList();
            //var userId = service.GetDataUserByBrandId(_BRAND_ID);
            var model = logDiaryService.GetLogdiarys().Where(p => p.StatusCall.Equals("BUY PRODUCT")&& p.DataUser.BrandId == _BRAND_ID && p.DateCreated.Date.Equals(dateFrom.Date) && p.Price_1 != 0).ToList();

            return View(model);
        }
        public ActionResult OrderByMonth(string lastMonth)
        {
            GetCookie();
            var listProduct = productService.GetAllProducts().Where(p => p.BrandId == _BRAND_ID).ToList();
            ViewBag.ListProduct = listProduct;
            //var day = dateFrom.Day;
            //var month = dateFrom.Month;

            //var from = lastDate.Split(' ')[0];
            //var dateFrom = DateTime.Parse(lastDate);
            var from = lastMonth.Split(' ')[0];
            var dateFrom = DateTime.Parse(from.Split('/')[1] + "/" + from.Split('/')[0] + "/" + from.Split('/')[2]);
            //var month = dateMonth.Month;

            //var listUser = service.GetDataUsers().Where(p => p.BrandId == _BRAND_ID);
            var model = logDiaryService.GetLogdiarys().Where(p => p.LastEditedTime.Month.Equals(dateFrom.Month) && p.StatusCall.Equals("BUY PRODUCT") && p.DataUser.BrandId == _BRAND_ID && p.IsActive == true && p.Deleted != true && p.OrderItems.Any()).ToList();
            //var model = logDiaryService.GetLogdiarys().Where(p=>p.StatusCall.Equals ("BUY PRODUCT") && p.LastEditedTime.Day == dateFrom.Day && p.LastEditedTime.Month == dateFrom.Month && p.LastEditedTime.Year == dateFrom.Year && p.DataUser.BrandId == _BRAND_ID).ToList();

            return View(model);
        }
    }
}