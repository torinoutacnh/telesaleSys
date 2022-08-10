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

namespace Labixa.Areas.Admin.Controllers
{
    public class autocallController : Controller
    {
        private List<int> currentUserDataMappings = new List<int>();
        readonly IDataUserService service;
        readonly IUserDataMappingService userDataMappingService;
        readonly IChanelDataService chanelDataService;
        readonly ILevelService levelService;
        readonly ILogdiaryService logDiaryService;
        private UserManager<User> _userManager;


        public autocallController(IDataUserService service, IUserDataMappingService userDataMappingService,
            IChanelDataService chanelDataService, ILevelService levelService, UserManager<User> userManager
            , ILogdiaryService logDiaryService)
        {
            this.service = service;

            this.userDataMappingService = userDataMappingService;
            //this.aspNetUserService = aspNetUserService;
            this.chanelDataService = chanelDataService;
            this.levelService = levelService;
            this.logDiaryService = logDiaryService;
            _userManager = userManager;
        }
        //
        // GET: /Admin/autocall/
        public ActionResult Index()
        {
            var user = _userManager.FindById(User.Identity.GetUserId());
            if (user!=null)
            {
            return View(user);
            }
            else
            {
                return Redirect("/Account/Login");
            }
        }
        public ActionResult GetData(string number)
        {

            return View();
        }

        public ActionResult Edit(string phone)
        {
          
            var entity = userDataMappingService.GetUserDataMappings().Where(p => p.DataUser.PhoneNumber.Equals(phone)).FirstOrDefault();
            entity.UserId = User.Identity.GetUserId();
            userDataMappingService.EditUserDataMapping(entity);
            var entity2 = userDataMappingService.GetUserDataMappings().Where(p => p.DataUser.PhoneNumber.Equals(phone)).FirstOrDefault();
            UserDataMappingFormModel userDataMappingFormModel = Mapper.Map<UserDataMapping, UserDataMappingFormModel>(entity2);
            userDataMappingFormModel.ListChanel = chanelDataService.GetChanelDatas().ToSelectListItems(-1);
            userDataMappingFormModel.ListLevel = levelService.GetLevels().ToSelectListItems(entity.DataUser.LevelId);
            //  userDataMappingFormModel.ListLogDiary = logDiaryService.GetLogdiarys().Where(p => p.UserDataId == userDataMappingFormModel.DataUserId).ToList();
            return View(model: userDataMappingFormModel);
        }
        [HttpPost]
        public ActionResult SubmitEdit(UserDataMappingFormModel userDataFromModel, FormCollection data)
        {

            var currData = new DataUser();
            string log = data["respon"], note = data["noteLog"];
            // ican not define my bug
            //if (log[log.Length - 1] == ',') log = log.Remove(log.Length - 1);
            // update Data
            //var data = Mapper.Map<DataUserFormModel, DataUser>(userDataFromModel.DataUser);
            // get data form DB
            currData = service.GetDataUserById(userDataFromModel.DataUserId);
            bool IsUpdate = false;
            if (currData.LevelId != userDataFromModel.DataUser.LevelId)
            {
                IsUpdate = true;
            }
            // map
            currData.FirstName = userDataFromModel.DataUser.FirstName;
            currData.LastName = userDataFromModel.DataUser.LastName;
            currData.PhoneNumber = userDataFromModel.DataUser.PhoneNumber;
            currData.Email = userDataFromModel.DataUser.Email;
            currData.Sex = userDataFromModel.DataUser.Sex;
            currData.ChannelDataId = userDataFromModel.DataUser.ChannelDataId;
            currData.LevelId = userDataFromModel.DataUser.LevelId;
            currData.Noted = userDataFromModel.DataUser.Noted;
            currData.Price_1 = userDataFromModel.DataUser.Price_1;
            currData.LastEditedTime = DateTime.Now;
            currData.TotalInteractive += 1;

            // update log if calling

            if (log != "")
            {
                //update log
                var logDiary = JsonConvert.DeserializeObject<LogHistory>(log);
                UpdateContent(note, userDataFromModel.DataUserId, logDiary, userDataFromModel.DataUser.LevelId);
                // has call -> change last call
                if (logDiary.ngayGoi != "")
                {
                    currData.LastCall = Convert.ToDateTime(logDiary.ngayGoi);
                }
            }
            
            service.EditDataUser(currData);
            // add to log history if level change
            LogHistory logHistory = new LogHistory();
            //if (log != null)
            if (IsUpdate)
            {
                var level = levelService.GetLevelById(userDataFromModel.DataUser.LevelId);
                // logHistory = new JavaScriptSerializer().Deserialize<LogHistory>(log);
                Logdiary logDiary = new Logdiary()
                {
                    Content = "Cập Nhật trạng thái thành " + currData.Level.CodeLevel + " - " + currData.Level.LevelName,
                    DateCreated = DateTime.Now,
                    Noted = note,
                    IsActive = true,
                    UserDataId = userDataFromModel.DataUserId,
                    LastEditedTime = DateTime.Now,
                    StatusCall = "ANSWERED",
                    LinkFile = "https://google.com",
                    TotalCall_Second = 10,
                    LevelId = userDataFromModel.DataUser.LevelId,
                    NameLevel = level.CodeLevel + "-" + level.LevelName,
                    temp1 = userDataFromModel.UserId,
                    TeleName = userDataFromModel.User.FirstName + " " + userDataFromModel.User.LastName,
                    TeleId = User.Identity.GetUserId()

                };
                logDiaryService.CreateLogdiary(logDiary);
            }
            //return RedirectToAction("Edit", "autocall", new { phone = currData.PhoneNumber });
            return RedirectToAction("Close", "autocall");
        }
        public ActionResult Close()
        {
            return View();
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
        public void UpdateContent(string content, int DataUserId, LogHistory logHistory, int LevelId = 0)
        {
            Logdiary obj = new Logdiary();
            obj.Content = content;
            obj.DateCreated = DateTime.Now;
            obj.IsActive = true;
            obj.LastEditedTime = DateTime.Now;
            obj.Deleted = false;
            obj.Position = 0;
            //LogHistory logHistory = (LogHistory)(new JavaScriptSerializer().DeserializeObject(resultLog));
            obj.CodeLogDiary = logHistory.key;
            obj.LinkFile = logHistory.linkFile;
            obj.StatusCall = logHistory.trangThai;
            obj.TotalCallSucceed += StringConvert.ToSeconds(logHistory.thoiGianThucGoi);
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