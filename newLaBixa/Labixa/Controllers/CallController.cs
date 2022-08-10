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
    public class CallController : Controller
    {
        private IStaffService _staffService;
        private static readonly HttpClient client = new HttpClient();

        public CallController(IStaffService staffService)
        {
            _staffService = staffService;
        }

        //
        // GET: /Call/
        public ActionResult Index()
        {           
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> CallV1Async(string phone,string ext)
        {
            var values = new Dictionary<string, string>
            {
                { "ServiceName", "CF-PBX0001442" },
                { "AuthUser", "ODS000953" },
                { "AuthKey", "d8e8990b-60c6-4b50-9fac-f766078a3f19" },
                { "Prefix", "0" },
                { "Ext", ext },
                { "PhoneName", "HMC" },
                { "PhoneNumber", phone },
                { "KeySearch", Guid.NewGuid().ToString("N")}
            };

            var content = new FormUrlEncodedContent(values);

            var response = await client.PostAsync("https://api.cloudfone.vn/api/CloudFone/AutoCall", content);

            var responseString = await response.Content.ReadAsStringAsync();

            Staff s = new Staff();
            s.Name = responseString;
            _staffService.CreateStaff(s);
            return View("Index");
        }

       // [Route("Popup")]
        [HttpPost]
        public async Task<ActionResult> GetPopUp(string ApiKey, string CallNumber, string CallName, string ReceiptNumber, string Key, string KeyRinging, string Status, string Direction, string NumberPBX, string Message, string Data)
        {
            string response = $"\"ApiKey\":{ApiKey},\"CallNumber\":{CallNumber},\"CallName\":{CallName},\"ReceiptNumber\":{ReceiptNumber},\"Key\":{Key},\"KeyRinging\":{KeyRinging},\"Status\":{Status}\"Direction\":{Direction},\"NumberPBX\":{NumberPBX},\"Message\":{Message},\"Data\":{Data}";

            Staff s = new Staff();
            s.Description = response;
            _staffService.CreateStaff(s);
            return View("Index");
        }
    }
}