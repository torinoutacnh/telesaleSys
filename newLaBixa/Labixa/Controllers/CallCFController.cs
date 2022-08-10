using Outsourcing.Data.Models;
using Outsourcing.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Labixa.Controllers
{
    public class CallCFController : Controller
    {
        private IStaffService _staffService;
        private static readonly HttpClient client = new HttpClient();

        public CallCFController(IStaffService staffService)
        {
            _staffService = staffService;
        }
        //
        // GET: /CallCF/
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public  JsonResult GetPopUp(string ApiKey, string CallNumber, string CallName, string ReceiptNumber, string Key, string KeyRinging, string Status, string Direction, string NumberPBX, string Message, string Data)
        {
            string response = $"\"ApiKey\":{ApiKey},\"CallNumber\":{CallNumber},\"CallName\":{CallName},\"ReceiptNumber\":{ReceiptNumber},\"Key\":{Key},\"KeyRinging\":{KeyRinging},\"Status\":{Status}\"Direction\":{Direction},\"NumberPBX\":{NumberPBX},\"Message\":{Message},\"Data\":{Data}";

            Staff s = new Staff();
            s.Description = response;
            _staffService.CreateStaff(s);
            return Json(response);
        }
    }
}