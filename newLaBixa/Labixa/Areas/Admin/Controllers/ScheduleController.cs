using AutoMapper;
using Labixa.Areas.Admin.ViewModel;
using Microsoft.AspNet.Identity;
using Outsourcing.Data.Models;
using Outsourcing.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using System.Security.Claims;
using System.Threading.Tasks;
using Outsourcing.Core.Extensions;
using Outsourcing.Core.Framework.Controllers;
using System.Globalization;

namespace Labixa.Areas.Admin.Controllers
{
    public class ScheduleController : BaseController
    {
        readonly IScheduleService _ScheduleService;
        readonly IUserDataMappingService _UserDataMappingService;

        readonly IDataUserService _DataUserService;
        readonly IStoreService _StoreService;
        private UserManager<User> _userManager;

        public object CultureInfor { get; private set; }

        public ScheduleController(IUserDataMappingService userDataMappingService,IScheduleService ScheduleService, UserManager<User> userManager, IDataUserService DataUserService, IStoreService StoreService)
        {
            _ScheduleService = ScheduleService;
            _DataUserService = DataUserService;
            _UserDataMappingService = userDataMappingService;
            _userManager = userManager;
            _StoreService = StoreService;
        }
        // GET: /Admin/Schedule/
        public ActionResult Index()
        {
            var list = _ScheduleService.GetSchedules().ToList();
            return View(model: list);
        }
        [HttpGet]
        public ActionResult Create()
        {
            var user = _userManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                ViewBag.first = user.Extention;
                ViewBag.role2 = _userManager.GetRoles(User.Identity.GetUserId()).FirstOrDefault();
                //ViewBag.role = User.IsInRole("Team Lead");
            }
            var listDataUser = _DataUserService.GetDataUsers().ToSelectListItems(-1);
            var list = new ScheduleFormModel { ListDataUser = listDataUser };
            return View(list);
            //ViewBag.first = User.Identity.GetUserName();
            //var mode = new ScheduleFormModel();
            //mode.Status = 1;
            //mode.IsActive = true;
            //return View(mode);
        }
        
        [Authorize]
        [ValidateInput(false)]
        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public ActionResult Create(ScheduleFormModel newSchedule, bool continueEditing)
        {
            if (ModelState.IsValid)
            {

                //
                newSchedule.CreateDate = DateTime.Now;
                newSchedule.Deadline = DateTime.Now;
                newSchedule.EditDate = DateTime.Now;
                var item = Mapper.Map<ScheduleFormModel, Schedule>(newSchedule);
                _ScheduleService.CreateSchedule(item);
                return continueEditing ? RedirectToAction("Edit", "Schedule", new { id = item.Id })
                                    : RedirectToAction("Index", "Schedule");
            }
            else
            {
                return View("Create", newSchedule);
            }
        }
        public ActionResult Delete(int id)
        {

            var item = _ScheduleService.GetScheduleById(id);
            item.IsDeleted = true;
            _ScheduleService.EditSchedule(item);
            return RedirectToAction("Index", "Schedule");
        }

        public ActionResult Edit(int id)
        {
            var Schedule = _ScheduleService.GetScheduleById(id); 
            ScheduleFormModel ScheduleFormModel = Mapper.Map<Schedule, ScheduleFormModel>(Schedule);
            ScheduleFormModel.ListDataUser = _DataUserService.GetDataUsers().ToSelectListItems(-1);
            ViewBag.Stores = _StoreService.GetStores();
            return View(model: ScheduleFormModel);
        }
        public ActionResult EditSchedule(int id)
        {
                var code = id + "_" + DateTime.Now.ToString();
                Schedule s = new Schedule()
                {
                    Active = false,
                    CreateDate = DateTime.Now,
                    EditDate = DateTime.Now,
                    UserDataId = id,
                    IsDeleted = false,
                    Code = code,
                    Deadline = DateTime.Now,
                };
                _ScheduleService.CreateSchedule(s);
               var Schedule = _ScheduleService.GetLastScheduleByCode(code);
            return RedirectToAction("Edit", "Schedule", new { id = Schedule.Id });
        }


        [ValidateInput(false)]
        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public ActionResult Edit(ScheduleFormModel obj, bool continueEditing, FormCollection form)
        {
            if (ModelState.IsValid)
            {
                //var deadline = form["deadline"];
                var loaiXe = form["loaiXe"];
                var bienSoXe = form["bienSoXe"];
                var ngayGio = form["ngayGio"];
                var loaiDichVu = form["loaiDichVu"];
                var chiNhanh = form["chiNhanh"];
                Schedule item = Mapper.Map<ScheduleFormModel, Schedule>(obj);
                try
                {
                    //item.Deadline = DateTime.Parse(deadline);
                    item.LoaiXe = loaiXe;
                    item.BienSoXe = bienSoXe;
                    item.NgayGioHen = DateTime.Parse(ngayGio);
                    item.LoaiDichVu = loaiDichVu; //missing
                    item.AddressStore = chiNhanh;
                }
                catch (Exception){}               
                item.EditDate = DateTime.Now;
                item.Active = true;
                _ScheduleService.EditSchedule(item);
                return continueEditing ? RedirectToAction("Edit", "Schedule", new { id = item.Id })
                    : RedirectToAction("Index", "Schedule");
            }
            else
                return View("Edit", obj);
        }
        public string HideSchedule(int ID)
        {
            if (ID!=0)
            {

            var schedule = _ScheduleService.GetScheduleById(ID);
                if (schedule!=null)
                {
                    schedule.Active = false;
                    _ScheduleService.EditSchedule(schedule);
                }
            }
            return "OK";
        }
        public ActionResult PopupSchedule()
        {
            //var list = service.GetAll().Where(p=>p.Deadline.Equals(DateTime.Now) && p.UserDataMapping.AspNetUserId=="id hien tai cua user")
            var list = new List<Schedule>();
            var listMaping = _UserDataMappingService.GetUserDataMappings().Where(p=>p.UserId.Equals(User.Identity.GetUserId())).ToList();
            foreach (var item in listMaping)
            {
                if (item.Schedules!=null && item.Schedules!=null)
                {
                    try
                    {
                        foreach (var schedule in item.Schedules.Where(p => p.CreateDate.Value.Date == DateTime.Now.Date && p.Active==true).ToList())
                        {
                            list.Add(schedule);
                        }
                    }
                    catch (Exception)
                    {
                        continue;
                    }
                }
            }
            return PartialView("_PartialSchedulePopup",list);
        }
    }

    public interface ISheduleService
    {
    }
}