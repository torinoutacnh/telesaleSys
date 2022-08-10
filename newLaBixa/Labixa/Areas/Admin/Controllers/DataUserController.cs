using System;
using System.Collections.Generic;
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
using log4net;

namespace Labixa.Areas.Admin.Controllers
{
    [Authorize]
    public class DataUserController : BaseController
    {
        private List<int> currentUserDataMappings = new List<int>();
        readonly IDataUserService service;
        readonly IUserDataMappingService userDataMappingService;
        readonly IChanelDataService chanelDataService;
        readonly ILevelService levelService;
        readonly ILogdiaryService logDiaryService;
        private UserManager<User> _userManager;
        private IUserService _userManager2;
        readonly IOrderService orderService;
        readonly IProductService productService;
        readonly IBrandService brandService;
        readonly IStoreService storeService;
        readonly IWardService wardService;
        readonly IDistrictService districtService;
        readonly IScheduleService scheduleService;
        readonly IQuestionService _questionService;
        readonly ISurveyService _surveyService;
        ILog log = log4net.LogManager.GetLogger(typeof(DataUserController));
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

        public DataUserController(IUserService userManager2, IDataUserService service, IUserDataMappingService userDataMappingService,
            IChanelDataService chanelDataService, ILevelService levelService, UserManager<User> userManager
            , ILogdiaryService logDiaryService, IOrderService orderService, IProductService productService
            , IStoreService storeService, IBrandService brandService, IWardService wardService, IDistrictService districtService, IScheduleService scheduleService, IQuestionService questionService, ISurveyService surveyService)
        {
            this.service = service;
            this.userDataMappingService = userDataMappingService;
            //this.aspNetUserService = aspNetUserService;
            this.chanelDataService = chanelDataService;
            this.levelService = levelService;
            this.logDiaryService = logDiaryService;
            _userManager = userManager;
            this.orderService = orderService;
            this.productService = productService;
            this.storeService = storeService;
            this.brandService = brandService;
            this.wardService = wardService;
            this.districtService = districtService;
            this.scheduleService = scheduleService;
            this._questionService = questionService;
            this._userManager2 = userManager2;
            this._surveyService = surveyService;

        }
        [HttpPost]
        public ActionResult MappingData(string UserId, int dataId)
        {

            var datamapping = new UserDataMapping();
            datamapping.UserId = UserId;
            datamapping.DataUserId = dataId;
            datamapping.DateCreated = DateTime.Now;
            datamapping.Deleted = false;
            datamapping.IsActive = true;
            datamapping.LastEditedTime = DateTime.Now;
            userDataMappingService.CreateUserDataMapping(datamapping);
            var _user = _userManager.FindById(UserId).UserName;
            var dataUser = service.GetDataUserById(dataId);
            dataUser.UserName = _user;
            service.EditDataUser(dataUser);
            return Redirect("/Admin/DataUser");
        }
        public ActionResult Index()
        {

            // check account
            // 2 option with admin and teleSale
            GetCookie();
            ViewBag.ChannelList = chanelDataService.GetChanelDatas().ToSelectListItems(-1);
            ViewBag.LevelList = levelService.GetLevels().ToSelectListItems(-1);
            ViewBag.listUser = _userManager2.Getusers().ToSelectListItems("-1");
            var user = _userManager.FindById(User.Identity.GetUserId());
            var roles = _userManager.GetRoles(user.Id);
            if (User.IsInRole("Admin"))
            {
                var entities = service.GetDataUsers().Where(p => p.BrandId == _BRAND_ID).OrderByDescending(p => p.Id).ToList();
                ViewBag.ShareData = userDataMappingService.GetUserDataMappings().Where(p => p.UserId == null && !p.IsActive).ToList();
                return View(entities);
            }
            return RedirectToAction("TeleIndex", "DataUser", new { id = User.Identity.GetUserId() });
        }


        [HttpPost]
        //public ActionResult Index(IEnumerable<int> stores, IEnumerable<int> wards, IEnumerable<int> districts, IEnumerable<string> teles, string type, string DateCreated, string historyCall, string sex)
        public ActionResult Index(IEnumerable<int> stores, IEnumerable<int> wards, IEnumerable<int> districts, IEnumerable<string> teles, IEnumerable<string> levels, string type, string DateCreated, string historyCall, string sex)
        {

            // check account
            // 2 option with admin and teleSale
            var user = _userManager.FindById(User.Identity.GetUserId());
            var roles = _userManager.GetRoles(user.Id);

            #region filter
            var idStoreByDistrict = new List<int>();
            var idStoreByWard = new List<int>();
            var idStore = new List<int>();
            var idOrder = new List<int>();
            var idSchedule = new List<int>();

            if (districts != null)
            {
                var listStoreByDistrict = districtService.GetDistricts().Where(p => districts.Contains(p.Id)).ToList();
                foreach (var item in listStoreByDistrict)
                {
                    idStoreByDistrict.Add(item.Id);
                }
            }

            if (wards != null)
            {
                var listStoreByWard = wardService.GetWards().Where(p => wards.Contains(p.Id)).ToList();
                foreach (var item in listStoreByWard)
                {
                    idStoreByWard.Add(item.Id);
                }
            }
            if (stores != null)
            {
                var listStore = storeService.GetStores().Where(p => stores.Contains(p.Id)).ToList();
                foreach (var item in listStore)
                {
                    idStore.Add(item.Id);
                }
            }

            if (idStoreByDistrict.Count > 0)
            {
                var listStore = storeService.GetStores().Where(p => idStoreByDistrict.Contains(p.Id)).ToList();
                foreach (var item in listStore)
                {
                    idStore.Add(item.Id);
                }
            }
            if (idStoreByWard.Count > 0)
            {
                var listStore = storeService.GetStores().Where(p => idStoreByWard.Contains(p.Id)).ToList();
                foreach (var item in listStore)
                {
                    idStore.Add(item.Id);
                }
            }

            var listOrder = orderService.GetOrders().Where(p => idStore.Contains(p.Id)).ToList();
            if (type != null)
            {
                if (type == "1")
                {
                    listOrder = listOrder.Where(p => p.Type == 2).ToList();//2 la ban hang
                }
                else if (type == "2")
                {
                    listOrder = listOrder.Where(p => p.Type == 1).ToList();//1 la dich vu
                }
            }
            foreach (var item in listOrder)
            {
                idOrder.Add(item.UserDataMappingId);
            }
            #endregion

            if (User.IsInRole("Admin"))
            {
                var entities = service.GetDataUsers().OrderByDescending(p => p.Id).ToList();

                #region filter where
                if (stores != null || wards != null || districts != null)
                {
                    entities = service.GetDataUsers().Where(p => idOrder.Contains(p.Id)).ToList();
                }
                if (teles != null)
                {
                    entities = entities.Where(p => teles.Contains(p.UserName)).ToList();
                }
                if (levels != null)
                {
                    entities = entities.Where(p => levels.Contains(p.LevelId.ToString())).ToList();
                }
                if (sex != null)
                {
                    if (sex == "1")
                    {
                        entities = entities.Where(p => p.Sex == true).ToList();
                    }
                    else if (sex == "2")
                    {
                        entities = entities.Where(p => p.Sex == false).ToList();
                    }
                }
                if (DateCreated != "")
                {
                    var from = DateCreated.Split(' ')[0];
                    var to = DateCreated.Split(' ')[2];

                    var dateFrom = DateTime.Parse(from.Split('/')[1] + "/" + from.Split('/')[0] + "/" + from.Split('/')[2]);
                    var dateTo = DateTime.Parse(to.Split('/')[1] + "/" + to.Split('/')[0] + "/" + to.Split('/')[2]);

                    entities = entities.Where(p => Convert.ToDateTime(p.DateCreated).Date >= Convert.ToDateTime(dateFrom).Date && Convert.ToDateTime(p.DateCreated).Date <= Convert.ToDateTime(dateTo).Date).ToList();
                    //entities = entities.Where(p => p.DateCreated >= dateFrom && p.DateCreated <= dateTo).ToList();
                }
                if (historyCall != null)
                {
                    var listSchedule = scheduleService.GetSchedules().ToList();
                    foreach (var item in listSchedule)
                    {
                        idSchedule.Add(item.UserDataId);
                    }
                    if (historyCall == "1")
                    {
                        entities = entities.Where(p => idSchedule.Contains(p.Id)).ToList();
                    }
                    else if (historyCall == "2")
                    {
                        entities = entities.Where(p => !idSchedule.Contains(p.Id)).ToList();
                    }
                }
                #endregion
                ViewBag.ShareData = userDataMappingService.GetUserDataMappings().Where(p => p.UserId == null && !p.IsActive).ToList();
                return View("Index", entities);
            }
            return RedirectToAction("TeleIndex", "DataUser", new { id = User.Identity.GetUserId() });
        }
        public ActionResult ReportTeleDashboard()
        {
            ViewBag.Id = User.Identity.GetUserId();
            return View();
        }
        public ActionResult ReportTeleCall()
        {
            ViewBag.Id = User.Identity.GetUserId();
            return View();
        }
        [HttpPost]
        public string GetCharTotalCall(DateTime dateVal)
        {
            GetCookie();
            var UserID = User.Identity.GetUserId();
            var model = new ReportModel();
            model.DateChoose = dateVal;
            var listDiary = new List<Logdiary>();
            var ListData = new List<DataUser>();
            #region get Total Level old
            //model.TotalData = ListData.Count();
            //model.TotalDataL0 = ListData.Where(p => p.LevelId == 9).ToList().Count();
            //model.TotalDataL1 = ListData.Where(p => p.LevelId == 1).ToList().Count();
            //model.TotalDataL2 = ListData.Where(p => p.LevelId == 2).ToList().Count();
            //model.TotalDataL3 = ListData.Where(p => p.LevelId == 3).ToList().Count();
            //model.TotalDataL4 = ListData.Where(p => p.LevelId == 4).ToList().Count();
            //model.TotalDataL5 = ListData.Where(p => p.LevelId == 5).ToList().Count();
            //model.TotalDataL6 = ListData.Where(p => p.LevelId == 6).ToList().Count();
            //model.TotalDataL7 = ListData.Where(p => p.LevelId == 7).ToList().Count();
            //model.TotalDataL8 = ListData.Where(p => p.LevelId == 8).ToList().Count();
            #endregion
            #region get Total Level new by hungnv
            var tempListLevel = new Dictionary<string, double>();
            var listIdDataUser = new List<int>();
            var listIdDataMapping = new List<int>();
            var dataUserMappingList = userDataMappingService.GetUserDataMappings().Where(p => p.UserId == UserID).ToList();
            foreach (var item in dataUserMappingList)
            {
                listIdDataMapping.Add(item.DataUserId);
            }
            if (_BRAND_ID != 0)
            {
                ListData = service.GetDataUsers().Where(p => !p.Deleted && p.BrandId == _BRAND_ID && listIdDataMapping.Contains(p.Id)).ToList();
                foreach (var item in ListData)
                {
                    listIdDataUser.Add(item.Id);
                }
                listDiary = logDiaryService.GetLogdiarys().Where(p => listIdDataUser.Contains(p.UserDataId) && p.DateCreated.Month == dateVal.Month && p.StatusCall != null && p.DataUser.BrandId == _BRAND_ID).ToList();
                tempListLevel = levelService.GetLevels().Where(p => p.BrandId == _BRAND_ID).ToDictionary(l => l.Id + " " + l.CodeLevel + " (" + l.LevelName + ")", t => (double)0);
            }
            else
            {
                ListData = service.GetDataUsers().Where(p => !p.Deleted && listIdDataMapping.Contains(p.Id)).ToList();
                foreach (var item in ListData)
                {
                    listIdDataUser.Add(item.Id);
                }
                listDiary = logDiaryService.GetLogdiarys().Where(p => listIdDataUser.Contains(p.UserDataId) && p.DateCreated.Month == dateVal.Month && p.StatusCall != null).ToList();
                tempListLevel = levelService.GetLevels().ToDictionary(l => l.Id + " " + l.CodeLevel + " (" + l.LevelName + ")", t => (double)0);
            }
            model.TotalData = ListData.Count();
            foreach (var item in tempListLevel)
            {
                try
                {
                    double total = ListData.Where(p => p.Level.CodeLevel == item.Key.Split(' ')[1]).ToList().Count();
                    model.ListTotalEachLevel.Add(item.Key, total);
                }
                catch (Exception ex)
                {
                    throw;
                }
            }

            #endregion
            var dateTime = Convert.ToDateTime(dateVal.ToLongDateString() + " 00:00:00.000");
            if (_BRAND_ID != 0)
            {
                model.TotalCall_Day = listDiary.Where(p => p.DateCreated.Date.Equals(dateVal.Date) && (p.StatusCall.Equals("ANSWERED") || p.StatusCall.Equals("NO ANSWER")) && p.DataUser.BrandId == _BRAND_ID).ToList().Count();
                model.TotalAnswer = (listDiary.Where(p => p.DateCreated.Date.Equals(dateVal.Date) && (p.StatusCall.Equals("ANSWERED")) && p.DataUser.BrandId == _BRAND_ID).ToList().Count());
                model.TotalAnswerMonth = (listDiary.Where(p => p.DateCreated.Month.Equals(dateVal.Month) && (p.StatusCall.Equals("ANSWERED")) && p.DataUser.BrandId == _BRAND_ID).ToList().Count());
                model.TotalAmount = (double)listDiary.Where(p => p.DateCreated >= dateTime && p.DataUser.BrandId == _BRAND_ID).Sum(p => p.Price_1);
            }
            else
            {
                model.TotalCall_Day = listDiary.Where(p => p.DateCreated.Date.Equals(dateVal.Date) && (p.StatusCall.Equals("ANSWERED") || p.StatusCall.Equals("NO ANSWER"))).ToList().Count();
                model.TotalAnswer = (listDiary.Where(p => p.DateCreated.Date.Equals(dateVal.Date) && (p.StatusCall.Equals("ANSWERED"))).ToList().Count());
                model.TotalAnswerMonth = (listDiary.Where(p => p.DateCreated.Month.Equals(dateVal.Month) && (p.StatusCall.Equals("ANSWERED"))).ToList().Count());
                model.TotalAmount = (double)listDiary.Where(p => p.DateCreated >= dateTime).Sum(p => p.Price_1);
            }
            #region TotalCallMonth + total Order month
            int count_Order = 0;
            var currentMonth = dateVal;
            var firstDayOfMonth = new DateTime(currentMonth.Year, currentMonth.Month, 1);
            var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
            var numDay = (lastDayOfMonth.Day - firstDayOfMonth.Day) + 1;
            for (int i = 1; i <= numDay; i++)
            {
                var count_day = 0;
                if (_BRAND_ID != 0)
                {
                    count_day = listDiary.Where(p => p.DateCreated.Date.Equals(firstDayOfMonth.Date) && (p.StatusCall.Equals("ANSWERED") || p.StatusCall.Equals("NO ANSWER")) && p.DataUser.BrandId == _BRAND_ID).ToList().Count();
                    count_Order = listDiary.Where(p => p.DateCreated.Date.Equals(firstDayOfMonth.Date) && p.DataUser.BrandId == _BRAND_ID && p.OrderItems.Any()).ToList().Count();
                }
                else
                {
                    count_day = listDiary.Where(p => p.DateCreated.Date.Equals(firstDayOfMonth.Date) && (p.StatusCall.Equals("ANSWERED") || p.StatusCall.Equals("NO ANSWER"))).ToList().Count();
                    count_Order = listDiary.Where(p => p.DateCreated.Date.Equals(firstDayOfMonth.Date) && p.OrderItems.Any()).ToList().Count();

                }

                if (count_day != 0)
                {
                    ChartModel chart = new ChartModel();
                    chart.Name = firstDayOfMonth.ToString("dd/MM");
                    chart.Value = count_day.ToString("#,##0");
                    model.ChartMonths.Add(chart);
                    model.TotalCall_Month += count_day;
                }
                else
                {
                    ChartModel chart = new ChartModel();
                    chart.Value = "0";
                    chart.Name = firstDayOfMonth.ToString("dd/MM");
                    model.ChartMonths.Add(chart);
                    model.TotalCall_Month += count_day;

                }

                if (count_Order != 0)
                {
                    ChartModel chart = new ChartModel();
                    chart.Name = firstDayOfMonth.ToString("dd/MM");
                    chart.Value = count_Order.ToString("#,##0");
                    model.ChartOrderMonths.Add(chart);
                    model.TotalOrder_Month += count_Order;
                }
                else
                {
                    ChartModel chart = new ChartModel();
                    chart.Value = "0";
                    chart.Name = firstDayOfMonth.ToString("dd/MM");
                    model.ChartOrderMonths.Add(chart);
                    model.TotalOrder_Month += count_Order;
                }
                firstDayOfMonth = firstDayOfMonth.Date.AddDays(1);
            }
            #endregion
            #region Tong hoa don + binh quan hoa don
            listDiary = logDiaryService.GetLogdiarys().Where(p => listIdDataUser.Contains(p.UserDataId) && p.DateCreated.Month == dateTime.Month && p.DateCreated.Year == dateTime.Year && p.StatusCall != null).ToList();
            if (_BRAND_ID != 0)
            {
                model.TotalOrder_Day = listDiary.Where(p => p.DateCreated.Date.ToShortDateString().Equals(dateTime.Date.ToShortDateString()) && p.DataUser.BrandId == _BRAND_ID && p.OrderItems.Any()).Count();
                //model.TotalOrder_Month = listDiary.Where(p => p.DateCreated.Month == dateVal.Month && p.DataUser.BrandId == _BRAND_ID && p.OrderItems.Any()).Count();
                model.TotalAmountMonth = double.Parse(listDiary.Where(p => p.DateCreated.Month == dateVal.Month && p.DataUser.BrandId == _BRAND_ID && p.OrderItems.Any()).Sum(p => p.Price_1).ToString());
                if (model.TotalOrder_Day == 0)
                {
                    model.AvgPaymentOrder_Day = 0;
                }
                else
                {
                    model.AvgPaymentOrder_Day = (listDiary.Where(p => p.DateCreated.Date == dateVal.Date && p.DataUser.BrandId == _BRAND_ID && p.OrderItems.Any()).Sum(p => p.Price_1) / model.TotalOrder_Day);
                }
                if (model.TotalOrder_Month == 0)
                {
                    model.AvgPaymentOrder_Month = 0;
                }
                else
                {
                    model.AvgPaymentOrder_Month = (listDiary.Where(p => p.DateCreated.Month == dateVal.Month && p.DataUser.BrandId == _BRAND_ID && p.OrderItems.Any()).Sum(p => p.Price_1) / model.TotalOrder_Month);
                }
            }
            else
            {
                model.TotalOrder_Day = listDiary.Where(p => p.DateCreated.Date == dateTime.Date && p.OrderItems.Any()).Count();
                //model.TotalOrder_Month = listDiary.Where(p => p.DateCreated.Month == dateVal.Month && p.OrderItems.Any()).Count();
                model.TotalAmountMonth = double.Parse(listDiary.Where(p => p.OrderItems.Any()).Sum(p => p.Price_1).ToString());
                if (model.TotalOrder_Day == 0)
                {
                    model.AvgPaymentOrder_Day = 0;
                }
                else
                {
                    model.AvgPaymentOrder_Day = (listDiary.Where(p => p.DateCreated.Date == dateVal.Date && p.OrderItems.Any()).Sum(p => p.Price_1) / model.TotalOrder_Day);
                }
                if (model.TotalOrder_Month == 0)
                {
                    model.AvgPaymentOrder_Month = 0;
                }
                else
                {
                    model.AvgPaymentOrder_Month = (listDiary.Where(p => p.DateCreated.Month == dateVal.Month && p.OrderItems.Any()).Sum(p => p.Price_1) / model.TotalOrder_Month);
                }

            }
            #endregion
            string output = JsonConvert.SerializeObject(model);
            return output;
        }
        // show data with level
        public string GetCallByDate(string fromDate, string toDate)
        {
            GetCookie();
            var model = new ReportAmount();
            var UserID = User.Identity.GetUserId();
            var listIdDataUserMapping = new List<int>();
            var listIdDataUser = new List<int>();
            var dataUserMappingList = userDataMappingService.GetUserDataMappings().Where(p => p.UserId == UserID).ToList();
            foreach (var item in dataUserMappingList)
            {
                listIdDataUserMapping.Add(item.DataUserId);
            }

            var dataUserList = service.GetDataUsers().Where(p => listIdDataUserMapping.Contains(p.Id)).ToList();
            foreach (var item in dataUserList)
            {
                listIdDataUser.Add(item.Id);
            }
            var dateStart = Convert.ToDateTime(fromDate + " 00:00:00.000");
            //dateStart = dateStart.AddDays();
            var dateEnd = Convert.ToDateTime(toDate + " 00:00:00.000");
            if (dateStart > dateEnd) dateStart = dateEnd;
            var listDiary = logDiaryService.GetLogdiarys().Where(l => l.DataUser.BrandId == _BRAND_ID && listIdDataUser.Contains(l.UserDataId) && l.DateCreated >= dateStart && l.DateCreated <= dateEnd && (l.StatusCall.Equals("ANSWERED") || l.StatusCall.Equals("NO ANSWER")));
            model.totalAmount = (double)listDiary.Where(l => l.DateCreated >= dateStart && l.DateCreated <= dateEnd).Count();
            model.totalCallAnswer = (double)listDiary.Where(l => l.DateCreated >= dateStart && l.DateCreated <= dateEnd && l.StatusCall.Equals("ANSWERED")).Count();
            model.totalCallNoAnswer = (double)listDiary.Where(l => l.DateCreated >= dateStart && l.DateCreated <= dateEnd && !l.StatusCall.Equals("ANSWERED")).Count();

            var dateTime = Convert.ToDateTime(DateTime.Now.ToLongDateString() + " 00:00:00.000");
            #region TotalCallWeek
            // get amount by date
            for (var i = dateStart; i <= dateEnd; i = i.AddDays((double)1))
            {
                var j = i;
                var amount_date = (double)listDiary.Where(p => p.DateCreated >= i && p.DateCreated < j.AddDays(1)).Count();
                ChartModel chart = new ChartModel();
                chart.Name = i.Day + "/" + i.Month;
                chart.Value = "" + (int)amount_date;
                chart.Answer = listDiary.Where(p => p.DateCreated >= i && p.DateCreated < j.AddDays(1) && p.StatusCall.Equals("ANSWERED")).Count().ToString();
                chart.NoAnswer = listDiary.Where(p => p.DateCreated >= i && p.DateCreated < j.AddDays(1) && !p.StatusCall.Equals("ANSWERED")).Count().ToString();
                model.date_amount.Add(chart);
            }
            var listChanel = chanelDataService.GetChanelDatas().Select(lv => new { lv.Code, lv.Id }).Distinct().ToList();
            // get amount by level
            foreach (var item in listChanel)
            {
                var amount_chanel = (double)listDiary.Where(p => p.DataUser.ChannelDataId == item.Id).Count();
                ChartModel chart_level = new ChartModel();
                chart_level.Name = item.Code;
                chart_level.Value = "" + (int)amount_chanel;
                //chart_level.Answer = listDiary.Where(p => p.DataUser.ChannelDataId == item.Id && p.StatusCall.Equals("ANSWERED")).Count().ToString();
                //chart_level.NoAnswer = listDiary.Where(p => p.DataUser.ChannelDataId == item.Id && !p.StatusCall.Equals("ANSWERED")).Count().ToString();
                model.level_amount.Add(chart_level);
            }
            // get call by agent
            var listAgent = _userManager.Users.ToList().Where(p => !p.Deleted && p.Activated).OrderByDescending(p => p.DateCreated).ToList();
            // get amount by level
            foreach (var item in listAgent)
            {
                var amount_agent = (double)listDiary.Where(p => p.TeleId == item.Id).Count();
                ChartModel chart_agent = new ChartModel();
                chart_agent.Name = item.UserName;
                chart_agent.Value = "" + (int)amount_agent;
                //chart_agent.Answer = listDiary.Where(p => p.DateCreated >= i && p.DateCreated < j.AddDays(1) && p.StatusCall.Equals("ANSWERED")).Count().ToString();
                //chart_agent.NoAnswer = listDiary.Where(p => p.DateCreated >= i && p.DateCreated < j.AddDays(1) && !p.StatusCall.Equals("ANSWERED")).Count().ToString();
                model.agent.Add(chart_agent);
            }
            // get call by date all
            //for (var i = dateStart; i <= dateEnd; i = i.AddDays((double)1))
            //{
            //    var j = i;
            //    var call_date = (double)listDiary.Where(p => p.DateCreated >= i && p.DateCreated < j.AddDays(1) && (p.StatusCall.Equals("ANSWERED") || p.StatusCall.Equals("NO ANSWER"))).Count();
            //    ChartModel chartCallAll = new ChartModel();
            //    chartCallAll.Name = i.Day + "/" + i.Month;
            //    chartCallAll.Value = "" + (int)call_date;
            //    model.date_call.Add(chartCallAll);
            //}
            #endregion
            #region TotalCallMonth
            #endregion
            string output = JsonConvert.SerializeObject(model);
            return output;
        }
        public ActionResult DataUserTele(int Id)//level
        {
            //ViewBag.Level = "Level " + level;
            var UserID = User.Identity.GetUserId();
            var listIdDataUser = new List<int>();
            var dataUserList = userDataMappingService.GetUserDataMappings().Where(p => p.UserId == UserID).ToList();
            foreach (var item in dataUserList)
            {
                listIdDataUser.Add(item.DataUserId);
            }
            var entity = levelService.GetLevels().Where(p => p.Id == Id).FirstOrDefault();//.FirstOrDefault(l => l.CodeLevel.ToLower().Equals(level.ToLower().Trim()));
            ViewBag.Level = "Level " + entity.CodeLevel;

            if (entity != null)
            {
                var listData = service.GetDataUsers().Where(d => d.LevelId == entity.Id && listIdDataUser.Contains(d.Id));
                var listIdData = new List<int>();
                foreach (var item in listData)
                {
                    listIdData.Add(item.Id);
                }
                ViewBag.ListDataMapping = userDataMappingService.GetUserDataMappings().Where(p => listIdData.Contains(p.DataUserId)).ToList();


                return View(model: listData);
            }
            return View();
        }
        [HttpPost]
        public ActionResult ShareDateUser(string store, IEnumerable<string> tele, int amount = 0)
        {
            if (tele == null)
            {
                //chia deu
                var listTeles = _userManager.Users.Where(p => p.RoleId == SystemRoles.Role03 && !p.Deleted && p.Activated).ToList();
                var shareData = userDataMappingService.GetUserDataMappings().Where(p => p.UserId == null && !p.IsActive).ToList();
                if (amount != 0)
                    shareData = shareData.Take(amount).ToList();

                var equalData = shareData.Count() / listTeles.Count();
                equalData = equalData > 0 ? equalData : 1;
                if (store != null)
                {
                    //chia cung cua hang
                    foreach (var item in listTeles)
                    {
                        var dataUserMappingByTele = userDataMappingService.GetUserDataMappings().Where(p => p.UserId == item.Id).FirstOrDefault();
                        var dataUserIdByTele = service.GetDataUsers().Where(p => p.Id == dataUserMappingByTele.DataUserId).FirstOrDefault(); //dataUserById.StoreId

                        var shareUserDataMaping = new List<UserDataMapping>();
                        foreach (var itemDataUser in shareData)
                        {
                            var dataUserByShare = service.GetDataUsers().Where(p => p.Id == itemDataUser.DataUserId && p.StoreId == dataUserIdByTele.StoreId).FirstOrDefault();
                            if (dataUserByShare != null)
                            {
                                shareUserDataMaping.Add(itemDataUser);
                            }
                        }
                        equalData = equalData > shareUserDataMaping.Count ? shareUserDataMaping.Count : equalData;
                        for (var i = 0; i < equalData; i++)
                        {
                            shareUserDataMaping[i].UserId = item.Id;
                            userDataMappingService.EditUserDataMapping(shareData[i]);
                        }
                        //bo qua data da dc chia
                        shareData = shareData.Skip(equalData).ToList();
                    }
                }
                else
                {
                    //khac cua hang
                    foreach (var item in listTeles)
                    {
                        for (var i = 0; i < equalData; i++)
                        {
                            shareData[i].UserId = item.Id;
                            userDataMappingService.EditUserDataMapping(shareData[i]);
                        }
                        //bo qua data da dc chia
                        shareData = shareData.Skip(equalData).ToList();
                    }
                }
            }
            else
            {
                var listTeles = _userManager.Users.Where(p => p.RoleId == SystemRoles.Role03 && !p.Deleted && p.Activated && tele.Contains(p.Id)).ToList();
                var shareData = userDataMappingService.GetUserDataMappings().Where(p => p.UserId == null && !p.IsActive).ToList();

                if (amount != 0)
                    shareData = shareData.Take(amount).ToList();

                var equalData = shareData.Count() / listTeles.Count();
                equalData = equalData > 0 ? equalData : 1;
                if (store != null)
                {
                    //chia cung cua hang
                    foreach (var item in listTeles)
                    {
                        var dataUserMappingByTele = userDataMappingService.GetUserDataMappings().Where(p => p.UserId == item.Id).FirstOrDefault();
                        var dataUserIdByTele = service.GetDataUsers().Where(p => p.Id == dataUserMappingByTele.DataUserId).FirstOrDefault(); //dataUserById.StoreId

                        var shareUserDataMaping = new List<UserDataMapping>();
                        foreach (var itemDataUser in shareData)
                        {
                            var dataUserByShare = service.GetDataUsers().Where(p => p.Id == itemDataUser.DataUserId && p.StoreId == dataUserIdByTele.StoreId).FirstOrDefault();
                            if (dataUserByShare != null)
                            {
                                shareUserDataMaping.Add(itemDataUser);
                            }
                        }
                        equalData = equalData > shareUserDataMaping.Count ? shareUserDataMaping.Count : equalData;
                        for (var i = 0; i < equalData; i++)
                        {
                            shareUserDataMaping[i].UserId = item.Id;
                            userDataMappingService.EditUserDataMapping(shareData[i]);
                        }
                        //bo qua data da dc chia
                        shareData = shareData.Skip(equalData).ToList();
                    }
                }
                else
                {
                    //khac cua hang
                    foreach (var item in listTeles)
                    {
                        for (var i = 0; i < equalData; i++)
                        {
                            shareData[i].UserId = item.Id;
                            userDataMappingService.EditUserDataMapping(shareData[i]);
                        }
                        //bo qua data da dc chia
                        shareData = shareData.Skip(equalData).ToList();
                    }
                }
            }
            return RedirectToAction("Index", "DataUser");
        }

        [HttpPost]
        public ActionResult ImportExcelOther(FormCollection formBrand, HttpPostedFileBase excelFile)
        {
            GetCookie();
            ViewBag.Message = "";
            var rootPath = "";
            bool download = false;
            var name = formBrand["dataUserBrand"];
            if (excelFile == null || excelFile.ContentLength == 0)
            {
                ViewBag.Error = "Please enter your file";
                return RedirectToAction("Index", "DataUser");
            }
            else
            {
                if (excelFile.FileName.EndsWith("xls") || excelFile.FileName.EndsWith("xlsx"))
                {
                    var path1 = Server.MapPath("~/DataExcel/");

                    //Excel.Application xlApp = new Excel.Application();
                    //Excel.Workbook xlWorkbook = xlApp.Workbooks.Open(@"importDuplicate.xlsx");
                    //string pathDuplicate1 = @"~\DataExcel\";


                    //import excel
                    var path = Path.Combine(path1, DateTime.Now.Day.ToString() + DateTime.Now.Month.ToString()
                                                    + DateTime.Now.Year.ToString() + excelFile.FileName);
                    if (System.IO.File.Exists(path)) System.IO.File.Delete(path);
                    excelFile.SaveAs(path);
                    var package = new ExcelPackage(new System.IO.FileInfo(path));

                    //export duplicate

                    var pathDuplicate = Path.Combine(path1, DateTime.Now.Day.ToString() + DateTime.Now.Month.ToString()
                                                    + DateTime.Now.Year.ToString() + "importDuplicate.xlsx");
                    rootPath = pathDuplicate;

                    if (System.IO.File.Exists(pathDuplicate)) System.IO.File.Delete(pathDuplicate);
                    System.IO.File.Copy(path1 + "importDuplicate.xlsx", pathDuplicate);
                    //xlWorkbook.SaveAs(pathDuplicate);
                    var packageDuplicate = new ExcelPackage(new System.IO.FileInfo(pathDuplicate));
                    ExcelWorksheet workbookDuplicate = packageDuplicate.Workbook.Worksheets.FirstOrDefault();
                    var rowDuplicate = 2;
                    var colDuplicate = 1;

                    #region read excel
                    ExcelWorksheet workbook = package.Workbook.Worksheets.FirstOrDefault(x => x.Name == "Kedu") ?? package.Workbook.Worksheets.FirstOrDefault();
                    List<DataUserModel> listCustomer = new List<DataUserModel>();
                    //List<DataUsersModel> listCustomerExist = new List<DataUsersModel>();
                    int startRow = 1;
                    int startCol = 1;
                    int totalData = 0;
                    string phone = "";
                    var dem = 0;
                    do
                    {
                        dem++;

                        phone = "";
                        startRow++;
                        //if (workbook.Cells[startRow, startCol + 10].Value == null)
                        //{
                        //    break;
                        //}
                        DataUserModel userModel = new DataUserModel();

                        userModel.LastEditedTime = workbook.Cells[startRow, startCol].Value == null ? (DateTime?)null : DateTime.Parse((workbook.Cells[startRow, startCol].Value).ToString());
                        userModel.MaVe = workbook.Cells[startRow, startCol + 1].Value == null ? "" : (workbook.Cells[startRow, startCol + 1].Value).ToString();
                        userModel.TenChang = workbook.Cells[startRow, startCol + 2].Value == null ? "" : (workbook.Cells[startRow, startCol + 2].Value).ToString();
                        userModel.Chieu = workbook.Cells[startRow, startCol + 3].Value == null ? "" : (workbook.Cells[startRow, startCol + 3].Value).ToString();
                        userModel.HinhThuc = workbook.Cells[startRow, startCol + 4].Value == null ? "" : (workbook.Cells[startRow, startCol + 4].Value).ToString();
                        userModel.LoaiXe = workbook.Cells[startRow, startCol + 5].Value == null ? "" : (workbook.Cells[startRow, startCol + 5].Value).ToString();
                        userModel.FullName = workbook.Cells[startRow, startCol + 6].Value == null ? "" : (workbook.Cells[startRow, startCol + 6].Value).ToString();
                        userModel.CMND = workbook.Cells[startRow, startCol + 7].Value == null ? "" : (workbook.Cells[startRow, startCol + 7].Value).ToString();
                        userModel.Email = workbook.Cells[startRow, startCol + 8].Value == null ? "" : (workbook.Cells[startRow, startCol + 8].Value).ToString();
                        userModel.Address = workbook.Cells[startRow, startCol + 9].Value == null ? "" : (workbook.Cells[startRow, startCol + 9].Value).ToString();
                        userModel.PhoneNumber = workbook.Cells[startRow, startCol + 10].Value == null ? "" : (workbook.Cells[startRow, startCol + 10].Value).ToString();
                        userModel.Phone_NguoiDi = workbook.Cells[startRow, startCol + 11].Value == null ? "" : (workbook.Cells[startRow, startCol + 11].Value).ToString();
                        userModel.TenTaiXe = workbook.Cells[startRow, startCol + 12].Value == null ? "" : (workbook.Cells[startRow, startCol + 12].Value).ToString();
                        userModel.BienSo = workbook.Cells[startRow, startCol + 13].Value == null ? "" : (workbook.Cells[startRow, startCol + 13].Value).ToString();
                        userModel.HT_ThanhToan = workbook.Cells[startRow, startCol + 14].Value == null ? "" : (workbook.Cells[startRow, startCol + 14].Value).ToString();
                        userModel.Price_1 = double.Parse(workbook.Cells[startRow, startCol + 15].Value == null ? "0" : (workbook.Cells[startRow, startCol + 15].Value).ToString());
                        userModel.Noted = workbook.Cells[startRow, startCol + 16].Value == null ? "" : (workbook.Cells[startRow, startCol + 16].Value).ToString();
                        userModel.Partner_Noted = workbook.Cells[startRow, startCol + 17].Value == null ? "" : (workbook.Cells[startRow, startCol + 17].Value).ToString();
                        userModel.Khong_Ghep_Duoc = workbook.Cells[startRow, startCol + 18].Value == null ? false : Convert.ToBoolean(workbook.Cells[startRow, startCol + 18].Value.ToString());
                        userModel.PhanHoi = workbook.Cells[startRow, startCol + 19].Value == null ? "" : (workbook.Cells[startRow, startCol + 19].Value).ToString();
                        userModel.Ten_Mien = workbook.Cells[startRow, startCol + 20].Value == null ? "" : (workbook.Cells[startRow, startCol + 20].Value).ToString();
                        userModel.QuangDuong = workbook.Cells[startRow, startCol + 21].Value == null ? "" : (workbook.Cells[startRow, startCol + 21].Value).ToString();
                        userModel.TietKiem = workbook.Cells[startRow, startCol + 22].Value == null ? "" : (workbook.Cells[startRow, startCol + 22].Value).ToString();
                        userModel.Thoi_Gian_Dat = workbook.Cells[startRow, startCol + 23].Value == null ? (DateTime?)null : DateTime.Parse((workbook.Cells[startRow, startCol + 23].Value).ToString());
                        userModel.Khach_Tu_Dat = workbook.Cells[startRow, startCol + 24].Value == null ? false : Convert.ToBoolean(workbook.Cells[startRow, startCol + 24].Value.ToString());
                        userModel.Doi_Tac_Van_Chuyen = workbook.Cells[startRow, startCol + 25].Value == null ? "" : (workbook.Cells[startRow, startCol + 25].Value).ToString();
                        userModel.District = workbook.Cells[startRow, startCol + 26].Value == null ? "" : (workbook.Cells[startRow, startCol + 26].Value).ToString();
                        userModel.Ma_Giam_Gia = workbook.Cells[startRow, startCol + 27].Value == null ? "" : (workbook.Cells[startRow, startCol + 27].Value).ToString();
                        userModel.Gia_Goc = double.Parse(workbook.Cells[startRow, startCol + 28].Value == null ? "0" : (workbook.Cells[startRow, startCol + 28].Value).ToString());
                        userModel.Ly_Do_Huy = workbook.Cells[startRow, startCol + 29].Value == null ? "" : (workbook.Cells[startRow, startCol + 29].Value).ToString();
                        userModel.Thoi_Gian_Huy = workbook.Cells[startRow, startCol + 30].Value == null ? (DateTime?)null : DateTime.Parse((workbook.Cells[startRow, startCol + 30].Value).ToString());
                        userModel.Hang_Khach_Chon = workbook.Cells[startRow, startCol + 31].Value == null ? "" : (workbook.Cells[startRow, startCol + 31].Value).ToString();
                        userModel.Ma_Ve_Doi_Tac = workbook.Cells[startRow, startCol + 32].Value == null ? "" : (workbook.Cells[startRow, startCol + 32].Value).ToString();
                        userModel.Tinh_Trang = workbook.Cells[startRow, startCol + 33].Value == null ? "" : (workbook.Cells[startRow, startCol + 33].Value).ToString();
                        userModel.Position = int.Parse(workbook.Cells[startRow, startCol + 34].Value == null ? "0" : (workbook.Cells[startRow, startCol + 34].Value).ToString());
                        userModel.UserName = workbook.Cells[startRow, startCol + 35].Value == null ? "" : (workbook.Cells[startRow, startCol + 35].Value).ToString();


                        userModel.DateCreated = DateTime.Now;
                        userModel.IsActive = false;
                        userModel.Deleted = false;
                        userModel.Brand_Name = _BRAND_NAME;
                        try
                        {
                            //MaVe = workbook.Cells[startRow, startCol + 34].Value == null ? "" : (workbook.Cells[startRow, startCol].Value).ToString(),

                            //StoreTemp = workbook.Cells[startRow, startCol].Value == null ? "" : (workbook.Cells[startRow, startCol].Value).ToString(),
                            //FormatNumberVotes = workbook.Cells[startRow, startCol + 1].Value == null ? "" : (workbook.Cells[startRow, startCol + 1].Value).ToString(),
                            //FullName = workbook.Cells[startRow, startCol + 2].Value == null ? "" : (workbook.Cells[startRow, startCol + 2].Value).ToString(),
                            //PhoneNumber = workbook.Cells[startRow, startCol + 3].Value == null ? "" : (workbook.Cells[startRow, startCol + 3].Value).ToString(),
                            //Address = (workbook.Cells[startRow, startCol + 4].Value == null ? "" : (workbook.Cells[startRow, startCol + 4].Value).ToString()) + ", " + (workbook.Cells[startRow, startCol + 5].Value == null ? "" : (workbook.Cells[startRow, startCol + 5].Value).ToString()),
                            //District = workbook.Cells[startRow, startCol + 4].Value == null ? "" : (workbook.Cells[startRow, startCol + 4].Value).ToString(),
                            //City = workbook.Cells[startRow, startCol + 5].Value == null ? "" : (workbook.Cells[startRow, startCol + 5].Value).ToString(),
                            //Sex = workbook.Cells[startRow, startCol + 6].Value == null ? true : (workbook.Cells[startRow, startCol + 6].Value).ToString().Equals("Nam") ? true : false,
                            //UserName = workbook.Cells[startRow, startCol + 7].Value == null ? "" : (workbook.Cells[startRow, startCol + 7].Value).ToString(),

                            listCustomer.Add(userModel);
                            phone = userModel.PhoneNumber;
                        }
                        catch (Exception ex)
                        {
                            log.Fatal(ex.Message);

                            #region export excel duplicate or ex
                            workbookDuplicate.Cells[rowDuplicate, colDuplicate].Value = workbook.Cells[startRow, startCol].Value;
                            workbookDuplicate.Cells[rowDuplicate, colDuplicate + 1].Value = workbook.Cells[startRow, startCol + 1].Value;
                            workbookDuplicate.Cells[rowDuplicate, colDuplicate + 2].Value = workbook.Cells[startRow, startCol + 2].Value;
                            workbookDuplicate.Cells[rowDuplicate, colDuplicate + 3].Value = workbook.Cells[startRow, startCol + 3].Value;
                            workbookDuplicate.Cells[rowDuplicate, colDuplicate + 4].Value = workbook.Cells[startRow, startCol + 4].Value;
                            workbookDuplicate.Cells[rowDuplicate, colDuplicate + 5].Value = workbook.Cells[startRow, startCol + 5].Value;
                            workbookDuplicate.Cells[rowDuplicate, colDuplicate + 6].Value = workbook.Cells[startRow, startCol + 6].Value;
                            workbookDuplicate.Cells[rowDuplicate, colDuplicate + 7].Value = workbook.Cells[startRow, startCol + 7].Value;
                            workbookDuplicate.Cells[rowDuplicate, colDuplicate + 8].Value = workbook.Cells[startRow, startCol + 8].Value;
                            workbookDuplicate.Cells[rowDuplicate, colDuplicate + 9].Value = workbook.Cells[startRow, startCol + 9].Value;
                            workbookDuplicate.Cells[rowDuplicate, colDuplicate + 10].Value = workbook.Cells[startRow, startCol + 10].Value;
                            workbookDuplicate.Cells[rowDuplicate, colDuplicate + 11].Value = workbook.Cells[startRow, startCol + 11].Value;
                            workbookDuplicate.Cells[rowDuplicate, colDuplicate + 12].Value = workbook.Cells[startRow, startCol + 12].Value;
                            workbookDuplicate.Cells[rowDuplicate, colDuplicate + 13].Value = workbook.Cells[startRow, startCol + 13].Value;
                            workbookDuplicate.Cells[rowDuplicate, colDuplicate + 14].Value = workbook.Cells[startRow, startCol + 14].Value;
                            workbookDuplicate.Cells[rowDuplicate, colDuplicate + 15].Value = workbook.Cells[startRow, startCol + 15].Value;
                            workbookDuplicate.Cells[rowDuplicate, colDuplicate + 16].Value = workbook.Cells[startRow, startCol + 16].Value;
                            workbookDuplicate.Cells[rowDuplicate, colDuplicate + 17].Value = workbook.Cells[startRow, startCol + 17].Value;
                            workbookDuplicate.Cells[rowDuplicate, colDuplicate + 18].Value = workbook.Cells[startRow, startCol + 18].Value;
                            workbookDuplicate.Cells[rowDuplicate, colDuplicate + 19].Value = workbook.Cells[startRow, startCol + 19].Value;
                            workbookDuplicate.Cells[rowDuplicate, colDuplicate + 20].Value = workbook.Cells[startRow, startCol + 20].Value;
                            workbookDuplicate.Cells[rowDuplicate, colDuplicate + 21].Value = workbook.Cells[startRow, startCol + 21].Value;
                            workbookDuplicate.Cells[rowDuplicate, colDuplicate + 22].Value = workbook.Cells[startRow, startCol + 22].Value;
                            workbookDuplicate.Cells[rowDuplicate, colDuplicate + 23].Value = workbook.Cells[startRow, startCol + 23].Value;
                            workbookDuplicate.Cells[rowDuplicate, colDuplicate + 24].Value = workbook.Cells[startRow, startCol + 24].Value;
                            workbookDuplicate.Cells[rowDuplicate, colDuplicate + 25].Value = workbook.Cells[startRow, startCol + 25].Value;
                            workbookDuplicate.Cells[rowDuplicate, colDuplicate + 26].Value = workbook.Cells[startRow, startCol + 26].Value;
                            workbookDuplicate.Cells[rowDuplicate, colDuplicate + 27].Value = workbook.Cells[startRow, startCol + 27].Value;
                            workbookDuplicate.Cells[rowDuplicate, colDuplicate + 28].Value = workbook.Cells[startRow, startCol + 28].Value;
                            workbookDuplicate.Cells[rowDuplicate, colDuplicate + 29].Value = workbook.Cells[startRow, startCol + 29].Value;
                            workbookDuplicate.Cells[rowDuplicate, colDuplicate + 30].Value = workbook.Cells[startRow, startCol + 30].Value;
                            workbookDuplicate.Cells[rowDuplicate, colDuplicate + 31].Value = workbook.Cells[startRow, startCol + 31].Value;
                            workbookDuplicate.Cells[rowDuplicate, colDuplicate + 32].Value = workbook.Cells[startRow, startCol + 32].Value;
                            workbookDuplicate.Cells[rowDuplicate, colDuplicate + 33].Value = workbook.Cells[startRow, startCol + 33].Value;
                            workbookDuplicate.Cells[rowDuplicate, colDuplicate + 34].Value = workbook.Cells[startRow, startCol + 34].Value;
                            workbookDuplicate.Cells[rowDuplicate, colDuplicate + 35].Value = workbook.Cells[startRow, startCol + 35].Value;
                            phone = (workbook.Cells[startRow, startCol + 10].Value).ToString();
                            rowDuplicate++;
                            #endregion
                            download = true;
                            continue;
                            //throw ex;
                            //ViewBag.Error = "Có lỗi xảy ra, vui lòng thử lại";
                            //return RedirectToAction("Index", "DataUser");
                        }
                    } while (phone != "");
                    #endregion


                    #region create data
                    totalData = listCustomer.Count;
                    int countUser = 0;
                    for (var i = 0; i <= listCustomer.Count - 1; i++)
                    {
                        //try
                        //{
                        DataUser user = new DataUser();
                        UserDataMapping userMapping = new UserDataMapping();
                        if (listCustomer[i].PhoneNumber != "" && listCustomer[i].Brand_Name != "")
                        {

                            listCustomer[i].PhoneNumber = listCustomer[i].PhoneNumber[0] != '0' ? "0" + listCustomer[i].PhoneNumber : listCustomer[i].PhoneNumber;
                            var checkPhone = Regex.Match(listCustomer[i].PhoneNumber, @"^([0-9]{10})$").Success;
                            user.PhoneNumber = checkPhone ? listCustomer[i].PhoneNumber : "";

                            user.FirstName = listCustomer[i].FullName;
                            user.Address = listCustomer[i].Address;
                            user.District = listCustomer[i].District;
                            //user.City = listSell[i].City;
                            //user.DayOfBith = DateTime.Parse(listSell[i].DateOfBirth);
                            user.DayOfBith = DateTime.Now;
                            //user.Sex = listCustomer[i].Sex;

                            user.ChannelDataId = 2;
                            user.LevelId = 1;
                            user.DateCreated = DateTime.Now;
                            user.IsActive = true;
                            user.Deleted = false;

                            user.LastEditedTime = listCustomer[i].LastEditedTime;
                            user.MaVe = listCustomer[i].MaVe;
                            user.TenChang = listCustomer[i].TenChang;
                            user.Chieu = listCustomer[i].Chieu;
                            user.HinhThuc = listCustomer[i].HinhThuc;
                            user.LoaiXe = listCustomer[i].LoaiXe;

                            user.CMND = listCustomer[i].CMND;
                            user.Email = listCustomer[i].Email;
                            user.PhoneNumber = listCustomer[i].PhoneNumber;
                            user.Phone_NguoiDi = listCustomer[i].Phone_NguoiDi;
                            user.BienSo = listCustomer[i].BienSo;
                            user.HT_ThanhToan = listCustomer[i].HT_ThanhToan;
                            user.Price_1 = listCustomer[i].Price_1==null?0: listCustomer[i].Price_1;
                            user.Noted = listCustomer[i].Noted;
                            user.Partner_Noted = listCustomer[i].Partner_Noted;
                            user.Khong_Ghep_Duoc = listCustomer[i].Khong_Ghep_Duoc;
                            user.PhanHoi = listCustomer[i].PhanHoi;
                            user.Ten_Mien = listCustomer[i].Ten_Mien;
                            user.QuangDuong = listCustomer[i].QuangDuong;
                            user.TietKiem = listCustomer[i].TietKiem;
                            user.Khach_Tu_Dat = listCustomer[i].Khach_Tu_Dat;
                            user.Doi_Tac_Van_Chuyen = listCustomer[i].Doi_Tac_Van_Chuyen;
                            user.Ma_Giam_Gia = listCustomer[i].Ma_Giam_Gia;
                            user.Gia_Goc = listCustomer[i].Gia_Goc;
                            user.Thoi_Gian_Huy = listCustomer[i].Thoi_Gian_Huy;
                            user.Hang_Khach_Chon = listCustomer[i].Hang_Khach_Chon;
                            user.Ma_Ve_Doi_Tac = listCustomer[i].Ma_Ve_Doi_Tac;
                            user.Tinh_Trang = listCustomer[i].Tinh_Trang;
                            user.Position = listCustomer[i].Position;
                            user.UserName = listCustomer[i].UserName;


                            //user.UserName = listCustomer[i].UserName;
                            user.Brand_Name = listCustomer[i].Brand_Name;



                            var entity = service.GetDataUsers().Where(p => p.PhoneNumber.Equals(user.PhoneNumber)).FirstOrDefault();
                            var brand = brandService.GetBrands().Where(p => p.Name.Equals(user.Brand_Name)).FirstOrDefault();
                            if (entity == null)
                            {
                                user.BrandId = _BRAND_ID;
                                user.Brand_Code = _BRAND_CODE;
                                service.CreateDataUser(user);
                                var teleNew = _userManager.FindByName(listCustomer[i].UserName);//Tìm theo tên userName
                                                                                                //var teleList = _userManager.Users.ToList().Count;
                                                                                                //shareTele = totalData / teleList + 1;

                                if (teleNew != null)
                                {
                                    // add to mapping
                                    //var data = service.GetDataUsers().Where(p => p.PhoneNumber.Equals(user.PhoneNumber)).FirstOrDefault();
                                    UserDataMapping userDataMapping = new UserDataMapping()
                                    {
                                        UserId = teleNew.Id,//null,//
                                        DateCreated = DateTime.Now,
                                        DataUserId = user.Id,
                                        IsActive = true,
                                        Deleted = false,
                                        Position = 0,
                                        LastEditedTime = DateTime.Now,
                                    };
                                    userDataMappingService.CreateUserDataMapping(userDataMapping);
                                    countUser++;
                                    ViewBag.Message = "Import thành công!";
                                }

                            }
                            else
                            {
                                if (entity.PhoneNumber == user.PhoneNumber && brand.Name == _BRAND_NAME)
                                {
                                    //DataUserModel userExistModel = new DataUserModel();

                                    //listCustomerExist.Add(listCustomer[i]);
                                    #region export excel duplicate or exception
                                    workbookDuplicate.Cells[rowDuplicate, colDuplicate].Value = listCustomer[i].LastEditedTime;
                                    workbookDuplicate.Cells[rowDuplicate, colDuplicate + 1].Value = listCustomer[i].MaVe;
                                    workbookDuplicate.Cells[rowDuplicate, colDuplicate + 2].Value = listCustomer[i].TenChang;
                                    workbookDuplicate.Cells[rowDuplicate, colDuplicate + 3].Value = listCustomer[i].Chieu;
                                    workbookDuplicate.Cells[rowDuplicate, colDuplicate + 4].Value = listCustomer[i].HinhThuc;
                                    workbookDuplicate.Cells[rowDuplicate, colDuplicate + 5].Value = listCustomer[i].LoaiXe;
                                    workbookDuplicate.Cells[rowDuplicate, colDuplicate + 6].Value = listCustomer[i].FullName;
                                    workbookDuplicate.Cells[rowDuplicate, colDuplicate + 7].Value = listCustomer[i].CMND;
                                    workbookDuplicate.Cells[rowDuplicate, colDuplicate + 8].Value = listCustomer[i].Email;
                                    workbookDuplicate.Cells[rowDuplicate, colDuplicate + 9].Value = listCustomer[i].Address;
                                    workbookDuplicate.Cells[rowDuplicate, colDuplicate + 10].Value = listCustomer[i].PhoneNumber;
                                    workbookDuplicate.Cells[rowDuplicate, colDuplicate + 11].Value = listCustomer[i].Phone_NguoiDi;
                                    workbookDuplicate.Cells[rowDuplicate, colDuplicate + 12].Value = listCustomer[i].TenTaiXe;
                                    workbookDuplicate.Cells[rowDuplicate, colDuplicate + 13].Value = listCustomer[i].BienSo;
                                    workbookDuplicate.Cells[rowDuplicate, colDuplicate + 14].Value = listCustomer[i].HT_ThanhToan;
                                    workbookDuplicate.Cells[rowDuplicate, colDuplicate + 15].Value = listCustomer[i].Price_1;
                                    workbookDuplicate.Cells[rowDuplicate, colDuplicate + 16].Value = listCustomer[i].Noted;
                                    workbookDuplicate.Cells[rowDuplicate, colDuplicate + 17].Value = listCustomer[i].Partner_Noted;
                                    workbookDuplicate.Cells[rowDuplicate, colDuplicate + 18].Value = listCustomer[i].Khong_Ghep_Duoc;
                                    workbookDuplicate.Cells[rowDuplicate, colDuplicate + 19].Value = listCustomer[i].PhanHoi;
                                    workbookDuplicate.Cells[rowDuplicate, colDuplicate + 20].Value = listCustomer[i].Ten_Mien;
                                    workbookDuplicate.Cells[rowDuplicate, colDuplicate + 21].Value = listCustomer[i].QuangDuong;
                                    workbookDuplicate.Cells[rowDuplicate, colDuplicate + 22].Value = listCustomer[i].TietKiem;
                                    workbookDuplicate.Cells[rowDuplicate, colDuplicate + 23].Value = listCustomer[i].Thoi_Gian_Dat;
                                    workbookDuplicate.Cells[rowDuplicate, colDuplicate + 24].Value = listCustomer[i].Khach_Tu_Dat;
                                    workbookDuplicate.Cells[rowDuplicate, colDuplicate + 25].Value = listCustomer[i].Doi_Tac_Van_Chuyen;
                                    workbookDuplicate.Cells[rowDuplicate, colDuplicate + 26].Value = listCustomer[i].District;
                                    workbookDuplicate.Cells[rowDuplicate, colDuplicate + 27].Value = listCustomer[i].Ma_Giam_Gia;
                                    workbookDuplicate.Cells[rowDuplicate, colDuplicate + 28].Value = listCustomer[i].Price_1;
                                    workbookDuplicate.Cells[rowDuplicate, colDuplicate + 29].Value = listCustomer[i].Ly_Do_Huy;
                                    workbookDuplicate.Cells[rowDuplicate, colDuplicate + 30].Value = listCustomer[i].Thoi_Gian_Huy;
                                    workbookDuplicate.Cells[rowDuplicate, colDuplicate + 31].Value = listCustomer[i].Hang_Khach_Chon;
                                    workbookDuplicate.Cells[rowDuplicate, colDuplicate + 32].Value = listCustomer[i].Ma_Ve_Doi_Tac;
                                    workbookDuplicate.Cells[rowDuplicate, colDuplicate + 33].Value = listCustomer[i].Tinh_Trang;
                                    workbookDuplicate.Cells[rowDuplicate, colDuplicate + 34].Value = listCustomer[i].Position;
                                    workbookDuplicate.Cells[rowDuplicate, colDuplicate + 35].Value = listCustomer[i].UserName;

                                    rowDuplicate++;
                                    #endregion
                                    download = true;
                                    continue;
                                }
                                user.Brand_Name = _BRAND_NAME;
                                user.BrandId = _BRAND_ID;
                                user.Brand_Code = _BRAND_CODE;
                                service.CreateDataUser(user);
                                //var tele = _userManager.FindByName("agent008");
                                var tele = _userManager.FindByName(listCustomer[i].UserName);//Tìm theo tên userName
                                                                                             //var teleList = _userManager.Users.ToList().Count;
                                                                                             //shareTele = totalData / teleList + 1;

                                if (tele != null)
                                {
                                    // add to mapping
                                    //var data = service.GetDataUsers().Where(p => p.PhoneNumber.Equals(user.PhoneNumber)).FirstOrDefault();
                                    UserDataMapping userDataMapping = new UserDataMapping()
                                    {
                                        UserId = tele.Id,//null,// 
                                        DateCreated = DateTime.Now,
                                        DataUserId = user.Id,
                                        IsActive = true,
                                        Deleted = false,
                                        Position = 0,
                                        LastEditedTime = DateTime.Now,
                                    };
                                    userDataMappingService.CreateUserDataMapping(userDataMapping);
                                    countUser++;
                                    ViewBag.Message = "Import thành công!";
                                }
                            }
                        }

                        //}
                        //catch (Exception ex)
                        //{
                        //    log.Fatal(ex.Message);
                        //    #region export excel duplicate
                        //    workbookDuplicate.Cells[rowDuplicate, colDuplicate].Value = listCustomer[i].LastEditedTime;
                        //    workbookDuplicate.Cells[rowDuplicate, colDuplicate + 1].Value = listCustomer[i].MaVe;
                        //    workbookDuplicate.Cells[rowDuplicate, colDuplicate + 2].Value = listCustomer[i].TenChang;
                        //    workbookDuplicate.Cells[rowDuplicate, colDuplicate + 3].Value = listCustomer[i].Chieu;
                        //    workbookDuplicate.Cells[rowDuplicate, colDuplicate + 4].Value = listCustomer[i].HinhThuc;
                        //    workbookDuplicate.Cells[rowDuplicate, colDuplicate + 5].Value = listCustomer[i].LoaiXe;
                        //    workbookDuplicate.Cells[rowDuplicate, colDuplicate + 6].Value = listCustomer[i].FullName;
                        //    workbookDuplicate.Cells[rowDuplicate, colDuplicate + 7].Value = listCustomer[i].CMND;
                        //    workbookDuplicate.Cells[rowDuplicate, colDuplicate + 8].Value = listCustomer[i].Email;
                        //    workbookDuplicate.Cells[rowDuplicate, colDuplicate + 9].Value = listCustomer[i].Address;
                        //    workbookDuplicate.Cells[rowDuplicate, colDuplicate + 10].Value = listCustomer[i].PhoneNumber;
                        //    workbookDuplicate.Cells[rowDuplicate, colDuplicate + 11].Value = listCustomer[i].Phone_NguoiDi;
                        //    workbookDuplicate.Cells[rowDuplicate, colDuplicate + 12].Value = listCustomer[i].TenTaiXe;
                        //    workbookDuplicate.Cells[rowDuplicate, colDuplicate + 13].Value = listCustomer[i].BienSo;
                        //    workbookDuplicate.Cells[rowDuplicate, colDuplicate + 14].Value = listCustomer[i].HT_ThanhToan;
                        //    workbookDuplicate.Cells[rowDuplicate, colDuplicate + 15].Value = listCustomer[i].Price_1;
                        //    workbookDuplicate.Cells[rowDuplicate, colDuplicate + 16].Value = listCustomer[i].Noted;
                        //    workbookDuplicate.Cells[rowDuplicate, colDuplicate + 17].Value = listCustomer[i].Partner_Noted;
                        //    workbookDuplicate.Cells[rowDuplicate, colDuplicate + 18].Value = listCustomer[i].Khong_Ghep_Duoc;
                        //    workbookDuplicate.Cells[rowDuplicate, colDuplicate + 19].Value = listCustomer[i].PhanHoi;
                        //    workbookDuplicate.Cells[rowDuplicate, colDuplicate + 20].Value = listCustomer[i].Ten_Mien;
                        //    workbookDuplicate.Cells[rowDuplicate, colDuplicate + 21].Value = listCustomer[i].QuangDuong;
                        //    workbookDuplicate.Cells[rowDuplicate, colDuplicate + 22].Value = listCustomer[i].TietKiem;
                        //    workbookDuplicate.Cells[rowDuplicate, colDuplicate + 23].Value = listCustomer[i].Thoi_Gian_Dat;
                        //    workbookDuplicate.Cells[rowDuplicate, colDuplicate + 24].Value = listCustomer[i].Khach_Tu_Dat;
                        //    workbookDuplicate.Cells[rowDuplicate, colDuplicate + 25].Value = listCustomer[i].Doi_Tac_Van_Chuyen;
                        //    workbookDuplicate.Cells[rowDuplicate, colDuplicate + 26].Value = listCustomer[i].District;
                        //    workbookDuplicate.Cells[rowDuplicate, colDuplicate + 27].Value = listCustomer[i].Ma_Giam_Gia;
                        //    workbookDuplicate.Cells[rowDuplicate, colDuplicate + 28].Value = listCustomer[i].Price_1;
                        //    workbookDuplicate.Cells[rowDuplicate, colDuplicate + 29].Value = listCustomer[i].Ly_Do_Huy;
                        //    workbookDuplicate.Cells[rowDuplicate, colDuplicate + 30].Value = listCustomer[i].Thoi_Gian_Huy;
                        //    workbookDuplicate.Cells[rowDuplicate, colDuplicate + 31].Value = listCustomer[i].Hang_Khach_Chon;
                        //    workbookDuplicate.Cells[rowDuplicate, colDuplicate + 32].Value = listCustomer[i].Ma_Ve_Doi_Tac;
                        //    workbookDuplicate.Cells[rowDuplicate, colDuplicate + 33].Value = listCustomer[i].Tinh_Trang;
                        //    workbookDuplicate.Cells[rowDuplicate, colDuplicate + 34].Value = listCustomer[i].Position;
                        //    workbookDuplicate.Cells[rowDuplicate, colDuplicate + 35].Value = listCustomer[i].UserName;

                        //    rowDuplicate++;
                        //    #endregion
                        //    download = true;
                        //    continue;
                        //}





                    }
                    #endregion
                    packageDuplicate.Save();
                    SetAlert("Import thành công!" + countUser + "/" + totalData, "success");
                    //ViewBag.LinkDownLoad = "lilotech-crm-admin.lilotech.com/DataExcel/" + pathDuplicate;
                    ViewBag.LinkDownLoad = "http://localhost:53981/Admin/DataUser/DataExcel/" + pathDuplicate;
                    if (download)
                    {
                        string names = Path.GetFileName(rootPath); //get file name
                        Response.AppendHeader("content-disposition", "attachment; filename=" + names);
                        Response.ContentType = "application/vnd.ms-excel";//"application/ms-excel";
                        Response.WriteFile(rootPath);

                        Response.End(); //give POP to user for file downlaod
                    }
                }
            }
            return RedirectToAction("Index", "DataUser");
        }
        [HttpPost]
        public ActionResult ImportFileExcel(HttpPostedFileBase excelFile)
        {
            if (excelFile == null || excelFile.ContentLength == 0)
            {
                ViewBag.Error = "Please enter your file";
                return RedirectToAction("Index", "DataUser");
            }
            else
            {
                if (excelFile.FileName.EndsWith("xls") || excelFile.FileName.EndsWith("xlsx"))
                {
                    //var path = Server.MapPath("~/DataExcel/" + DateTime.Now.Day.ToString() + DateTime.Now.Month.ToString()
                    //                                + DateTime.Now.Year.ToString() + excelFile.FileName);
                    var path1 = Server.MapPath("~/DataExcel/");
                    var path = Path.Combine(path1, DateTime.Now.Day.ToString() + DateTime.Now.Month.ToString()
                                                    + DateTime.Now.Year.ToString() + excelFile.FileName);
                    if (System.IO.File.Exists(path)) System.IO.File.Delete(path);
                    excelFile.SaveAs(path);
                    var package = new ExcelPackage(new System.IO.FileInfo(path));
                    if (package.Workbook.Worksheets.Count >= 1)
                    {
                        #region Reading Excel

                        ExcelWorksheet workbook = package.Workbook.Worksheets.FirstOrDefault(x => x.Name == "BanHang") ?? package.Workbook.Worksheets.FirstOrDefault();
                        ExcelWorksheet workbookService = package.Workbook.Worksheets.Where(x => x.Name == "DichVu").FirstOrDefault() ?? package.Workbook.Worksheets.FirstOrDefault();
                        List<DataUserModel> listProducts = new List<DataUserModel>();
                        List<DataUserModel> listService = new List<DataUserModel>();
                        List<DataUsersModel> listSell = new List<DataUsersModel>();
                        int startRow = 1;
                        int startCol = 1;
                        int shareTele = 0;
                        int totalData = 0;
                        string phone = "";
                        string phoneSell = "";

                        #region read sheet dich vu
                        do
                        {
                            phone = "";
                            startRow++;
                            try
                            {
                                // get data from excel
                                DataUserModel userModel = new DataUserModel()
                                {
                                    StoreTemp = workbookService.Cells[startRow, startCol].Value == null ? "" : (workbookService.Cells[startRow, startCol].Value).ToString(),
                                    FormatNumberVotes = workbookService.Cells[startRow, startCol + 1].Value == null ? "" : (workbookService.Cells[startRow, startCol + 1].Value).ToString(),
                                    FullName = workbookService.Cells[startRow, startCol + 2].Value == null ? "" : (workbookService.Cells[startRow, startCol + 2].Value).ToString(),
                                    PhoneNumber = workbookService.Cells[startRow, startCol + 3].Value == null ? "" : (workbookService.Cells[startRow, startCol + 3].Value).ToString(),
                                    Address = (workbookService.Cells[startRow, startCol + 4].Value == null ? "" : (workbookService.Cells[startRow, startCol + 4].Value).ToString()) + ", " + (workbookService.Cells[startRow, startCol + 5].Value == null ? "" : (workbookService.Cells[startRow, startCol + 5].Value).ToString()),
                                    District = workbookService.Cells[startRow, startCol + 4].Value == null ? "" : (workbookService.Cells[startRow, startCol + 4].Value).ToString(),
                                    City = workbookService.Cells[startRow, startCol + 5].Value == null ? "" : (workbookService.Cells[startRow, startCol + 5].Value).ToString(),
                                    Sex = workbookService.Cells[startRow, startCol + 6].Value == null ? true : (workbookService.Cells[startRow, startCol + 6].Value).ToString().Equals("Nam") ? true : false,
                                    //Model = workbookService.Cells[startRow, startCol + 7].Value == null ? "" : (workbookService.Cells[startRow, startCol + 7].Value).ToString(),
                                    //NumberOfKilometer = workbookService.Cells[startRow, startCol + 8].Value == null ? "" : (workbookService.Cells[startRow, startCol + 8].Value).ToString(),
                                    //LicensePlate = workbookService.Cells[startRow, startCol + 9].Value == null ? "" : (workbookService.Cells[startRow, startCol + 9].Value).ToString(),
                                    //MaterialPrice = workbookService.Cells[startRow, startCol + 10].Value == null ? "0" : (workbookService.Cells[startRow, startCol + 10].Value).ToString(),
                                    //Fee = workbookService.Cells[startRow, startCol + 11].Value == null ? "0" : (workbookService.Cells[startRow, startCol + 11].Value).ToString(),
                                    //TotalMoney = workbookService.Cells[startRow, startCol + 12].Value == null ? "0" : (workbookService.Cells[startRow, startCol + 12].Value).ToString(),
                                    //Repairer = workbookService.Cells[startRow, startCol + 13].Value == null ? "" : (workbookService.Cells[startRow, startCol + 13].Value).ToString(),
                                    //Checker = workbookService.Cells[startRow, startCol + 14].Value == null ? "" : (workbookService.Cells[startRow, startCol + 14].Value).ToString(),
                                    //Cashier = workbookService.Cells[startRow, startCol + 15].Value == null ? "" : (workbookService.Cells[startRow, startCol + 15].Value).ToString(),
                                    //Reciever = workbookService.Cells[startRow, startCol + 16].Value == null ? "" : (workbookService.Cells[startRow, startCol + 16].Value).ToString(),
                                    //CustomerRequest = workbookService.Cells[startRow, startCol + 17].Value == null ? "" : (workbookService.Cells[startRow, startCol + 17].Value).ToString(),
                                    DateCreated = DateTime.Now,
                                    IsActive = false,
                                    Deleted = false,

                                };
                                listService.Add(userModel);

                                //DataUser user = new DataUser();

                                //String chanel = "0";
                                //String level = "0";
                                ////String chanel = workbook.Cells[startRow, startCol + 10].Value == null ? "0" : (workbook.Cells[startRow, startCol + 10].Value).ToString();
                                ////String level = workbook.Cells[startRow, startCol + 11].Value == null ? "0" : (workbook.Cells[startRow, startCol + 11].Value).ToString();
                                ////String campaign = workbook.Cells[startRow, startCol + 16].Value == null ? "" : (workbook.Cells[startRow, startCol + 15].Value).ToString();

                                //user.ChannelDataId = int.Parse(chanel.ToString());
                                //user.LevelId = int.Parse(level.ToString());
                                phone = userModel.PhoneNumber;


                                //if ((phone != "") && (user.UserName != ""))
                                //{
                                //    // add to db
                                //    user.PhoneNumber = "0" + user.PhoneNumber;
                                //    var entity = service.GetDataUsers().Where(p => p.PhoneNumber.Equals(user.PhoneNumber)).FirstOrDefault();
                                //    if (entity == null)
                                //    {
                                //        service.CreateDataUser(user);
                                //        var tele = _userManager.FindByName(user.UserName);
                                //        if (tele != null)
                                //        {
                                //            // add to mapping
                                //            var data = service.GetDataUsers().Where(p => p.PhoneNumber.Equals(user.PhoneNumber)).FirstOrDefault();
                                //            UserDataMapping userDataMapping = new UserDataMapping()
                                //            {
                                //                UserId = tele.Id,
                                //                DateCreated = DateTime.Now,
                                //                DataUserId = data.Id,
                                //                IsActive = true,
                                //                Deleted = false,
                                //                Position = 0,
                                //                LastEditedTime = DateTime.Now,
                                //            };
                                //            userDataMappingService.CreateUserDataMapping(userDataMapping);
                                //        }
                                //    }
                                //}
                            }
                            catch (Exception ex)
                            {
                                ViewBag.Error = "Có lỗi xảy ra, vui lòng thử lại";
                                //return RedirectToAction("Index", "DataUser");
                            }
                        } while (phone != "");
                        #endregion

                        #region read sheet ban hang

                        startRow = 1;
                        startCol = 1;
                        do
                        {
                            phoneSell = "";
                            startRow++;
                            try
                            {
                                // get data from excel
                                DataUsersModel userModelSell = new DataUsersModel()
                                {
                                    StoreTemp = workbook.Cells[startRow, startCol].Value == null ? "" : (workbook.Cells[startRow, startCol].Value).ToString(),
                                    DateBuy = workbook.Cells[startRow, startCol + 1].Value == null ? "" : (workbook.Cells[startRow, startCol + 1].Value).ToString(),
                                    FullName = workbook.Cells[startRow, startCol + 2].Value == null ? "" : (workbook.Cells[startRow, startCol + 2].Value).ToString(),
                                    PhoneNumber = workbook.Cells[startRow, startCol + 3].Value == null ? "" : (workbook.Cells[startRow, startCol + 3].Value).ToString(),
                                    Address = workbook.Cells[startRow, startCol + 4].Value == null ? "" : (workbook.Cells[startRow, startCol + 4].Value).ToString(),
                                    Ward = workbook.Cells[startRow, startCol + 5].Value == null ? "" : (workbook.Cells[startRow, startCol + 5].Value).ToString(),
                                    District = workbook.Cells[startRow, startCol + 6].Value == null ? "" : (workbook.Cells[startRow, startCol + 6].Value).ToString(),
                                    City = workbook.Cells[startRow, startCol + 7].Value == null ? "" : (workbook.Cells[startRow, startCol + 7].Value).ToString(),
                                    DateOfBirth = workbook.Cells[startRow, startCol + 8].Value == null ? "" : (workbook.Cells[startRow, startCol + 8].Value).ToString(),
                                    Sex = workbook.Cells[startRow, startCol + 9].Value == null ? true : (workbook.Cells[startRow, startCol + 9].Value).ToString().Equals("Nam") ? true : false,
                                    Jobs = workbook.Cells[startRow, startCol + 10].Value == null ? "" : (workbook.Cells[startRow, startCol + 10].Value).ToString(),
                                    Model = workbook.Cells[startRow, startCol + 11].Value == null ? "" : (workbook.Cells[startRow, startCol + 11].Value).ToString(),
                                    Color = workbook.Cells[startRow, startCol + 12].Value == null ? "" : (workbook.Cells[startRow, startCol + 12].Value).ToString(),
                                    FrameNumber = workbook.Cells[startRow, startCol + 13].Value == null ? "" : (workbook.Cells[startRow, startCol + 13].Value).ToString(),
                                    MachineNumber = workbook.Cells[startRow, startCol + 14].Value == null ? "" : (workbook.Cells[startRow, startCol + 14].Value).ToString(),
                                    SaleStaff = workbook.Cells[startRow, startCol + 15].Value == null ? "" : (workbook.Cells[startRow, startCol + 15].Value).ToString(),
                                    SuggestedPrice = workbook.Cells[startRow, startCol + 16].Value == null ? "" : (workbook.Cells[startRow, startCol + 16].Value).ToString(),
                                    Price = workbook.Cells[startRow, startCol + 17].Value == null ? "" : (workbook.Cells[startRow, startCol + 17].Value).ToString(),
                                    TotalMoney = workbook.Cells[startRow, startCol + 18].Value == null ? "" : (workbook.Cells[startRow, startCol + 18].Value).ToString(),
                                    PricePlate = workbook.Cells[startRow, startCol + 19].Value == null ? "" : (workbook.Cells[startRow, startCol + 19].Value).ToString(),
                                    SaleOff = workbook.Cells[startRow, startCol + 20].Value == null ? "" : (workbook.Cells[startRow, startCol + 20].Value).ToString(),
                                    OtherPrice = workbook.Cells[startRow, startCol + 21].Value == null ? "" : (workbook.Cells[startRow, startCol + 21].Value).ToString(),
                                    NeedPay = workbook.Cells[startRow, startCol + 22].Value == null ? "" : (workbook.Cells[startRow, startCol + 22].Value).ToString(),
                                    TotalPay = workbook.Cells[startRow, startCol + 23].Value == null ? "" : (workbook.Cells[startRow, startCol + 23].Value).ToString(),
                                    Note = workbook.Cells[startRow, startCol + 24].Value == null ? "" : (workbook.Cells[startRow, startCol + 24].Value).ToString(),
                                    DateCreated = DateTime.Now,
                                    IsActive = false,
                                    Deleted = false,
                                };
                                listSell.Add(userModelSell);

                                //DataUser user = new DataUser();
                                //String chanel = "0";
                                //String level = "0";
                                ////String chanel = workbook.Cells[startRow, startCol + 10].Value == null ? "0" : (workbook.Cells[startRow, startCol + 10].Value).ToString();
                                ////String level = workbook.Cells[startRow, startCol + 11].Value == null ? "0" : (workbook.Cells[startRow, startCol + 11].Value).ToString();
                                ////String campaign = workbook.Cells[startRow, startCol + 16].Value == null ? "" : (workbook.Cells[startRow, startCol + 15].Value).ToString();

                                //user.ChannelDataId = int.Parse(chanel.ToString());
                                //user.LevelId = int.Parse(level.ToString());
                                phoneSell = userModelSell.PhoneNumber;

                                //if ((phoneService != "") && (user.UserName != ""))
                                //{
                                //    // add to db
                                //    user.PhoneNumber = "0" + user.PhoneNumber;
                                //    var entity = service.GetDataUsers().Where(p => p.PhoneNumber.Equals(user.PhoneNumber)).FirstOrDefault();
                                //    if (entity == null)
                                //    {
                                //        service.CreateDataUser(user);
                                //        var tele = _userManager.FindByName(user.UserName);
                                //        if (tele != null)
                                //        {
                                //            // add to mapping
                                //            var data = service.GetDataUsers().Where(p => p.PhoneNumber.Equals(user.PhoneNumber)).FirstOrDefault();
                                //            UserDataMapping userDataMapping = new UserDataMapping()
                                //            {
                                //                UserId = tele.Id,
                                //                DateCreated = DateTime.Now,
                                //                DataUserId = data.Id,
                                //                IsActive = true,
                                //                Deleted = false,
                                //                Position = 0,
                                //                LastEditedTime = DateTime.Now,
                                //            };
                                //            userDataMappingService.CreateUserDataMapping(userDataMapping);
                                //        }
                                //    }
                                //}
                            }
                            catch (Exception ex)
                            {
                                ViewBag.Error = "Có lỗi xảy ra, vui lòng thử lại";
                                //return RedirectToAction("Index", "DataUser");
                            }
                        } while (phoneSell != "");
                        #endregion

                        #region Create Data

                        totalData = listSell.Count + listService.Count;
                        //Create Ban hang
                        for (var i = 0; i <= listSell.Count - 1; i++)
                        {
                            DataUser user = new DataUser();
                            Order oder = new Order();
                            Product product = new Product();
                            Store store = new Store();
                            Brand brand = new Brand();

                            if ((listSell[i].PhoneNumber != ""))
                            {
                                listSell[i].PhoneNumber = listSell[i].PhoneNumber[0] != '0' ? "0" + listSell[i].PhoneNumber : listSell[i].PhoneNumber;
                                var checkPhone = Regex.Match(listSell[i].PhoneNumber, @"^([0-9]{10})$").Success;
                                user.PhoneNumber = checkPhone ? listSell[i].PhoneNumber : "";

                                user.FirstName = listSell[i].FullName;
                                user.Address = listSell[i].Address;
                                user.Ward = listSell[i].Ward;
                                user.District = listSell[i].District;
                                //user.City = listSell[i].City;
                                //user.DayOfBith = DateTime.Parse(listSell[i].DateOfBirth);
                                user.DayOfBith = DateTime.Now;
                                user.Sex = listSell[i].Sex;
                                user.temp1 = listSell[i].Jobs;

                                user.ChannelDataId = 2;
                                user.LevelId = 1;
                                user.DateCreated = DateTime.Now;
                                user.IsActive = true;
                                user.Deleted = false;

                                // add to db
                                var entity = service.GetDataUsers().Where(p => p.PhoneNumber.Equals(user.PhoneNumber)).FirstOrDefault();
                                if (entity == null)
                                {
                                    service.CreateDataUser(user);
                                    var tele = _userManager.FindByName("vuonghtt");
                                    //var teleList = _userManager.Users.ToList().Count;
                                    //shareTele = totalData / teleList + 1;
                                    if (tele != null)
                                    {
                                        // add to mapping
                                        var data = service.GetDataUsers().Where(p => p.PhoneNumber.Equals(user.PhoneNumber)).FirstOrDefault();
                                        UserDataMapping userDataMapping = new UserDataMapping()
                                        {
                                            UserId = tele.Id,//null,// 
                                            DateCreated = DateTime.Now,
                                            DataUserId = data.Id,
                                            IsActive = true,
                                            Deleted = false,
                                            Position = 0,
                                            LastEditedTime = DateTime.Now,
                                        };

                                        var dataUserMappingItem = userDataMappingService.CreateUserDataMappingReturnItem(userDataMapping);

                                        //add store
                                        //var dataBrand = brandService.GetBrandById(5);
                                        store.BrandId = 5;// 5 honda hoang viet
                                        store.IsActive = true;
                                        store.Store_Code = listSell[i].StoreTemp;

                                        var recordStore = storeService.GetStores().Where(x => x.Store_Code == store.Store_Code).FirstOrDefault();
                                        if (recordStore == null)
                                        {
                                            var dataStore = storeService.CreateStoreReturnId(store);
                                            oder.StoreId = dataStore.Id;
                                        }
                                        else
                                        {
                                            oder.StoreId = recordStore.Id;
                                        }

                                        //add product
                                        product.Model = listSell[i].Model;
                                        product.Color = listSell[i].Color;
                                        //product.PlateNumber = listSell[i];
                                        product.Price = int.Parse(listSell[i].Price);
                                        product.DateCreated = product.LastEditedTime = DateTime.Now;
                                        product.ProductCategoryId = 3;//add tam mot category
                                        product.IsHomePage = false;
                                        product.IsPublic = true;
                                        product.Deleted = false;
                                        product.Position = 0;

                                        var itemProduct = productService.GetProducts().Where(x => x.Model == product.Model && x.Color == product.Color).FirstOrDefault();
                                        if (itemProduct == null)
                                        {
                                            var dataProduct = productService.CreateProductReturnItem(product);
                                            oder.ProductId = dataProduct.Id;
                                        }
                                        else
                                        {
                                            oder.ProductId = itemProduct.Id;
                                        }

                                        //add order
                                        //var dataMapping = userDataMappingService.GetUserDataMappingById(userDataMapping.Id);
                                        var dateBuy = DateTime.Parse(listSell[i].DateBuy);
                                        oder.UserDataMappingId = dataUserMappingItem.Id;
                                        oder.CreateDate = DateTime.Now;
                                        oder.OrderDate = DateTime.Now;
                                        oder.Staff = listSell[i].SaleStaff;
                                        oder.MaterialNumber = listSell[i].MachineNumber;
                                        oder.TotalPrice = float.Parse(listSell[i].TotalMoney);
                                        oder.PlatePrice = float.Parse(listSell[i].PricePlate);
                                        oder.Discount = float.Parse(listSell[i].SaleOff);
                                        oder.OtherPrice = float.Parse(listSell[i].OtherPrice);
                                        oder.Debt = float.Parse(listSell[i].NeedPay);
                                        //oder.OrderTotal = float.Parse(listSell[i].TotalPay);
                                        oder.Note = listSell[i].Note;
                                        oder.Type = 2; //2 sell - 1 dich vu

                                        var orderItem = orderService.CreateOrderReturnItem(oder);

                                    }
                                }
                            }
                        }

                        //Create dich vu
                        for (var i = 0; i <= listService.Count - 1; i++)
                        {
                            DataUser user = new DataUser();
                            Order oder = new Order();
                            Product product = new Product();
                            Store store = new Store();
                            Brand brand = new Brand();

                            //if (i == 1500)
                            //{
                            //    var t = 1;
                            //}

                            if ((listService[i].PhoneNumber != ""))
                            {
                                listService[i].PhoneNumber = listService[i].PhoneNumber[0] != '0' ? "0" + listService[i].PhoneNumber : listService[i].PhoneNumber;
                                var checkPhone = Regex.Match(listService[i].PhoneNumber, @"^([0-9]{10})$").Success;
                                user.PhoneNumber = checkPhone ? listService[i].PhoneNumber : "";
                                user.DayOfBith = DateTime.Now;
                                user.FirstName = listService[i].FullName;
                                user.Address = listService[i].Address;
                                user.District = listService[i].District;
                                //user.City = listService[i].City;
                                user.Sex = listService[i].Sex;

                                user.ChannelDataId = 8;
                                user.LevelId = 1;
                                user.DateCreated = DateTime.Now;
                                user.IsActive = true;
                                user.Deleted = false;

                                // add to db
                                var entity = service.GetDataUsers().Where(p => p.PhoneNumber.Equals(user.PhoneNumber)).FirstOrDefault();
                                if (entity == null)
                                {
                                    service.CreateDataUser(user);
                                    var tele = _userManager.FindByName("vuonghtt");
                                    if (tele != null)
                                    {
                                        // add to mapping
                                        var data = service.GetDataUsers().Where(p => p.PhoneNumber.Equals(user.PhoneNumber)).FirstOrDefault();
                                        UserDataMapping userDataMapping = new UserDataMapping()
                                        {
                                            UserId = tele.Id,//null,// 
                                            DateCreated = DateTime.Now,
                                            DataUserId = data.Id,
                                            IsActive = true,
                                            Deleted = false,
                                            Position = 0,
                                            LastEditedTime = DateTime.Now,
                                        };
                                        var dataUserMappingItem = userDataMappingService.CreateUserDataMappingReturnItem(userDataMapping);


                                        //add store
                                        //var dataBrand = brandService.GetBrandById(5);
                                        store.BrandId = 5;// 5 honda hoang viet
                                        store.IsActive = true;
                                        store.Store_Code = listService[i].StoreTemp;

                                        var recordStore = storeService.GetStores().Where(x => x.Store_Code == store.Store_Code).FirstOrDefault();
                                        if (recordStore == null)
                                        {
                                            var dataStore = storeService.CreateStoreReturnId(store);
                                            oder.StoreId = dataStore.Id;
                                        }
                                        else
                                        {
                                            oder.StoreId = recordStore.Id;
                                        }

                                        //add product
                                        product.Model = listService[i].Model;
                                        //product.NumberOfKilometer = listService[i].NumberOfKilometer;

                                        //product.MachineNumber = listService[i].MachineNumber;
                                        //product.LicensePlate = listService[i].LicensePlate;
                                        product.Price = int.Parse(listService[i].TotalMoney);

                                        product.DateCreated = product.LastEditedTime = DateTime.Now;
                                        product.ProductCategoryId = 3;//add tam mot category
                                        product.IsHomePage = false;
                                        product.IsPublic = true;
                                        product.Deleted = false;
                                        product.Position = 0;

                                        var itemProduct = productService.GetProducts().Where(x => x.Model == product.Model && x.Color == product.Color).FirstOrDefault();
                                        if (itemProduct == null)
                                        {
                                            var dataProduct = productService.CreateProductReturnItem(product);
                                            oder.ProductId = dataProduct.Id;
                                        }
                                        else
                                        {
                                            oder.ProductId = itemProduct.Id;
                                        }

                                        //add order
                                        //var dataMapping = userDataMappingService.GetUserDataMappingById(userDataMapping.Id);
                                        oder.UserDataMappingId = dataUserMappingItem.Id;
                                        //oder.Order_Code = listService[i].FormatNumberVotes;

                                        oder.CreateDate = DateTime.Now;
                                        oder.OrderDate = DateTime.Now;

                                        oder.PlateNumber = listService[i].LicensePlate;
                                        oder.MaterialPrice = float.Parse(listService[i].MaterialPrice);
                                        oder.SupportPrice = float.Parse(listService[i].Fee);
                                        oder.TotalPrice = float.Parse(listService[i].TotalMoney);
                                        oder.Repairer = listService[i].Repairer;
                                        oder.Checker = listService[i].Checker;
                                        oder.Casher = listService[i].Cashier;
                                        oder.Staff = listService[i].Reciever;
                                        oder.Note = listService[i].CustomerRequest;
                                        oder.Type = 1; //2 sell - 1 dich vu

                                        var orderItem = orderService.CreateOrderReturnItem(oder);

                                    }
                                }
                            }
                        }
                        #endregion

                        ViewBag.ListProducts = listProducts;
                        #endregion

                    }
                    return RedirectToAction("Index", "DataUser");
                }
            }
            SetAlert("File không đúng! Vui lòng chọn file excel", "danger");
            //ViewBag.Error = "File is incorrect! Please select a excel file";
            return RedirectToAction("Index", "DataUser");

        }

        [HttpPost]
        public ActionResult Import(HttpPostedFileBase excelFile)
        {
            if (excelFile == null || excelFile.ContentLength == 0)
            {
                ViewBag.Error = "Please enter your file";
                return RedirectToAction("Index", "DataUser");
            }
            else
            {
                if (excelFile.FileName.EndsWith("xls") || excelFile.FileName.EndsWith("xlsx"))
                {
                    //var path = Server.MapPath("~/DataExcel/" + DateTime.Now.Day.ToString() + DateTime.Now.Month.ToString()
                    //                                + DateTime.Now.Year.ToString() + excelFile.FileName);
                    var path1 = Server.MapPath("~/DataExcel/");
                    var path = Path.Combine(path1, DateTime.Now.Day.ToString() + DateTime.Now.Month.ToString()
                                                    + DateTime.Now.Year.ToString() + excelFile.FileName);
                    if (System.IO.File.Exists(path)) System.IO.File.Delete(path);
                    excelFile.SaveAs(path);
                    var package = new ExcelPackage(new System.IO.FileInfo(path));
                    if (package.Workbook.Worksheets.Count >= 1)
                    {
                        #region Reading Excel
                        ExcelWorksheet workbook = package.Workbook.Worksheets.FirstOrDefault(x => x.Name == "DataUser") ?? package.Workbook.Worksheets.FirstOrDefault();
                        List<DataUser> listProducts = new List<DataUser>();
                        int startRow = 1;
                        int startCol = 1;
                        string phone = "";
                        do
                        {
                            phone = "";
                            startRow++;
                            try
                            {
                                // get data from excel
                                DataUser user = new DataUser()
                                {
                                    CodeData = workbook.Cells[startRow, startCol].Value == null ? "" : (workbook.Cells[startRow, startCol].Value).ToString(),
                                    FirstName = workbook.Cells[startRow, startCol + 1].Value == null ? "" : (workbook.Cells[startRow, startCol + 1].Value).ToString(),
                                    LastName = workbook.Cells[startRow, startCol + 2].Value == null ? "" : (workbook.Cells[startRow, startCol + 2].Value).ToString(),
                                    DayOfBith = workbook.Cells[startRow, startCol + 3].Value == null ? DateTime.Now : DateTime.FromOADate((double)workbook.Cells[startRow, startCol + 3].Value),
                                    PhoneNumber = workbook.Cells[startRow, startCol + 4].Value == null ? "" : (workbook.Cells[startRow, startCol + 4].Value).ToString(),
                                    Address = workbook.Cells[startRow, startCol + 5].Value == null ? "" : (workbook.Cells[startRow, startCol + 5].Value).ToString(),
                                    Email = workbook.Cells[startRow, startCol + 6].Value == null ? "" : (workbook.Cells[startRow, startCol + 6].Value).ToString(),
                                    Facebook = workbook.Cells[startRow, startCol + 7].Value == null ? "" : (workbook.Cells[startRow, startCol + 7].Value).ToString(),
                                    Zalo = workbook.Cells[startRow, startCol + 8].Value == null ? "" : (workbook.Cells[startRow, startCol + 8].Value).ToString(),
                                    Sex = workbook.Cells[startRow, startCol + 9].Value == null ? true : (workbook.Cells[startRow, startCol + 9].Value).ToString().Equals("Male") ? true : false,
                                    UserName = workbook.Cells[startRow, startCol + 12].Value == null ? "" : (workbook.Cells[startRow, startCol + 12].Value).ToString(),
                                    Content = workbook.Cells[startRow, startCol + 13].Value == null ? "" : (workbook.Cells[startRow, startCol + 13].Value).ToString(),
                                    Noted = workbook.Cells[startRow, startCol + 14].Value == null ? "" : (workbook.Cells[startRow, startCol + 14].Value).ToString(),
                                    DateCreated = DateTime.Now,
                                    TotalCallAgains = 0,
                                    TotalCallRefuse = 0,
                                    TotalCallSucess = 0,
                                    TotalInteractive = 0,
                                    TotalNotCall = 0,
                                    TotalOrder = 0,
                                    IsActive = true,
                                    Deleted = false,
                                    StoreId = 1, // add tạm vô đã

                                };
                                String chanel = "0";
                                String level = "0";
                                //String chanel = workbook.Cells[startRow, startCol + 10].Value == null ? "0" : (workbook.Cells[startRow, startCol + 10].Value).ToString();
                                //String level = workbook.Cells[startRow, startCol + 11].Value == null ? "0" : (workbook.Cells[startRow, startCol + 11].Value).ToString();
                                //String campaign = workbook.Cells[startRow, startCol + 16].Value == null ? "" : (workbook.Cells[startRow, startCol + 15].Value).ToString();

                                user.ChannelDataId = int.Parse(chanel.ToString());
                                user.LevelId = int.Parse(level.ToString());
                                phone = user.PhoneNumber;

                                if ((phone != "") && (user.UserName != ""))
                                {
                                    // add to db
                                    user.PhoneNumber = "0" + user.PhoneNumber;
                                    var entity = service.GetDataUsers().Where(p => p.PhoneNumber.Equals(user.PhoneNumber)).FirstOrDefault();
                                    if (entity == null)
                                    {
                                        service.CreateDataUser(user);
                                        var tele = _userManager.FindByName(user.UserName);
                                        if (tele != null)
                                        {
                                            // add to mapping
                                            var data = service.GetDataUsers().Where(p => p.PhoneNumber.Equals(user.PhoneNumber)).FirstOrDefault();
                                            UserDataMapping userDataMapping = new UserDataMapping()
                                            {

                                                UserId = tele.Id,
                                                DateCreated = DateTime.Now,
                                                DataUserId = data.Id,
                                                IsActive = true,
                                                Deleted = false,
                                                Position = 0,
                                                LastEditedTime = DateTime.Now,

                                            };
                                            userDataMappingService.CreateUserDataMapping(userDataMapping);
                                        }
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                ViewBag.Error = "Có lỗi xảy ra, vui lòng thử lại";
                                //return RedirectToAction("Index", "DataUser");
                            }
                        } while (phone != "");

                        ViewBag.ListProducts = listProducts;
                        #endregion

                    }
                    return RedirectToAction("Index", "DataUser");
                }
            }
            ViewBag.Error = "File is incorrect! Please select a excel file";
            return RedirectToAction("Index", "DataUser");

        }

        public ActionResult TeleIndex(string id)
        {
            ViewBag.ChannelList = chanelDataService.GetChanelDatas().ToSelectListItems(-1);
            ViewBag.LevelList = levelService.GetLevels().ToSelectListItems(-1);

            #region lay cookie
            GetCookie();
            #endregion
            // de tam la 100 da
            int numDataIndate = 100;
            var UserId = User.Identity.GetUserId();
            ViewBag.TeleId = UserId;
            string listUserDataMapping = "";
            List<UserDataMapping> listDataNotCall = null;
            var dateTime = Convert.ToDateTime(DateTime.Now.ToLongDateString() + " 00:00:00.000");
            var countDataInDate = userDataMappingService.GetUserDataMappings().Where(p => p.UserId.Equals(UserId) && p.DataUser.LastCall >= dateTime).Count();
            if (countDataInDate < numDataIndate)
            {
                //listDataNotCall = userDataMappingService.GetUserDataMappings().Where(p =>p.DataUser.BrandId==_BRAND_ID && p.UserId.Equals(UserId) && (p.DataUser.LastCall == null)).Take(numDataIndate - countDataInDate).ToList();
                listDataNotCall = userDataMappingService.GetUserDataMappings().Where(p => p.DataUser.BrandId == _BRAND_ID && p.UserId.Equals(UserId)).OrderByDescending(p => p.Id).ToList();
                if (listDataNotCall.Any())
                {
                    listUserDataMapping += listDataNotCall.ElementAt(0).Id;
                    for (int i = 1; i < listDataNotCall.Count(); i++)
                    {
                        listUserDataMapping += ";" + listDataNotCall.ElementAt(i).Id;
                    }
                }
                else
                {
                    listUserDataMapping = "";
                }
                // add cookie for next data
                ViewBag.ListId = listUserDataMapping;
                Session[UserId] = listUserDataMapping;
            }
            return View(model: listDataNotCall);
        }
        [HttpPost]
        public ActionResult TeleIndex(string id, IEnumerable<int> stores, IEnumerable<int> wards, IEnumerable<int> districts, IEnumerable<string> levels, string type, string DateCreated, string historyCall, string sex)
        {
            // de tam la 100 da
            int numDataIndate = 100;
            var UserId = User.Identity.GetUserId();
            ViewBag.TeleId = UserId;
            string listUserDataMapping = "";
            List<UserDataMapping> listDataNotCall = null;
            var dateTime = Convert.ToDateTime(DateTime.Now.ToLongDateString() + " 00:00:00.000");
            var countDataInDate = userDataMappingService.GetUserDataMappings().Where(p => p.UserId.Equals(UserId) && p.DataUser.LastCall >= dateTime).Count();
            if (countDataInDate < numDataIndate)
            {
                listDataNotCall = userDataMappingService.GetUserDataMappings().Where(p => p.UserId.Equals(UserId)).OrderByDescending(p => p.Id).ToList();//&& (p.DataUser.LastCall == null).Take(numDataIndate - countDataInDate)

                #region filter
                var idStoreByDistrict = new List<int>();
                var idStoreByWard = new List<int>();
                var idStore = new List<int>();
                var idOrder = new List<int>();
                var idSchedule = new List<int>();

                if (districts != null)
                {
                    var listStoreByDistrict = districtService.GetDistricts().Where(p => districts.Contains(p.Id)).ToList();
                    foreach (var item in listStoreByDistrict)
                    {
                        idStoreByDistrict.Add(item.Id);
                    }
                }
                if (wards != null)
                {
                    var listStoreByWard = wardService.GetWards().Where(p => wards.Contains(p.Id)).ToList();
                    foreach (var item in listStoreByWard)
                    {
                        idStoreByWard.Add(item.Id);
                    }
                }
                if (stores != null)
                {
                    var listStore = storeService.GetStores().Where(p => stores.Contains(p.Id)).ToList();
                    foreach (var item in listStore)
                    {
                        idStore.Add(item.Id);
                    }
                }
                if (idStoreByDistrict.Count > 0)
                {
                    var listStore = storeService.GetStores().Where(p => idStoreByDistrict.Contains(p.Id)).ToList();
                    foreach (var item in listStore)
                    {
                        idStore.Add(item.Id);
                    }
                }
                if (idStoreByWard.Count > 0)
                {
                    var listStore = storeService.GetStores().Where(p => idStoreByWard.Contains(p.Id)).ToList();
                    foreach (var item in listStore)
                    {
                        idStore.Add(item.Id);
                    }
                }

                var listOrder = orderService.GetOrders().Where(p => idStore.Contains(p.Id)).ToList();
                if (type != null)
                {
                    if (type == "1")
                    {
                        listOrder = listOrder.Where(p => p.Type == 2).ToList();//2 la ban hang
                    }
                    else if (type == "2")
                    {
                        listOrder = listOrder.Where(p => p.Type == 1).ToList();//1 la dich vu
                    }
                }
                foreach (var item in listOrder)
                {
                    idOrder.Add(item.UserDataMappingId);
                }
                #endregion

                var entities = service.GetDataUsers().ToList();
                var idData = new List<int>();

                #region filter where
                if (stores != null || wards != null || districts != null)
                {
                    entities = service.GetDataUsers().Where(p => idOrder.Contains(p.Id)).ToList();
                }
                if (levels != null)
                {
                    entities = entities.Where(p => levels.Contains(p.LevelId.ToString())).ToList();
                }
                if (sex != null)
                {
                    if (sex == "1")
                    {
                        entities = entities.Where(p => p.Sex == true).ToList();
                    }
                    else if (sex == "2")
                    {
                        entities = entities.Where(p => p.Sex == false).ToList();
                    }
                }
                if (DateCreated != "")
                {
                    var from = DateCreated.Split(' ')[0];
                    var to = DateCreated.Split(' ')[2];
                    var dateFrom = DateTime.Parse(from.Split('/')[1] + "/" + from.Split('/')[0] + "/" + from.Split('/')[2]);
                    var dateTo = DateTime.Parse(to.Split('/')[1] + "/" + to.Split('/')[0] + "/" + to.Split('/')[2]);

                    entities = entities.Where(p => Convert.ToDateTime(p.DateCreated).Date >= Convert.ToDateTime(dateFrom).Date && Convert.ToDateTime(p.DateCreated).Date <= Convert.ToDateTime(dateTo).Date).ToList();
                }
                if (historyCall != null)
                {
                    var listSchedule = scheduleService.GetSchedules().ToList();
                    foreach (var item in listSchedule)
                    {
                        idSchedule.Add(item.UserDataId);
                    }
                    if (historyCall == "1")
                    {
                        entities = entities.Where(p => idSchedule.Contains(p.Id)).ToList();
                    }
                    else if (historyCall == "2")
                    {
                        entities = entities.Where(p => !idSchedule.Contains(p.Id)).ToList();
                    }
                }
                #endregion

                foreach (var item in entities)
                {
                    idData.Add(item.Id);
                }
                if (idData.Count > 0)
                {
                    listDataNotCall = listDataNotCall.Where(p => idData.Contains(p.DataUserId)).ToList();
                }
                if (listDataNotCall.Count > 0)
                {
                    listUserDataMapping += listDataNotCall.ElementAt(0).Id;
                }
                for (int i = 1; i < listDataNotCall.Count(); i++)
                {
                    listUserDataMapping += ";" + listDataNotCall.ElementAt(i).Id;
                }
                // add cookie for next data
                ViewBag.ListId = listUserDataMapping;
                Session[UserId] = listUserDataMapping;
            }
            return View(model: listDataNotCall);
        }
        #region Danh sách hóa đơn theo ngày cho tele
        public ActionResult OrderTeleDay(string lastDate, string teleId)
        {
            GetCookie();
            var from = lastDate.Split(' ')[0];
            var dateFrom = DateTime.Parse(from.Split('/')[2] + "/" + from.Split('/')[1] + "/" + from.Split('/')[0]);
            //var TeleId = teleId;
            var day = dateFrom.Day;
            //var listDiary = new List<Logdiary>();
            //var listUser = service.GetDataUsers().Where(p => p.BrandId == _BRAND_ID);
            //var model = logDiaryService.GetLogdiarys().Where(p => p.DateCreated.Date.Equals(dateFrom.Date) && p.StatusCall.Equals("BUY PRODUCT") && p.IsActive == true && p.Deleted != true && p.DataUser.BrandId == _BRAND_ID && p.OrderItems.Any()).ToList();
            //listDiary = logDiaryService.GetLogdiarys().Where(p => p.DateCreated.Month == dateFrom.Month && p.DateCreated.Year == dateFrom.Year && p.StatusCall != null).ToList();
            //var userId = service.GetDataUserByBrandId(_BRAND_ID);
            var model = logDiaryService.GetLogdiarys().Where(p => p.StatusCall.Equals("BUY PRODUCT") && p.DataUser.BrandId == _BRAND_ID && p.DateCreated.Date.Equals(dateFrom.Date) && p.IsActive == true && p.Deleted != true && p.TeleId == teleId && p.OrderItems.Any()).ToList();

            return View(model);
            //return View();
        }
        #endregion
        #region Danh sách hóa đơn theo tháng cho tele
        public ActionResult OrderTeleMonth(string lastDate, string teleId)
        {
            GetCookie();
            var from = lastDate.Split(' ')[0];
            var dateFrom = DateTime.Parse(from.Split('/')[2] + "/" + from.Split('/')[1] + "/" + from.Split('/')[0]);
            //var TeleId = teleId;
            var day = dateFrom.Month;
            //var listDiary = new List<Logdiary>();
            //var listUser = service.GetDataUsers().Where(p => p.BrandId == _BRAND_ID);
            //var model = logDiaryService.GetLogdiarys().Where(p => p.DateCreated.Date.Equals(dateFrom.Date) && p.StatusCall.Equals("BUY PRODUCT") && p.IsActive == true && p.Deleted != true && p.DataUser.BrandId == _BRAND_ID && p.OrderItems.Any()).ToList();
            //listDiary = logDiaryService.GetLogdiarys().Where(p => p.DateCreated.Month == dateFrom.Month && p.DateCreated.Year == dateFrom.Year && p.StatusCall != null).ToList();
            //var userId = service.GetDataUserByBrandId(_BRAND_ID);
            var model = logDiaryService.GetLogdiarys().Where(p => p.StatusCall.Equals("BUY PRODUCT") && p.DataUser.BrandId == _BRAND_ID && p.DateCreated.Date.Month.Equals(dateFrom.Month) && p.IsActive == true && p.Deleted != true && p.TeleId == teleId && p.OrderItems.Any()).ToList();

            return View(model);
            //return View();
        }
        #endregion
        public ActionResult Edit(int id, string teleName = "")
        {
            #region GetBrandid Cookie
            int? BrandID = 0;
            //string BrandName = "";
            HttpCookie brandCookie = Request.Cookies["BrandId"];
            var entity = userDataMappingService.GetUserDataMappings().Where(p => p.DataUser.Id.Equals(id)).FirstOrDefault();
            UserDataMappingFormModel userDataMappingFormModel = Mapper.Map<UserDataMapping, UserDataMappingFormModel>(entity);
            if (brandCookie != null)
            {
                BrandID = int.Parse(brandCookie.Value);
                //BrandName = brandCookie.Value;
                //userDataMappingFormModel.ListLevel = levelService.GetLevels().ToSelectListItems(entity.DataUser.LevelId);
                userDataMappingFormModel.ListLevel = levelService.GetLevels().Where(p => p.BrandId == BrandID).ToSelectListItems(entity.DataUser.LevelId);
            }
            else
            {
                userDataMappingFormModel.ListLevel = levelService.GetLevels().ToSelectListItems(entity.DataUser.LevelId);
                BrandID = 0;
            }
            #endregion
            ViewBag.TeleId = User.Identity.GetUserId();
            ViewBag.TeleName = User.Identity.Name;
            userDataMappingFormModel.ListChanel = chanelDataService.GetChanelDatas().ToSelectListItems(-1);
            //userDataMappingFormModel.ListLevel = levelService.GetLevels().ToSelectListItems(entity.DataUser.LevelId);
            userDataMappingFormModel.liQuestion = _questionService.GetQuestions().ToList();
            //  userDataMappingFormModel.ListLogDiary = logDiaryService.GetLogdiarys().Where(p => p.UserDataId == userDataMappingFormModel.DataUserId).ToList();
            return View(model: userDataMappingFormModel);
        }
        [HttpPost]
        public ActionResult SubmitAnswer(FormCollection liQuestion, int Id)//này nhận bằng string or dùn model cũng đc
        {

            //var id = liQuestion.Get("liQuestion[""]");
            Survey sur = new Survey();
            var dataUser = new DataUser();
            string noteSurvey1 = liQuestion["noteSurvey1"];
            string noteSurvey2 = liQuestion["noteSurvey2"];
            var user = userDataMappingService.GetUserDataMappingById(Id);
            var level = levelService.GetLevelById(user.DataUser.LevelId);
            Logdiary logDiary = new Logdiary()
            {
                Content = "Cập nhật khảo sát " + user.DataUser.Level.CodeLevel + " - " + user.DataUser.Level.LevelName,
                DateCreated = DateTime.Now,
                Noted = noteSurvey1,
                IsActive = true,
                UserDataId = user.DataUserId,
                LastEditedTime = DateTime.Now,
                StatusCall = "Survey",
                LinkFile = "https://google.com",
                TotalCall_Second = 10,
                LevelId = user.DataUser.LevelId,
                NameLevel = level.CodeLevel + "-" + level.LevelName,
                temp1 = user.UserId,
                TeleName = user.User.FirstName + " " + user.User.LastName,
                TeleId = User.Identity.GetUserId()
            };
            //logDiaryService.CreateLogdiary(logDiary);
            var log = logDiaryService.CreateLogdiaryWithResponse(logDiary);

            //sur.Logdiary = log;
            var listIDs = _questionService.GetQuestions();
            //string point = liQuestion["@item.Id"];
            //var name = liQuestion["point"];
            //foreach (var item in listIDs)
            //{

            //    if (item.Type == 1 && item.IsActive == true)
            //    {
            //        sur.QuestionId = item.Id;

            //        foreach (var temp in name.Split(','))
            //        {
            //            if (!temp.Equals("Điểm đánh giá"))
            //            {
            //                sur.Point = int.Parse(temp);
            //            }
            //        }
            //        //var x = form[id];

            //    }
            //}
            foreach (var temp in listIDs)
            {
                sur.LogdiaryId = log.Id;
                sur.Note = log.Noted;
                sur.QuestionId = temp.Id;
                string tempId = "liQuestion[" + temp.Id + "]";
                string point = liQuestion.Get(tempId);
                int resultPoint = 0;
                int.TryParse(point, out resultPoint);
                sur.Point = resultPoint;
                if (temp.Type.Equals(1))
                {
                    sur.Note = noteSurvey1;
                }
                else
                {
                    sur.Note = noteSurvey2;
                }
                _surveyService.CreateSurvey(sur);
            }

            string isAllowed = Request.Params["id"];
            return RedirectToAction("TeleIndex", "Admin/DataUser", new { id = User.Identity.GetUserId() });

        }

        [HttpPost]
        public ActionResult SubmitEdit(UserDataMappingFormModel userDataFromModel, FormCollection data)
        {
            int newID = -1;
            var newSession = "";
            bool hasLastCall = false;
            var currData = new DataUser();
            string log = data["respon"], note = data["noteLog"], linkFile = data["link_file"], Code = data["Code"], status = data["status"], tongThoiGianGoi = data["totalTime"], key = data["Code"];
            var Gender = data["Gender"];
            var teleName = data["assistant"];
            bool sex = true;//true === Nam, false === Nữ
            if (Gender.Equals("Nữ"))
            {
                sex = false;
            }
            var birthday = data["birthday"];

            #region save linfile
            //save linkfile to server
            if (linkFile != null && linkFile != "0")
            {
                string pathFileToServe = Server.MapPath("~/Images/media/");
                var nameFileAudio = DateTime.Now.Ticks.ToString();
                using (var client = new WebClient())
                {
                    client.DownloadFile(linkFile, pathFileToServe + nameFileAudio + ".wav");
                }
                linkFile = Request.Url.Scheme + "://" + Request.Url.Authority + "/Images/media/" + nameFileAudio + ".wav";
            }
            #endregion
            // ican not define my bug
            //if (log[log.Length - 1] == ',') log = log.Remove(log.Length - 1);
            // update Data
            //var data = Mapper.Map<DataUserFormModel, DataUser>(userDataFromModel.DataUser);
            // get data form DB
            currData = service.GetDataUserById(userDataFromModel.DataUserId);
            bool IsUpdate = true;
            userDataFromModel.DataUser.LevelId = userDataFromModel.DataUser.LevelId == 0 ? currData.LevelId : userDataFromModel.DataUser.LevelId;
            if (currData.LevelId != userDataFromModel.DataUser.LevelId)
            {
                IsUpdate = true;
            }
            // map
            currData.FirstName = userDataFromModel.DataUser.FirstName;
            currData.LastName = userDataFromModel.DataUser.LastName;
            currData.PhoneNumber = userDataFromModel.DataUser.PhoneNumber;
            currData.Email = userDataFromModel.DataUser.Email;
            currData.Sex = sex;
            currData.ChannelDataId = userDataFromModel.DataUser.ChannelDataId != 0 ? userDataFromModel.DataUser.ChannelDataId : currData.ChannelDataId;
            currData.LevelId = userDataFromModel.DataUser.LevelId;
            currData.Noted = userDataFromModel.DataUser.Noted;
            currData.Price_1 = userDataFromModel.DataUser.Price_1==null?0: userDataFromModel.DataUser.Price_1;
            currData.Address = userDataFromModel.DataUser.Address;

            //new field
            currData.MaVe = userDataFromModel.DataUser.MaVe;
            currData.TenChang = userDataFromModel.DataUser.TenChang;
            currData.Chieu = userDataFromModel.DataUser.Chieu;
            currData.HinhThuc = userDataFromModel.DataUser.HinhThuc;
            currData.LoaiXe = userDataFromModel.DataUser.LoaiXe;
            currData.CMND = userDataFromModel.DataUser.CMND;
            currData.Phone_NguoiDi = userDataFromModel.DataUser.Phone_NguoiDi;
            currData.TenTaiXe = userDataFromModel.DataUser.TenTaiXe;
            currData.BienSo = userDataFromModel.DataUser.BienSo;
            currData.HT_ThanhToan = userDataFromModel.DataUser.HT_ThanhToan;
            currData.Partner_Noted = userDataFromModel.DataUser.Partner_Noted;

            var khongGhepDuoc = data["Khong_Ghep_Duoc"];
            currData.Khong_Ghep_Duoc = true;//true === yes, false === no
            //if (khongGhepDuoc.Equals("No"))
            //{
            //    currData.Khong_Ghep_Duoc = false;
            //}
            //currData.Khong_Ghep_Duoc = userDataFromModel.DataUser.Khong_Ghep_Duoc;
            currData.PhanHoi = userDataFromModel.DataUser.PhanHoi;
            currData.Ten_Mien = userDataFromModel.DataUser.Ten_Mien;
            currData.QuangDuong = userDataFromModel.DataUser.QuangDuong;
            currData.TietKiem = userDataFromModel.DataUser.TietKiem;

            var thoiGianDat = data["Thoi_Gian_Dat"];
            currData.Thoi_Gian_Dat = (DateTime?)null;// thoiGianDat == "" ? (DateTime?)null : DateTime.Parse(thoiGianDat); //userDataFromModel.DataUser.Thoi_Gian_Dat;

            var khachTuDat = data["Khach_Tu_Dat"];
            currData.Khach_Tu_Dat = true;//true === yes, false === no
            //if (khachTuDat.Equals("No"))
            //{
            //    currData.Khach_Tu_Dat = false;
            //}
            //currData.Khach_Tu_Dat = userDataFromModel.DataUser.Khach_Tu_Dat;
            currData.Doi_Tac_Van_Chuyen = userDataFromModel.DataUser.Doi_Tac_Van_Chuyen;
            currData.Ma_Giam_Gia = userDataFromModel.DataUser.Ma_Giam_Gia;
            currData.Gia_Goc = userDataFromModel.DataUser.Gia_Goc;
            currData.Ly_Do_Huy = userDataFromModel.DataUser.Ly_Do_Huy;

            var thoiGianHuy = data["Thoi_Gian_Huy"];
            currData.Thoi_Gian_Huy = (DateTime?)null;// thoiGianHuy == "" ? (DateTime?)null : DateTime.Parse(thoiGianHuy); //userDataFromModel.DataUser.Thoi_Gian_Huy;
            currData.Hang_Khach_Chon = userDataFromModel.DataUser.Hang_Khach_Chon;
            currData.Ma_Ve_Doi_Tac = userDataFromModel.DataUser.Ma_Ve_Doi_Tac;
            currData.Tinh_Trang = userDataFromModel.DataUser.Tinh_Trang;

            //end new

            currData.LastEditedTime = DateTime.Now;
            currData.TotalInteractive += 1;
            if (String.IsNullOrEmpty(birthday))
            {
                currData.DayOfBith = null;
            }
            else
            {
                currData.DayOfBith = DateTime.Parse(birthday);
            }

            service.EditDataUser(currData);

            // update log if calling

            // var logDiary = JsonConvert.DeserializeObject<LogHistory>(log);
            //if (log != "")
            //{
            //    //update log
            //    UpdateContent(linkFile,note, userDataFromModel.DataUserId, logDiary, userDataFromModel.DataUser.LevelId);
            //    // has call -> change last call
            //    if (logDiary.ngayGoi != "")
            //    {
            //        currData.LastCall = Convert.ToDateTime(logDiary.ngayGoi);
            //        hasLastCall = true;
            //    }
            //    return RedirectToAction("Edit", "DataUser", new { id = userDataFromModel.Id });
            //}
            // add to log history if level change
            LogHistory logHistory = new LogHistory();
            // logHistory = new JavaScriptSerializer().Deserialize<LogHistory>(log);    
            //if (log != null)
            if (IsUpdate)
            {
                var level = levelService.GetLevelById(userDataFromModel.DataUser.LevelId);
                // logHistory = new JavaScriptSerializer().Deserialize<LogHistory>(log);
                Logdiary logDiary = new Logdiary()
                {
                    Content = "Cập Nhật trạng thái thành công" + currData.Level.CodeLevel + " - " + currData.Level.LevelName,
                    DateCreated = DateTime.Now,
                    Noted = note,
                    IsActive = true,
                    UserDataId = userDataFromModel.DataUserId,
                    LastEditedTime = DateTime.Now,
                    StatusCall = status,
                    LinkFile = linkFile,
                    temp5 = data["link_file"],
                    CodeLogDiary = Code,
                    TotalCall_Second = StringConvert.ToSeconds(tongThoiGianGoi),
                    TotalCallSucceed = StringConvert.ToSeconds(tongThoiGianGoi),
                    LevelId = userDataFromModel.DataUser.LevelId,
                    NameLevel = level.CodeLevel + "-" + level.LevelName,
                    temp1 = userDataFromModel.UserId,
                    TeleName = userDataFromModel.User.FirstName + " " + userDataFromModel.User.LastName,
                    TeleId = User.Identity.GetUserId()
                };
                logDiaryService.CreateLogdiary(logDiary);
            }
            else
            {
                var level = levelService.GetLevelById(userDataFromModel.DataUser.LevelId);
                // logHistory = new JavaScriptSerializer().Deserialize<LogHistory>(log);
                Logdiary logDiary = new Logdiary()
                {
                    Content = "Cập Nhật trạng thái thành công" + currData.Level.CodeLevel + " - " + currData.Level.LevelName,
                    DateCreated = DateTime.Now,
                    Noted = note,
                    IsActive = true,
                    UserDataId = userDataFromModel.DataUserId,
                    LastEditedTime = DateTime.Now,
                    StatusCall = "NO ANSWER",
                    LinkFile = "",
                    TotalCall_Second = 0,
                    LevelId = userDataFromModel.DataUser.LevelId,
                    NameLevel = level.CodeLevel + "-" + level.LevelName,
                    temp1 = userDataFromModel.UserId,
                    TeleName = userDataFromModel.User.FirstName + " " + userDataFromModel.User.LastName,
                    TeleId = User.Identity.GetUserId()
                };
                logDiaryService.CreateLogdiary(logDiary);
            }
            // manage session
            // get new Id
            // get new session
            if (Session[User.Identity.GetUserId()] != null && !Session[User.Identity.GetUserId()].Equals(""))
            {
                var listID = Session[User.Identity.GetUserId()].ToString().Split(';');
                var temp = "";
                for (int i = 0; i < listID.Count(); i++)
                {
                    if (!listID.ElementAt(i).Equals(userDataFromModel.Id.ToString()))
                    {
                        temp += listID.ElementAt(i);
                        if (i != listID.Count() - 1) temp += ";";
                    }
                }
                if (temp.Length > 0 && temp[temp.Length - 1] == ';') temp = temp.Remove(temp.Length - 1);
                listID = temp.Split(';');
                // new session
                if (!listID.ElementAt(0).Equals(""))
                {
                    newID = int.Parse(listID.ElementAt(0));
                }
                Session[User.Identity.GetUserId()] = temp;
            }

            //return RedirectToAction("TeleIndex", "DataUser", new { id = User.Identity.GetUserId() });
            //if (newID == -1) return RedirectToAction("TeleIndex", "DataUser", new { id = User.Identity.GetUserId() });
            //return RedirectToAction("Edit", "DataUser", new { id = newID });
            return RedirectToAction("Edit", "DataUser", new { id = userDataFromModel.DataUserId });
        }
        [HttpPost]
        public ActionResult SubmitCustomer(FormCollection liCustomer)
        {
            GetCookie();
            ViewBag.Message = "";
            #region lay du lieu tu form
            var entities = service.GetDataUsers().ToList();
            var firstName = liCustomer["firstName"];
            var fullName = liCustomer["fullName"];
            var Gender = liCustomer["sex"];
            var LevelId = liCustomer["LevelId"];
            var numberPhone = liCustomer["numberPhone"];
            var address = liCustomer["address"];
            var email = liCustomer["email"];
            var assistant = liCustomer["assistant"];
            var ChannelId = liCustomer["ChannelDataId"];
            //var brandName = liCustomer["brandName"];
            #endregion

            #region create customer for admin
            var user = _userManager.FindById(User.Identity.GetUserId());
            var roles = _userManager.GetRoles(user.Id);
            if (User.IsInRole("Admin"))
            {

                #region create dataUser

                DataUser dtUser = new DataUser();
                if (numberPhone != "")
                {
                    //numberPhone = numberPhone != '0' ? "0" + numberPhone : numberPhone;
                    var checkPhone = Regex.Match(numberPhone, @"^([0-9]{10})$").Success;

                    dtUser.PhoneNumber = checkPhone ? numberPhone : "";
                    dtUser.FirstName = firstName;
                    dtUser.LastName = fullName;
                    dtUser.UserName = assistant;//người phụ trách
                    if (Gender.Equals("Nam"))
                    {
                        dtUser.Sex = true;
                    }
                    else if (Gender.Equals("Nữ"))
                    {
                        dtUser.Sex = false;
                    }
                    //if (birthday == "")
                    //{
                    //    dtUser.DayOfBith = DateTime.Now;
                    //}
                    //else
                    //{
                    //    dtUser.DayOfBith = Convert.ToDateTime(birthday);
                    //}
                    dtUser.Address = address;
                    dtUser.Email = email;
                    dtUser.DateCreated = DateTime.Now;
                    dtUser.IsActive = true;
                    dtUser.Deleted = false;
                    dtUser.LevelId = int.Parse(LevelId);
                    dtUser.StoreId = 2;
                    dtUser.ChannelDataId = int.Parse(ChannelId);
                    dtUser.Brand_Name = _BRAND_NAME;
                    var entity = service.GetDataUsers().Where(p => p.PhoneNumber.Equals(dtUser.PhoneNumber)).LastOrDefault();
                    //var brand = brandService.GetBrands().Where(p => p.Name.Equals(dtUser.Brand_Name)).FirstOrDefault();
                    if (entity == null)
                    {
                        dtUser.BrandId = _BRAND_ID;
                        dtUser.Brand_Code = _BRAND_CODE;
                        service.CreateDataUser(dtUser);
                        var teleNew = _userManager.FindByName(dtUser.UserName);//Tìm theo tên userName
                                                                               //var userTele = service.GetDataUsers().Where(p => p.UserName.Equals(user.UserName)).FirstOrDefault();

                        //var tele = _userManager.FindByName(userTele.UserName);
                        if (teleNew != null)
                        {
                            //var data = service.GetDataUsers().Where(p => p.PhoneNumber.Equals(dtUser.PhoneNumber)).FirstOrDefault();
                            UserDataMapping userDataMapping = new UserDataMapping()
                            {
                                UserId = teleNew.Id,//null,// 
                                DateCreated = DateTime.Now,
                                DataUserId = dtUser.Id,
                                IsActive = true,
                                Deleted = false,
                                Position = 0,
                                LastEditedTime = DateTime.Now,
                            };
                            userDataMappingService.CreateUserDataMapping(userDataMapping);
                        }
                    }
                    else
                    {
                        //ViewBag.Mesage = "";
                        if (entity.PhoneNumber == dtUser.PhoneNumber && entity.Brand_Name == _BRAND_NAME)
                        {
                            SetAlert("Số điện thoại bị trùng. Vui lòng nhập lại!", "warning");
                            return RedirectToAction("Index", "Admin/DataUser");
                        }
                        dtUser.BrandId = _BRAND_ID;
                        dtUser.Brand_Code = _BRAND_CODE;
                        service.CreateDataUser(dtUser);
                        var tele = _userManager.FindByName(dtUser.UserName);//Tìm theo tên userName
                                                                            //var userTele = service.GetDataUsers().Where(p => p.UserName.Equals(user.UserName)).FirstOrDefault();

                        //var tele = _userManager.FindByName(userTele.UserName);
                        if (tele != null)
                        {
                            //var data = service.GetDataUsers().Where(p => p.PhoneNumber.Equals(dtUser.PhoneNumber)).FirstOrDefault();
                            UserDataMapping userDataMapping = new UserDataMapping()
                            {
                                UserId = tele.Id,//null,// 
                                DateCreated = DateTime.Now,
                                DataUserId = dtUser.Id,
                                IsActive = true,
                                Deleted = false,
                                Position = 0,
                                LastEditedTime = DateTime.Now,
                            };
                            userDataMappingService.CreateUserDataMapping(userDataMapping);
                        }
                    }
                }
                SetAlert("Thêm khách hàng thành công!", "success");
                #endregion create datUser
                return RedirectToAction("Index", "Admin/DataUser");
            }
            #endregion

            #region create dataUser of Tele

            DataUser dtUserTele = new DataUser();
            if (numberPhone != "")
            {
                //numberPhone = numberPhone != '0' ? "0" + numberPhone : numberPhone;
                var checkPhone = Regex.Match(numberPhone, @"^([0-9]{10})$").Success;
                var TeleId = _userManager.FindById(user.Id);
                //userDataMappingService.GetUserDataMappings().Where(p => p.UserId == user.Id).FirstOrDefault();

                dtUserTele.PhoneNumber = checkPhone ? numberPhone : "";
                dtUserTele.FirstName = firstName;
                dtUserTele.LastName = fullName;
                dtUserTele.UserName = TeleId.UserName;
                if (Gender.Equals("Nam"))
                {
                    dtUserTele.Sex = true;
                }
                else if (Gender.Equals("Nữ"))
                {
                    dtUserTele.Sex = false;
                }
                //if (birthday == "")
                //{
                //    dtUserTele.DayOfBith = DateTime.Now;
                //}
                //else
                //{
                //    dtUserTele.DayOfBith = Convert.ToDateTime(birthday);
                //}
                dtUserTele.Address = address;
                dtUserTele.Email = email;
                dtUserTele.DateCreated = DateTime.Now;
                dtUserTele.IsActive = true;
                dtUserTele.Deleted = false;
                dtUserTele.LevelId = 1;
                dtUserTele.StoreId = 1;
                dtUserTele.ChannelDataId = int.Parse(ChannelId);
                dtUserTele.Brand_Name = _BRAND_NAME;
                var entity = service.GetDataUsers().Where(p => p.PhoneNumber.Equals(dtUserTele.PhoneNumber)).LastOrDefault();
                //var brand = brandService.GetBrands().Where(p => p.Name.Equals(dtUserTele.Brand_Name)).FirstOrDefault();

                if (entity == null)
                {
                    dtUserTele.BrandId = _BRAND_ID;
                    dtUserTele.Brand_Code = _BRAND_CODE;
                    service.CreateDataUser(dtUserTele);
                    var tele = _userManager.FindByName(dtUserTele.UserName);//Tìm theo tên userName
                                                                            //var userTele = service.GetDataUsers().Where(p => p.UserName.Equals(user.UserName)).FirstOrDefault();

                    //var tele = _userManager.FindByName(userTele.UserName);
                    if (tele != null)
                    {
                        //var data = service.GetDataUsers().Where(p => p.PhoneNumber.Equals(dtUserTele.PhoneNumber)).FirstOrDefault();
                        UserDataMapping userDataMapping = new UserDataMapping()
                        {
                            UserId = tele.Id,//null,// 
                            DateCreated = DateTime.Now,
                            DataUserId = dtUserTele.Id,
                            IsActive = true,
                            Deleted = false,
                            Position = 0,
                            LastEditedTime = DateTime.Now,
                        };
                        userDataMappingService.CreateUserDataMapping(userDataMapping);
                    }
                }
                else
                {
                    if (entity.PhoneNumber == dtUserTele.PhoneNumber && entity.Brand_Name == _BRAND_NAME)
                    {
                        SetAlert("Số điện thoại hoặc brand bị trùng. Vui lòng kiểm tra lại", "warning");
                        return RedirectToAction("TeleIndex", "Admin/DataUser");
                    }
                    else
                    {
                        dtUserTele.BrandId = _BRAND_ID;
                        dtUserTele.Brand_Code = _BRAND_CODE;
                        service.CreateDataUser(dtUserTele);
                        var tele = _userManager.FindByName(dtUserTele.UserName);//Tìm theo tên userName
                                                                                //var userTele = service.GetDataUsers().Where(p => p.UserName.Equals(user.UserName)).FirstOrDefault();

                        //var tele = _userManager.FindByName(userTele.UserName);
                        if (tele != null)
                        {
                            //var data = service.GetDataUsers().Where(p => p.PhoneNumber.Equals(dtUserTele.PhoneNumber)).FirstOrDefault();
                            UserDataMapping userDataMapping = new UserDataMapping()
                            {
                                UserId = tele.Id,//null,// 
                                DateCreated = DateTime.Now,
                                DataUserId = dtUserTele.Id,
                                IsActive = true,
                                Deleted = false,
                                Position = 0,
                                LastEditedTime = DateTime.Now,
                            };
                            userDataMappingService.CreateUserDataMapping(userDataMapping);
                        }
                        SetAlert("Tạo thành công!", "success");
                        return RedirectToAction("TeleIndex", "Admin/DataUser", new { id = User.Identity.GetUserId() });
                    }
                }
            }

            #endregion create datUser of Tele


            SetAlert("Tạo thành công!", "success");

            return RedirectToAction("TeleIndex", "Admin/DataUser", new { id = User.Identity.GetUserId() });

            //var entities = service.GetDataUsers().ToList();

        }
        public ActionResult ListLog(int id)
        {
            var dataUserMapping = userDataMappingService.GetUserDataMappingById(id);
            var listDiary = new List<Logdiary>();
            ViewBag.DataUserId = dataUserMapping.DataUserId;
            ViewBag.dataUserMapping = id;
            if (dataUserMapping == null)
            {
                return PartialView("_ListLog", listDiary);
            }
            else
            {
                ViewBag.ListLevel = levelService.GetLevels().ToSelectListItems(dataUserMapping.DataUser.LevelId);
                listDiary = logDiaryService.GetLogdiarys().Where(p => p.UserDataId == dataUserMapping.DataUserId).ToList();
                return PartialView("_ListLog", listDiary);

            }
        }
        public ActionResult DataUserBrand()
        {
            var listDataUserBrand = brandService.GetBrands().ToList();
            return PartialView("_DataUserBrand", listDataUserBrand);
        }
        public ActionResult ListHistoryOrders(int id)
        {
            var listOrders = orderService.GetOrders().Where(p => p.UserDataMappingId == id).OrderByDescending(p => p.CreateDate).ToList();
            return PartialView("_ListHistoryOrders", listOrders);
        }
        public ActionResult ListItemProduct(int id)
        {
            var listProduct = productService.GetProducts().Where(p => p.Id.Equals(id)).ToList();
            return PartialView("_ListItemProduct", listProduct);
        }
        public ActionResult ListTeles()
        {
            var listTeles = _userManager.Users.Where(p => p.RoleId == SystemRoles.Role03 && !p.Deleted && p.Activated).ToList();
            return PartialView("_ListTeles", listTeles);
        }
        public ActionResult ListStores()
        {
            var listStores = storeService.GetStores().Where(p => !p.IsDelete && p.IsActive).ToList();
            return PartialView("_ListStores", listStores);
        }
        public ActionResult ListWards()
        {
            var listWards = wardService.GetWards().Where(p => !p.IsDeleted && p.IsActive).ToList();
            return PartialView("_ListWards", listWards);
        }
        public ActionResult ListDistricts()
        {
            var listDistricts = districtService.GetDistricts().Where(p => !p.IsDeleted && p.IsActive).ToList();
            return PartialView("_ListDistricts", listDistricts);
        }
        public ActionResult ListTypeOrder(int id)
        {
            var userMapping = userDataMappingService.GetUserDataByDataUserId(id);
            var listOrder = orderService.GetOrders().Where(p => p.UserDataMappingId == userMapping.Id).ToList();
            return PartialView("_ListTypeOrder", listOrder);
        }
        public ActionResult ItemStoreBrand(int id)
        {
            var itemStore = storeService.GetStores().Where(p => p.Id.Equals(id)).FirstOrDefault();
            var itemBrand = brandService.GetBrands().Where(p => p.Id.Equals(itemStore.BrandId)).ToList();
            return PartialView("_OrderStored", itemBrand);
        }
        public ActionResult ListHistoryOrder(int id)
        {
            var listLog = logDiaryService.GetLogdiarys().Where(l => l.UserDataId == id && l.OrderItems.Count != 0);
            return PartialView("_ListHistoryOrder", listLog);
        }
        public ActionResult ListLevels()
        {
            var listLevel = levelService.GetLevels().ToList();
            return PartialView("_ListLevels", listLevel);
        }

        public void UpdateContent(string linkFile, string note, int DataUserId, LogHistory logHistory, int LevelId = 0)
        {
            Logdiary obj = new Logdiary();
            obj.Noted = note;
            obj.DateCreated = DateTime.Now;
            obj.IsActive = true;
            obj.LastEditedTime = DateTime.Now;
            obj.Deleted = false;
            obj.Position = 0;
            obj.LinkFile = linkFile;
            //LogHistory logHistory = (LogHistory)(new JavaScriptSerializer().DeserializeObject(resultLog));
            obj.CodeLogDiary = logHistory.key;
            obj.temp5 = logHistory.linkFile;
            obj.StatusCall = logHistory.trangThai;
            obj.TotalCallSucceed += int.Parse(logHistory.tongThoiGianGoi);
            obj.TimeCall = logHistory.ngayGoi;
            obj.PhoneReceived = logHistory.soNhan;
            obj.TotalCall_Second += StringConvert.ToSeconds(logHistory.tongThoiGianGoi);




            // them user
            //var datausermapping = userDataMappingService.GetUserDataMappingById(dataUserMappingId);
            var dataUser = service.GetDataUserById(DataUserId);
            var user = _userManager.FindById(User.Identity.GetUserId());
            //get datausermapping
            if (dataUser != null)
            {
                obj.UserDataId = dataUser.Id;
                obj.PhoneReceived = dataUser.PhoneNumber;
                obj.temp1 = User.Identity.GetUserId();
                obj.TeleName = user.DisplayName;
                obj.TeleId = User.Identity.GetUserId();
            }
            else
            {

            }
            //them level Id
            var level = levelService.GetLevelById(LevelId);
            if (level != null)
            {
                obj.LevelId = level.Id;
                obj.NameLevel = level.CodeLevel + " - " + level.LevelName;
                dataUser.LevelId = level.Id;
                service.EditDataUser(dataUser);

            }
            logDiaryService.CreateLogdiary(obj);
            //return PartialView("_logItem", obj);
        }

    }

}
