using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using Labixa.Models;
using Outsourcing.Data.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Net.Http;
using System.Text;
using System.Dynamic;
using System.Net.Http.Headers;
using System.Security.Cryptography.Xml;
using Newtonsoft.Json.Linq;
using Outsourcing.Service;
using Outsourcing.Core.Extensions;

namespace Labixa.Controllers
{
    [Authorize]
    public class AccountController : BaseHomeController
    {

        private UserManager<User> _userManager;
        private IUserRoleStore<User> _userRoleManager;
        private ITrackingAttendenceService _TrackingAttendenceService;
        private IBrandService _BrandService;


        //public AccountController(UserManager<User> userManager, ITrackingAttendenceService TrackingAttendenceService)
        public AccountController(UserManager<User> userManager, ITrackingAttendenceService TrackingAttendenceService, IBrandService _brandService)
        {
            _userManager = userManager;
            _TrackingAttendenceService = TrackingAttendenceService;
            this._BrandService = _brandService;
        }

        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            //_userManager.ChangePassword("c21a1169-1e5d-4aa7-9604-1595514608bc", "123456", "labixa@123");
            HttpCookie brandIdCookie = Request.Cookies["BrandId"];
            var selectId = -1;
            //if (brandIdCookie!=null)
            //{
            //    selectId = int.Parse(brandIdCookie.Value);
            //}
            var listBrand = _BrandService.GetBrands().ToSelectListItems(selectId);
            ViewBag.ListBrand = listBrand;
            ViewBag.ReturnUrl = returnUrl;

            LoginViewModel model = new LoginViewModel();
            return View(model);
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindAsync(model.Email, model.Password);
                if (user != null)
                {
                    await SignInAsync(user, model.RememberMe);
                    if (!model.Email.Equals("admin"))
                    {
                        #region update FireBase login
                        ModelFirebase json = new ModelFirebase();
                        HttpClient httpClient = new HttpClient();
                        json = await CommonHttpClient.GetAsync(httpClient,
                                "https://kedusale-e8cff.firebaseio.com/Working/" +
                                user.Extention + ".json?auth=p3fiOyJhb8uZXehJB34YqOd5NlDNiupqtlkofNoB");
                        if (json != null)
                        {
                            json.DisplayName = user.DisplayName;
                            json.Name = user.UserName;
                            json.Id = user.Id;
                            json.Working = "ON";
                            if (json.DateStart != DateTime.Now)
                            {
                                json.DurationOnline = DateTime.Now;
                            }
                            json.DateStart = DateTime.Now;
                            var jsonToSend = JsonConvert.SerializeObject(json, Formatting.None, new IsoDateTimeConverter());
                            HttpContent httpContent = new StringContent(jsonToSend, Encoding.UTF8, "application/json");
                            httpClient = new HttpClient();
                            var responseMessage = await CommonHttpClient.PatchAsync(httpClient,
                                    new Uri("https://kedusale-e8cff.firebaseio.com/Working/" +
                                    user.Extention + ".json?auth=p3fiOyJhb8uZXehJB34YqOd5NlDNiupqtlkofNoB"), httpContent);
                        }
                        #region create tracking
                        var tracking = new TrackingAttendence();
                        tracking = new TrackingAttendence();
                        tracking.DateCreated = DateTime.Now;
                        tracking.DateStart = json.DateStart;
                        tracking.DateEnd = DateTime.Now;
                        tracking.IsActive = true;
                        tracking.IsDeleted = false;
                        tracking.LastEditedTime = DateTime.Now;
                        tracking.TeleId = json.Id;
                        tracking.TeleName = json.DisplayName;
                        _TrackingAttendenceService.CreateTrackingAttendence(tracking);
                        #endregion
                        #endregion
                    }

                    #region set cookie
                    var brand = _BrandService.GetBrandById(int.Parse(model.BrandId.ToString()));
                    HttpCookie brandIdCookie = Request.Cookies["BrandId"];
                    HttpCookie brandNameCookie = Request.Cookies["BrandName"];
                    HttpCookie brandCodeCookie = Request.Cookies["BrandCode"];
                    if (brandIdCookie != null)
                    {
                        brandIdCookie.Value = brand.Id.ToString();
                        brandNameCookie.Value = brand.Name;
                        brandCodeCookie.Value = brand.Code;
                    }
                    else
                    {
                        brandIdCookie = new HttpCookie("BrandId");
                        brandNameCookie = new HttpCookie("BrandName");
                        brandCodeCookie = new HttpCookie("BrandCode");
                        brandIdCookie.Value = brand.Id.ToString();
                        brandNameCookie.Value = brand.Name;
                        brandCodeCookie.Value = brand.Code;
                        brandIdCookie.Expires = brandNameCookie.Expires = brandCodeCookie.Expires = DateTime.Now.AddYears(1);
                    }
                    Response.Cookies.Add(brandIdCookie);
                    Response.Cookies.Add(brandNameCookie);
                    Response.Cookies.Add(brandCodeCookie);
                    #endregion
                    if (user.UserName.Equals("quanly"))
                    {
                        return Redirect("/Report/Dashboard");

                    }
                    else
                    {

                        return RedirectToLocal(returnUrl);
                    }
                }
                else
                {
                    SetAlert("Tài khoản hoặc mật khẩu sai!", "danger");
                    return RedirectToAction("Login", "Account", model);
                    //ModelState.AddModelError("", "Invalid username or password.");
                }
            }

            // If we got this far, something failed, redisplay form
            return RedirectToAction("Login", model);
        }

        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new User() { UserName = model.UserName };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    AddErrors(result);
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }


        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string returnUrl)
        {
            //_userManager.ChangePassword("c21a1169-1e5d-4aa7-9604-1595514608bc", "123456", "labixa@123");
            HttpCookie brandIdCookie = Request.Cookies["BrandId"];
            var selectId = -1;
            //if (brandIdCookie!=null)
            //{
            //    selectId = int.Parse(brandIdCookie.Value);
            //}
            var listBrand = _BrandService.GetBrands().ToSelectListItems(selectId);
            ViewBag.ListBrand = listBrand;
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }
        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model, string url)
        {
            bool hasPassword = HasPassword();
            ViewBag.HasLocalPassword = hasPassword;
            ViewBag.ReturnUrl = Url.Action("ResetPassword");
            var user1 = _userManager.FindById(User.Identity.GetUserId());

            var user = await _userManager.FindAsync(model.Email, model.OldPassword);

            if (hasPassword)
            {
                if (ModelState.IsValid)
                {
                    IdentityResult result = await _userManager.ChangePasswordAsync(User.Identity.GetUserId(), user.PasswordHash, model.NewPassword);
                    if (result.Succeeded)
                    {
                        SetAlert("Cập nhật thành công !", "success");
                        return RedirectToAction("ResetPassword", new { Message = ManageMessageId.ChangePasswordSuccess });
                    }
                    else
                    {
                        SetAlert("Cập nhật thất bại !", "danger");
                        AddErrors(result);
                    }
                }
            }
            else
            {
                // User does not have a password so remove any validation errors caused by a missing OldPassword field
                ModelState state = ModelState["OldPassword"];
                if (state != null)
                {
                    SetAlert("Cập nhật thất bại !", "danger");
                    state.Errors.Clear();
                }

                if (ModelState.IsValid)
                {
                    IdentityResult result = await _userManager.AddPasswordAsync(User.Identity.GetUserId(), model.NewPassword);
                    if (result.Succeeded)
                    {
                        SetAlert("Cập nhật thành công !", "success");
                        return RedirectToAction("ResetPassword", new { Message = ManageMessageId.SetPasswordSuccess });
                    }
                    else
                    {
                        SetAlert("Cập nhật thất bại !", "danger");
                        AddErrors(result);
                    }
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // POST: /Account/Disassociate
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Disassociate(string loginProvider, string providerKey)
        {
            ManageMessageId? message = null;
            IdentityResult result = await _userManager.RemoveLoginAsync(User.Identity.GetUserId(), new UserLoginInfo(loginProvider, providerKey));
            if (result.Succeeded)
            {
                message = ManageMessageId.RemoveLoginSuccess;
            }
            else
            {
                message = ManageMessageId.Error;
            }
            return RedirectToAction("Manage", new { Message = message });
        }

        //
        // GET: /Account/Manage
        public ActionResult Manage(ManageMessageId? message, string erturn = "")
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
                : message == ManageMessageId.SetPasswordSuccess ? "Your password has been set."
                : message == ManageMessageId.RemoveLoginSuccess ? "The external login was removed."
                : message == ManageMessageId.Error ? "An error has occurred."
                : "";
            ViewBag.HasLocalPassword = HasPassword();
            ViewBag.ReturnUrl = Url.Action("Manage");
            return View();
        }

        //
        // POST: /Account/Manage
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Manage(ManageUserViewModel model)
        {
            bool hasPassword = HasPassword();
            ViewBag.HasLocalPassword = hasPassword;
            ViewBag.ReturnUrl = Url.Action("Manage");
            if (hasPassword)
            {
                if (ModelState.IsValid)
                {
                    IdentityResult result = await _userManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
                    if (result.Succeeded)
                    {
                        SetAlert("Cập nhật thành công !", "success");
                        return RedirectToAction("Manage", new { Message = ManageMessageId.ChangePasswordSuccess });
                    }
                    else
                    {
                        SetAlert("Cập nhật không thành công !", "danger");
                        AddErrors(result);
                    }
                }
            }
            else
            {
                // User does not have a password so remove any validation errors caused by a missing OldPassword field
                ModelState state = ModelState["OldPassword"];
                if (state != null)
                {
                    SetAlert("Cập nhật không thành công !", "danger");
                    state.Errors.Clear();
                }

                if (ModelState.IsValid)
                {
                    IdentityResult result = await _userManager.AddPasswordAsync(User.Identity.GetUserId(), model.NewPassword);
                    if (result.Succeeded)
                    {
                        SetAlert("Cập nhật thành công !", "success");
                        return RedirectToAction("Manage", new { Message = ManageMessageId.SetPasswordSuccess });
                    }
                    else
                    {
                        SetAlert("Cập nhật không thành công !", "danger");
                        AddErrors(result);
                    }
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // Sign in the user with this external login provider if the user already has a login
            var user = await _userManager.FindAsync(loginInfo.Login);
            if (user != null)
            {
                await SignInAsync(user, isPersistent: false);
                return RedirectToLocal(returnUrl);
            }
            else
            {
                // If the user does not have an account, then prompt the user to create an account
                ViewBag.ReturnUrl = returnUrl;
                ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { UserName = loginInfo.DefaultUserName });
            }
        }

        //
        // POST: /Account/LinkLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LinkLogin(string provider)
        {
            // Request a redirect to the external login provider to link a login for the current user
            return new ChallengeResult(provider, Url.Action("LinkLoginCallback", "Account"), User.Identity.GetUserId());
        }

        //
        // GET: /Account/LinkLoginCallback
        public async Task<ActionResult> LinkLoginCallback()
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync(XsrfKey, User.Identity.GetUserId());
            if (loginInfo == null)
            {
                return RedirectToAction("Manage", new { Message = ManageMessageId.Error });
            }
            var result = await _userManager.AddLoginAsync(User.Identity.GetUserId(), loginInfo.Login);
            if (result.Succeeded)
            {
                return RedirectToAction("Manage");
            }
            return RedirectToAction("Manage", new { Message = ManageMessageId.Error });
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Manage");
            }

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new User() { UserName = model.UserName };
                var result = await _userManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await _userManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInAsync(user, isPersistent: false);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut();
            return RedirectToAction("Index", "Home");
        }
        public async Task<ActionResult> SignOut()
        {
            AuthenticationManager.SignOut();
            if (User.IsInRole("Telesale"))
            {
                #region update FireBase OFF
                var user = _userManager.FindById(User.Identity.GetUserId());
                if (user != null)
                {

                    ModelFirebase json = new ModelFirebase();
                    HttpClient httpClient = new HttpClient();
                    TrackingAttendence tracking = new TrackingAttendence();
                    json = await CommonHttpClient.GetAsync(httpClient,
                            "https://kedusale-e8cff.firebaseio.com/Working/" +
                            user.Extention + ".json?auth=p3fiOyJhb8uZXehJB34YqOd5NlDNiupqtlkofNoB");
                    if (json != null)
                    {
                        tracking = _TrackingAttendenceService.GetTrackingAttendences().Where(p => p.DateStart.Value.Date == json.DateStart.Value.Date && p.DurationDate == null && p.TeleId.Equals(json.Id)).FirstOrDefault();
                        if (tracking != null)
                        {
                            tracking.DateCreated = DateTime.Now;
                            //tracking.DateStart = json.DateStart;
                            tracking.DateEnd = DateTime.Now;
                            tracking.IsActive = true;
                            tracking.IsDeleted = false;
                            tracking.LastEditedTime = DateTime.Now;
                            tracking.TeleId = json.Id;
                            tracking.TeleName = json.DisplayName;
                            tracking.DurationDate = DateTime.Now.Subtract(json.DateStart.Value.TimeOfDay);
                            _TrackingAttendenceService.EditTrackingAttendence(tracking);
                        }
                        else
                        {
                            //tracking = new TrackingAttendence();
                            //tracking.DateCreated = DateTime.Now;
                            //tracking.DateStart = json.DateStart;
                            //tracking.DateEnd = DateTime.Now;
                            //tracking.IsActive = true;
                            //tracking.IsDeleted = false;
                            //tracking.LastEditedTime = DateTime.Now;
                            //tracking.TeleId = json.Id;
                            //tracking.TeleName = json.DisplayName;
                            //_TrackingAttendenceService.CreateTrackingAttendence(tracking);
                        }
                        json.Working = "OFF";
                        var jsonToSend = JsonConvert.SerializeObject(json, Formatting.None, new IsoDateTimeConverter());
                        HttpContent httpContent = new StringContent(jsonToSend, Encoding.UTF8, "application/json");
                        httpClient = new HttpClient();

                        var sendMessage = await CommonHttpClient.PatchAsync(httpClient,
                                new Uri("https://kedusale-e8cff.firebaseio.com/Working/" +
                                user.Extention + ".json?auth=p3fiOyJhb8uZXehJB34YqOd5NlDNiupqtlkofNoB"), httpContent);
                    }
                }
                #endregion
            }
            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        [ChildActionOnly]
        public ActionResult RemoveAccountList()
        {
            var linkedAccounts = _userManager.GetLogins(User.Identity.GetUserId());
            ViewBag.ShowRemoveButton = HasPassword() || linkedAccounts.Count > 1;
            return (ActionResult)PartialView("_RemoveAccountPartial", linkedAccounts);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && _userManager != null)
            {
                _userManager.Dispose();
                _userManager = null;
            }
            base.Dispose(disposing);
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private async Task SignInAsync(User user, bool isPersistent)
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
            var identity = await _userManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
            AuthenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = isPersistent }, identity);
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private bool HasPassword()
        {
            var user = _userManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PasswordHash != null;
            }
            return false;
        }

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            Error
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        private class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri) : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties() { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
    public static class CommonHttpClient
    {
        public static async Task<HttpResponseMessage> PatchAsync(this HttpClient client, Uri requestUri, HttpContent iContent)
        {
            var method = new HttpMethod("PATCH");

            var request = new HttpRequestMessage(method, requestUri)
            {
                Content = iContent
            };

            HttpResponseMessage response = new HttpResponseMessage();
            // In case you want to set a timeout
            //CancellationToken cancellationToken = new CancellationTokenSource(60).Token;

            try
            {
                response = await client.SendAsync(request);
                // If you want to use the timeout you set
                //response = await client.SendRequestAsync(request).AsTask(cancellationToken);
            }
            catch (TaskCanceledException e)
            {
            }

            return response;
        }
        public static async Task<ModelFirebase> GetAsync(this HttpClient client, string Uri)
        {
            var resutl_Json = "";
            var firebase = new ModelFirebase();
            HttpResponseMessage response = new HttpResponseMessage();
            try
            {
                response = await client.GetAsync(Uri);
                // If you want to use the timeout you set
                //response = await client.SendRequestAsync(request).AsTask(cancellationToken);
                response.EnsureSuccessStatusCode();
                resutl_Json = await response.Content.ReadAsStringAsync();
                firebase = new ModelFirebase(resutl_Json);
                response.Dispose();
            }
            catch (TaskCanceledException e)
            {
            }

            return firebase;
        }
    }
    public class ModelFirebase
    {
        public ModelFirebase(string json = "")
        {
            if (json != "")
            {

                JObject jObject = JObject.Parse(json);
                Working = (string)jObject["Working"];
                DisplayName = (string)jObject["DisplayName"];
                Id = (string)jObject["Id"];
                Name = (string)jObject["Name"];
                DateStart = (DateTime?)jObject["DateStart"];
                DurationOnline = (DateTime?)jObject["DurationOnline"];
            }
        }
        public string DisplayName { get; set; }
        public string Name { get; set; }
        public string Id { get; set; }
        public string Working { get; set; }
        public DateTime? DateStart { get; set; }
        public DateTime? DurationOnline { get; set; }
    }
}