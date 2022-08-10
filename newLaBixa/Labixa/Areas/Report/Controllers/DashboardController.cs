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
using Outsourcing.Core.Common;
using Newtonsoft.Json;

namespace Labixa.Areas.Report.Controllers
{
    public class DashboardController : BaseReportController
    {

        readonly IDataUserService service;
        readonly IUserDataMappingService userDataMappingService;
        readonly IChanelDataService chanelDataService;
        readonly ILevelService levelService;
        readonly ILogdiaryService logDiaryService;
        readonly ISurveyService surveyService;
        readonly IQuestionService questionServicem;
        readonly IBrandService brandService;
        private UserManager<User> _userManager;

        int _BRAND_ID = 0;
        string _BRAND_CODE = "THT";
        string _BRAND_NAME = "Toàn Hệ Thống";

        public DashboardController(IDataUserService service, IUserDataMappingService userDataMappingService,
            IChanelDataService chanelDataService, ILevelService levelService, UserManager<User> userManager
                        , ILogdiaryService logDiaryService, ISurveyService surveyService, IQuestionService questionServicem, IBrandService brandService)
        {
            this.service = service;
            this.userDataMappingService = userDataMappingService;
            //this.aspNetUserService = aspNetUserService;
            this.chanelDataService = chanelDataService;
            this.levelService = levelService;
            this.logDiaryService = logDiaryService;
            this.surveyService = surveyService;
            this.brandService = brandService;
            this.questionServicem = questionServicem;
            _userManager = userManager;



        }
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
        //
        // GET: /Report/Dashboard/
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ReportByDay()
        {
            return View();
        }
        public ActionResult ReportCall()
        {
            return View();
        }
        [HttpPost]
        public string GetCharTotalCall(DateTime dateVal)
        {
            GetCookie();
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
            if (_BRAND_ID != 0)
            {
                ListData = service.GetDataUsers().Where(p => !p.Deleted && p.BrandId == _BRAND_ID).ToList();
                listDiary = logDiaryService.GetLogdiarys().Where(p => p.DateCreated.Month == dateVal.Month && p.StatusCall != null && p.DataUser.BrandId == _BRAND_ID).ToList();
                tempListLevel = levelService.GetLevels().Where(p => p.BrandId == _BRAND_ID).ToDictionary(l => l.Id + " " + l.CodeLevel + " (" + l.LevelName + ")", t => (double)0);
            }
            else
            {
                ListData = service.GetDataUsers().Where(p => !p.Deleted).ToList();
                listDiary = logDiaryService.GetLogdiarys().Where(p => p.DateCreated.Month == dateVal.Month && p.StatusCall != null).ToList();
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
                model.TotalCall_Day = listDiary.Where(p => p.DateCreated.Date.Equals(dateVal.Date) && (p.StatusCall.Equals("ANSWERED") || p.StatusCall.Equals("NO ANSWER") || p.StatusCall.Equals("BUSY")) && p.DataUser.BrandId == _BRAND_ID).ToList().Count();
                model.TotalAnswer = (listDiary.Where(p => p.DateCreated.Date.Equals(dateVal.Date) && (p.StatusCall.Equals("ANSWERED")) && p.DataUser.BrandId == _BRAND_ID).ToList().Count());
                model.TotalAnswerMonth = (listDiary.Where(p => p.DateCreated.Month.Equals(dateVal.Month) && (p.StatusCall.Equals("ANSWERED")) && p.DataUser.BrandId == _BRAND_ID).ToList().Count());
                model.TotalAmount = (double)listDiary.Where(p => p.DateCreated.Date.Equals(dateVal.Date) && p.DataUser.BrandId == _BRAND_ID).Sum(p => p.Price_1);// >= dateTime
            }
            else
            {
                model.TotalCall_Day = listDiary.Where(p => p.DateCreated.Date.Equals(dateVal.Date) && (p.StatusCall.Equals("ANSWERED") || p.StatusCall.Equals("NO ANSWER") || p.StatusCall.Equals("BUSY")) ).ToList().Count();
                model.TotalAnswer = (listDiary.Where(p => p.DateCreated.Date.Equals(dateVal.Date) && (p.StatusCall.Equals("ANSWERED"))).ToList().Count());
                model.TotalAnswerMonth = (listDiary.Where(p => p.DateCreated.Month.Equals(dateVal.Month) && (p.StatusCall.Equals("ANSWERED"))).ToList().Count());
                model.TotalAmount = (double)listDiary.Where(p => p.DateCreated.Date.Equals(dateVal.Date)).Sum(p => p.Price_1);// >= dateTime
            }
            #region TotalCallMonth + total Order month
            int count_Order = 0;
            var currentMonth = dateVal;
            var firstDayOfMonth = new DateTime(currentMonth.Year, currentMonth.Month, 1);
            var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
            var numDay = (lastDayOfMonth.Day - firstDayOfMonth.Day)+1;
            for (int i = 1; i <= numDay; i++)
            {
                var count_day = 0;
                if (_BRAND_ID != 0)
                {
                    count_day = listDiary.Where(p => p.DateCreated.Day.Equals(firstDayOfMonth.Day) && (p.StatusCall.Equals("ANSWERED") || p.StatusCall.Equals("NO ANSWER") || p.StatusCall.Equals("BUSY")) && p.DataUser.BrandId == _BRAND_ID).ToList().Count();//p.DateCreated.Date.Equals(firstDayOfMonth.Date)
                    count_Order = listDiary.Where(p => p.DateCreated.Day.Equals(firstDayOfMonth.Day) && p.DataUser.BrandId == _BRAND_ID && p.OrderItems.Any()).ToList().Count();
                }
                else
                {
                    count_day = listDiary.Where(p => p.DateCreated.Day.Equals(firstDayOfMonth.Day) && (p.StatusCall.Equals("ANSWERED") || p.StatusCall.Equals("NO ANSWER") || p.StatusCall.Equals("BUSY")) ).ToList().Count();
                    count_Order = listDiary.Where(p => p.DateCreated.Day.Equals(firstDayOfMonth.Day) && p.OrderItems.Any()).ToList().Count();

                }

                if (count_day != 0)
                {
                    ChartModel chart = new ChartModel();
                    chart.Name = firstDayOfMonth.ToString("dd");
                    chart.Value = count_day.ToString("#,##0");
                    model.ChartMonths.Add(chart);
                    model.TotalCall_Month += count_day;
                }
                else
                {
                    ChartModel chart = new ChartModel();
                    chart.Value = "0";
                    chart.Name = firstDayOfMonth.ToString("dd");
                    model.ChartMonths.Add(chart);
                    model.TotalCall_Month += count_day;

                }

                if (count_Order != 0)
                {
                    ChartModel chart = new ChartModel();
                    chart.Name = firstDayOfMonth.ToString("dd");
                    chart.Value = count_Order.ToString("#,##0");
                    model.ChartOrderMonths.Add(chart);
                    model.TotalOrder_Month += count_Order;
                }
                else
                {
                    ChartModel chart = new ChartModel();
                    chart.Value = "0";
                    chart.Name = firstDayOfMonth.ToString("dd");
                    model.ChartOrderMonths.Add(chart);
                    model.TotalOrder_Month += count_Order;
                }
                firstDayOfMonth = firstDayOfMonth.Date.AddDays(1);
            }
            #endregion
            #region Tong hoa don + binh quan hoa don
            listDiary = logDiaryService.GetLogdiarys().Where(p => p.DateCreated.Month == dateTime.Month && p.DateCreated.Year == dateTime.Year && p.StatusCall != null).ToList();
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
        [HttpPost]
        public string GetAmountByDay(string fromDate, string toDate)
        {
            var model = new ReportAmount();
            var dateStart = Convert.ToDateTime(fromDate + " 00:00:00.000");
            //dateStart = dateStart.AddDays();
            var dateEnd = Convert.ToDateTime(toDate + " 00:00:00.000");
            if (dateStart > dateEnd) dateStart = dateEnd;
            var listDiary = logDiaryService.GetLogdiarys().Where(l => l.DateCreated >= dateStart && l.DateCreated <= dateEnd);
            model.totalAmount = (double)listDiary.Where(l => l.DateCreated >= dateStart && l.DateCreated <= dateEnd).Sum(a => a.Price_1);
            var dateTime = Convert.ToDateTime(DateTime.Now.ToLongDateString() + " 00:00:00.000");
            #region TotalCallWeek
            // get amount by date
            for (var i = dateStart; i <= dateEnd; i = i.AddDays((double)1))
            {
                var j = i;
                var amount_date = (double)listDiary.Where(p => p.DateCreated >= i && p.DateCreated < j.AddDays(1)).Sum(a => a.Price_1);
                ChartModel chart = new ChartModel();
                chart.Name = i.Day + "/" + i.Month;
                chart.Value = "" + (int)amount_date;
                model.date_amount.Add(chart);
            }
            var listChanel = chanelDataService.GetChanelDatas().Select(lv => new { lv.Code, lv.Id }).Distinct().ToList();
            // get amount by level
            foreach (var item in listChanel)
            {
                var amount_chanel = (double)listDiary.Where(p => p.DataUser.ChannelDataId == item.Id).Sum(a => a.Price_1);
                ChartModel chart_level = new ChartModel();
                chart_level.Name = item.Code;
                chart_level.Value = "" + (int)amount_chanel;
                model.level_amount.Add(chart_level);
            }
            // get call by agent
            var listAgent = _userManager.Users.ToList().Where(p => !p.Deleted && p.Activated).OrderByDescending(p => p.DateCreated).ToList();
            // get amount by level
            foreach (var item in listAgent)
            {
                var amount_agent = (double)listDiary.Where(p => p.TeleId == item.Id).Sum(a => a.Price_1);
                ChartModel chart_agent = new ChartModel();
                chart_agent.Name = item.UserName;
                chart_agent.Value = "" + (int)amount_agent;
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
        // show data with level
        public string GetCallByDate(string fromDate, string toDate)
        {
            GetCookie();
            var model = new ReportAmount();
            var dateStart = Convert.ToDateTime(fromDate + " 00:00:00.000");
            //dateStart = dateStart.AddDays();
            var dateEnd = Convert.ToDateTime(toDate + " 00:00:00.000");
            if (dateStart > dateEnd) dateStart = dateEnd;
            var listDiary = logDiaryService.GetLogdiarys().Where(l =>l.DataUser.BrandId==_BRAND_ID && l.DateCreated >= dateStart && l.DateCreated <= dateEnd && (l.StatusCall.Equals("ANSWERED") || l.StatusCall.Equals("NO ANSWER") || l.StatusCall.Equals("NO ANSWER")));
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
        public ActionResult DataUser(int Id)//level
        {
            //ViewBag.Level = "Level " + level;

            var entity = levelService.GetLevels().Where(p => p.Id == Id).FirstOrDefault();//.FirstOrDefault(l => l.CodeLevel.ToLower().Equals(level.ToLower().Trim()));
            ViewBag.Level = "Level " + entity.CodeLevel;

            if (entity != null)
            {
                var listData = service.GetDataUsers().Where(d => d.LevelId == entity.Id);
                var listIdData = new List<int>();
                foreach(var item in listData)
                {
                    listIdData.Add(item.Id);
                }
                ViewBag.ListDataMapping = userDataMappingService.GetUserDataMappings().Where(p => listIdData.Contains(p.DataUserId)).ToList();
                return View(model: listData);
            }
            return View();
        }
        public ActionResult ReportSurvey()
        {
            return View();
        }
        public ActionResult SurveyData()
        {
            List<SurveyData> model = new List<SurveyData>();

            var listSurvey = surveyService.GetSurveys().GroupBy(p => p.LogdiaryId, (key, g) => new { LogdiaryId = key, list = g.ToList() });
            foreach (var i in listSurvey)
            {
                try
                {
                    var logdiaryID = logDiaryService.GetLogdiarys().Where(p => p.Id == i.LogdiaryId).FirstOrDefault();
                    var userID = service.GetDataUsers().Where(p => p.Id == logdiaryID.UserDataId).FirstOrDefault();
                    var surveyID = surveyService.GetSurveys().Where(p => p.LogdiaryId == logdiaryID.Id).FirstOrDefault();

                    var surveyList = surveyService.GetSurveys().Where(p => p.LogdiaryId == logdiaryID.Id).ToList();
                    var questionType = questionServicem.GetQuestions().Where(p => p.Id == surveyID.QuestionId).FirstOrDefault();
                    float point = 0;
                    foreach (var item in surveyList)
                    {
                        point += item.Point;
                    }
                    if (surveyList.Count() > 0)
                        point = point / surveyList.Count();
                    SurveyData data = new SurveyData();
                    data.Id = userID.Id;
                    data.Name = userID.FirstName + userID.LastName;
                    data.Category = questionType.Type == 1 ? "Bán hàng" : "Dịch vụ";
                    data.Score = point.ToString();

                    model.Add(data);
                }
                catch (Exception ex)
                {
                    model = new List<SurveyData>();
                }
            }

            return PartialView("_SurveyData", model);
        }
        public ActionResult ListBrand()
        {
            var model = brandService.GetBrands();
            return PartialView("_ListBrand", model);
        }
    }

    public class SurveyData
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public string Score { get; set; }
    }
    public class ChartModel
    {
        public ChartModel()
        {
            Answer = NoAnswer = "0";
        }
        public string Name { get; set; }
        public string Value { get; set; }
        public string Answer { get; set; }
        public string NoAnswer { get; set; }

    }
    public class ReportModel
    {
        public ReportModel()
        {
            TargetCall_Weeks = TargetCall_Month = 0;
            #region old   
            TotalData = TotalDataL2 = TotalDataL3 = TotalDataL4 = TotalDataL5
                = TotalDataL6 = TotalDataL7 = TotalDataL8 = TotalDataL0 = TotalCall_Day = TotalCall_Month = TotalCall_Week = 0;
            #endregion
            #region new by hungnv
            #endregion
            TotalCall_Day = TotalCall_Month = TotalCall_Week = 0;
            ChartWeeks = new List<ChartModel>();
            ChartMonths = new List<ChartModel>();
            ChartLastWeeks = new List<ChartModel>();
            ChartOrderMonths = new List<ChartModel>();
            ListTotalEachLevel = new Dictionary<string, double>();
            DateChoose = new DateTime();
        }
        public int TotalCall_Day { get; set; }
        public DateTime DateChoose { get; set; }
        public string TotalIncome_Day { get; set; }
        public int TotalCall_Month { get; set; }
        public int TotalCall_Week { get; set; }
        public string TotalIncome_Month { get; set; }
        public int TargetCall_Weeks { get; set; }
        public int TargetCall_Month { get; set; }
        #region aLong new 28/9
        public int TotalOrder_Day { get; set; }
        public int TotalOrder_Month { get; set; }
        public double? AvgPaymentOrder_Day { get; set; }
        public double? AvgPaymentOrder_Month { get; set; }
        #endregion
        public double TotalData { get; set; }
        #region old
        public double TotalDataL0 { get; set; }
        public double TotalDataL1 { get; set; }
        public double TotalDataL2 { get; set; }
        public double TotalDataL3 { get; set; }
        public double TotalDataL4 { get; set; }
        public double TotalDataL5 { get; set; }
        public double TotalDataL6 { get; set; }
        public double TotalDataL7 { get; set; }
        public double TotalDataL8 { get; set; }
        #endregion
        #region new by hungnv
        public Dictionary<string, double> ListTotalEachLevel;
        #endregion
        public double TotalAmount { get; set; }
        public double TotalAmountMonth { get; set; }
        public double? AnsPerTotal { get; set; }
        public int TotalAnswer { get; set; }
        public int TotalAnswerMonth { get; set; }
        public List<ChartModel> ChartWeeks { get; set; }
        public List<ChartModel> ChartLastWeeks { get; set; }
        public List<ChartModel> ChartMonths { get; set; }
        public List<ChartModel> ChartOrderMonths { get; set; }
    }
    public class ReportAmount
    {
        public ReportAmount()
        {
            totalAmount = totalCallAnswer = totalCallNoAnswer = 0;
            date_amount = new List<ChartModel>();
            level_amount = new List<ChartModel>();
            date_call = new List<ChartModel>();
            level_call = new List<ChartModel>();
            call_status = new List<ChartModel>();
            agent = new List<ChartModel>();

        }
        public double totalAmount { get; set; }
        public double totalCallAnswer { get; set; }
        public double totalCallNoAnswer { get; set; }
        public List<ChartModel> date_amount { get; set; }
        public List<ChartModel> level_amount { get; set; }
        public List<ChartModel> date_call { get; set; }
        public List<ChartModel> level_call { get; set; }
        public List<ChartModel> call_status { get; set; }
        public List<ChartModel> agent { get; set; }



    }
    public class ReportCall
    {
        public ReportCall()
        {
            date_call = new List<ChartModel>();
            level_call = new List<ChartModel>();
            call_status = new List<ChartModel>();
            agent = new List<ChartModel>();

        }
        public int totalCall { get; set; }
        public int totalCallAnswer { get; set; }
        public int totalCallNoAnswer { get; set; }

        public List<ChartModel> call_answer { get; set; }
        public List<ChartModel> call_noanswer { get; set; }
        public List<ChartModel> date_call { get; set; }
        public List<ChartModel> level_call { get; set; }
        public List<ChartModel> call_status { get; set; }
        public List<ChartModel> agent { get; set; }



    }
}