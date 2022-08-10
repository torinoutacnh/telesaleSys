//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Data.Entity;
//using System.Linq;
//using System.Net;
//using System.Text;
//using System.Web;
//using System.Web.Mvc;
//using Outsourcing.Data.Models;
//using Outsourcing.Service;

//namespace Labixa.Areas.Admin.Controllers
//{
//    public class AspNetUserController : BaseController
//    {
//        readonly IAspNetUserService service;
//        readonly IAspNetRoleService roleService;

//        public AspNetUserController(IAspNetUserService service, IAspNetRoleService aspNetRole)
//        {
//            this.service = service;
//            this.roleService = aspNetRole;
//        }
        
//        // GET: /Admin/AspNetUsers/
//        public ActionResult Index()
//        {
//            var entities = service.GetAll().ToList();
//            return View(entities);
//        }

//        // GET: /Admin/AspNetUsers/Details/5
//        public ActionResult Details(string id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            AspNetUser aspNetUser = service.GetById(id);
//            if (aspNetUser == null)
//            {
//                return HttpNotFound();
//            }
//            return View(aspNetUser);
//        }

//        // GET: /Admin/AspNetUsers/Create
//        public ActionResult Create()
//        {
//            var roles = roleService.GetAll();
//            ViewData["roles"] = roles;
//            return View();

//        }

//        // POST: /Admin/AspNetUsers/Create
//        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
//        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult Create([Bind(Include = "Id,FirstName,LastName,Address,Gender,RoleId,DateOfBirth,Email,PasswordHash,PhoneNumber,UserName")] AspNetUser aspNetUser)
//        {
//            bool IsSuccess = false;
//            if (ModelState.IsValid)
//            {
//                IsSuccess = service.Created(aspNetUser);
//                ViewData["IsSuccess"] = IsSuccess;
//                return RedirectToAction("Index");
//            }
//            return View(aspNetUser);
//        }

//        // GET: /Admin/AspNetUsers/Edit/5
//        public ActionResult Edit(string id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            AspNetUser aspNetUser = service.GetById(id);
//            if (aspNetUser == null)
//            {
//                return HttpNotFound();
//            }
//            return View(aspNetUser);
//        }

//        // POST: /Admin/AspNetUsers/Edit/5
//        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
//        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult Edit([Bind(Include = "Id,FirstName,LastName,Address,DateCreated,LastLoginTime,Activated,Gender,RoleId,DateOfBirth,Deleted,Email,EmailConfirmed,PasswordHash,SecurityStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEndDateUtc,LockoutEnabled,AccessFailedCount,UserName")] AspNetUser aspNetUser)
//        {
//            if (ModelState.IsValid)
//            {
//                // do something
//                return RedirectToAction("Index");
//            }
//            return View(aspNetUser);
//        }

//        // GET: /Admin/AspNetUsers/Delete/5
//        public ActionResult Delete(string id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            AspNetUser aspNetUser = service.GetById(id);
//            if (aspNetUser == null)
//            {
//                return HttpNotFound();
//            }
//            return View(aspNetUser);
//        }

//        // POST: /Admin/AspNetUsers/Delete/5
//        [HttpPost, ActionName("Delete")]
//        [ValidateAntiForgeryToken]
//        public ActionResult DeleteConfirmed(string id)
//        {
//            // do somehting
//            return RedirectToAction("Index");
//        }

//        protected override void Dispose(bool disposing)
//        {
//            if (disposing)
//            {
//                // dosomething
//            }
//            base.Dispose(disposing);
//        }
//    }
//}
