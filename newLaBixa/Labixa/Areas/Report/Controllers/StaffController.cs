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


    public class StaffController : BaseReportController
    {
        readonly IDataUserService service;
        readonly IUserDataMappingService userDataMappingService;
        readonly IChanelDataService chanelDataService;
        readonly ILevelService levelService;
        readonly ILogdiaryService logDiaryService;
        private ITrackingAttendenceService _TrackingAttendenceService;
        private UserManager<User> _userManager;
        private ISurveyService surveyService;
        private IDataUserService dataUserService;
        public StaffController(IDataUserService service, IUserDataMappingService userDataMappingService,
                IChanelDataService chanelDataService, ILevelService levelService, UserManager<User> userManager
                , ILogdiaryService logDiaryService, ITrackingAttendenceService TrackingAttendenceService, ISurveyService surveyService, IDataUserService _dataUserService)
        {
            this.service = service;
            this.userDataMappingService = userDataMappingService;
            //this.aspNetUserService = aspNetUserService;
            this.chanelDataService = chanelDataService;
            this.levelService = levelService;
            this.logDiaryService = logDiaryService;
            _TrackingAttendenceService = TrackingAttendenceService;
            _userManager = userManager;
            this.surveyService = surveyService;
            this.dataUserService = _dataUserService;
        }
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
        //
        // GET: /Report/Staff/
        [Authorize]
        public ActionResult Index()
        {
            GetCookie();
            var liststaff = _userManager.Users.ToList().Where(p => p.BrandId == _BRAND_ID && !p.Deleted).OrderByDescending(p => p.DateCreated).ToList();
            return View(liststaff);
        }
        [HttpPost]
        public string GetTotalCall(string fromdate, string todate)
        {
            #region required date
            //required date
            if (todate != null && fromdate == null)
                fromdate = todate;
            if (fromdate != null && todate == null)
                todate = fromdate;

            if (todate == null && fromdate == null)
            {
                fromdate = DateTime.Now.Date.ToShortDateString();
                todate = DateTime.Now.Date.ToShortDateString();
            }
            if (Convert.ToDateTime(DateTime.Parse(todate).ToShortDateString()) < Convert.ToDateTime(DateTime.Parse(fromdate).ToShortDateString()))
            {
                var temp = todate;
                todate = fromdate;
                fromdate = temp;
            }
            #endregion

            GetCookie();
            var fromDate = DateTime.Parse(fromdate);
            var toDate = DateTime.Parse(todate);
            var listLog = logDiaryService.GetLogdiarys().Where(p => p.DateCreated.Month == fromDate.Month && p.DateCreated.Year == fromDate.Year && p.DataUser.BrandId == _BRAND_ID && (p.StatusCall.Equals("ANSWERED") || p.StatusCall.Equals("NO ANSWER") || p.StatusCall.Equals("BUSY"))).ToList();
            //if (todate != null)
            //{
            //    listLog = logDiaryService.GetLogdiarys().Where(p => Convert.ToDateTime(p.DateCreated.ToShortDateString()) >= Convert.ToDateTime(fromDate.ToShortDateString()) && Convert.ToDateTime(p.DateCreated.ToShortDateString()) <= Convert.ToDateTime(toDate.ToShortDateString()) && p.DataUser.BrandId == _BRAND_ID && (p.StatusCall.Equals("ANSWERED") || p.StatusCall.Equals("NO ANSWER"))).ToList();
            //}
            //else {
            //    listLog = logDiaryService.GetLogdiarys().Where(p => p.DateCreated.ToShortDateString() == fromDate.ToShortDateString() && p.DataUser.BrandId == _BRAND_ID && (p.StatusCall.Equals("ANSWERED") || p.StatusCall.Equals("NO ANSWER"))).ToList();
            //}

            TotalCallModel list = new TotalCallModel();
            var listAttend = new List<TrackingAttendence>();
            if (todate != null)
                listAttend = _TrackingAttendenceService.GetTrackingAttendences().Where(p => Convert.ToDateTime(p.DateCreated.ToShortDateString()) >= Convert.ToDateTime(fromDate.ToShortDateString()) && Convert.ToDateTime(p.DateCreated.ToShortDateString()) <= Convert.ToDateTime(toDate.ToShortDateString()) && p.IsActive && !p.IsDeleted && p.DurationDate != null).ToList();
            else
                listAttend = _TrackingAttendenceService.GetTrackingAttendences().Where(p => p.DateCreated.Date == fromDate.Date && p.IsActive && !p.IsDeleted && p.DurationDate != null).ToList();

            var listUser = _userManager.Users.Where(p => p.BrandId == _BRAND_ID && !p.Deleted && p.Activated).OrderByDescending(p => p.DateCreated).ToList();
            var listData = dataUserService.GetDataUsers().Where(p => p.BrandId == _BRAND_ID);//Convert.ToDateTime(p.DateCreated).Date == Date_parsing.Date &&

            var firstDayOfMonth = new DateTime(fromDate.Year, fromDate.Month, 1);
            var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
            foreach (var item in listUser)
            {
                #region get totalCall in day
                var log = new List<Logdiary>();
                if (listLog != null && listLog.Count() > 0)
                {
                    if (todate != null)
                        log = listLog.Where(p => Convert.ToDateTime(p.DateCreated.ToShortDateString()) >= Convert.ToDateTime(fromDate.ToShortDateString()) && Convert.ToDateTime(p.DateCreated.ToShortDateString()) <= Convert.ToDateTime(toDate.ToShortDateString()) && p.TeleId.Equals(item.Id)).ToList();
                    else
                        log = listLog.Where(p => p.DateCreated.Day == fromDate.Day && p.TeleId.Equals(item.Id)).ToList();
                }
                if (log != null && log.Count() > 0)
                {
                    CallModel obj = new CallModel();
                    obj.TeleId = item.Id;
                    obj.TeleName = item.DisplayName;
                    if (todate != null)
                        obj.TotalCall = listLog.Where(p => Convert.ToDateTime(p.DateCreated.ToShortDateString()) >= Convert.ToDateTime(fromDate.ToShortDateString()) && Convert.ToDateTime(p.DateCreated.ToShortDateString()) <= Convert.ToDateTime(toDate.ToShortDateString()) && p.TeleId.Equals(obj.TeleId)).ToList().Count();
                    else
                        obj.TotalCall = listLog.Where(p => p.DateCreated.Day == fromDate.Day && p.TeleId.Equals(obj.TeleId)).ToList().Count();

                    obj.TotalDataUser = listData.Where(p => p.UserName.Equals(item.UserName)).Count();
                    if (obj.TotalDataUser == 0)
                    {
                        obj.TotalCall = 0;
                        obj.TotalDataUser = 1;
                    }
                    var listLogRealCall = new List<Logdiary>();
                    if (todate != null)
                        listLogRealCall = listLog.Where(p => Convert.ToDateTime(p.DateCreated.ToShortDateString()) >= Convert.ToDateTime(fromDate.ToShortDateString()) && Convert.ToDateTime(p.DateCreated.ToShortDateString()) <= Convert.ToDateTime(toDate.ToShortDateString()) && p.TeleId.Equals(obj.TeleId)).ToList();
                    else
                        listLogRealCall = listLog.Where(p => p.DateCreated.Day == fromDate.Day && p.TeleId.Equals(obj.TeleId)).ToList();

                    foreach (var RealCall in listLogRealCall)
                    {
                        obj.RealCall += RealCall.TotalCallSucceed;
                    }
                    list.ListTotalCall.Add(obj);
                }
                else
                {
                    CallModel obj = new CallModel();
                    obj.TeleId = item.Id;
                    obj.TeleName = item.DisplayName;
                    obj.TotalCall = 0;
                    obj.TotalDataUser = listData.Where(p => p.UserName.Equals(item.UserName)).Count();
                    if (obj.TotalDataUser == 0)
                    {
                        obj.TotalDataUser = 1;
                    }
                    list.ListTotalCall.Add(obj);
                }
                #endregion
                #region get attendance
                if (listAttend != null && listAttend.Count() > 0)
                {
                    foreach (var attend in listAttend.Where(p => p.TeleId.Equals(item.Id)).ToList())
                    {
                        var temp = attend.DurationDate.Value;
                        var obj = list.ListTotalCall.Where(p => p.TeleId.Equals(item.Id)).FirstOrDefault();
                        if (obj != null)
                        {
                            obj.Duration = obj.Duration.Add(temp.TimeOfDay);
                            list.ListTotalCall.Where(p => p.TeleId.Equals(item.Id)).FirstOrDefault().Duration = obj.Duration;
                        }
                        else
                        {
                            obj = new CallModel();
                            obj.Duration += obj.Duration.Add(temp.TimeOfDay);
                            obj.TeleId = item.Id;
                            obj.TeleName = item.DisplayName;
                            obj.TotalCall = 0;
                            list.ListTotalCall.Add(obj);
                        }
                    }
                }
                else
                {
                    var a = new TimeSpan(0, 0, 0, 0);
                    var obj = list.ListTotalCall.Where(p => p.TeleId.Equals(item.Id)).FirstOrDefault();
                    if (obj != null)
                    {
                        list.ListTotalCall.Where(p => p.TeleId.Equals(item.Id)).FirstOrDefault().Duration.Add(a);
                    }
                    else
                    {
                        obj = new CallModel();
                        obj.Duration += obj.Duration.Add(a);
                        obj.TeleId = item.Id;
                        obj.TeleName = item.DisplayName;
                        obj.TotalCall = 0;
                        list.ListTotalCall.Add(obj);
                    }
                }
                #endregion

                #region total Call in month with staffs and chart
                var numDay = (lastDayOfMonth.Day - firstDayOfMonth.Day) + 1;
                list.NumDay = numDay;
                StaffCallMonth staffMonth = new StaffCallMonth();
                staffMonth.name = item.UserName;
                for (int i = 1; i <= numDay; i++)
                {
                    staffMonth.data.Add(listLog.Where(p => p.TeleName.Contains(item.FirstName) && p.DateCreated.Day == firstDayOfMonth.Day).Count());
                    firstDayOfMonth = firstDayOfMonth.Date.AddDays(1);
                }
                firstDayOfMonth = new DateTime(fromDate.Year, fromDate.Month, 1);
                list.LineChartCallMonthStaffs.Add(staffMonth);
            }
            #endregion
            return JsonConvert.SerializeObject(list);
        }
        [HttpPost]
        public string GetCallReport(string fromdate, string todate)
        {
            GetCookie();
            var fromDate = DateTime.Parse(fromdate);
            var toDate = DateTime.Parse(todate);
            var listLog = logDiaryService.GetLogdiarys().Where(p => p.DateCreated.Month == fromDate.Month && p.DateCreated.Year == fromDate.Year && p.DataUser.BrandId == _BRAND_ID).ToList();
            //if (todate != null)new List<Logdiary>();
            //{
            //    listLog = logDiaryService.GetLogdiarys().Where(p => Convert.ToDateTime(p.DateCreated.ToShortDateString()) >= Convert.ToDateTime(fromDate.ToShortDateString()) && Convert.ToDateTime(p.DateCreated.ToShortDateString()) <= Convert.ToDateTime(toDate.ToShortDateString()) && p.DataUser.BrandId == _BRAND_ID).ToList();
            //}
            //else
            //{
            //    listLog = logDiaryService.GetLogdiarys().Where(p => p.DateCreated.Day == fromDate.Day && p.DataUser.BrandId == _BRAND_ID).ToList();
            //}

            var listUser = _userManager.Users.Where(p => p.BrandId == _BRAND_ID && !p.Deleted && p.Activated).OrderByDescending(p => p.DateCreated).ToList();

            //TotalCallModel list = new TotalCallModel();
            var list = new
            {
                DayModel = new List<TotalCallReport>(),
                MonthModel = new List<TotalCallReport>()
            };
            foreach (var item in listUser)
            {
                TotalCallReport callDay = new TotalCallReport();
                //report day
                var _1min = 0;
                var _2min = 0;
                var _3min = 0;
                var _4min = 0;
                //var callTotal = 0;
                var listLogDay = new List<Logdiary>();
                if (todate != null)
                {
                    listLogDay = listLog.Where(p => Convert.ToDateTime(p.DateCreated.ToShortDateString()) >= Convert.ToDateTime(fromDate.ToShortDateString()) && Convert.ToDateTime(p.DateCreated.ToShortDateString()) <= Convert.ToDateTime(toDate.ToShortDateString()) && p.TeleId.Equals(item.Id) && p.StatusCall.Equals("ANSWERED")).ToList();
                    callDay.TotalNoAnswer = listLog.Where(p => Convert.ToDateTime(p.DateCreated.ToShortDateString()) >= Convert.ToDateTime(fromDate.ToShortDateString()) && Convert.ToDateTime(p.DateCreated.ToShortDateString()) <= Convert.ToDateTime(toDate.ToShortDateString()) && p.TeleId.Equals(item.Id) && (p.StatusCall.Equals("NO ANSWER") || p.StatusCall.Equals("BUSY"))).ToList().Count();
                    callDay.TotalCall = listLog.Where(p => Convert.ToDateTime(p.DateCreated.ToShortDateString()) >= Convert.ToDateTime(fromDate.ToShortDateString()) && Convert.ToDateTime(p.DateCreated.ToShortDateString()) <= Convert.ToDateTime(toDate.ToShortDateString()) && p.TeleId.Equals(item.Id) && (p.StatusCall.Equals("ANSWERED") || p.StatusCall.Equals("NO ANSWER") || p.StatusCall.Equals("BUSY"))).Count();
                    callDay.TotalNA = listLog.Where(p => Convert.ToDateTime(p.DateCreated.ToShortDateString()) >= Convert.ToDateTime(fromDate.ToShortDateString()) && Convert.ToDateTime(p.DateCreated.ToShortDateString()) <= Convert.ToDateTime(toDate.ToShortDateString()) && p.TeleId.Equals(item.Id) && p.StatusCall.Equals("N/A")).ToList().Count();
                    callDay.TotalBuy = listLog.Where(p => Convert.ToDateTime(p.DateCreated.ToShortDateString()) >= Convert.ToDateTime(fromDate.ToShortDateString()) && Convert.ToDateTime(p.DateCreated.ToShortDateString()) <= Convert.ToDateTime(toDate.ToShortDateString()) && p.TeleId.Equals(item.Id) && p.StatusCall.Equals("BUY PRODUCT")).ToList().Count();
                }
                else
                {
                    listLogDay = listLog.Where(p => p.DateCreated.Day == fromDate.Day && p.TeleId.Equals(item.Id) && p.StatusCall.Equals("ANSWERED")).ToList();
                    callDay.TotalNoAnswer = listLog.Where(p => p.DateCreated.Day == fromDate.Day && p.TeleId.Equals(item.Id) && p.TeleId.Equals(item.Id) && (p.StatusCall.Equals("NO ANSWER") || p.StatusCall.Equals("BUSY"))).ToList().Count();
                    callDay.TotalCall = listLog.Where(p => p.DateCreated.Day == fromDate.Day && p.TeleId.Equals(item.Id) && (p.StatusCall.Equals("ANSWERED") || p.StatusCall.Equals("NO ANSWER") || p.StatusCall.Equals("BUSY"))).Count();
                    callDay.TotalNA = listLog.Where(p => p.DateCreated.Day == fromDate.Day && p.TeleId.Equals(item.Id) && p.StatusCall.Equals("N/A")).Count();
                    callDay.TotalBuy = listLog.Where(p => p.DateCreated.Day == fromDate.Day && p.TeleId.Equals(item.Id) && p.StatusCall.Equals("BUY PRODUCT")).Count();
                }

                foreach (var to in listLogDay)
                {
                    if (((float)to.TotalCallSucceed / 60) <= 1)
                    {
                        _1min++;
                    }
                    if (((float)to.TotalCallSucceed / 60) > 1 && ((float)to.TotalCallSucceed / 60) <= 2)
                    {
                        _2min++;
                    }
                    if (((float)to.TotalCallSucceed / 60) > 2 && ((float)to.TotalCallSucceed / 60) <= 3)
                    {
                        _3min++;
                    }
                    if (((float)to.TotalCallSucceed / 60) > 3)
                    {
                        _4min++;
                    }
                    //callTotal++;
                }
                callDay.TeleName = item.DisplayName;
                callDay.TotalLess1min = _1min;
                callDay.TotalLess2min = _2min;
                callDay.TotalLess3min = _3min;
                callDay.TotalMore4min = _4min;
                //callDay.TotalCall = callTotal;

                list.DayModel.Add(callDay);

                //report month
                TotalCallReport callMonth = new TotalCallReport();
                _1min = 0;
                _2min = 0;
                _3min = 0;
                _4min = 0;
                //callTotal = 0;
                var listLogMonth = listLog.Where(p => p.TeleId.Equals(item.Id) && p.StatusCall.Equals("ANSWERED")).ToList();
                callMonth.TotalNoAnswer = listLog.Where(p => p.TeleId.Equals(item.Id) && (p.StatusCall.Equals("NO ANSWER") || p.StatusCall.Equals("BUSY"))).ToList().Count();
                callMonth.TotalCall = listLog.Where(p => p.TeleId.Equals(item.Id) && (p.StatusCall.Equals("ANSWERED") || p.StatusCall.Equals("NO ANSWER") || p.StatusCall.Equals("BUSY"))).Count();
                callMonth.TotalNA = listLog.Where(p => p.TeleId.Equals(item.Id) && p.StatusCall.Equals("N/A")).Count();
                callMonth.TotalBuy = listLog.Where(p => p.TeleId.Equals(item.Id) && p.StatusCall.Equals("BUY PRODUCT")).Count();
                foreach (var to in listLogMonth)
                {
                    if (((float)to.TotalCallSucceed / 60) <= 1)
                    {
                        _1min++;
                    }
                    if (((float)to.TotalCallSucceed / 60) > 1 && ((float)to.TotalCallSucceed / 60) <= 2)
                    {
                        _2min++;
                    }
                    if (((float)to.TotalCallSucceed / 60) > 2 && ((float)to.TotalCallSucceed / 60) <= 3)
                    {
                        _3min++;
                    }
                    if (((float)to.TotalCallSucceed / 60) > 3)
                    {
                        _4min++;
                    }
                    //callTotal++;
                }
                callMonth.TeleName = item.DisplayName;
                callMonth.TotalLess1min = _1min;
                callMonth.TotalLess2min = _2min;
                callMonth.TotalLess3min = _3min;
                callMonth.TotalMore4min = _4min;
                //callMonth.TotalCall = callTotal;

                list.MonthModel.Add(callMonth);
            }


            return JsonConvert.SerializeObject(list);
        }
        //public ActionResult Detail(string username)
        public ActionResult Detail(string Id)
        {
            GetCookie();

            //var listData = userDataMappingService.GetUserDataMappings().Where(d => d.User.Id.Equals(username));
            //var user = _userManager.Users.Where(p => !p.Deleted && p.Activated && p.Id.Equals(username)).FirstOrDefault();
            //Session["BrandId"] = Session["BrandId"] == null ? 0 : Session["BrandId"];
            var BrandId = _BRAND_ID;
            ViewBag.Id = Id;
            var user = _userManager.Users.Where(p => !p.Deleted && p.Activated && p.Id.Equals(Id)).FirstOrDefault();
            var logTele = logDiaryService.GetLogdiarys().Where(p => p.TeleId.Equals(user.Id) && p.DateCreated.Month == DateTime.Now.Month && p.DateCreated.Year == DateTime.Now.Year);
            //var listDataUsers = service.GetDataUsers().Where(p => !p.Deleted && p.BrandId == 1).ToList();

            var listDataUsers = new List<DataUser>();
            if (BrandId == 0)
            {
                listDataUsers = service.GetDataUsers().Where(p => !p.Deleted).ToList();
            }
            else
            {
                listDataUsers = service.GetDataUsers().Where(p => !p.Deleted && p.BrandId == BrandId).ToList();
            }
            var listIdDataUsers = new List<int>();
            foreach (var idMap in listDataUsers)
            {
                listIdDataUsers.Add(idMap.Id);
            }
            var listData = userDataMappingService.GetUserDataMappings().Where(d => d.UserId.Equals(Id) && listIdDataUsers.Contains(d.DataUserId));//d.User.Id.Equals(Id)
            List<UserDataMapping> listUserDataFormModel = new List<UserDataMapping>();
            foreach (var item in listData)
            {
                UserDataMapping userDataMappingFormModel = Mapper.Map<UserDataMapping>(item);
                listUserDataFormModel.Add(userDataMappingFormModel);
            }
            //var listSurveys = surveyService.GetSurveys().Where(s => s.Logdiary.TeleId.Equals(user.Id)).OrderBy(l => l.LogdiaryId);
            //ViewBag.TotalSurvey = listSurveys.Count();
            //var totalCall = logDiaryService.GetLogdiarys().Where(l => l.StatusCall.Equals("ANSWERED") && l.TeleId.Equals(user.Id)).Count();

            var totalCallAnswer = logTele.Where(l => l.StatusCall.Equals("ANSWERED") && listIdDataUsers.Contains(l.UserDataId)).Count();
            ViewBag.TotalCallAnswer = totalCallAnswer;
            var dateTime = Convert.ToDateTime(DateTime.Now.ToLongDateString() + " 00:00:00.000");

            var totalAmountMonth = (double)logTele.Where(l => l.StatusCall.Equals("BUY PRODUCT")).Sum(l => l.Price_1);
            ViewBag.TotalAmounMonth = totalAmountMonth.ToString("#,##0");
            var totalOrder = (double)logTele.Where(l => l.StatusCall.Equals("BUY PRODUCT")).Count();
            ViewBag.totalOrder = totalOrder;

            ViewBag.LevelModel = levelService.GetLevels();
            return View(model: listUserDataFormModel);
        }

        public ActionResult GetDetailDayReport(string Id, string lastDate)//
        {
            GetCookie();

            //var listData = userDataMappingService.GetUserDataMappings().Where(d => d.User.Id.Equals(username));
            //var user = _userManager.Users.Where(p => !p.Deleted && p.Activated && p.Id.Equals(username)).FirstOrDefault();
            var dateTo = DateTime.Parse(lastDate.Split('/')[1] + "/" + lastDate.Split('/')[0] + "/" + lastDate.Split('/')[2]);
            var BrandId = _BRAND_ID;
            ViewBag.Id = Id;
            //var listDataUsers = service.GetDataUsers().Where(p => !p.Deleted && p.BrandId == 1).ToList();

            var listDataUsers = new List<DataUser>();
            if (BrandId == 0)
            {
                listDataUsers = service.GetDataUsers().Where(p => !p.Deleted).ToList();
            }
            else
            {
                listDataUsers = service.GetDataUsers().Where(p => !p.Deleted && p.BrandId == BrandId).ToList();
            }
            var listIdDataUsers = new List<int>();
            foreach (var idMap in listDataUsers)
            {
                listIdDataUsers.Add(idMap.Id);
            }

            var listData = userDataMappingService.GetUserDataMappings().Where(d => d.UserId.Equals(Id) && d.DateCreated.Month == dateTo.Month && d.DateCreated.Year == dateTo.Year && listIdDataUsers.Contains(d.DataUserId));//d.User.Id.Equals(Id)
            //var user = _userManager.Users.Where(p => !p.Deleted && p.Activated && p.Id.Equals(Id)).FirstOrDefault();
            List<UserDataMapping> listUserDataFormModel = new List<UserDataMapping>();
            foreach (var item in listData)
            {
                UserDataMapping userDataMappingFormModel = Mapper.Map<UserDataMapping>(item);
                listUserDataFormModel.Add(userDataMappingFormModel);
            }
            //var listSurveys = surveyService.GetSurveys().Where(s => s.Logdiary.TeleId.Equals(user.Id)).OrderBy(l => l.LogdiaryId);
            //ViewBag.TotalSurvey = listSurveys.Count();
            //var totalCall = logDiaryService.GetLogdiarys().Where(l => l.StatusCall.Equals("ANSWERED") && l.TeleId.Equals(user.Id)).Count();
            //var totalCall = logDiaryService.GetLogdiarys().Where(l => l.StatusCall.Equals("ANSWERED") && l.TeleId.Equals(user.Id) && listIdDataUsers.Contains(l.UserDataId)).Count();
            //ViewBag.TotalCall = totalCall;
            //var dateTime = Convert.ToDateTime(DateTime.Now.ToLongDateString() + " 00:00:00.000");

            //var totalAmountDay = (double)logDiaryService.GetLogdiarys().Where(l => l.TeleId.Equals(user.Id) && l.DateCreated >= dateTime).Sum(l => l.Price_1);
            //ViewBag.TotalAmounDay = totalAmountDay;
            //var totalAmount = (double)logDiaryService.GetLogdiarys().Where(l => l.TeleId.Equals(user.Id)).Sum(l => l.Price_1);
            //ViewBag.TotalAmountAll = totalAmount;

            ViewBag.LevelModel = levelService.GetLevels();
            return PartialView("_ReportCustomer", listUserDataFormModel);
            //return JsonConvert.SerializeObject(listUserDataFormModel);
        }
        [HttpPost]
        public string UpdateBlock(string Id, string lastDate)
        {
            GetCookie();
            var user = _userManager.Users.Where(p => !p.Deleted && p.Activated && p.Id.Equals(Id)).FirstOrDefault();
            var dateTo = DateTime.Parse(lastDate.Split('/')[1] + "/" + lastDate.Split('/')[0] + "/" + lastDate.Split('/')[2]);
            var BrandId = _BRAND_ID;
            var listDataUsers = new List<DataUser>();
            if (BrandId == 0)
            {
                listDataUsers = service.GetDataUsers().Where(p => !p.Deleted).ToList();
            }
            else
            {
                listDataUsers = service.GetDataUsers().Where(p => !p.Deleted && p.BrandId == BrandId).ToList();
            }
            var listIdDataUsers = new List<int>();
            foreach (var idMap in listDataUsers)
            {
                listIdDataUsers.Add(idMap.Id);
            }
            Dictionary<string, string> listDic = new Dictionary<string, string>();
            var logTele = logDiaryService.GetLogdiarys().Where(p => p.TeleId.Equals(user.Id) && p.DateCreated.Month == dateTo.Month && p.DateCreated.Year == dateTo.Year);
            var listData = userDataMappingService.GetUserDataMappings().Where(d => d.UserId.Equals(Id) && d.DateCreated.Month == dateTo.Month && d.DateCreated.Year == dateTo.Year && listIdDataUsers.Contains(d.DataUserId));//d.User.Id.Equals(Id)

            var totalCallAnswer = logTele.Where(l => l.StatusCall.Equals("ANSWERED") && listIdDataUsers.Contains(l.UserDataId)).Count();
            listDic.Add("TotalCallAnswer", totalCallAnswer.ToString());
            var totalAmountMonth = (double)logTele.Where(l => l.StatusCall.Equals("BUY PRODUCT")).Sum(l => l.Price_1);
            listDic.Add("totalAmountMonth", totalAmountMonth.ToString("#,##0"));
            var totalOrder = (double)logTele.Where(l => l.StatusCall.Equals("BUY PRODUCT")).Count();
            listDic.Add("totalOrder", totalOrder.ToString());
            var totalCall = listData.Count();
            listDic.Add("totalCall", totalCall.ToString());

            var json = new JavaScriptSerializer().Serialize(listDic.ToDictionary(item => item.Key.ToString(), item => item.Value.ToString()));
            return JsonConvert.SerializeObject(json);
        }
        public ActionResult SetVariable(string key, string value)
        {
            Session[key] = value;

            return this.Json(new { success = true });
        }
        [HttpPost]
        public string GetCharTotalCall(string Id)
        {
            var model = new ReportModel();

            Session["BrandId"] = Session["BrandId"] == null ? 0 : Session["BrandId"];
            var BrandId = int.Parse(Session["BrandId"].ToString());
            //var Id = "c61ac0b4-18fa-40bd-8ecd-816aa5d4b69c";
            var listMapping = userDataMappingService.GetUserDataMappings().Where(p => p.UserId == Id).ToList();
            List<int> listIdData = new List<int>();
            foreach (var i in listMapping)
            {
                listIdData.Add(i.DataUserId);
            }
            var listDiary = logDiaryService.GetLogdiarys().Where(p => p.DateCreated.Month == DateTime.Now.Month && p.StatusCall != null).ToList();
            var ListData = service.GetDataUsers().Where(p => !p.Deleted && listIdData.Contains(p.Id) && p.BrandId == BrandId).ToList();// 
            var leveId = new List<int>();
            foreach (var lev in ListData)
            {
                leveId.Add(lev.LevelId);
            }
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
            model.TotalData = ListData.Count();
            var tempListLevel = levelService.GetLevels().Where(p => leveId.Contains(p.Id)).ToDictionary(l => l.Id + " " + l.CodeLevel + " (" + l.LevelName + ")", t => (double)0);
            foreach (var item in tempListLevel)
            {
                try
                {
                    double total = ListData.Where(p => p.Level.CodeLevel == item.Key.Split(' ')[1] && p.Level.Id.ToString() == item.Key.Split(' ')[0]).ToList().Count();
                    model.ListTotalEachLevel.Add(item.Key, total);
                }
                catch (Exception ex)
                {

                    throw;
                }
            }

            #endregion

            #region TotalCallWeek

            #endregion
            #region TotalCallMonth
            #endregion
            string output = JsonConvert.SerializeObject(model);
            return output;
        }

    }
    public class CallModel
    {
        public CallModel()
        {
            TotalCall = 0;
            Duration = new TimeSpan(0, 0, 0, 0);
            RealCall = TotalDataUser = 0;
        }
        public string TeleId { get; set; }
        public string TeleName { get; set; }
        public int TotalCall { get; set; }
        public int TotalDataUser { get; set; }
        public TimeSpan Duration { get; set; }
        public int RealCall { get; set; }
    }


    public class StaffCallMonth
    {
        public StaffCallMonth()
        {
            data = new List<int>();
        }
        public string name { get; set; }
        public List<int> data { get; set; }
    }
    public class TotalCallModel
    {
        public TotalCallModel()
        {
            ListTotalCall = new List<CallModel>();
            LineChartCallMonthStaffs = new List<StaffCallMonth>();
            DateValue = new DateTime();
            NumDay = 31;
        }
        public List<CallModel> ListTotalCall { get; set; }
        public List<StaffCallMonth> LineChartCallMonthStaffs { get; set; }
        public DateTime DateValue { get; set; }
        public int NumDay { get; set; }

    }
    public class TotalCallReport
    {
        public string TeleName { get; set; }
        public int TotalLess1min { get; set; }
        public int TotalLess2min { get; set; }
        public int TotalLess3min { get; set; }
        public int TotalMore4min { get; set; }
        public int TotalNoAnswer { get; set; }
        public int TotalCall { get; set; }
        public int TotalNA { get; set; }
        public int TotalBuy { get; set; }
    }
}