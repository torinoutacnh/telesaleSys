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

namespace Labixa.Areas.Autocall.Controllers
{
    public class CallController : Controller
    {
        readonly IDataUserService service;
        readonly IUserDataMappingService userDataMappingService;
        readonly IChanelDataService chanelDataService;
        readonly ILevelService levelService;
        readonly ILogdiaryService logDiaryService;
        private ITrackingAttendenceService _TrackingAttendenceService;
        private UserManager<User> _userManager;
        public CallController(IDataUserService service, IUserDataMappingService userDataMappingService,
                IChanelDataService chanelDataService, ILevelService levelService, UserManager<User> userManager
                , ILogdiaryService logDiaryService, ITrackingAttendenceService TrackingAttendenceService)
        {
            this.service = service;
            this.userDataMappingService = userDataMappingService;
            //this.aspNetUserService = aspNetUserService;
            this.chanelDataService = chanelDataService;
            this.levelService = levelService;
            this.logDiaryService = logDiaryService;
            _TrackingAttendenceService = TrackingAttendenceService;
            _userManager = userManager;
        }
        //
        // GET: /Autocall/Call/
        public ActionResult Index()
        {
            var liststaff = _userManager.Users.ToList().Where(p => !p.Deleted && p.Activated).OrderByDescending(p => p.DateCreated).ToList();
            return View(liststaff);
        }
        public string GetList()
        {
            var list = service.GetDataUsers().Where(p => p.LastCall == null).Select(p => p.PhoneNumber).ToList();
            return JsonConvert.SerializeObject(list);
        }
        [HttpPost]
        public string GetTotalCall(string date)
        {
            var Date_parsing = DateTime.Parse(date);
            var listLog = logDiaryService.GetLogdiarys().Where(p => p.DateCreated.Date == Date_parsing.Date).ToList();

            TotalCallModel list = new TotalCallModel();
            var listAttend = _TrackingAttendenceService.GetTrackingAttendences().Where(p => p.DateCreated.Date == Date_parsing.Date && p.IsActive && !p.IsDeleted && p.DurationDate != null).ToList();
            var listUser = _userManager.Users.Where(p => !p.Deleted && p.Activated).OrderByDescending(p => p.DateCreated).ToList();

            foreach (var item in listUser)
            {
                var log = new List<Logdiary>();
                if (listLog != null && listLog.Count() > 0)
                {
                    log = listLog.Where(p => p.TeleId.Equals(item.Id)).ToList();
                }
                if (log != null && log.Count() > 0)
                {
                    CallModel obj = new CallModel();
                    obj.TeleId = item.Id;
                    obj.TeleName = item.DisplayName;
                    obj.TotalCall = listLog.Where(p => p.TeleId.Equals(obj.TeleId)).ToList().Count();
                    list.ListTotalCall.Add(obj);
                }
                else
                {
                    CallModel obj = new CallModel();
                    obj.TeleId = item.Id;
                    obj.TeleName = item.DisplayName;
                    obj.TotalCall = 0;
                    list.ListTotalCall.Add(obj);
                }
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
            }

            return JsonConvert.SerializeObject(list.ListTotalCall);
        }
    }
    public class CallModel
    {
        public CallModel()
        {
            TotalCall = 0;
            Duration = new TimeSpan(0, 0, 0, 0);
        }
        public string TeleId { get; set; }
        public string TeleName { get; set; }
        public int TotalCall { get; set; }
        public TimeSpan Duration { get; set; }
    }
    public class TotalCallModel
    {
        public TotalCallModel()
        {
            ListTotalCall = new List<CallModel>();
        }
        public List<CallModel> ListTotalCall { get; set; }
    }
}